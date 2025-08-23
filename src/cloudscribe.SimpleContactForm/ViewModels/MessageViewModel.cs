﻿// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2017-04-25
// 

using System.ComponentModel.DataAnnotations;

namespace cloudscribe.SimpleContactForm.ViewModels
{
    public class MessageViewModel
    {
        [Required(ErrorMessage = "The FormId field is required.")]
        public string FormId { get; set; }

        [Required(ErrorMessage = "The Name field is required.")]
        [NoUrl(ErrorMessage = "URLs are not allowed in the Name field.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The Email field is required.")]
        [EmailAddress(ErrorMessage ="The Email field is not a valid email address.")]
        [NoUrl(ErrorMessage = "URLs are not allowed in the Email field.")]
        public string Email { get; set; }

        [NoUrl(ErrorMessage = "URLs are not allowed in the Subject field.")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "The Message field is required.")]
        [NoUrl(ErrorMessage = "URLs are not allowed in the Message field.")]
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
