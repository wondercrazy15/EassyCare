using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using EasyCare.Core.Constants;
using EasyCare.Interface;
using EasyCare.iOS.Renderers;
using EasyCare.Models.Chat;
using EasyCare.Services;
using EasyCare.ViewModels.Dashboard.Calendar;
using Foundation;
using GlobalToast;
using Twilio;
using Twilio.Chat.iOS;
using Twilio.Rest.Chat.V2.Service.Channel;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(TwillioChatHelper))]
namespace EasyCare.iOS.Renderers
{
    public class TwillioChatHelper : TwilioChatDelegate, ITwilioChatHelper
    {
        public static string authToken;
        public static string identity;
        public static TwilioChatClient chatClient;
        public static Channel _channel;
        TwilioToken twilioToken = new TwilioToken();
        public TwillioChatHelper()
        {

        }

        public void RemoveMember(string friendlyName, string Useridentity)
        {
            chatClient.ChannelsList.ChannelWithSidOrUniqueName(friendlyName, (resu, channels) =>
            {

                if (resu.IsSuccessful)
                {
                    try
                    {
                        _channel = channels;
                        TwilioClient.Init(twilioToken.twilioAccountSid, twilioToken.authToken);
                        var pathChannelSid = _channel.Sid;

                        MemberResource.Delete(
                        pathServiceSid: twilioToken.serviceSid,
                        pathChannelSid: pathChannelSid,
                        pathSid: Useridentity
                        );

                        //var members = MemberResource.Read(
                        // pathServiceSid: "IS9613cfe0039243df84535e7b48cf5344",
                        // pathChannelSid: pathChannelSid,
                        // limit: 20
                        //);
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });


        }

        public void CreateChannel(string friendlyName)
        {
            _channel = null;

            chatClient.ChannelsList.ChannelWithSidOrUniqueName(friendlyName, (resu, channels) =>
            {

                if (resu.IsSuccessful)
                {
                    try
                    {
                        _channel = channels;

                        //TwilioClient.Init("AC1cf545dd9abaa20cec5aefbbc9a092c4", authToken);
                        //var pathChannelSid = _channel.Sid;

                        //MemberResource.Delete(
                        //pathServiceSid: "IS9613cfe0039243df84535e7b48cf5344",
                        //pathChannelSid: pathChannelSid,
                        //pathSid: identity
                        //);
                        //   var members = MemberResource.Read(
                        //    pathServiceSid: "IS9613cfe0039243df84535e7b48cf5344",
                        //    pathChannelSid: pathChannelSid,
                        //    limit: 20
                        //);

                        _channel.JoinWithCompletion((creationResult) =>
                        {
                            if (creationResult.IsSuccessful)
                            {
                                GetMessegeList();
                                TwilioChannelDelegate twilioChannelDelegate = new TwilioChannelDelegate(_channel);
                                TwilioChatDelegate twilioChatDelegate = new TwilioChatDelegate(_channel);
                            }
                            else
                            {
                                GetMessegeList();
                                TwilioChannelDelegate twilioChannelDelegate = new TwilioChannelDelegate(_channel);
                                TwilioChatDelegate twilioChatDelegate = new TwilioChatDelegate(_channel);
                            }

                        });
                    }
                    catch (Exception ex)
                    {

                    }

                }
                else
                {

                    //channel = null;
                    var options = new NSDictionary(
                        Constants.ChannelOptionFriendlyName, NSObject.FromObject(friendlyName));


                    chatClient.ChannelsList.CreateChannelWithOptions(options, (Result result, Channel channel) =>
                    {
                        //Logger.Info($"TwilioChatHelper", $"Created channel with attributes and sid {channel.Sid}");
                        if (result.IsSuccessful)
                        {
                            _channel = channel;
                            _channel.JoinWithCompletion((resultss) =>
                            {
                                _channel.SetUniqueName(friendlyName, (res) => {
                                    TwilioChannelDelegate twilioChannelDelegate = new TwilioChannelDelegate(_channel);
                                    TwilioChatDelegate twilioChatDelegate = new TwilioChatDelegate(_channel);
                                    GetMessegeList();

                                });

                            });

                        }
                        else
                        {
                            var error = result.Error.Description;
                        }
                    });
                }
            });


        }

        public void CreateClient(string chatToken, string email)
        {
            chatClient = null;
            authToken = chatToken;
            identity = email;
            TwilioChatClient.LogLevel = LogLevel.Info;
            var properties = new TwilioChatClientProperties();

            properties.CommandTimeout = (ulong)CommandTimeout.Min;

            TwilioChatClient.ChatClientWithToken(
                chatToken, properties, this, (result, chatClients) =>
                {
                    if (result.IsSuccessful)
                    {

                        chatClient = chatClients;
                        chatClient.ChannelsList
                                            .PublicChannelDescriptorsWithCompletion(HandleChannelDescriptorPaginatorCompletion);

                    }
                    else
                    {
                        //Get Twilio Token

                        //UserSettings.TwilioToken.Replace("\"", string.Empty);
                        var token = twilioToken.getTwilioToken(email);
                        ITwilioChatHelper twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                        twilioChatHelper.CreateClient(token.Replace("\"", string.Empty), email);

                        var error = result.Error.Description;
                    }

                });
        }

        void HandleChannelDescriptorPaginatorCompletion(Result result, ChannelDescriptorPaginator channelDescriptorPaginator)
        {
            if (result.IsSuccessful)
            {

                if (channelDescriptorPaginator.HasNextPage)
                {
                    channelDescriptorPaginator.RequestNextPageWithCompletion(HandleChannelDescriptorPaginatorCompletion);
                }
            }
            else
            {

            }
        }

        public void TypingIndicator()
        {
            if (_channel != null)
            {
                _channel.Typing();
            }
        }
        List<ChatMessage> getMessegeList = new List<ChatMessage>();
        public void GetMessegeList()
        {
            try
            {
                if (_channel != null)
                {
                    _channel.Messages.GetLastMessagesWithCount(100, (results, messages) =>
                    {
                        getMessegeList.Clear();
                        if (results.IsSuccessful)
                        {
  
                            foreach (Message message in messages)
                            {
                                GetMesseges(message);

                            };
                            MessagingCenter.Send<List<ChatMessage>>(getMessegeList, "MessegeList");
                        }
                        else
                        {

                        }

                    });


                }


            }
            catch (Exception ex)
            {
                Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
            }

        }
        ChatMessage MediaList = new ChatMessage();
        public void GetMesseges(Message message)
        {
            try
            {
                var i = (int)message.Index;
                //var m = message.Attributes.GetDictionaryOfValuesFromKeys(keys);

                string attr = "";
                var keys = new[]
                       { new NSString("SupervisiorId"),new NSString("IconName"),new NSString("calenderevent"),new NSString("ProfileImage")};
                var DicAtt = message.Attributes.Dictionary;
                if (DicAtt != null && DicAtt.Count > 0)
                    attr = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("SupervisiorId")) ? DicAtt["SupervisiorId"].ToString() : "";

                //var attr = message.Attributes.GetDictionaryOfValuesFromKeys(keys) != null ? message.Attributes.GetDictionaryOfValuesFromKeys(keys) : new JsonAttributes();
                var documents = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var directoryname = System.IO.Path.Combine(documents, "EasyCare");
                if (!System.IO.Directory.Exists(directoryname))
                {
                    System.IO.Directory.CreateDirectory(directoryname);
                }
                if (message.HasMedia)
                {
                    if (message.MediaType.Equals("image/png"))
                    {
                        string filePath = System.IO.Path.Combine(directoryname, message.MediaFilename);
                        if (File.Exists(filePath))
                        {
                            if (File.Exists(filePath))
                            {
                                getMessegeList.Add(new ChatMessage
                                {
                                    MsgIndex = i,
                                    ImagePath = filePath,
                                    IsMesseges = false,
                                    IsImage = true,
                                    Attributes = attr,
                                    ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),
                                    ContentType = message.MediaType,
                                    ImageMessage = null,
                                    HasMedia = true,
                                    ActualImage = ImageSource.FromFile(filePath),
                                    user = message.Author,
                                    Time = Convert.ToDateTime(message.Timestamp),
                                    DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                                });

                            }
                        }
                        else
                        {

                            //NSUrl nSUrl = new NSUrl(directoryname);

                            //var nsURL = NSFileManager.DefaultManager.GetTemporaryDirectory();
                            var temp = (NSString)directoryname;
                            temp = temp.AppendPathComponent((NSString)message.MediaFilename);
                            NSUrl nSUrl;
                            NSData pngImage;
                            byte[] myByteArray;
                            NSOutputStream nSOutputStream = NSOutputStream.CreateFile(temp, false);

                            message.GetMediaWithOutputStream(nSOutputStream, () => { }, (bytes) => { }, (mediaSid) => { },
                                    (Result result) =>
                                    {
                                        nSUrl = new NSUrl(temp);

                                        pngImage = NSData.FromUrl(nSUrl);
                                        myByteArray = System.IO.File.ReadAllBytes(nSUrl.ToString());

                                        MediaList.MsgIndex = i;
                                        MediaList.IsImage = true;
                                        MediaList.IsMesseges = false;
                                        MediaList.HasMedia = true;
                                        MediaList.Message = message.Body;
                                        MediaList.user = message.Author;
                                        MediaList.Attributes = attr;
                                        MediaList.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");

                                        MediaList.ImageMessage = myByteArray;
                                        MediaList.ActualImage = ImageSource.FromStream(() => new MemoryStream(myByteArray));
                                        MediaList.Time = Convert.ToDateTime(message.Timestamp);
                                        MediaList.DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt");
                                        MessagingCenter.Send<ChatMessage>(MediaList, "AddMediaMessege");

                                    });

                            nSUrl = new NSUrl(temp);

                            pngImage = NSData.FromUrl(nSUrl);
                            myByteArray = System.IO.File.ReadAllBytes(nSUrl.ToString());

                            getMessegeList.Add(new ChatMessage
                            {
                                MsgIndex = i,
                                IsMesseges = false,
                                IsImage = true,
                                ContentType = message.MediaType,
                                ImageMessage = myByteArray,
                                HasMedia = true,
                                Attributes = attr,
                                ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),

                                ActualImage = ImageSource.FromStream(() => new MemoryStream(myByteArray)),
                                user = message.Author,
                                Time = Convert.ToDateTime(message.Timestamp),
                                DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                            });

                        }
                    }
                    else
                    {
                        string filePath = System.IO.Path.Combine(directoryname, message.MediaFilename);
                        if (File.Exists(filePath))
                        {
                            if (File.Exists(filePath))
                            {
                                getMessegeList.Add(new ChatMessage
                                {
                                    MsgIndex = i,
                                    ImagePath = filePath,
                                    IsMesseges = false,
                                    IsImage = false,
                                    Message = message.MediaFilename,
                                    ContentType = message.MediaType,
                                    ImageMessage = null,
                                    HasMedia = true,
                                    IsotherMedia = true,
                                    Attributes = attr,
                                    ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),

                                    ActualImage = ImageSource.FromFile(filePath),
                                    user = message.Author,
                                    Time = Convert.ToDateTime(message.Timestamp),
                                    DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                                });

                            }
                        }
                        else
                        {

                            //NSUrl nSUrl = new NSUrl(directoryname);

                            //var nsURL = NSFileManager.DefaultManager.GetTemporaryDirectory();
                            var temp = (NSString)directoryname;
                            temp = temp.AppendPathComponent((NSString)message.MediaFilename);
                            NSUrl nSUrl;
                            NSData pngImage;
                            byte[] myByteArray;
                            NSOutputStream nSOutputStream = NSOutputStream.CreateFile(temp, false);

                            message.GetMediaWithOutputStream(nSOutputStream, () => { }, (bytes) => { }, (mediaSid) => { },
                                    (Result result) =>
                                    {
                                        nSUrl = new NSUrl(temp);

                                        pngImage = NSData.FromUrl(nSUrl);
                                        myByteArray = System.IO.File.ReadAllBytes(nSUrl.ToString());

                                        MediaList.MsgIndex = i;
                                        MediaList.ImagePath = filePath;
                                        MediaList.IsImage = false;
                                        MediaList.IsMesseges = false;
                                        MediaList.HasMedia = true;
                                        MediaList.IsotherMedia = true;
                                        MediaList.Message = message.MediaFilename;
                                        MediaList.user = message.Author;
                                        MediaList.ImageMessage = myByteArray;
                                        MediaList.Attributes = attr;
                                        MediaList.ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg");

                                        MediaList.ActualImage = ImageSource.FromStream(() => new MemoryStream(myByteArray));
                                        MediaList.Time = Convert.ToDateTime(message.Timestamp);
                                        MediaList.DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt");
                                        MessagingCenter.Send<ChatMessage>(MediaList, "AddMediaMessege");

                                    });

                            nSUrl = new NSUrl(temp);

                            pngImage = NSData.FromUrl(nSUrl);
                            myByteArray = System.IO.File.ReadAllBytes(nSUrl.ToString());

                            getMessegeList.Add(new ChatMessage
                            {
                                MsgIndex = i,
                                ImagePath = filePath,
                                IsMesseges = false,
                                Message = message.MediaFilename,
                                IsImage = false,
                                ContentType = message.MediaType,
                                ImageMessage = myByteArray,
                                HasMedia = true,
                                Attributes = attr,
                                ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),

                                IsotherMedia = true,
                                ActualImage = ImageSource.FromStream(() => new MemoryStream(myByteArray)),
                                user = message.Author,
                                Time = Convert.ToDateTime(message.Timestamp),
                                DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                            });

                        }
                    }

                    // await Task.Delay(1000);
                }
                else
                {
                    string memberImage = "";
                    if (DicAtt != null && DicAtt.Count > 2)
                    {
                        if (DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("calenderevent")))
                        {
                            var calendereventIcon = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("IconName")) ? DicAtt["IconName"].ToString() : "";

                            if (DicAtt.Count == 5)
                            {
                                memberImage = DicAtt.GetDictionaryOfValuesFromKeys(keys).ContainsKey(new NSString("ProfileImage")) ? DicAtt["ProfileImage"].ToString() : "";

                            }
                            getMessegeList.Add(new ChatMessage
                            {
                                MsgIndex = i,
                                HasMedia = false,
                                Message = message.Body,
                                user = message.Author,
                                IsImage = false,
                                IsMesseges = false,
                                IsCalender = true,
                                Icon = calendereventIcon,
                                MemberImagePath = memberImage,
                                IsMemberVisible = (string.IsNullOrEmpty(memberImage) ? false : true),
                                Attributes = attr,
                                ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),
                                Time = Convert.ToDateTime(message.Timestamp),
                                DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                            });
                        }
                    }
                    else
                    {
                        getMessegeList.Add(new ChatMessage
                        {
                            MsgIndex = i,
                            HasMedia = false,
                            IsMesseges = true,
                            IsImage = false,
                            Message = message.Body,
                            user = message.Author,
                            Attributes = attr,
                            ProfileImagePath = (string.IsNullOrEmpty(attr) ? GlobalConstant.Url : GlobalConstant.Url + "/EasyCare/User/" + attr + ".jpg"),

                            Time = Convert.ToDateTime(message.Timestamp),
                            DisTime = Convert.ToDateTime(message.Timestamp).ToString("hh:mm tt")
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
            }
        }

        public void GetToken()
        {

            //Get Token from API code
        }

        public void SendMessage(string Msg, string id, string Groupid)
        {
            if (_channel != null)
            {

                try
                {
                    // var atts = new JsonAttributes();
                    var keys = new[]
                        { new NSString("SupervisiorId"),new NSString("GroupId")};
                    var values = new NSObject[2];
                    values[0] = new NSString(id);
                    values[1] = new NSString(Groupid);
                    //var k = new NSString("SupervisiorId");
                    var dicionary = new NSDictionary<NSString, NSObject>(keys, values);
                    var atts = new JsonAttributes().WithDictionary(dicionary);
                    // atts.SetValuesForKeysWithDictionary(dicionary);
                    MessageOptions messageOptions = new MessageOptions().WithBody(Msg).WithAttributes(atts, null);

                    _channel.Messages.SendMessageWithOptions(messageOptions, (results, message) =>
                    {
                        if (results.IsSuccessful)
                        {
                            //GetMessegeList();
                        }
                        else
                        {

                        }
                    });
                }
                catch (Exception ex)
                {
                    Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
                }
            }
        }

        //Get file extesion
        static private string GetType(string filePath)
        {
            string extinsion = "";
            for (int i = filePath.Length - 1; i >= 0; i--)
            {
                if (filePath[i] != '.')
                    extinsion += filePath[i];
                else
                    break;
            }

            string reversedExtinson = ".";
            for (int i = extinsion.Length - 1; i >= 0; i--)
                reversedExtinson += extinsion[i].ToString().ToLower();

            return filesType[reversedExtinson];
        }
        //Dictionary with extensions
        static private Dictionary<string, string> filesType = new Dictionary<string, string>
        {
            { ".doc", "application/msword" },
            { ".jpg", "image/jpeg" },
            { ".jpeg", "image/jpeg" },
            { ".log", "text/plain" },
            { ".mp3", "audio/mp3" },
            { ".mp4", "video/mp4" },
            { ".pdf", "application/pdf" },
            { ".png", "image/png" },
            { ".rar", "application/x-rar-compressed" },
            { ".rtf", "application/rtf" },
            { ".txt", "text/plain" },
            { ".zip", "application/zip" },
        };
        public void SendMediaMessage(string url, bool ISMedia, string id, string Groupid)
        {
            if (_channel != null)
            {

                try
                {
                    var keys = new[]
                       { new NSString("SupervisiorId"),new NSString("GroupId")};
                    var values = new NSObject[2];
                    values[0] = new NSString(id);
                    values[1] = new NSString(Groupid);
                    var dicionary = new NSDictionary<NSString, NSObject>(keys, values);
                    var atts = new JsonAttributes().WithDictionary(dicionary);

                    var filename = Path.GetFileName(url);
                    var contentType = GetType(url);
                    NSUrl nSUrl = new NSUrl(url);
                   NSInputStream nSInputStream = NSInputStream.FromFile(nSUrl.ToString());
                    MessageOptions messageOptions = new MessageOptions().WithMediaStream(nSInputStream, contentType.Equals("image/jpeg") ? "image/png" : contentType, filename, () => { }, (bytes) => { }, (mediaSid) => { }).WithAttributes(atts, null);

                    _channel.Messages.SendMessageWithOptions(messageOptions, (results, message) =>
                    {
                        if (results.IsSuccessful)
                        {
                            
                        }
                        else
                        {

                        }
                    });
                }
                catch (Exception ex)
                {
                    Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
                }
            }
        }

        public void SendCalenderEvent(CalandarAgendaModel Msg, bool IsMedia, string id, string Groupid)
        {
            if (_channel != null)
            {
                try
                {
                    var keys = new[]
                       { new NSString("SupervisiorId"),new NSString("GroupId"),new NSString("IconName"),new NSString("calenderevent"),new NSString("ProfileImage")};
                    var values = new NSObject[5];
                    values[0] = new NSString(id);
                    values[1] = new NSString(Groupid);
                    values[2] = new NSString(Msg.Icon);
                    values[3] = new NSString("calenderevent");
                    if (Msg.IsProfileAvailable)
                    {
                        values[4] = new NSString(Msg.Imagepath);
                    }
                    else
                    {
                        values[4] = new NSString("");
                    }
                    var dicionary = new NSDictionary<NSString, NSObject>(keys, values);
                    var atts = new JsonAttributes().WithDictionary(dicionary);

                    MessageOptions messageOptions = new MessageOptions().WithBody(Msg.Subject).WithAttributes(atts, null);

                    _channel.Messages.SendMessageWithOptions(messageOptions, (results, message) =>
                    {
                        if (results.IsSuccessful)
                        {
                          
                        }
                        else
                        {

                        }
                    });
                }
                catch (Exception ex)
                {
                    Toast.MakeToast("Something went wrong").SetDuration(0.5).Show();
                }
            }
        }
    }
}
