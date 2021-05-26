using System;
using System.Text.Json.Serialization;
using SQLite;

namespace EasyCare.Models
{
    public class PushMessage
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DeviceId { get; set; }
        public string SeniorName { get; set; }
        public string MessageType { get; set; }
        public string  BPM { get; set; }
        [JsonPropertyName("iothub - enqueuedtime")]
        public DateTime Date { get; set; }
        public string Description { get=> string.Format("{0} von {1} hat eine {2}-Nachricht gesendet. Aktueller Puls: {3}", DeviceId, SeniorName, MessageType, BPM); }
    }
}
