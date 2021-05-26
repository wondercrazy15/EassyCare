using EasyCare.Client.Clients;
using EasyCare.Client.Contact;

namespace EasyCare.Client
{
    public interface IClientFactory
    {
        IDeviceClient DeviceClient { get; }
        
        INotificationsClient NotificationsClient { get; }
        
        ISeniorClient SeniorClient { get; }
        
        ISensorMessagesClient SensorMessagesClient { get; }
        
        ISignedUpUsersClient SignedUpUsersClient { get; }
        
        ISupervisorClient SupervisorClient { get; }
        
        ITanClient TanClient { get; }
        
        ICalendarEventClient CalendarEventClient { get; }

        IMessageClient MessageClient { get; }

        INotificationMessageClient NotificationMessageClient { get; }

        IUserClient UserClient { get; }

        IDrugsClient DrugsClient { get; }

        IGroupClient GroupClient { get; }

        ITokenClient TokenClient { get; }
    }
}