// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2017-04-01
// 

using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContactForm.ViewModels
{
    public class MessageViewModel
    {
        [Required(ErrorMessage = "The FormId field is required.")]
        public string FormId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress]
        public string Email { get; set; }

        public string Subject { get; set; }

        [Required(ErrorMessage = "The Message field is required.")]
        public string Message { get; set; }

        public string RecaptchaPublicKey { get; set; } = string.Empty;
        public bool UseInvisibleCaptcha { get; set; } = false;

        public string CustomField1 { get; set; }
        public string CustomField2 { get; set; }
        public string CustomField3 { get; set; }
        public string CustomField4 { get; set; }
        public string CustomField5 { get; set; }
        public string CustomField6 { get; set; }
        public string CustomField7 { get; set; }
        public string CustomField8 { get; set; }
        public string CustomField9 { get; set; }
        public string CustomField10 { get; set; }
        public string CustomField11 { get; set; }
        public string CustomField12 { get; set; }
    }
}
