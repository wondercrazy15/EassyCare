using EasyCare.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EasyCare.Core.Domain.Entities
{
    public class Drugs : BaseEntity
    {
        public Guid? SeniorId { get; set; }
        public Guid? SupervisorId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [StringLength(250)]
        public TimeSpan Timing { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int Days { get; set; }
        public int Dosis { get; set; }
    }

    public class CustomDrugs 
    {
        public Guid Id { get; set; }
        public Guid? SeniorId { get; set; }
        public Guid? SupervisorId { get; set; }
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [StringLength(250)]
        public TimeSpan Timing { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public int Days { get; set; }
        // [NotMapped]
        public string Time { get; set; }
        public int Dosis { get; set; }
    }


}
