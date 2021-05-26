using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using EasyCare.Models;
using EasyCare.Models.Chat;
using EasyCare.ViewModels.Dashboard.Calendar;
using EasyCare.ViewModels.Dashboard.Chat;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EasyCare.ViewModels.Dashboard.Monitoring
{
    /// <summary>
    /// 
    /// </summary>
    [Preserve(AllMembers = true)]
    public class HeartRateViewModel : BaseViewModel
    {
        #region Private Fields
        /// <summary>
        /// 
        /// </summary>
        private int heartrate = new int();
        #endregion

        #region Public Properties
        /// <summary>
        /// 
        /// </summary>
        public int HeartRate { get; set; }
        #endregion

        #region Constructor
        /// <summary>
        /// 
        /// </summary>
        public HeartRateViewModel()
        {

        }
        #endregion

    }
}
