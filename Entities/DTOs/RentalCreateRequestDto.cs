using Core.Entities;
using System;

namespace Entities.DTOs
{
    public class RentalCreateRequestDto : IDto
    {
        public int CarId { get; set; }
        public int StartLocationId { get; set; }
        public int EndLocationId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
