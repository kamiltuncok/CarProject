using Core.Entities;
using Entities.Enums;
using System;

namespace Entities.DTOs
{
    public class CarDetailDto : IDto
    {
        public int Id { get; set; }
        public int ModelYear { get; set; }
        public string Description { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public string LocationName { get; set; }
        public string LocationCity { get; set; }
        public string SegmentName { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal Deposit { get; set; }
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int GearId { get; set; }
        public int FuelId { get; set; }
        public int SegmentId { get; set; }
        public CarStatus Status { get; set; }
        public string PlateNumber { get; set; }
        public int KM { get; set; }
        public string FuelName { get; set; }
        public string GearName { get; set; }
    }
}
