using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Dto
{
    public class DrugsDto
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
        public int Days { get; set; }
        public string Time { get; set; }
        public int Dosis { get; set; }
    }

    public class DrugDto
    {
        public string Time { get; set; }
        public List<DrugsDto> Drugs { get; set; }

        public static implicit operator List<object>(DrugDto v)
        {
            throw new NotImplementedException();
        }
    }
}
