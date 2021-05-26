using System;
using System.Collections.Generic;
using System.IO;
using EasyCare.Core.Constants;
using EasyCare.Models.Chat;
using EasyCare.Services;
using Foundation;
using GlobalToast;
using Twilio.Chat.iOS;
using Xamarin.Forms;

namespace EasyCare.iOS.Renderers
{
    public class TwilioChatDelegate : TwilioChatClientDelegate
    {


        public TwilioChatDelegate()
        {

        }
        public static Channel _channel;

        public TwilioChatDelegate(Channel channel)
        {
            _channel = channel;
        }

        public override void ConnectionStateUpdated(TwilioChatClient client, ClientConnectionState state)
        {
            //Logger.Info($"ChatClient: {client}", $"ConnectionStateChange: {state}");
        }

        public override void SynchronizationStatusUpdated(TwilioChatClient client, ClientSynchronizationStatus status)
        {
            //Logger.Info($"ChatClient: {client}", $"SynchronizationStatusUpdated: {status}");
            if (status.Equals(ClientSynchronizationStatus.Completed))
            {
                client.ChannelsList.SubscribedChannelsSortedBy(ChannelSortingCriteria.LastMessage, ChannelSortingOrder.Ascending);
            }
        }

        public override void ChannelAdded(TwilioChatClient client, Channel channel)
        {
            //Logger.Info($"ChatClient: {client}", $"ChannelAdded: {channel.Sid}");
            //Logger.Info($"ChatClient: {client}", $"Channel attributes: {channel.Attributes.ToDebugLog()}");
        }

        public override void ChannelUpdated(TwilioChatClient client, Channel channel, ChannelUpdate updated)
        {
            //Logger.Info($"ChatClient: {client}", $"ChannelUpdated: {channel.Sid}, reason: {updated}");
            //Logger.Info($"ChatClient: {client}", $"Channel attributes: {channel.Attributes.ToDebugLog()}");
        }

        public override void ChannelSynchronizationStatusUpdated(TwilioChatClient client, Channel channel, ChannelSynchronizationStatus status)
        {
            //Logger.Info($"ChatClient: {client}", $"ChannelSynchronizationStatusUpdated for channel: {channel.Sid}, status: {status}");
            //Logger.Info($"ChatClient: {client}", $"Channel attributes: {channel.Attributes.ToDebugLog()}");

            if (status.Equals(ChannelSynchronizationStatus.All) && channel.Status.Equals(ChannelStatus.Joined))
            {
                //Logger.Info($"ChatClient: {client}", $"Got joined channel: {channel.Sid}");
                channel.Delegate = new TwilioChannelDelegate();
                //Logger.Info($"Channel: {channel.Sid}", $"Notification level: {channel.NotificationLevel}");

                channel.GetMessagesCountWithCompletion((result, count) =>
                {
                    if (result.IsSuccessful)
                    {
                        //Logger.Info($"Channel: {channel.Sid}", $"Messages count: {count}");
                    }
                    else
                    {
                        //Logger.Error($"Channel: {channel.Sid}",
                        //             $"Error: {result.Error}, " +
                        //             $"code: {result.ResultCode}, " +
                        //             $"text: {result.ResultText}");
                    }
                });

                channel.GetMembersCountWithCompletion((result, count) =>
                {
                    if (result.IsSuccessful)
                    {
                        //Logger.Info($"Channel: {channel.Sid}", $"Members count: {count}");
                    }
                    else
                    {
                        //Logger.Error($"Channel: {channel.Sid}",
                        //$"Error: {result.Error}, " +
                        //$"code: {result.ResultCode}, " +
                        //$"text: {result.ResultText}");
                    }
                });
                channel.Members.MembersWithCompletion((result, members) =>
                {
                    if (result.IsSuccessful)
                    {
                        //Logger.Info($"Channel: {channel.Sid}", $"Members: {result}");
                        foreach (Member member in members.Items)
                        {
                            //Logger.Info($"Channel: {channel.Sid}", $"Got member: {member.Sid} with type {member.Type}");
                        };
                    }
                    else
                    {
                        //Logger.Error($"Channel: {channel.Sid}",
                        //$"Error: {result.Error}, " +
                        //$"code: {result.ResultCode}, " +
                        //$"text: {result.ResultText}");
                    }
                });

                channel.Messages.GetLastMessagesWithCount(10, (result, messages) =>
                {
                    if (result.IsSuccessful)
                    {
                        //Logger.Info($"Channel: {channel.Sid}", $"Messages: {result}");
                        foreach (Message message in messages)
                        {
                            //Logger.Info($"Channel: {channel.Sid}", $"Got message: {message.Sid} created on {message.TimestampAsDate} with type {message.Type} from member {message.MemberSid}");
                        };
                    }
                    else
                    {
                        //Logger.Error($"Channel: {channel.Sid}",
                        //$"Error: {result.Error}, " +
                        //$"code: {result.ResultCode}, " +
                        //$"text: {result.ResultText}");
                    }

                });
            }
            else if (status.Equals(ChannelSynchronizationStatus.All) && channel.Status.Equals(ChannelStatus.Invited))
            {
                //Logger.Info($"ChatClient: {client}", $"Got invited channel: {channel.Sid}");
            }

        }

        public override void ChannelDeleted(TwilioChatClient client, Channel channel)
        {
            //Logger.Info($"ChatClient: {client}", $"ChannelDeleted: {channel.Sid}");
        }

        public override void MemberJoined(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MemberJoined: {member.Sid}");
            //Logger.Info($"ChatClient: {client}", $"Member attributes: {member.Attributes.ToDebugLog()}");
        }

        public override void MemberUpdated(TwilioChatClient client, Channel channel, Member member, MemberUpdate updated)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MemberUpdated: {member.Sid}, reason: {updated}");
            if (updated == MemberUpdate.Attributes)
            {
                //Logger.Info($"ChatClient: {client}", $"Member attributes: {member.Attributes.ToDebugLog()}");
            }
        }

        public override void MemberLeft(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MemberLeft: {member.Sid}");
        }

        public override void MessageAdded(TwilioChatClient client, Channel channel, Message newmessage)
        {
            try
            {
                ChatMessage newMsg = new ChatMessage();

                string attr = "";
                var keys = new[]
                        { new NSString("SupervisiorId"),new NSString("IconName"),new NSString("calenderevent"),new NSString("ProfileImage")};
                var DicAtt = newmessage.Attributes.Dictionary;

                attr = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("SupervisiorId")) ? DicAtt["SupervisiorId"].ToString() : "";

                if (channel != null)
                {
                    if (channel == _channel)
                    {
                        var i = (int)newmessage.Index;

                        if (newmessage.HasMedia)
                        {
                            var nsURL = NSFileManager.DefaultManager.GetTemporaryDirectory();
                            var temp = (NSString)System.IO.Path.GetTempPath();
                            temp = temp.AppendPathComponent((NSString)newmessage.MediaFilename);

                            NSOutputStream nSOutputStream = NSOutputStream.CreateFile(temp, false);

                            var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                            var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                            if (!System.IO.Directory.Exists(directoryname))
                            {
                                System.IO.Directory.CreateDirectory(directoryname);
                            }
                            string filePath = System.IO.Path.Combine(directoryname, newmessage.MediaFilename);
                            if (File.Exists(filePath))
                            {
                                if (newmessage.MediaType.Equals("image/png"))
                                {
                                    newMsg.MsgIndex = i;
                                    newMsg.IsImage = true;
                                    newMsg.IsMesseges = false;
                                    newMsg.HasMedia = true;
                                    newMsg.Message = newmessage.Body;
                                    newMsg.user = newmessage.Author;
                                    newMsg.ImageMessage = null;
                                    newMsg.Attributes = attr;
                                    newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                    newMsg.ActualImage = ImageSource.FromFile(filePath);
                                    newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                    newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                                }
                                else
                                {
                                    newMsg.MsgIndex = i;
                                    newMsg.IsImage = false;
                                    newMsg.IsotherMedia = true;
                                    newMsg.IsMesseges = false;
                                    newMsg.HasMedia = true;
                                    newMsg.Message = newmessage.MediaFilename;
                                    newMsg.user = newmessage.Author;
                                    newMsg.ImageMessage = null;
                                    newMsg.ImagePath = filePath;
                                    newMsg.Attributes = attr;
                                    newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                    newMsg.ActualImage = ImageSource.FromFile(filePath);
                                    newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                    newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                                }
                                MessagingCenter.Send<ChatMessage>(newMsg, "AddNewMessege");
                            }
                            else
                            {

                                newmessage.GetMediaWithOutputStream(nSOutputStream, () => { }, (bytes) => { }, (mediaSid) => { }, (Result result) =>
                                {
                                    if (result.IsSuccessful)
                                    {

                                        NSUrl nSUrl = new NSUrl(temp);

                                        NSData pngImage = NSData.FromUrl(nSUrl);
                                        byte[] myByteArray = System.IO.File.ReadAllBytes(nSUrl.ToString());

                                        if (newmessage.MediaType.Equals("image/png"))
                                        {
                                            newMsg.MsgIndex = i;
                                            newMsg.IsImage = true;
                                            newMsg.IsMesseges = false;
                                            newMsg.HasMedia = true;
                                            newMsg.Message = newmessage.Body;
                                            newMsg.user = newmessage.Author;
                                            newMsg.Attributes = attr;
                                            newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                            newMsg.ImageMessage = myByteArray;
                                            newMsg.ActualImage = ImageSource.FromStream(() => new MemoryStream(myByteArray));
                                            newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                            newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                                        }
                                        else
                                        {
                                            newMsg.MsgIndex = i;
                                            newMsg.IsImage = false;
                                            newMsg.IsotherMedia = true;
                                            newMsg.IsMesseges = false;
                                            newMsg.HasMedia = true;
                                            newMsg.ImagePath = filePath;
                                            newMsg.Attributes = attr;
                                            newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                            newMsg.Message = newmessage.MediaFilename;
                                            newMsg.user = newmessage.Author;
                                            newMsg.ImageMessage = null;
                                            newMsg.ActualImage = ImageSource.FromFile(filePath);
                                            newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                            newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                                        }
                                        MessagingCenter.Send<ChatMessage>(newMsg, "AddNewMessege");
                                    }

                                });

                            }

                        }
                        else
                        {
                            if (DicAtt != null && DicAtt.Count > 2)
                            {
                                string memberImage = "";
                                var calendereventIcon = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("IconName")) ? DicAtt["IconName"].ToString() : "";
                                if (DicAtt.Count == 5)
                                {
                                    memberImage = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("ProfileImage")) ? DicAtt["ProfileImage"].ToString() : "";

                                }
                                newMsg.MsgIndex = i;
                                newMsg.HasMedia = false;
                                newMsg.Message = newmessage.Body;
                                newMsg.user = newmessage.Author;
                                newMsg.IsImage = false;
                                newMsg.IsMesseges = false;
                                newMsg.IsCalender = true;
                                newMsg.Icon = calendereventIcon;
                                newMsg.Attributes = attr;
                                newMsg.MemberImagePath = memberImage;
                                newMsg.IsMemberVisible = (string.IsNullOrEmpty(memberImage) ? false : true);
                                newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                            }
                            else
                            {
                                newMsg.MsgIndex = i;
                                newMsg.HasMedia = false;
                                newMsg.Message = newmessage.Body;
                                newMsg.user = newmessage.Author;
                                newMsg.IsImage = false;
                                newMsg.IsMesseges = true;
                                newMsg.Attributes = attr;
                                newMsg.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");
                                newMsg.Time = Convert.ToDateTime(newmessage.Timestamp);
                                newMsg.DisTime = Convert.ToDateTime(newmessage.Timestamp).ToString("hh:mm tt");
                            }
                            MessagingCenter.Send<ChatMessage>(newMsg, "AddNewMessege");

                        }

                    }



                    //channel.Messages.GetLastMessagesWithCount(100, (results, messages) =>
                    //{
                    //    getMessegeList.Clear();
                    //    if (results.IsSuccessful)
                    //    {
                    //        //Logger.Info($"Channel: {channel.Sid}", $"Messages: {result}");
                    //        foreach (Message message in messages)
                    //        {
                    //            getMessegeList.Add(new ChatMessage
                    //            {
                    //                Message = message.Body,
                    //                user = message.Author,
                    //                Time = Convert.ToDateTime(message.Timestamp),
                    //                DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                    //            });
                    //        };
                    //        MessagingCenter.Send<List<ChatMessage>>(getMessegeList, "AddNewMessege");
                    //    }
                    //    else
                    //    {

                    //    }

                    //});
                }
            }
            catch (Exception ex)
            {

            }
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MessageAdded: {message.Sid}");
            //Logger.Info($"ChatClient: {client}", $"Message attributes: {message.Attributes.ToDebugLog()}");
        }

        TwilioToken twilioToken = new TwilioToken();

        public override void TokenWillExpire(TwilioChatClient client)
        {
            try
            {

                InvokeOnMainThread(async () =>
                {
                    var token = twilioToken.getTwilioToken(GlobalConstant.Email);
                    //ITwilioChatHelper twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                    //twilioChatHelper.CreateClient(token.Replace("\"", string.Empty));

                    client.UpdateToken(token, async (result) =>
                    {
                        if (result.IsSuccessful)
                        {

                        }
                        else
                        {

                        }
                    });
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async void onFailiure(Twilio.Chat.iOS.Error error)
        {
            try
            {
                Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
            }
            catch (Exception ex)
            {

            }
        }

        public override void TokenExpired(TwilioChatClient client)
        {
            try
            {
                InvokeOnMainThread(async () =>
                {
                    var token = twilioToken.getTwilioToken(GlobalConstant.Email);
                    //ITwilioChatHelper twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                    //twilioChatHelper.CreateClient(token.Replace("\"", string.Empty));

                    client.UpdateToken(token, async (result) =>
                    {
                        if (result.IsSuccessful)
                        {

                        }
                        else
                        {

                        }
                    });
                });
            }
            catch (Exception ex)
            {

            }
        }

        public override void MessageUpdated(TwilioChatClient client, Channel channel, Message message, MessageUpdate updated)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MessageUpdated: {message.Sid}, reason: {updated}");
            if (updated == MessageUpdate.Attributes)
            {
                //Logger.Info($"ChatClient: {client}", $"Message attributes: {message.Attributes.ToDebugLog()}");
            }
        }

        public override void MessageDeleted(TwilioChatClient client, Channel channel, Message message)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} MessageDeleted: {message.Sid}");
        }

        public override void ErrorReceived(TwilioChatClient client, Error error)
        {
            if (error.UserInfo != null && error.UserInfo.ContainsKey(Constants.ErrorMsgKey))
            {
                //Logger.Info($"ChatClient: {client}",
                //$"Error: {error}, " +
                //$"userInfo: {error.UserInfo.ObjectForKey(Constants.ErrorMsgKey)} " +
                //$"code: {error.Code}, " +
                //$"domain: {error.Domain}");
            }
            else
            {
                //Logger.Info($"ChatClient: {client}", $"Error: {error}, code: {error.Code}, domain: {error.Domain}");
            }
        }

        public override void TypingStartedOnChannel(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} TypingStartedOnChannel: {member.Sid}");
        }

        public override void TypingEndedOnChannel(TwilioChatClient client, Channel channel, Member member)
        {
            //Logger.Info($"ChatClient: {client}", $"Channel: {channel.Sid} TypingEndedOnChannel: {member.Sid}");
        }

        public override void NotificationNewMessageReceivedForChannelSid(TwilioChatClient client, string channelSid, nuint messageIndex)
        {
            //Logger.Info($"ChatClient: {client}", $"NotificationNewMessageReceivedForChannelSid: ChannelSid: {channelSid}, MessageIndex: {messageIndex}");
        }

        public override void NotificationAddedToChannelWithSid(TwilioChatClient client, string channelSid)
        {
            //Logger.Info($"ChatClient: {client}", $"NotificationAddedToChannelWithSid: ChannelSid: {channelSid}");
        }

        public override void NotificationInvitedToChannelWithSid(TwilioChatClient client, string channelSid)
        {
            //Logger.Info($"ChatClient: {client}", $"NotificationInvitedToChannelWithSid: ChannelSid: {channelSid}");
        }

        public override void NotificationRemovedFromChannelWithSid(TwilioChatClient client, string channelSid)
        {
            //Logger.Info($"ChatClient: {client}", $"NotificationRemovedFromChannelWithSid: ChannelSid: {channelSid}");
        }

        public override void NotificationUpdatedBadgeCount(TwilioChatClient client, nuint badgeCount)
        {
            //Logger.Info($"ChatClient: {client}", $"NotificationUpdatedBadgeCount:  {badgeCount}");
        }

        public override void UserUpdated(TwilioChatClient client, User user, UserUpdate updated)
        {
            //Logger.Info($"ChatClient: {client}", $"UserUpdated: {user.Identity}, reason: {updated}");
            if (updated == UserUpdate.Attributes)
            {
                //Logger.Info($"ChatClient: {client}", $"User attributes: {user.Attributes.ToDebugLog()}");
            }
        }

        public override void UserSubscribed(TwilioChatClient client, User user)
        {
            //Logger.Info($"ChatClient: {client}", $"UserSubscribed: {user.Identity}");
            //Logger.Info($"ChatClient: {client}", $"User attributes: {user.Attributes.ToDebugLog()}");
        }

        public override void UserUnsubscribed(TwilioChatClient client, User user)
        {
            //Logger.Info($"ChatClient: {client}", $"UserUnsubscribed: {user.Identity}");
        }

    }
}
