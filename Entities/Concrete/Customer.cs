using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class Customer : IEntity
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public CustomerType CustomerType { get; set; }

        /// <summary>
        /// Null for guest customers, set for registered (auth) customers.
        /// </summary>
        public int? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
