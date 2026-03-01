namespace Entities.DTOs
{
    public class LocationManagerDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int LocationId { get; set; }
        public string LocationName { get; set; }
        public int LocationUserRoleId { get; set; } // Can be used when revoking
    }
}
