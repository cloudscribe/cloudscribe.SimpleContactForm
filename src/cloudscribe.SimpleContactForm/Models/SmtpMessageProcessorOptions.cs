namespace cloudscribe.SimpleContactForm.Models
{
    public class SmtpMessageProcessorOptions
    {
        public string NotificationTextViewName { get; set; } = "EmailTemplates/ContactFormTextEmail";
        public string NotificationHtmlViewName { get; set; } = "EmailTemplates/ContactFormHtmlEmail";

        public string SubmitterNotificationTextViewName { get; set; } = "EmailTemplates/ContactFormSubmitterTextEmail";
        public string SubmitterNotificationHtmlViewName { get; set; } = "EmailTemplates/ContactFormSubmitterHtmlEmail";
    }
}
