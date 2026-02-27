namespace Core.Entities.Requests
{
    public class PricingRequest
    {
        public int CarId { get; set; }
        public decimal CurrentPrice { get; set; }
        public int RentalCount { get; set; }
    }
}