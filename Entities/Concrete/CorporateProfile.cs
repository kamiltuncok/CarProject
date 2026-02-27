using Core.Entities;

namespace Entities.Concrete
{
    /// <summary>
    /// Extended profile for Corporate customers. 1-to-1 with Customer (CustomerType = Corporate).
    /// </summary>
    public class CorporateProfile : IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }  // FK -> Customer.Id
        public string CompanyName { get; set; }
        public string TaxNumber { get; set; }
    }
}
