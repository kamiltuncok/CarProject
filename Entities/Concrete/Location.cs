using Core.Entities;

namespace Entities.Concrete
{
    public class Location : IEntity
    {
        public int Id { get; set; }
        public string LocationName { get; set; }

        /// <summary>
        /// FK → LocationCity.Id — replaces the old free-text LocationCity string.
        /// </summary>
        public int LocationCityId { get; set; }

        public string Address { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
