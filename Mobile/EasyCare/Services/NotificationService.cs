using System;
using System.Collections.Generic;

namespace EasyCare.Services
{
    public interface INotificationService
    {
        void Clear();

        void Add(string message);

        Dictionary<DateTime, string> Get();
    }
    
    public class NotificationService : INotificationService
    {
        private Dictionary<DateTime, string> _data;

        public NotificationService()
        {
            _data = new Dictionary<DateTime, string>();
        }
        
        public void Clear()
        {
            _data.Clear();;
        }

        public void Add(string message)
        {
            _data.Add(DateTime.UtcNow, message);
        }

        public Dictionary<DateTime, string> Get()
        {
            return _data;
        }
    }
}