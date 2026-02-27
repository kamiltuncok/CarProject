namespace Entities.Enums
{
    /// <summary>
    /// Represents the type of customer relationship.
    /// Role management (Admin, LocationManager) is handled via OperationClaim, not CustomerType.
    /// </summary>
    public enum CustomerType
    {
        Individual = 1,
        Corporate = 2
    }
}
