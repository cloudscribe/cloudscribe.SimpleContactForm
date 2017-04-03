// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-19
// 

using cloudscribe.SimpleContactForm.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using cloudscribe.SimpleContactForm.ViewModels;
using cloudscribe.Web.Common.Extensions;

namespace cloudscribe.SimpleContactForm.Controllers
{
    public class ContactController : Controller
    {
        public ContactController(
            ContactFormService formService,
            IStringLocalizer<SimpleContactFormStringResources> localizer,
            ILogger<ContactController> logger
            )
        {
            sr = localizer;
            log = logger;
            this.formService = formService;
        }

        private ContactFormService formService;
        private IStringLocalizer sr;
        private ILogger log;

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = sr["Contact"];
            
            var isConfigured = await formService.IsConfigured();
            if(!isConfigured)
            {
                return View("NotConfigured");
            }
            var model = new MessageViewModel();
            var form = await formService.GetFormSettings();
            model.FormId = form.Id;
            if(!HttpContext.User.Identity.IsAuthenticated)
            {
                var captchaKeys = await formService.GetRecaptchaKeys();
                model.RecaptchaPublicKey = captchaKeys.PublicKey;
                model.UseInvisibleCaptcha = captchaKeys.Invisible;
            }
            

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Index(MessageViewModel model)
        {
            ViewData["Title"] = sr["Contact"];
            var captchaKeys = await formService.GetRecaptchaKeys();
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
                    ModelState.AddModelError("recaptchaerror", sr["reCAPTCHA Error occured. Please try again"]);
                    return View(model);
                }
            }

            var ipAddress = HttpContext.GetIpV4Address(); 
            var result = await formService.ProcessMessage(model, ipAddress);
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
