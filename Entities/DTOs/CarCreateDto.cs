using Entities.Enums;

namespace Entities.DTOs
{
    public class CarCreateDto
    {
        public int BrandId { get; set; }
        public int ColorId { get; set; }
        public int CurrentLocationId { get; set; }
        public int ModelYear { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal Deposit { get; set; }
        public string PlateNumber { get; set; }
        public int KM { get; set; }
        public CarStatus Status { get; set; }
        public int FuelId { get; set; }
        public int GearId { get; set; }
        public int SegmentId { get; set; }
        public string Description { get; set; }
    }
}
