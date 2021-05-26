using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using EasyCare.Models.Settings;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Calendar
{
    public class MemberListPopupViewModel : BaseViewModel
    {


        private ObservableCollection<MemberModel> usercollection = new ObservableCollection<MemberModel>();
        private List<MemberModel> userSelectedCollection = new List<MemberModel>();

        public Command<object> SelectionTapClickCommand { get; set; }
       
        public MemberListPopupViewModel()
        {
            SelectionTapClickCommand = new Command<object>(SelectedItemTap);
          
        }

        public async void CreateNewGroup()
        {
            string result = await App.Current.MainPage.DisplayPromptAsync(null, "What's your group name?");
            
        }



        public Command addClickCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        foreach (var item in Usercollection)
                        {
                            if (item.IsSelected)
                            {
                                userSelectedCollection.Add(new MemberModel
                                {
                                    Name = item.Name,
                                    Type = item.Type,
                                    IsSelected = item.IsSelected,
                                    BackgroundColor = item.BackgroundColor,

                                });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        
                    }
                });
            }
        }

        private void SelectedItemTap(object obj)
        {
            var data = obj as MemberModel;
            if (data != null)
            {
               MemberModel Member = data;
               Member.BackgroundColor = data.IsSelected ? "#BDBDBD": "#47F6EE";
               Member.IsSelected = data.IsSelected? false : true;

               var index = Usercollection.IndexOf(data);
               Usercollection.RemoveAt(index);
               Usercollection.Insert(index, Member);
            }
        }


        private double _heightList;
        public double HeightList
        {
            get
            {
                return _heightList;
            }
            set
            {
                _heightList = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<MemberModel> Usercollection
        {
            get
            {
                return usercollection;
            }
            set
            {
                usercollection = value;
                NotifyPropertyChanged();
            }
        }

    }

}
