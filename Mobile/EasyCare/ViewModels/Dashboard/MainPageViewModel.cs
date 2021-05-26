using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasyCare.Core.Dto;
using EasyCare.Models;
using EasyCare.Models.Chat;
using EasyCare.ViewModels.Dashboard.Calendar;
using EasyCare.ViewModels.Dashboard.Chat;
using EasyCare.ViewModels.Dashboard.Monitoring;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EasyCare.ViewModels.Dashboard
{
    /// <summary>
    /// 
    /// </summary>
    [Preserve(AllMembers = true)]
    public class MainPageViewModel : BaseViewModel
    {

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public MonitoringOverviewViewModel MonitoringTab { get; set; }
        public ChatMainViewModel ChatTab { get; set; }
        public CalendarOverviewViewModel CalendarTab { get; set; }
        #endregion

        #region Constructor

        public MainPageViewModel(SeniorDto senior, SupervisorDto supervisor, Guid deviceId, INavigation navigation)
        {

            ChatTab = new ChatMainViewModel(supervisor);

            MonitoringTab = new MonitoringOverviewViewModel(navigation,senior, deviceId);
       
            CalendarTab = new CalendarOverviewViewModel(navigation);
        }
        #endregion

    }
}
