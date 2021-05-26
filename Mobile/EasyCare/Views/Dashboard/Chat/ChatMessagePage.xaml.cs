using System.Collections.Generic;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.Interface;
using EasyCare.Models.Chat;
using EasyCare.ViewModels.Chat;
using EasyCare.ViewModels.Dashboard.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class ChatMessagePage : ContentPage
    {
        private ChatOverviewViewModel vm;
        private string _channelName;
        private object chatData;

        ChatUserList ChatUserInfo = new ChatUserList();
        private ITwilioChatHelper twilioChatHelper;

        public ChatMessagePage()
        {
            try
            {
                vm = new ChatOverviewViewModel();
                InitializeComponent();
                BindingContext = vm;

                Title.Text = "TestGeneral";


            }
            catch (System.Exception ex)
            {

            }

        }

        public ChatMessagePage(ChatUserList chatData)
        {
            try
            {
                vm = new ChatOverviewViewModel();
                InitializeComponent();
                BindingContext = vm;
                ChatUserInfo = chatData;
                GlobalConstant.groupName = ChatUserInfo.GroupName;
                chatImage.Source = chatData.ImagePath;
                vm.GroupKey=chatData.GroupId;
                Title.Text = chatData.GroupName;
                
            }
            catch (System.Exception ex)
            {

            }
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        protected async override void OnAppearing()
        {
            try
            {
                chatImage.Source = ChatUserInfo.ImagePath;
                Title.Text = GlobalConstant.groupName;
                await (BindingContext as ChatOverviewViewModel).GetMessages();
            }
            catch (System.Exception ex)
            {

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<List<ChatMessage>>(this, "MessegeList");
            MessagingCenter.Unsubscribe<ChatMessage>(this, "AddNewMessege");
            MessagingCenter.Unsubscribe<string>(this, "ImageSelectedPath");
        }
        void ChatList_ItemSelected(System.Object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
        {
            try
            {
                if (e.SelectedItem == null)
                {
                    return;
                }

                ChatList.SelectedItem = null;
            }
            catch (System.Exception ex)
            {

            }
        }

        void Image_Tapped(System.Object sender, System.EventArgs e)
        {
            Navigation.PushAsync(new ChatProfileChangePage(ChatUserInfo));
        }
    }
}