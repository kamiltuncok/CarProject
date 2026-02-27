using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.DTOs
{
    public class RentalDetailDto : IDto
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public int CustomerId { get; set; }

        // Car info
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SegmentName { get; set; }
        public string FuelName { get; set; }
        public string GearName { get; set; }
        public int ModelYear { get; set; }
        public decimal DailyPrice { get; set; }
        public string Description { get; set; }
        public string PlateNumber { get; set; }

        // Location info
        public string StartLocationName { get; set; }
        public string StartLocationCity { get; set; }
        public string EndLocationName { get; set; }
        public string EndLocationCity { get; set; }

        // Dates
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Financial snapshot
        public decimal RentedDailyPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal DepositAmount { get; set; }
        public decimal DepositDeductedAmount { get; set; }
        public DateTime? DepositRefundedDate { get; set; }
        public DepositStatus DepositStatus { get; set; }

        // Status
        public RentalStatus RentalStatus { get; set; }
        public CustomerType CustomerType { get; set; }

        // Customer info
        public string CustomerEmail { get; set; }
        public string CustomerPhone { get; set; }
    }
}
