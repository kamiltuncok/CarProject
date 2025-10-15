namespace Core.Entities.Requests
{
    public class PricingRequest
    {
        public int CarId { get; set; }
        public int CurrentPrice { get; set; }
        public int RentalCount { get; set; }
    }
}