using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Acr.UserDialogs;
using Autofac;
using EasyCare.Client;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Interface;
using EasyCare.Models.Monitoring;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    public class AddNewMedikamenteViewModel : BaseViewModel
    {

        private INavigation _navigation;
        public AddNewMedikamenteViewModel(INavigation navigation)
        {
            _navigation = navigation;
            MinDate = DateTime.Today;
            SelectedDate = DateTime.Today;
            SelectedEndDate = DateTime.Today;
            DoseTime = TimeSpan.Parse("12:00:00");
            DosisDay = 1;
            isImage = false;
            isImageLable = true;
            DrugList = new List<AddDrug>();
        }
        public AddNewMedikamenteViewModel()
        {

        }
        byte[] _image;
        public byte[] Image
        {
            get
            {
                return _image;
            }

            set
            {
                _image = value;
                NotifyPropertyChanged();
            }
        }

        public string Id { get; set; }
        public Command SaveCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        UserDialogs.Instance.ShowLoading("Please wait...", MaskType.Gradient);
                        var client = AppContainer.Container.Resolve<IClientFactory>().DrugsClient;
                        string imagestring = "";
                        if (Image!=null)
                        {
                            imagestring = Convert.ToBase64String(Image);
                           
                        }
                        //(SelectedDate.Date + DoseTime).ToString("yyyy-MM-dd HH:mm:ss"),
                        //(SelectedEndDate.Date + DoseTime).ToString("yyyy-MM-dd HH:mm:ss"),

                        var dto = new DrugsDto
                        {
                            StartDate = SelectedDate.ToString("yyyy-MM-dd"),
                            EndDate = SelectedEndDate.ToString("yyyy-MM-dd"),
                            Image = imagestring,
                            Name = Name,
                            IsActive = false,
                            Dosis = DosisDay,
                            Timing = DoseTime,
                            SeniorId = (Guid)Application.Current.Properties["senior_id"],
                            SupervisorId = (Guid)Application.Current.Properties["supervisor_id"]
                        };


                        if (Id != null && Id != "")
                        {
                            dto.Id = new Guid(Id);
                            var result = await client.PutItem(dto.Id, dto);

                            if (result.Id != Guid.Empty)
                            {
                                DependencyService.Get<IToast>().Show("Edit Drug successfully");
                                await _navigation.PopAsync();
                            }
                        }
                        else
                        {
                            var result = await client.PostItem(dto);

                            if (result.Id != Guid.Empty)
                            {
                                UserDialogs.Instance.HideLoading();
                                DependencyService.Get<IToast>().Show("Added Drug successfully");
                                await _navigation.PopAsync();
                            }
                            else
                            {
                                DependencyService.Get<IToast>().Show("Drug not added");
                            }
                        }
                        UserDialogs.Instance.HideLoading();
                    }
                    catch (Exception ex)
                    {
                        UserDialogs.Instance.HideLoading();
                        // TODO Add logger
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public void EditDataInfo(DrugsModel drug,INavigation navigation)
        {
            
            try
            {
                _navigation = navigation;
                SelectedDate = drug.StartDate.ParseMyFormatDateTime();
                SelectedEndDate =drug.EndDate.ParseMyFormatDateTime(); ;
                // imagestring,
                Id = drug.Id.ToString();
                Name = drug.Name;
                DosisDay = drug.Dosis;
                DoseTime = drug.Timing;
                if (Image != null && Image.Length>0)
                {
                    isImage = true;
                    isImageLable = false;
                }
                else
                {
                    isImage = false;
                    isImageLable = true;
                }
            }
            catch (Exception ex)
            {

            }

        }

        public string _name;
        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                NotifyPropertyChanged();
            }
        }
        public int _dosisDay;
        public int DosisDay
        {
            get
            {
                return _dosisDay;
            }

            set
            {
                _dosisDay = value;
                NotifyPropertyChanged();
            }
        }
        public TimeSpan _doseTime;
        public TimeSpan DoseTime
        {
            get
            {
                return _doseTime;
            }

            set
            {
                _doseTime = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime _MinDate;
        public DateTime MinDate
        {
            get
            {
                return _MinDate;
            }

            set
            {
                _MinDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime _maxDate;
        public DateTime MaxDate
        {
            get
            {
                return _maxDate;
            }

            set
            {
                _maxDate = value;
                NotifyPropertyChanged();
            }
        }

        public DateTime _selectedDate;
        public DateTime SelectedDate
        {
            get
            {
                return _selectedDate;
            }

            set
            {
                _selectedDate = value;
                NotifyPropertyChanged();
            }
        }
        public DateTime _selectedEndDate;
        public DateTime SelectedEndDate
        {
            get
            {
                return _selectedEndDate;
            }

            set
            {
                _selectedEndDate = value;
                NotifyPropertyChanged("SelectedEndDate");
            }
        }
        

        private bool _erinnerungSwicherIsToggled;
        public bool ErinnerungSwicherIsToggled
        {
            get
            {
                return _erinnerungSwicherIsToggled;
            }
            set
            {
                _erinnerungSwicherIsToggled = value;
                NotifyPropertyChanged();
            }
        }
        

        private bool _isImageLable;
        public bool isImageLable
        {
            get
            {
                return _isImageLable;
            }
            set
            {
                _isImageLable = value;
                NotifyPropertyChanged();
            }
        }

        private bool _isImage;
        public bool isImage
        {
            get
            {
                return _isImage;
            }
            set
            {
                _isImage = value;
                NotifyPropertyChanged();
            }
        }

        private bool _einnahmeSwicherIsToggled;
        public bool EinnahmeSwicherIsToggled
        {
            get
            {
                return _einnahmeSwicherIsToggled;
            }
            set
            {
                _einnahmeSwicherIsToggled = value;
                NotifyPropertyChanged();
            }
        }

    }
}
