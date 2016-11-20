// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-19
// 


namespace cloudscribe.SimpleContactForm.Models
{
    public class ContactFormSettings
    {
        public string Id { get; set; } = "default";
        public string NotificationEmailCsv { get; set; } = string.Empty;

        public string NotificationSubject { get; set; } = "Contact Form Submission";

        public bool CopySubmitterEmailOnSubmission { get; set; } = false;
    }
}
