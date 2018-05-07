// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2018-05-07
// 

using cloudscribe.SimpleContactForm.Components;
using cloudscribe.SimpleContactForm.Models;
using cloudscribe.SimpleContactForm.ViewModels;
using cloudscribe.Web.Common.Extensions;
using cloudscribe.Web.Common.Recaptcha;
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
            IRecaptchaServerSideValidator recaptchServerSideValidator,
            IStringLocalizer<SimpleContactFormStringResources> localizer,
            ILogger<ContactController> logger
            )
        {
            StringLocalizer = localizer;
            Log = logger;
            FormPopulator = formPopulator;
            FormService = formService;
            RecaptchServerSideValidator = recaptchServerSideValidator;
        }

        protected ContactFormService FormService { get; private set; }
        protected IPrePopulateContactForm FormPopulator { get; private set; }
        protected IRecaptchaServerSideValidator RecaptchServerSideValidator { get; private set; }
        protected IStringLocalizer StringLocalizer { get; private set; }
        protected ILogger Log { get; private set; }

        [HttpGet]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Index()
        {
            ViewData["Title"] = StringLocalizer["Contact"];
            
            var isConfigured = await FormService.IsConfigured();
            if(!isConfigured)
            {
                return View("NotConfigured");
            }
            var model = new MessageViewModel();
            var form = await FormService.GetFormSettings();
            model.FormId = form.Id;
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                var captchaKeys = await FormService.GetRecaptchaKeys();
                model.RecaptchaPublicKey = captchaKeys.PublicKey;
                model.UseInvisibleCaptcha = captchaKeys.Invisible;
            }
            else
            {
                await FormPopulator.Prepopulate(model, User);
            }
            

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public virtual async Task<IActionResult> Index(MessageViewModel model)
        {
            ViewData["Title"] = StringLocalizer["Contact"];
            var captchaKeys = await FormService.GetRecaptchaKeys();
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
                var captchaResponse = await RecaptchServerSideValidator.ValidateRecaptcha(Request, captchaKeys.PrivateKey);
                if (!captchaResponse.Success)
                {
                    ModelState.AddModelError("recaptchaerror", StringLocalizer["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var ipAddress = HttpContext.GetIpV4Address(); 
            var result = await FormService.ProcessMessage(model, ipAddress);
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
