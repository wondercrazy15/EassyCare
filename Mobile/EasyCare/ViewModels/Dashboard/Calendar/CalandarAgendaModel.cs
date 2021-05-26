using System;
using EasyCare.Core.Constants;
using Xamarin.Forms;

namespace EasyCare.ViewModels.Dashboard.Calendar
{
    public class CalandarAgendaModel
    {
        private int no;
        private string subject;
        private string starttime;
        private string endtime;
        public string icon;
        public string id;
        public string imagepath;
        public bool isProfileAvailable;
        public string iconName;


        public int No
        {
            get { return no; }
            set { this.no = value; }
        }

        public string Imagepath
        {
            get { return imagepath; }
            set { this.imagepath = value; }
        }

        public string Subject
        {
            get { return subject; }
            set { this.subject = value; }
        }

        public string Icon
        {
            get { return icon; }
            set { this.icon = value; }
        }
        
        public string IconName
        {
            get { return iconName; }
            set { this.iconName = value; }
        }

        public string Id
        {
            get { return id; }
            set { this.id = value; }
        }

        public bool IsProfileAvailable
        {
            get { return isProfileAvailable; }
            set { this.isProfileAvailable = value; }
        }


        public string StartTime
        {
            get { return starttime; }
            set { this.starttime = value; }
        }

        public string EndTime
        {
            get { return this.endtime; }
            set { this.endtime = value; }
        }
        public bool isCalenderEvent;
        public bool IsCalenderEvent
        {
            get { return isCalenderEvent; }
            set { this.isCalenderEvent = value; }
        }

        public CalandarAgendaModel(bool isprofAvialble,string imagepath, string id, int no, string subject, string starttime, string endtime,string icon,string iconName="")
        {
            this.IsProfileAvailable = isprofAvialble;
            this.Imagepath = imagepath;
            this.Id = id;
            this.No = no;
            this.Icon = icon;
            this.Subject = subject;
            //this.StartTime = DateTime.Parse(starttime).ToString("HH:mm");
            //this.EndTime = DateTime.Parse(endtime).ToString("HH:mm");
            this.StartTime=starttime.ParseMyFormatDateTime().ToString("HH:mm");
            this.EndTime = endtime.ParseMyFormatDateTime().ToString("HH:mm");
            this.IconName = iconName;
            if ((string.IsNullOrEmpty(iconName) ? "" : iconName).Equals(IconsConstant.IconImportantEvent))
                IsCalenderEvent = true;
            else
                IsCalenderEvent = false;
        }
    }
}
