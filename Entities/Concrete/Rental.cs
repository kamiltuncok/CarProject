using Core.Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
   public class Rental:IEntity
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public bool isReturned { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
