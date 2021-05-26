using System;
using System.ComponentModel;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace EasyCare.Models.Chat
{
    /// <summary>
    /// Model for chat message 
    /// </summary>
    [Preserve(AllMembers = true)]
    public class ChatMessage : INotifyPropertyChanged
    {
        #region Fields


        private string message;

        private byte[] imageMessage;

        public string user;

        private DateTime time;

        

        private string distime;

        private string contentType;

        private bool hasMedia;

        private ImageSource ImageSource;


        private string attributes;

        public string Attributes
        {
            get
            {
                return this.attributes;
            }

            set
            {
                this.attributes = value;
                this.OnPropertyChanged("Attributes");
            }
        }

        private int msgIndex;

        public int MsgIndex
        {
            get
            {
                return this.msgIndex;
            }

            set
            {
                this.msgIndex = value;
                this.OnPropertyChanged("MsgIndex");
            }
        }

        private bool isMesseges;

        public bool IsMesseges
        {
            get
            {
                return this.isMesseges;
            }

            set
            {
                this.isMesseges = value;
                this.OnPropertyChanged("IsMesseges");
            }
        }

        private bool isImage;

        public bool IsImage
        {
            get
            {
                return this.isImage;
            }

            set
            {
                this.isImage = value;
                this.OnPropertyChanged("IsImage");
            }
        }

        private bool isCalender;
        public bool IsCalender
        {
            get
            {
                return this.isCalender;
            }

            set
            {
                this.isCalender = value;
                this.OnPropertyChanged("IsCalender");
            }
        }

        private string icon;

        public string Icon
        {
            get
            {
                return this.icon;
            }

            set
            {
                this.icon = value;
                this.OnPropertyChanged("Icon");
            }
        }

        private bool isotherMedia;

        public bool IsotherMedia
        {
            get
            {
                return this.isotherMedia;
            }

            set
            {
                this.isotherMedia = value;
                this.OnPropertyChanged("IsotherMedia");
            }
        }

        public ImageSource ActualImage
        {
            get
            {
                return this.ImageSource;
            }

            set
            {
                this.ImageSource = value;
                this.OnPropertyChanged("ActualImage");
            }
        }

        //imageSelect.Source = ImageSource.FromStream(() => new MemoryStream(sender));
        #endregion

        #region Event

        /// <summary>
        /// The declaration of property changed event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get
            {
                return this.message;
            }

            set
            {
                this.message = value;
                this.OnPropertyChanged("Message");
            }
        }



        /// <summary>
        /// Gets or sets the message sent/received time.
        /// </summary>
        public DateTime Time
        {
            get
            {
                return this.time;
            }

            set
            {
                this.time = value;
                this.OnPropertyChanged("Time");
            }
        }

        public bool HasMedia
        {
            get
            {
                return this.hasMedia;
            }

            set
            {
                this.hasMedia = value;
                this.OnPropertyChanged("HasMedia");
            }
        }

        public string DisTime
        {
            get
            {
                return this.distime;
            }

            set
            {
                this.distime = value;
                this.OnPropertyChanged("DisTime");
            }
        }

        /// <summary>
        /// Gets or sets the profile image.
        /// </summary>
        /// 
        private string imagePath;
        public string ImagePath
        {
            get
            {
                return this.imagePath;
            }

            set
            {
                this.imagePath = value;
                this.OnPropertyChanged("ImagePath");
            }
        }

        private bool isMemberVisible;

        public bool IsMemberVisible
        {
            get
            {
                return this.isMemberVisible;
            }

            set
            {
                this.isMemberVisible = value;
                this.OnPropertyChanged("IsMemberVisible");
            }
        }


        private string profileImagePath;
        public string ProfileImagePath
        {
            get
            {
                return this.profileImagePath;
            }

            set
            {
                this.profileImagePath = value;
                this.OnPropertyChanged("ProfileImagePath");
            }
        }

        private string memberImagePath;
        public string MemberImagePath
        {
            get
            {
                return this.memberImagePath;
            }

            set
            {
                this.memberImagePath = value;
                this.OnPropertyChanged("MemberImagePath");
            }
        }

        public string ContentType
        {
            get
            {
                return this.contentType;
            }

            set
            {
                this.contentType = value;
                this.OnPropertyChanged("ContentType");
            }
        }

        public byte[] ImageMessage
        {
            get
            {
                return this.imageMessage;
            }

            set
            {
                this.imageMessage = value;
                this.OnPropertyChanged("Url");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the message is received or sent.
        /// </summary>
        public bool IsReceived { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// The PropertyChanged event occurs when property value is changed.
        /// </summary>
        /// <param name="property">property name</param>
        protected void OnPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        #endregion
    }
}
