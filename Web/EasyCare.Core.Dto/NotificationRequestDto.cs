using System;

namespace EasyCare.Core.Dto
{
    public class NotificationRequestDto
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
    }

    public class NotificationScheduleRequestDto
    {
        public string Text { get; set; }
        public string Action { get; set; }
        public string Title { get; set; }
        public string Type { get; set; }
        public string[] Tags { get; set; } = Array.Empty<string>();
        public bool Silent { get; set; }
        public DateTime ScheduleDate { get; set; }
    }
}