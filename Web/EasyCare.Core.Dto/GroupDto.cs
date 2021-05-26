using System;
using System.Collections.Generic;
using System.Text;

namespace EasyCare.Core.Dto
{
    public class GroupDto
    {
        public Guid Id { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string SupervisorId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Isactive { get; set; }
    }

   
}
