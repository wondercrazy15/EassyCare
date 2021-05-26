using System;
namespace EasyCare.Models.Settings
{
    public class MemberModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Email { get; set; }
        public string ProfilePicPath { get; set; }
        public bool IsModerator { get; set; }
        public bool IsSenior { get; set; }
        public bool IsSelected { get; set; }
        public string BackgroundColor { get; set; }
    }
}
