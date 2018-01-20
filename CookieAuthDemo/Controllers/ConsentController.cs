using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CookieAuthDemo.Services;
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
        private readonly ConsenService _consenService;


        public ConsentController(ConsenService consenService)
        {
            _consenService = consenService;
        }

        // GET: /<controller>/
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var model = await _consenService.BuildConsenViewModelAsync(returnUrl);

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
            var result = await _consenService.ProcessConsentAsync(viewModel);
            if (result.IsRedirect)
            {
                return Redirect(result.ReturnUrl);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(result.ValidationError))
                {
                    ModelState.AddModelError(string.Empty, result.ValidationError);
                }

                return View(result.ConsentViewModel);
            }
        }
    }
}
