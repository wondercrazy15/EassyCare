using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyCare.Interface;
using EasyCare.Models.Settings;
using EasyCare.ViewModels.Dashboard.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EasyCare.Views.Dashboard.Settings
{
    public partial class InviteNewMemberPage : ContentPage
    {
        InviteNewMemberViewModel vm;
        public InviteNewMemberPage()
        {
            try
            {
                InitializeComponent();
                vm = new InviteNewMemberViewModel(Navigation);

            }
            catch (Exception ex)
            {

            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ListUserView.ItemsSource = vm.Usercollection;
        }

        async void AddNewMember_Clicked(System.Object sender, System.EventArgs e)
        {
            if(Application.Current.Properties.ContainsKey("group_code") && Application.Current.Properties["group_code"] != null)
            {
                await ShareGroupkey(Application.Current.Properties["group_code"].ToString());
            }
            else
            {
                DependencyService.Get<IToast>().Show("Please create a group first");
            }

          
        }

        public async Task ShareGroupkey(string text)
        {
            await Share.RequestAsync(new ShareTextRequest
            {
                Text = text,
                Title = "Gruppenschlüssel teilen"
            });
        }

        async void UserItem_Tapped(System.Object sender, System.EventArgs e)
        {
            Grid stack = sender as Grid;
            MemberModel member= stack.BindingContext as MemberModel;

            string supervisorid = Application.Current.Properties["supervisor_id"].ToString();
            if (!member.Id.Equals(supervisorid))
            {
                int index = vm.Usercollection.ToList().FindIndex(s => s.Id.Equals(supervisorid));
                if (index != -1)
                {
                    bool IsModerator = vm.Usercollection[index].IsModerator;
                    if (IsModerator)
                    {
                        string action = await DisplayActionSheet("Maria Schmidt", "Abbrechen", null, "Senior anlegen", "Zum Administrator machen", "Aus der Gruppe entfernen");

                        var c = action;
                        if(action.Equals("Senior anlegen"))
                        {
                            vm.MakeSenior(member);
                        }
                        else if(action.Equals("Zum Administrator machen"))
                        {
                            vm.MakeModerator(member);
                        }
                        else if (action.Equals("Aus der Gruppe entfernen"))
                        {
                            //remove participant from group
                            vm.RemoveFromGroup(member);
                        }
                        if (action.Equals("Abbrechen"))
                        {

                        }
                    }

                }
            }

           
         
          
           
        }
    }
}
