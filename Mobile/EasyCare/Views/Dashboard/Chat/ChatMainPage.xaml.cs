using System;
using System.Collections.Generic;
using EasyCare.Interface;
using EasyCare.Models.Chat;
using EasyCare.Services;
using EasyCare.ViewModels.Dashboard.Chat;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class ChatMainPage : ContentPage
    {
        private ITwilioChatHelper twilioChatHelper;

        public ChatMainPage()
        {
            InitializeComponent();
        }


        async void Group_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var args = (TappedEventArgs)e;
                ChatUserList ChatData = (ChatUserList)args.Parameter;
                twilioChatHelper = Xamarin.Forms.DependencyService.Get<ITwilioChatHelper>();
                twilioChatHelper.CreateChannel(ChatData.GroupId);
                await Navigation.PushAsync(new ChatMessagePage(ChatData));

            }
            catch (Exception ex)
            {
                TwilioToken twilioToken = new TwilioToken();
                var token = twilioToken.getTwilioToken(Application.Current.Properties["Email"].ToString());
                twilioChatHelper.CreateClient(token.Replace("\"", string.Empty), Application.Current.Properties["Email"].ToString());
            }
        }
        protected override bool OnBackButtonPressed()
        {
            return base.OnBackButtonPressed();
        }
        protected override void OnAppearing()
        {
            try
            {
                MessagingCenter.Unsubscribe<byte[]>(this, "ImageSelected");
                
                    GroupList.ItemsSource = (BindingContext as ChatMainViewModel).usercollection;
                    (BindingContext as ChatMainViewModel).AddGroupList();
            }
            catch (System.Exception ex)
            {

            }
        }



        async void AddNewMember_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new MemberSelectionPage());
            }
            catch (Exception ex)
            {
                // TODO Add logger

            }
        }
    }
}
