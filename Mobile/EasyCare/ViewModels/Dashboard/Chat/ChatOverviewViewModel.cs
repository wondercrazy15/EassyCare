using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Input;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Chat;
using EasyCare.Views.Dashboard.Chat;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Syncfusion.XForms.Buttons;
using Syncfusion.XForms.Chat;
using Syncfusion.XForms.PopupLayout;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Chat
{
    public class ChatOverviewViewModel : BaseViewModel
    {
        private readonly IMessageClient _client;
        private ObservableCollection<object> _messages;
        private Author _currentUser;
        private SupervisorDto _author;
        private Timer timer;
        SfPopupLayout imagePopup;
        internal SfPopupLayout attachmentPopup;
        public Grid mainstacklayoutOfAttachment;
        SfButton buttonclose;

        public ChatOverviewViewModel()
        {
            
            try
            {
                IsLoad = true;
                IsVisibleMsg = false;
                twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                AttachmentCommand = new Command(AttachmentButtonTapped);
                AttachmentChatCommand = new Command(AttachmentChatTapped);
                CalenderChatCommand = new Command(CalenderChatTapped);
                ActivityCommand = new Command(ActivityTapped);
                LocationCommand = new Command(LocationTapped);
                PulseCommand = new Command(PulseTapped);
                StepsCommand = new Command(StepsTapped);
                BatteryCommand = new Command(BatteryTapped);
                GlobalConstant.Email = Application.Current.Properties["Email"].ToString();
                MessagingCenter.Subscribe<List<ChatMessage>>(this, "MessegeList", (sender) =>
                {
                    var msgList = sender;
                    Messages = new ObservableCollection<ChatMessage>();
                    if (msgList != null)
                    {
                        foreach (var item in msgList)
                        {
                            Messages.Insert(0, new ChatMessage()
                            {
                                HasMedia = item.HasMedia,
                                ContentType = item.ContentType,
                                ImageMessage = item.ImageMessage,
                                ImagePath = item.ImagePath,
                                Message = item.Message,
                                user = item.user,
                                Time = item.Time,
                                ProfileImagePath = item.ProfileImagePath,
                                DisTime = item.DisTime,
                                ActualImage = item.ActualImage,
                                IsMesseges = item.IsMesseges,
                                IsImage = item.IsImage,
                                IsCalender=item.IsCalender,
                                Icon=item.Icon,
                                IsotherMedia = item.IsotherMedia,
                                IsMemberVisible = item.IsMemberVisible,
                                MemberImagePath = item.MemberImagePath
                            });
                        }
                    }
                    IsLoad = false;
                    IsVisibleMsg = true;
                });


                MessagingCenter.Subscribe<ChatMessage>(this, "AddMediaMessege", (sender) =>
                {
                    try
                    {
                        var messageReverseLst = messages.Reverse();
                        var index = messageReverseLst.ToList().IndexOf(messages[sender.MsgIndex]);

                        var Message = new ChatMessage()
                        {
                            HasMedia = sender.HasMedia,
                            ContentType = sender.ContentType,
                            ImageMessage = sender.ImageMessage,
                            ImagePath = sender.ImagePath,
                            Message = sender.Message,
                            user = sender.user,
                            Time = sender.Time,
                            ProfileImagePath = sender.ProfileImagePath,
                            DisTime = sender.DisTime,
                            ActualImage = sender.ActualImage,
                            IsMesseges = sender.IsMesseges,
                            IsImage = sender.IsImage,
                            IsCalender = sender.IsCalender,
                            Icon = sender.Icon,
                            IsotherMedia = sender.IsotherMedia,
                            IsMemberVisible=sender.IsMemberVisible,
                            MemberImagePath=sender.MemberImagePath
                           
                        };

                        Messages[index] = Message;

                    }
                    catch (Exception ex)
                    {

                    }
                });

                MessagingCenter.Subscribe<ChatMessage>(this, "AddNewMessege", (sender) =>
                {
                    Messages.Insert(0, new ChatMessage()
                    {
                        HasMedia = sender.HasMedia,
                        ContentType = sender.ContentType,
                        ImageMessage = sender.ImageMessage,
                        ImagePath = sender.ImagePath,
                        ProfileImagePath=sender.ProfileImagePath,
                        Message = sender.Message,
                        user = sender.user,
                        Time = sender.Time,
                        DisTime = sender.DisTime,
                        ActualImage = sender.ActualImage,
                        IsMesseges = sender.IsMesseges,
                        IsImage = sender.IsImage,
                        IsCalender = sender.IsCalender,
                        Icon = sender.Icon,
                        IsotherMedia = sender.IsotherMedia,
                        IsMemberVisible = sender.IsMemberVisible,
                        MemberImagePath = sender.MemberImagePath
                    });
                });
                
            }
            catch (Exception ex)
            {
                IsLoad = false;
                IsVisibleMsg = true;
            }
        }

        private bool isLoad;
        public bool IsLoad
        {
            get
            {
                return isLoad;
            }

            set
            {
                isLoad = value;
                NotifyPropertyChanged(nameof(IsLoad));
            }
        }

        private bool isVisibleMsg;
        public bool IsVisibleMsg
        {
            get
            {
                return isVisibleMsg;
            }

            set
            {
                isVisibleMsg = value;
                NotifyPropertyChanged(nameof(IsVisibleMsg));
            }
        }


        private string groupKey;
        public string GroupKey
        {
            get
            {
                return groupKey;
            }

            set
            {
                groupKey = value;
                NotifyPropertyChanged(nameof(GroupKey));
            }
        }

        private string textToSend;
        public string TextToSend
        {
            get
            {
                return textToSend;
            }

            set
            {
                textToSend = value;
                NotifyPropertyChanged(nameof(TextToSend));
            }
        }

        public Command SendMessageCommand
        {
            get
            {
                return new Command(async (object obj) =>
                {
                    try
                    {
                        
                        if (!string.IsNullOrEmpty(textToSend))
                        {
                            twilioChatHelper.SendMessage(TextToSend, Application.Current.Properties["supervisor_id"].ToString(),GroupKey);
                            textToSend = string.Empty;
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }
       
        private void BatteryTapped(object obj)
        {
            BatteryChatView popupTemplate = new BatteryChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            SetModel(bodyTemplateView);
        }

        private void SetModel(DataTemplate bodyTemplateView)
        {
            attachmentPopup = new SfPopupLayout();
            attachmentPopup.PopupView.ShowHeader = false;
            attachmentPopup.PopupView.ShowFooter = false;
            attachmentPopup.PopupView.AnimationMode = AnimationMode.Fade;
            attachmentPopup.PopupView.PopupStyle.BorderThickness = 0;
            attachmentPopup.PopupView.PopupStyle.CornerRadius = 15;
            attachmentPopup.PopupView.WidthRequest = ((mainstacklayoutOfAttachment) as View).Width;


            attachmentPopup.PopupView.ContentTemplate = bodyTemplateView;
            attachmentPopup.Closing += AttachmentPopup_Closing;
            attachmentPopup.ShowRelativeToView((mainstacklayoutOfAttachment) as View, RelativePosition.AlignTop, absoluteY: -5);
            buttonclose.RotateTo(45);
        }

        private void StepsTapped(object obj)
        {
            StepsChatView popupTemplate = new StepsChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            SetModel(bodyTemplateView);
        }

        private void PulseTapped(object obj)
        {
            PulseChatView popupTemplate = new PulseChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            SetModel(bodyTemplateView);
        }

        private void LocationTapped(object obj)
        {
            LocationChatView popupTemplate = new LocationChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            SetModel(bodyTemplateView);
        }

        private void ActivityTapped(object obj)
        {
            ActivityChatView popupTemplate = new ActivityChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });

            SetModel(bodyTemplateView);
        }

        private void AttachmentChatTapped(object args)
        {
            AttachmentChatView popupTemplate = new AttachmentChatView();
            popupTemplate.ViewModel = this;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });


            SetModel(bodyTemplateView);
        }
        private void CalenderChatTapped(object args)
        {
            try
            {
                CalenderChatView popupTemplate = new CalenderChatView();
                popupTemplate.ViewModel = this;

                DataTemplate bodyTemplateView = new DataTemplate(() =>
                {
                    return popupTemplate;
                });


                SetModel(bodyTemplateView);
            }
            catch (Exception ex)
            {

            }
        }


        private void AttachmentButtonTapped(object args)
        {
            AttachmentPopupView popupTemplate = new AttachmentPopupView();
            popupTemplate.ViewModel = this;

            attachmentPopup = new SfPopupLayout();
            attachmentPopup.PopupView.ShowHeader = false;
            attachmentPopup.PopupView.ShowFooter = false;
            attachmentPopup.PopupView.AnimationMode = AnimationMode.Fade;
            attachmentPopup.PopupView.PopupStyle.BorderThickness = 0;
            attachmentPopup.PopupView.PopupStyle.CornerRadius = 15;
            attachmentPopup.PopupView.WidthRequest = ((args as Grid) as View).Width;

            DataTemplate bodyTemplateView = new DataTemplate(() =>
            {
                return popupTemplate;
            });



            attachmentPopup.PopupView.ContentTemplate = bodyTemplateView;
            attachmentPopup.Closing += AttachmentPopup_Closing;
            attachmentPopup.ShowRelativeToView((args as Grid) as View, RelativePosition.AlignTop, absoluteY: -5);
            mainstacklayoutOfAttachment = args as Grid;
            buttonclose = (args as Grid).Children[0] as SfButton;
            buttonclose.RotateTo(45);
        }

        private void AttachmentPopup_Closing(object sender, Syncfusion.XForms.Core.CancelEventArgs e)
        {
            buttonclose.RotateTo(0);
        }


        public ObservableCollection<ChatMessage> messages;
        public ObservableCollection<ChatMessage> Messages
        {
            get
            {
                return messages;
            }
            set
            {
                messages = value;
                NotifyPropertyChanged();
            }
        }

        public Author CurrentUser
        {
            get
            {
                return _currentUser;
            }
            set
            {
                _currentUser = value;
                NotifyPropertyChanged();
            }
        }

        public ICommand AttachmentCommand
        {
            get;
            set;
        }

        public ICommand AttachmentChatCommand
        {
            get;
            set;
        }

        public ICommand CalenderChatCommand
        {
            get;
            set;
        }

        public ICommand ActivityCommand
        {
            get;
            set;
        }
        public ICommand StepsCommand
        {
            get;
            set;
        }
        public ICommand BatteryCommand
        {
            get;
            set;
        }

        private ITwilioChatHelper twilioChatHelper;

        public ICommand LocationCommand
        {
            get;
            set;
        }
        public ICommand PulseCommand
        {
            get;
            set;
        }






        //public Command SendMessageCommand
        //{
        //    get
        //    {
        //        return new Command(async (object obj) =>
        //        {
        //            try
        //            {
        //                var args = obj as SendMessageEventArgs;
        //                if (args != null && !String.IsNullOrEmpty(args.Message.Text))
        //                {
        //                    var dto = new MessageDto
        //                    {
        //                        AuthorId = _author.Id,
        //                        SendDate = DateTime.UtcNow,
        //                        ReceiverId = Guid.Empty,
        //                        Content = args.Message.Text
        //                    };
        //                    var json = JsonConvert.SerializeObject(dto);
        //                    await _client.PostItem(dto);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                Debug.WriteLine(ex.Message);
        //            }
        //        });
        //    }
        //}

        public async Task GetMessages()
        {
            try
            {



            }
            catch (Exception ex)
            {

            }
        }
    }
}

