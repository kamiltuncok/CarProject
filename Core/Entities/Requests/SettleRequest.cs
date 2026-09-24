using System.Collections.Generic;

namespace Core.Entities.Requests
{
    /// <summary>
    /// Gecikmeli ödül kapatma isteği — karar penceresinde GERÇEKLEŞEN talep,
    /// RL motoruna Q güncellemesi için geri gönderilir.
    /// </summary>
    public class SettleRequest
    {
        public int CarId { get; set; }
        public int SegmentId { get; set; }
        public string StateKey { get; set; }
        public double Action { get; set; }
        public decimal NewPrice { get; set; }
        /// <summary>İptal edilenler dahil, pencerede yapılan tüm rezervasyonlar.</summary>
        public int RealizedRentals { get; set; }

        /// <summary>Brüt ciro eksi iadeler. Ödül BUNU kullanır.</summary>
        public decimal NetRevenue { get; set; }

        public double WindowDays { get; set; }
    }

    public class SettleResponse
    {
        public int Settled { get; set; }
        public List<SettleDetail> Details { get; set; }
    }

    public class SettleDetail
    {
        public int CarId { get; set; }
        public double Reward { get; set; }
        public double NewQ { get; set; }
    }
}
