using Core.Entities;
using System;

namespace Entities.Concrete
{
    public abstract class Customer : IEntity
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string IdentityNumber { get; set; }

        /// <summary>
        /// Null for guest customers, set for registered (auth) customers.
        /// </summary>
        public int? UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
