// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-19
// 

using cloudscribe.SimpleContactForm.Components;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.SimpleContactForm.ViewModels;
using cloudscribe.Web.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Controllers
{
    public class ContactController : Controller
    {
        public ContactController(
            ContactFormService formService,
            IPrePopulateContactForm formPopulator,
            IStringLocalizer<SimpleContactFormStringResources> localizer,
            ILogger<ContactController> logger
            )
        {
            _sr = localizer;
            _log = logger;
            _formPopulator = formPopulator;
            _formService = formService;
        }

        private ContactFormService _formService;
        private IPrePopulateContactForm _formPopulator;
        private IStringLocalizer _sr;
        private ILogger _log;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = _sr["Contact"];
            
            var isConfigured = await _formService.IsConfigured();
            if(!isConfigured)
            {
                return View("NotConfigured");
            }
            var model = new MessageViewModel();
            var form = await _formService.GetFormSettings();
            model.FormId = form.Id;
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                var captchaKeys = await _formService.GetRecaptchaKeys();
                model.RecaptchaPublicKey = captchaKeys.PublicKey;
                model.UseInvisibleCaptcha = captchaKeys.Invisible;
            }
            else
            {
                await _formPopulator.Prepopulate(model, User);
            }
            

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(MessageViewModel model)
        {
            ViewData["Title"] = _sr["Contact"];
            var captchaKeys = await _formService.GetRecaptchaKeys();
            if (!HttpContext.User.Identity.IsAuthenticated)
            {
                model.RecaptchaPublicKey = captchaKeys.PublicKey;
                model.UseInvisibleCaptcha = captchaKeys.Invisible;
            }
            
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            
            if((!HttpContext.User.Identity.IsAuthenticated) && (!string.IsNullOrEmpty(captchaKeys.PrivateKey)))
            {
                var captchaResponse = await this.ValidateRecaptcha(Request, captchaKeys.PrivateKey);

                if (!captchaResponse.Success)
                {
                    ModelState.AddModelError("recaptchaerror", _sr["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var ipAddress = HttpContext.GetIpV4Address(); 
            var result = await _formService.ProcessMessage(model, ipAddress);
            if(result.Succeeded)
            {
                return View("Success");
            }
            else
            {
                return View("Fail");
            }

            
        }


    }
}
