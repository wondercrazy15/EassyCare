using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    //[Table("SensorMessage")] // Specifies table explicitly to be Seniors
    public class SensorMessage : BaseEntity
    {
        public Guid DeviceId { get; set; }
        public int HeartRate { get; set; }
        public string Location { get; set; }
        public int BatterySoC { get; set; }
        public int StepCount { get; set; }
        public DateTime Date { get; set; }
    }
}
