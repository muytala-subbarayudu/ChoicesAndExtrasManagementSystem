using ChoicesExtrasManagement.Models;

namespace ChoicesExtrasManagement.ViewModels
{
    public class NotificationVM
    {
        public Plot? Plot { get; set; }
        
        public List<Notification> NotificationsList { get; set; }
    }
}
