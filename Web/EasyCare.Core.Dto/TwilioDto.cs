using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Dto
{
    public class TwilioDto
    {
        public string ChannelSid { get; set; }
        public string ClientIdentity { get; set; }
        public string EventType { get; set; }
        public string InstanceSid { get; set; }
        public string Attributes { get; set; }
        public DateTime DateCreated { get; set; }
        public string Index { get; set; }
        public string From { get; set; }
        public string MessageSid { get; set; }
        public string Body { get; set; }
        public string AccountSid { get; set; }
        public string Source { get; set; }
    }
}
