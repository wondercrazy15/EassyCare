using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.Models.Settings;
using EasyCare.ViewModels.Dashboard.Calendar;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Calendar
{
    public partial class MemberListPopup : PopupPage
    {
        private ObservableCollection<MemberModel> Membercollection = new ObservableCollection<MemberModel>();
        MemberListPopupViewModel vm;
        private List<SupervisorDto> memberList;

        public MemberListPopup()
        {
            InitializeComponent();
            vm = new MemberListPopupViewModel();

        }

        public MemberListPopup(ObservableCollection<MemberModel> Membercollection, List<SupervisorDto> memberList)
        {
            try
            {
                InitializeComponent();

                vm = new MemberListPopupViewModel();

                foreach (var item in Membercollection)
                {
                    bool isSelected=item.IsSelected;
                    if (memberList.Count > 0) {
                        foreach (var items in memberList)
                        {
                            if (items.Id.ToString() == item.Id)
                                isSelected = true;
                        }
                    }
                    var Imagepath = GlobalConstant.Url + "/EasyCare/User/" + item.Id.ToString() + ".jpg";
                    
                    vm.Usercollection.Add(new MemberModel()
                    {
                        Name = item.Name,
                        Id = item.Id.ToString(),
                        IsModerator = item.IsModerator,
                        IsSenior = item.IsSenior,
                        Email = item.Email,
                        Type = item.Type,
                        ProfilePicPath= Imagepath,
                        BackgroundColor = "#47F6EE",
                        IsSelected = isSelected,
                    });
                   
                }

                ListUserView.HeightRequest = Membercollection.Count * 70;
                ListUserView.ItemsSource = vm.Usercollection;

            }
            catch (Exception ex)
            {

            }
        }

      

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Send<App, ObservableCollection<MemberModel>>((App)Application.Current, "SelectedMemberPopup", vm.Usercollection);
        }
        async void TapGestureRecognizer_Tapped(System.Object sender, System.EventArgs e)
        {
            await PopupNavigation.Instance.PopAllAsync();
          }

        async void SfButton_Clicked(System.Object sender, System.EventArgs e)
        {
           await PopupNavigation.Instance.PopAllAsync();
        }
    }
}
