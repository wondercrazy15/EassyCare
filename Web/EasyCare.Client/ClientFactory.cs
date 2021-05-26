using EasyCare.Client.Clients;
using EasyCare.Client.Contact;

namespace EasyCare.Client
{
    public class ClientFactory : IClientFactory
    {
        private IDeviceClient _deviceClient;
        private INotificationsClient _notificationsClient;
        private ISeniorClient _seniorClient;
        private ISensorMessagesClient _sensorMessagesClient;
        private ISignedUpUsersClient _signedUpUserClient;
        private ISupervisorClient _supervisorClient;
        private ITanClient _tanClient;
        private ICalendarEventClient _calendarEventClient;
        private IMessageClient _messageClient;
        private INotificationMessageClient _notificationMessageClient;
        private IDrugsClient _drugsClient;
        private IUserClient _userClient;
        private IGroupClient _groupClient;
        private ITokenClient _tokenClient;

        public static IClientFactory Create(Options options)
        {
            return new ClientFactory(options);
        }

        private readonly Options _options;

        private ClientFactory(Options options)
        {
            _options = options;
        }

        public IDeviceClient DeviceClient => 
            _deviceClient ?? (_deviceClient = new DeviceClient(_options));

        public INotificationsClient NotificationsClient =>
            _notificationsClient ?? (_notificationsClient = new NotificationsClient(_options));

        public ISeniorClient SeniorClient =>
            _seniorClient ?? (_seniorClient ?? (_seniorClient = new SeniorClient(_options)));

        public ISensorMessagesClient SensorMessagesClient =>
            (_sensorMessagesClient ?? (_sensorMessagesClient = new SensorMessageClient(_options)));

        public ISignedUpUsersClient SignedUpUsersClient =>
            (_signedUpUserClient ?? (_signedUpUserClient = new SignedUpUsersClient(_options)));

        public ISupervisorClient SupervisorClient =>
            (_supervisorClient ?? (_supervisorClient = new SupervisorClient(_options)));

        public ITanClient TanClient =>
            (_tanClient ?? (_tanClient = new TanClient(_options)));

        public ICalendarEventClient CalendarEventClient =>
            (_calendarEventClient ?? (_calendarEventClient = new CalendarEventClient(_options)));

        public IMessageClient MessageClient =>
            (_messageClient ?? (_messageClient = new MessageClient(_options)));

        public INotificationMessageClient NotificationMessageClient =>
            (_notificationMessageClient ?? (_notificationMessageClient = new NotificationMessageClient(_options)));

        public IDrugsClient DrugsClient =>
            (_drugsClient ?? (_drugsClient = new DrugsClient(_options)));

        public IUserClient UserClient =>
             (_userClient ?? (_userClient = new UserClient(_options)));

        public IGroupClient GroupClient =>
             (_groupClient ?? (_groupClient = new GroupClinet(_options)));

        public ITokenClient TokenClient =>
             (_tokenClient ?? (_tokenClient = new TokenClient(_options)));
    }
}