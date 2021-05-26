using System;

namespace EasyCare.Core.Dto
{
    public class SeniorDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Street { get; set; }
        public int    PostCode { get; set; }
        public string City { get; set; }
    }
}
