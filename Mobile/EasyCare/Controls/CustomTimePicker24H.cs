using System;
using Xamarin.Forms;

namespace EasyCare.Controls
{
    public class CustomTimePicker24H : TimePicker
    {
        public static readonly BindableProperty MinimumTimeProperty =
      BindableProperty.Create("MinimumTime", typeof(TimeSpan), typeof(CustomTimePicker24H), new TimeSpan(0, 0, 0));

        /// <summary>
        /// The MaximumTime property
        /// </summary>
        public static readonly BindableProperty MaximumTimeProperty =
            BindableProperty.Create("MaximumTime", typeof(TimeSpan), typeof(CustomTimePicker24H), new TimeSpan(24, 0, 0));

        /// <summary>
        /// Gets or sets the minimum time
        /// </summary>
        /// <value>The minimum time.</value>
        public TimeSpan MinimumTime
        {
            get { return (TimeSpan)GetValue(MinimumTimeProperty); }
            set { SetValue(MinimumTimeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the maximum time
        /// </summary>
        /// <value>The maximum time.</value>
        public TimeSpan MaximumTime
        {
            get { return (TimeSpan)GetValue(MaximumTimeProperty); }
            set { SetValue(MaximumTimeProperty, value); }
        }

    }
}
