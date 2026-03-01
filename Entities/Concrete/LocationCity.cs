using Core.Entities;

namespace Entities.Concrete
{
    /// <summary>
    /// Lookup table for city names. Allows multiple Location offices within the same city.
    /// </summary>
    public class LocationCity : IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } // e.g., "İstanbul", "Ankara", "İzmir"
    }
}
