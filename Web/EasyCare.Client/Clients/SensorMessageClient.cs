using EasyCare.Client.Clients.Base;
using EasyCare.Client.Contact;
using EasyCare.Core.Dto;

namespace EasyCare.Client.Clients
{
    public class SensorMessageClient: BaseCrudClient<SensorMessageDto>, ISensorMessagesClient
    {
        public SensorMessageClient(Options options) : base(options, "SensorMessage")
        {
        }
    }
}