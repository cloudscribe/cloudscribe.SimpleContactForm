using System.Threading.Tasks;

namespace cloudscribe.SimpleContactForm.Models
{
    public interface IProcessMessages
    {
        Task<MessageResult> Process(ContactFormMessage message);

    }
}
