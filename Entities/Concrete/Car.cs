using Core.Entities;
using Entities.Enums;

namespace Entities.Concrete
{
    public class Car : IEntity
    {
        public int Id { get; set; }
        public int BrandId { get; set; }        // FK -> Brand.Id
        public int ColorId { get; set; }        // FK -> Color.Id
        public int CurrentLocationId { get; set; } // FK -> Location.Id (where car physically is)
        public int ModelYear { get; set; }
        public decimal DailyPrice { get; set; }
        public decimal Deposit { get; set; }
        public string PlateNumber { get; set; } // unique
        public int KM { get; set; }
        public CarStatus Status { get; set; }   // Available, Rented, Maintenance, Reserved
        public int FuelId { get; set; }         // FK -> Fuel.Id
        public int GearId { get; set; }         // FK -> Gear.Id
        public int SegmentId { get; set; }      // FK -> Segment.Id
        public string Description { get; set; }
    }
}
