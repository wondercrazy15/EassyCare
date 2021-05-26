using EasyCare.Core.Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EasyCare.Core.Domain.Entities
{
    public class Group : BaseEntity
    {
        [StringLength(250)]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Image { get; set; }
        [StringLength(250)]
        public string Code { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public bool Isactive { get; set; }
    }
}
