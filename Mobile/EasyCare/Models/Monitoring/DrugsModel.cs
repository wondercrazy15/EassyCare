using System;
using System.Collections.Generic;
using EasyCare.Core.Constants;

namespace EasyCare.Models.Monitoring
{
    public class DrugsModel
    {
        public Guid Id { get; set; }
        public Guid? SeniorId { get; set; }
        public Guid? SupervisorId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        public TimeSpan Timing { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int Dosis { get; set; }
        public string Time { get; set; }
        public string Star { get; set; }
        
        public string ActualImage
        {
            get
            {
                return $"{GlobalConstant.Url}{Image}";
            }
        }

    }

    public class DrugModel
    {
        public string Time { get; set; }
        public string icon
        {
            get
            {
               return Time.Equals("Morning") ? "\uE91E" : Time.Equals("Afternoon") ? "\uE91f" : Time.Equals("Evening") ? "\uE920" : "\uE924";
            }
        }
        public List<DrugsModel> Drugs { get; set; }
    }

}
