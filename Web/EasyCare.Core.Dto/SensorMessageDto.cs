using System;

namespace EasyCare.Core.Dto
{
    public class SensorMessageDto
    {
        public Guid Id { get; set; }
        public Guid DeviceId { get; set; }
        public int HeartRate { get; set; }
        public string Location { get; set; }
        public int BatterySoC { get; set; }
        public bool BatteryIsCharge { get; set; }
        public int StepCount { get; set; }
        public DateTime Date { get; set; }
    }
}
