using System.ComponentModel.DataAnnotations;

namespace EasyCare.Core.Services.Notification
{
    public class NotificationHubOptions
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string ConnectionString { get; set; }
    }
}
