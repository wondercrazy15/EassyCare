using System;
using System.Collections.Generic;

namespace EasyCare.Models.Monitoring
{
    public class AddDrug
    {
        public int id { get; set; }
        public string Name { get; set; }
        public int DosisDay { get; set; }
        public byte[] image { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan DoseTime { get; set; }
        public int time { get; set; }
        
    }

    public class DisplayDrug
    {
        public int number { get; set; }
        public AddDrug ListofDrug { get; set; }
    }

}
