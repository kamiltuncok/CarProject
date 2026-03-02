namespace Entities.DTOs
{
    public class LocationManagerUpdateDto
    {
        public int UserId { get; set; }
        public int OldLocationId { get; set; }
        public int NewLocationId { get; set; }
    }
}
