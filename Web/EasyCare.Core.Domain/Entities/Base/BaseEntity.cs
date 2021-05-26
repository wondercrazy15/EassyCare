using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyCare.Core.Domain.Entities.Base
{
    public class BaseEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
    }
}