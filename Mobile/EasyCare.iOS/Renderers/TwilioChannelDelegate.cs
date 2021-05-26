using System;
using System.Collections.Generic;
using EasyCare.Models.Chat;
using Foundation;
using Twilio.Chat.iOS;
using Xamarin.Forms;

namespace EasyCare.iOS.Renderers
{
    public class TwilioChannelDelegate : ChannelDelegate
    {
        public static Channel _channel;
       
        public TwilioChannelDelegate(Channel channel)
        {
            _channel = channel;
        }

        public TwilioChannelDelegate()
        {

        }

        public override void ChannelUpdated(TwilioChatClient client, Channel channel, ChannelUpdate updated)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"ChannelUpdated reason: {updated}");
            if (updated == ChannelUpdate.Attributes)
            {
                //Logger.Info($"Channel: {channel.Sid}", $"Channel attributes: {channel.Attributes.ToDebugLog()}");
            }
        }

        public override void ChannelSynchronizationStatusUpdated(TwilioChatClient client, Channel channel, ChannelSynchronizationStatus status)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"SynchronizationStatusUpdated: {status}");
        }

        public override void ChannelDeleted(TwilioChatClient client, Channel channel)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"ChannelDeleted");
        }

        public override void MemberJoined(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"MemberJoined: {member.Sid}");
            //Logger.Info($"Channel: {channel.Sid}", $"Member attributes: {member.Attributes.ToDebugLog()}");
        }

        public override void MemberUpdated(TwilioChatClient client, Channel channel, Member member, MemberUpdate updated)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"MemberUpdated: {member.Sid}, reason: {updated}");
            if (updated == MemberUpdate.Attributes)
            {
                //Logger.Info($"Channel: {channel.Sid}", $"Member attributes: {member.Attributes.ToDebugLog()}");
            }
        }

        public override void MemberLeft(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"MemberLeft: {member.Sid}");
        }

        public List<ChatMessage> getMessegeList = new List<ChatMessage>();
        public override void MessageAdded(TwilioChatClient client, Channel channel, Message newmessage)
        {
            try
            {
               
            }
            catch (Exception ex)
            {

            }
        }

      

        public override void MessageUpdated(TwilioChatClient client, Channel channel, Message message, MessageUpdate updated)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"MessageUpdated: {message.Sid}, reason: {updated}");
            if (updated == MessageUpdate.Attributes)
            {
                //Logger.Info($"Channel: {channel.Sid}", $"Message attributes: {message.Attributes.ToDebugLog()}");
            }
        }

        public override void MessageDeleted(TwilioChatClient client, Channel channel, Message message)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"MessageDeleted: {message.Sid}");
        }

        public override void TypingStartedOnChannel(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"TypingStartedOnChannel: {member.Sid}");
        }

        public override void TypingEndedOnChannel(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"TypingEndedOnChannel: {member.Sid}");
        }

        public override void UserUpdated(TwilioChatClient client, Channel channel, Member member, User user, UserUpdate updated)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"UserUpdated: {user.Identity}, member: {member.Sid}, reason: {updated}");
            if (updated == UserUpdate.Attributes)
            {
                //Logger.Info($"Channel: {channel.Sid}", $"User attributes: {user.Attributes.ToDebugLog()}");
            }
        }

        public override void UserSubscribed(TwilioChatClient client, Channel channel, Member member, User user)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"UserSubscribed: {user.Identity}, member: {member.Sid}");
            //Logger.Info($"Channel: {channel.Sid}", $"User attributes: {user.Attributes.ToDebugLog()}");
        }

        public override void UserUnsubscribed(TwilioChatClient client, Channel channel, Member member, User user)
        {
            //Logger.Info($"Channel: {channel.Sid}", $"UserUnsubscribed: {user.Identity}, member: {member.Sid}");
        }

    }
}