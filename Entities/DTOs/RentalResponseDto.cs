using Core.Entities;

namespace Entities.DTOs
{
    public class RentalResponseDto : IDto
    {
        public int RentalId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
