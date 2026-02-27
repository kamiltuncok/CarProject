using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.Concrete
{
    public class Rental : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }     // FK -> Customer.Id
        public int CarId { get; set; }          // FK -> Car.Id

        // Location snapshot: where car was picked up and where it will be dropped off
        public int StartLocationId { get; set; } // FK -> Location.Id
        public int EndLocationId { get; set; }   // FK -> Location.Id

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Financial snapshot — prices at time of booking (immutable)
        public decimal RentedDailyPrice { get; set; }
        public decimal TotalPrice { get; set; }

        // Deposit workflow
        public decimal DepositAmount { get; set; }
        public DepositStatus DepositStatus { get; set; }
        public decimal DepositDeductedAmount { get; set; }
        public DateTime? DepositRefundedDate { get; set; }

        // Lifecycle
        public RentalStatus Status { get; set; }

        // Concurrency token — EF uses this to detect parallel updates
        [System.ComponentModel.DataAnnotations.Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
