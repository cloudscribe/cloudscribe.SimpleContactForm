# cloudscribe.SimpleContactForm
a simple yet flexible contact form for ASP.NET Core. Get the big picture on cloudscribe projects at [cloudscribe.com](https://www.cloudscribe.com)

### Build Status

| Windows  | Linux/Mac |
| ------------- | ------------- |
| [![Build status](https://ci.appveyor.com/api/projects/status/b0erueoiou4oghev/branch/master?svg=true)](https://ci.appveyor.com/project/joeaudette/cloudscribe-simplecontactform/branch/master)  | [![Build Status](https://travis-ci.org/cloudscribe/cloudscribe.SimpleContactForm.svg?branch=master)](https://travis-ci.org/cloudscribe/cloudscribe.SimpleContactForm)  |

It uses recaptcha unless you are authenticated.

Additional content and styling can be customized in /Views/Contact/index.cshtml

It uses a strongly typed message model and Razor views for rendering both the html and plain text email body.

The Notification templates are under Views/Shared/EmailTemplates and they can be customized as well.

It uses the submitted email address in the "replyto" of the notification email so that it may be possible to reply to the submitter from the notification message depending on email system and whether it supports replyto.

There is an option to copy the submitter with the email notification, this is set to true by default. The submitter notification uses different email templates to leave out the ipaddress information and to allow it to be customized independently of the admin notification.

It supports up to 12 additional custom fields named CustomField1 through CustomField12. Those can be added to the index.cshtml to add more fields to the form and to the notification templates. You can of course use custom labels within the cshtml files so the custom fields can be labelled any way you like.

See appsettings.json in the WebApp for configuration, these are the settings:

    "ContactFormSettings": {
    "Id": "default",
    "NotificationEmailCsv": "",
    "NotificationSubject": "Contact Form Submission",
    "CopySubmitterEmailOnSubmission": "true"
      },
      "SmtpOptions": {
        "Server": "",
        "Port": "25",
        "User": "",
        "Password": "",
        "UseSsl": "false",
        "RequiresAuthentication": "false",
        "DefaultEmailFromAddress": "",
        "DefaultEmailFromAlias": ""
      },
      "RecaptchaKeys": {
        "PublicKey": "",
        "PrivateKey": "",
	    "Invisible": false
      },
      "SmtpMessageProcessorOptions": {
        "NotificationTextViewName": "EmailTemplates/ContactFormTextEmail",
        "NotificationHtmlViewName": "EmailTemplates/ContactFormHtmlEmail",
        "SubmitterNotificationTextViewName": "EmailTemplates/ContactFormSubmitterTextEmail",
        "SubmitterNotificationHtmlViewName": "EmailTemplates/ContactFormSubmitterHtmlEmail"
      }
	  

Note that you should always get keys for Invisible Captcha, the Invisible Captcha keys work when configured both as visible and invisible. Whereas keys not created for Invisible captcha won't work if you set invisible to true.
