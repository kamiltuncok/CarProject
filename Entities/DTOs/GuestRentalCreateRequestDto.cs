using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.DTOs
{
    public class GuestRentalCreateRequestDto : RentalCreateRequestDto
    {
        public CustomerType CustomerType { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IdentityNumber { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
    }
}
