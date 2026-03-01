namespace Entities.DTOs
{
    public class LocationManagerUpdateDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int OldLocationId { get; set; }
        public int NewLocationId { get; set; }
    }
}
