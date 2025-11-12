using Core.Entities;
using Entities.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class RentalDetailDto : IDto
    {
        public int RentalId { get; set; }
        public int CarId { get; set; }
        public int UserId { get; set; }
        public int CustomerId { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string SegmentName { get; set; }
        public string FuelName { get; set; }
        public string GearName { get; set; }
        public string LocationName { get; set; }
        public int ModelYear { get; set; }
        public int DailyPrice { get; set; }
        public string Description { get; set; }
        public DateTime RentDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }
        public bool IsReturned { get; set; }
        public int Deposit { get; set; }
        public CustomerType CustomerType { get; set; }
    }
}
