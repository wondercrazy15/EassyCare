using System;
namespace EasyCare.Models.Chat
{
    public class ChatUserList
    {
        /// <summary>
        /// Gets or sets the profile image path.
        /// </summary>

        public string ImagePath { get; set; }
        public string Code { get; set; }
        public string SupervisorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Isactive { get; set; }

        public string GroupId { get; set; }
        /// <summary>
        /// Gets or sets the profile name.
        /// </summary>
        public string GroupName { get; set; }

        public string LastMessage { get; set; }

        /// <summary>
        /// Gets or sets the message sent/received time.
        /// </summary>
        public string Time { get; set; }
    }
}