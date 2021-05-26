using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using EasyCare.Client;
using EasyCare.Client.Contact;
using EasyCare.Core.Constants;
using EasyCare.Core.Dto;
using EasyCare.DI;
using EasyCare.Models.Monitoring;
using EasyCare.Views.Dashboard.Monitoring;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    public class MedikamenteViewModel: BaseViewModel
    {
        INavigation _navigation;
        private ObservableCollection<DisplayDrug> DisplayDrug { get; set; } = new ObservableCollection<DisplayDrug>();
        private readonly IDrugsClient _client;
        public Command<object> DrugTap { get; set; }
        
        public MedikamenteViewModel(INavigation navigation)
        {
            _navigation = navigation;
            _client = AppContainer.Container.Resolve<IClientFactory>().DrugsClient;
            SelectedDate = DateTime.Today.ToString("yyyy-MM-dd");
            var logininfo = JsonConvert.DeserializeObject<UsersDto>((string)Application.Current.Properties["Login_info"]);
            DrugTap = new Command<object>(DrugTapClick);
            if (logininfo.senior != null)
            {
                Imagepath = GlobalConstant.Url + "/EasyCare/User/" + logininfo.senior.Id + ".jpg";
            }
            else
            {
                Imagepath = GlobalConstant.Url;
            }

        }

        async void DrugTapClick(object obj)
        {
            try
            {
                var DrugInfo = obj as DrugsModel;
                await _navigation.PushAsync(new AddNewMedikamentePage(DrugInfo));
            }
            catch (Exception ex)
            {

            }
            
        }

        public ObservableCollection<DrugModel> LstDrugs { get; set; } = new ObservableCollection<DrugModel>();

        private string _Imagepath;
        public string Imagepath
        {
            get
            {
                return _Imagepath;
            }
            set
            {
                _Imagepath = value;
                NotifyPropertyChanged();
            }
        }
        public async void GetDrugs()
        {
            try
            {
                string seniorId = ((Guid)Application.Current.Properties["senior_id"]).ToString();
                var drugs = await _client.GetDrugsByDate(SelectedDate, seniorId);
                if (drugs != null && drugs.Count()>0)
                {
                    ISNoDataFound = false;
                    LstDrugs.Clear();
                    drugs = drugs.OrderBy(s => s.Time.Equals("Night")).ThenBy(s => s.Time.Equals("Evening")).ThenBy(s => s.Time.Equals("Afternoon")).ToList();
                    foreach (var item in drugs)
                    {

                        List<DrugsModel> drugsLstwisetime = new List<DrugsModel>();
                        foreach (var obj in item.Drugs)
                        {
                            string stars = "";
                            for (int i = 0; i < obj.Dosis; i++)
                            {
                                stars += "*";
                            }
                           
                            drugsLstwisetime.Add(new DrugsModel()
                            {
                                Dosis = obj.Dosis,
                                Time = obj.Time,
                                Star=stars,
                                SeniorId = obj.SeniorId,
                                StartDate = obj.StartDate,
                                EndDate = obj.EndDate,
                                Description = obj.Description,
                                Id = obj.Id,
                                SupervisorId = obj.SupervisorId,
                                Image = obj.Image,
                                Name = obj.Name,
                                Timing = obj.Timing
                            });
                        }

                        LstDrugs.Add(new DrugModel()
                        {
                            Drugs = drugsLstwisetime,
                            Time = item.Time
                        });
                    }
                }
                else
                {
                    LstDrugs.Clear();
                    ISNoDataFound = true;
                }
            }
            catch (Exception ex)
            {

            }
          
        }

        private void LoadData()
        {
            try
            {
                if (DrugList != null && DrugList.Count > 0)
                {
                    List<AddDrug> list = DrugList.Where(x => x.StartDate <= DateTime.Today && x.EndDate >= DateTime.Today).ToList();

                       if (list != null && list.Count > 0)
                        {
                            foreach (var item in list)
                            {
                                if (item.time == 1)
                                {
                                    DisplayDrug.Add(new Models.Monitoring.DisplayDrug
                                    {
                                        number = 1,
                                        ListofDrug = item
                                    });
                                }
                                if (item.time == 2)
                                {
                                    DisplayDrug.Add(new Models.Monitoring.DisplayDrug
                                    {
                                        number = 2,
                                        ListofDrug = item
                                    });
                                }
                                if (item.time == 3)
                                {
                                    DisplayDrug.Add(new Models.Monitoring.DisplayDrug
                                    {
                                        number = 3,
                                        ListofDrug = item
                                    });
                                }
                                if (item.time == 4)
                                {
                                    DisplayDrug.Add(new Models.Monitoring.DisplayDrug
                                    {
                                        number = 4,
                                        ListofDrug = item
                                    });
                                }
                            }
                        }
                }
            }
            catch (Exception ex)
            {

            }
        }



        private string _SelectedDate;
        public string SelectedDate
        {
            get
            {
                return _SelectedDate;
            }
            set
            {
                _SelectedDate = value;
                NotifyPropertyChanged();
            }
        }

        

        public Command EditDrug
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                       
                    }
                    catch (Exception ex)
                    {
                        // TODO Add logger
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public Command PreviousCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                       var Date= Convert.ToDateTime(SelectedDate).AddDays(-1);
                       SelectedDate = Date.ToString("yyyy-MM-dd");
                       GetDrugs();
                    }
                    catch (Exception ex)
                    {
                        // TODO Add logger
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        public Command NextCommand
        {
            get
            {
                return new Command(async () =>
                {
                    try
                    {
                        var Date = Convert.ToDateTime(SelectedDate).AddDays(1);
                        SelectedDate = Date.ToString("yyyy-MM-dd");
                        GetDrugs();
                    }
                    catch (Exception ex)
                    {
                        // TODO Add logger
                        Debug.WriteLine(ex.Message);
                    }
                });
            }
        }

        private Double _widthStack;
        public Double WidthStack
        {
            get
            {
                return _widthStack;
            }
            set
            {
                _widthStack = value;
                NotifyPropertyChanged();
            }
        }

        private bool _ISNoDataFound;
        public bool ISNoDataFound
        {
            get
            {
                return _ISNoDataFound;
            }
            set
            {
                _ISNoDataFound = value;
                NotifyPropertyChanged();
            }
        }
        
    }
}
