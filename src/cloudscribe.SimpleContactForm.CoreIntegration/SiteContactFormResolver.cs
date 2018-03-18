using cloudscribe.Core.Models;
using cloudscribe.SimpleContactForm.Components;
using cloudscribe.SimpleContactForm.Models;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.CoreIntegration
{
    public class SiteContactFormResolver : ConfigContactFormResolver
    {
        public SiteContactFormResolver(
            SiteContext currentSite,
            IOptions<ContactFormSettings> contactFormAccessor
            ) : base(contactFormAccessor)
        {
            _currentSite = currentSite;
        }

        private SiteContext _currentSite;

        public override async Task<ContactFormSettings> GetCurrentContactForm()
        {
            var form = await base.GetCurrentContactForm();
            if (!string.IsNullOrWhiteSpace(_currentSite.AccountApprovalEmailCsv))
            {
                var newForm = new ContactFormSettings
                {
                    Id = _currentSite.Id.ToString(),
                    CopySubmitterEmailOnSubmission = form.CopySubmitterEmailOnSubmission,
                    NotificationSubject = form.NotificationSubject,
                    NotificationEmailCsv = _currentSite.AccountApprovalEmailCsv
                };

                return newForm;
            }

            return form;
        }

    }
}
