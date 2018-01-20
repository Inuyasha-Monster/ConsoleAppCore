using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthDemo.ViewModels;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace CookieAuthDemo.Services
{
    public class ConsenService
    {
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IIdentityServerInteractionService _identityServerInteractionService;
        public ConsenService(IClientStore clientStore, IResourceStore resourceStore, IIdentityServerInteractionService identityServerInteractionService)
        {
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _identityServerInteractionService = identityServerInteractionService;
        }

        #region Private Methods
        private ConsentViewModel CreateConsenViewModel(AuthorizationRequest request, Client client,
                 Resources resources, InputConsentViewModel model)
        {

            var selectedScopes = model?.ScopesConsented ?? Enumerable.Empty<string>();
            var vm = new ConsentViewModel();
            vm.ClientId = client.ClientId;
            vm.ClientLogoUrl = client.LogoUri;
            vm.ClientUrl = client.ClientUri;
            vm.RememberConsent = model?.RememberConsent ?? true; //client.AllowRememberConsent;
            vm.IdentityScopes = resources.IdentityResources.Select(x => CreateScopeViewModel(x, selectedScopes.Contains(x.Name) || model == null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => CreateScopeViewModel(x, selectedScopes.Contains(x.Name) || model == null));
            return vm;
        }

        private ScopeViewModel CreateScopeViewModel(IdentityResource identityResource, bool check)
        {
            return new ScopeViewModel()
            {
                Name = identityResource.Name,
                DisplayName = identityResource.DisplayName,
                Description = identityResource.Description,
                Checked = identityResource.Required || check,
                Required = identityResource.Required,
                Emphasize = identityResource.Emphasize
            };
        }

        private ScopeViewModel CreateScopeViewModel(Scope scope, bool check)
        {
            return new ScopeViewModel()
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Checked = scope.Required || check,
                Required = scope.Required,
                Emphasize = scope.Emphasize
            };
        }


        #endregion

        public async Task<ConsentViewModel> BuildConsenViewModelAsync(string returnUrl, InputConsentViewModel model = null)
        {
            var request = await _identityServerInteractionService.GetAuthorizationContextAsync(returnUrl);
            if (request == null) return null;

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);

            return CreateConsenViewModel(request, client, resources, model);
        }

        public async Task<ProcessConsentResult> ProcessConsentAsync(InputConsentViewModel viewModel)
        {

            var result = new ProcessConsentResult();

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

                result.ValidationError = "请至少选择一个权限";
            }

            if (consentResponse != null)
            {
                var request = await _identityServerInteractionService.GetAuthorizationContextAsync(viewModel.ReturnUrl);
                await _identityServerInteractionService.GrantConsentAsync(request, consentResponse);

                result.ReturnUrl = viewModel.ReturnUrl;
            }
            else
            {
                var consentViewModel = await BuildConsenViewModelAsync(viewModel.ReturnUrl, viewModel);
                result.ConsentViewModel = consentViewModel;
            }

            return result;
        }
    }
}
