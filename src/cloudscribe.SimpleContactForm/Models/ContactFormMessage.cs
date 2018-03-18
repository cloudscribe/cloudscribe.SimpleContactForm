// Copyright (c) Source Tree Solutions, LLC. All rights reserved.
// Licensed under the Apache License, Version 2.0.
// Author:					Joe Audette
// Created:					2016-11-19
// Last Modified:			2016-11-20
// 


using cloudscribe.SimpleContactForm.ViewModels;

namespace cloudscribe.SimpleContactForm.Models
{
    public class ContactFormMessage
    {
        public object Tenant { get; set; } = null;
        public string FormId { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string Subject { get; set; }

        public string Message { get; set; }

        public string IpAddress { get; set; }

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

        public static ContactFormMessage FromViewModel(MessageViewModel model, string ipAddress)
        {
            var message = new ContactFormMessage();
            message.FormId = model.FormId;
            message.Email = model.Email;
            message.Name = model.Name;
            message.Subject = model.Subject;
            message.Message = model.Message;
            message.IpAddress = ipAddress;
            message.CustomField1 = model.CustomField1;
            message.CustomField10 = model.CustomField10;
            message.CustomField11 = model.CustomField11;
            message.CustomField12 = model.CustomField12;
            message.CustomField2 = model.CustomField2;
            message.CustomField3 = model.CustomField3;
            message.CustomField4 = model.CustomField4;
            message.CustomField5 = model.CustomField5;
            message.CustomField6 = model.CustomField6;
            message.CustomField7 = model.CustomField7;
            message.CustomField8 = model.CustomField8;
            message.CustomField9 = model.CustomField9;

            return message;
        }
    }
}
