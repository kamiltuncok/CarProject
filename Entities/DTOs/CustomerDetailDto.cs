using Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class CustomerDetailDto : IDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdentityNumber { get; set; }
        public int? UserId { get; set; }
        public DateTime CreatedDate { get; set; }

        // Individual Fields
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Corporate Fields
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }

        public bool IsCorporate => !string.IsNullOrEmpty(CompanyName);
    }
}
