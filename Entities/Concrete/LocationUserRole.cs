using Core.Entities;

namespace Entities.Concrete
{
    /// <summary>
    /// Location-scoped RBAC — ties a User to a specific Location with a specific role (e.g. LocationManager).
    /// </summary>
    public class LocationUserRole : IEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; }            // FK -> Users.Id
        public int LocationId { get; set; }        // FK -> Location.Id
        public int OperationClaimId { get; set; }  // FK -> OperationClaims.Id
    }
}
