using Core.Entities;
using System;

namespace Entities.DTOs
{
    public class CorporateRegisterDto : IDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
        public string PhoneNumber { get; set; }
    }
}
