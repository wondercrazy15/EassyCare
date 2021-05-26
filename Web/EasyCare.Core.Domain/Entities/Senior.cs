using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EasyCare.Core.Domain.Entities.Base;

namespace EasyCare.Core.Domain.Entities
{
    [Table("Seniors")] // Specifies table explicitly to be Seniors
    public class Senior : BaseEntity
    {
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Street { get; set; }
        public int    PostCode { get; set; }
        public string City { get; set; }
    }
}
