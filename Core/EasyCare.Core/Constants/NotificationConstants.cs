namespace EasyCare.Core.Constants
{
    public class NotificationConstants
    {
        public static string[] SubscriptionTags = { "default" };
        
        public const string NotificationHubName = "easynotificationhub";

        public const string FullAccessConnectionString = 
            "Endpoint=sb://easynotificationhub-0714.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=s6yQq27Wb6D3upuiNPxRTy6Y084iUoyOERUUW+mihDE=";

        public const string FullListenSharedAccessString =
            "Endpoint=sb://easynotificationhub-0714.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=X36Ex0H4bdfUQL0y2npOwingcS4/9pzIDRJ8vc2LBqo=";
        
        public const string APNTemplateBody = "{\"aps\":{\"alert\":\"$(messageParam)\"}}";
    }
}