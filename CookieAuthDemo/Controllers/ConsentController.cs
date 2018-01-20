using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthDemo.ViewModels;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CookieAuthDemo.Controllers
{
    public class ConsentController : Controller
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;

        public ConsentController(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        private async Task<ConsentViewModel> BuildConsenViewModel(string returnUrl)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null) return null;

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            return CreateConsenViewModel(request, client, resources);
        }

        private ConsentViewModel CreateConsenViewModel(AuthorizationRequest request, Client client,
            Resources resources)
        {
            var vm = new ConsentViewModel();
            vm.ClientId = client.ClientId;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RememberConsent = client.AllowRememberConsent;
            vm.IdentityScopes = resources.IdentityResources.Select(CreateScopeViewModel);
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(CreateScopeViewModel);
            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource)
        {
            return new ScopeViewModel()
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = identityResource.Required,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope)
        {
            return new ScopeViewModel()
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = scope.Required,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await BuildConsenViewModel(returnUrl);

            if (model == null)
            {
                return BadRequest();
            }
            model.ReturnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(InputConsentViewModel viewModel)
        {
            ConsentResponse consentResponse = null;
            if (viewModel.Button == "no")
            {
                consentResponse = ConsentResponse.Denied;
            }
            else if (viewModel.Button == "yes")
            {
                if (viewModel.ScopesConsented != null && viewModel.ScopesConsented.Any())
                {
                    consentResponse = new ConsentResponse()
                    {
                        ScopesConsented = viewModel.ScopesConsented,
                        RememberConsent = viewModel.RememberConsent
                    };
                }
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                return Redirect(viewModel.ReturnUrl);
            }
            else
            {
                return View();
            }
        }
    }
}
