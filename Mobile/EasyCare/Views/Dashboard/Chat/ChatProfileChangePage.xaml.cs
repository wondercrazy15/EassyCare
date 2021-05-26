using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Chat;
using EasyCare.Models.Monitoring;
using FFImageLoading.Cache;
using FFImageLoading.Forms;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Chat
{
    public partial class ChatProfileChangePage : ContentPage
    {
        private ChatUserList chatUserInfo;
        string image;
        string selectedimage;
        string groupId;
        private IGroupClient _clients;
        public ChatProfileChangePage()
        {

        }

        public ChatProfileChangePage(ChatUserList chatUserInfo)
        {

            try
            {
                InitializeComponent();
                this.chatUserInfo = chatUserInfo;
                groupId = chatUserInfo.GroupId;
                _clients = AppContainer.Container.Resolve<IClientFactory>().GroupClient;
                GroupImage.Source = chatUserInfo.ImagePath;
                image = chatUserInfo.ImagePath;
                GroupName.Text = chatUserInfo.GroupName;
                MessagingCenter.Subscribe<byte[]>(this, "ImageSelected", (sender) =>
                {
                    selectedimage = Convert.ToBase64String(sender);
                    GroupImage.Source = ImageSource.FromStream(() => new MemoryStream(sender));
                });
                byte[] imageByte = (new WebClient()).DownloadData(chatUserInfo.ImagePath);
                selectedimage = Convert.ToBase64String(imageByte);
            }
            catch (Exception ex)
            {
                selectedimage = null;
            }
        }

        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            try
            {
                var action = await DisplayActionSheet("Foto hinzufügen", "Abbrechen", null, "Aus der Galerie wählen", "Foto aufnehmen");

                if (action == "Aus der Galerie wählen")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var fileName = SetImageFileName();
                        DependencyService.Get<CameraInterface>().LaunchGallery(FileFormatEnum.JPEG, null);
                    });
                }
                else if (action == "Foto aufnehmen")
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var fileName = SetImageFileName();
                        DependencyService.Get<CameraInterface>().LaunchCamera(FileFormatEnum.JPEG, fileName);
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        private string SetImageFileName(string fileName = null)
        {
            try
            {
                if (Device.RuntimePlatform == Device.Android)
                {
                    if (fileName != null)
                        App.ImageIdToSave = fileName;
                    else
                        App.ImageIdToSave = App.DefaultImageId;

                    return App.ImageIdToSave;
                }
                else
                {
                    if (fileName != null)
                    {
                        App.ImageIdToSave = fileName;
                        return fileName;
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        async void UpdateButton_Clicked(System.Object sender, System.EventArgs e)
        {
            try
            {                                                                                                                                                    UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
              GroupDto group = new GroupDto();
              if(selectedimage != null)
              {
                 group.Image = selectedimage;
              }
              group.Code = chatUserInfo.Code;
              group.Name = GroupName.Text;
              group.Id = new Guid(groupId);
                group.CreatedDate = chatUserInfo.CreatedDate;
                group.ModifiedDate = chatUserInfo.ModifiedDate;
                group.Isactive = chatUserInfo.Isactive;
                group.SupervisorId = chatUserInfo.SupervisorId;
              var updatedGroup = await _clients.PutItem(group.Id,group);
              if (updatedGroup != null)
              {
                  GlobalConstant.groupName = updatedGroup.Name;
                  
                    DependencyService.Get<IToast>().Show("Update succesfully");
                    await CachedImage.InvalidateCache(GlobalConstant.Url+updatedGroup.Image, CacheType.All, true);
                    
              }
                UserDialogs.Instance.HideLoading();
            }
            catch (Exception ex)
            {
                DependencyService.Get<IToast>().Show("Something went wrong!");
                UserDialogs.Instance.HideLoading();
            }
        }
    }
}
