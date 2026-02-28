using System;

namespace Entities.DTOs
{
    public class CarAvailabilityFilterDto
    {
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int[] FuelIds { get; set; }
        public int[] GearIds { get; set; }
        public int[] SegmentIds { get; set; }
    }
}
