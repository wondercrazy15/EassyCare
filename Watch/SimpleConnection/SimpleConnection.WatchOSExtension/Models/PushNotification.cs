using System;
namespace SimpleConnection.WatchOSExtension.Models
{
    public class PushNotification
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
    }
}