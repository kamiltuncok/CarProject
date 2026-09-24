using Core.Entities;
using System;

namespace Entities.Concrete
{
    /// <summary>
    /// RL motorunun verdiği her fiyat kararının günlüğü.
    ///
    /// Geri besleme döngüsünü kapatmak için var: karar anında ödül hesaplanmaz.
    /// Karar penceresi dolduktan sonra (bkz. IPricingService.SettleRewardsAsync)
    /// o pencerede GERÇEKLEŞEN kiralamalar sayılır, gerçek gelir hesaplanır ve
    /// Q-güncellemesi o zaman yapılır. Eskiden ödül, sentetik bir talep
    /// simülatöründen geliyordu — ajan gerçek sonucu hiç görmüyordu.
    /// </summary>
    public class PriceDecision : IEntity
    {
        public int Id { get; set; }

        public int CarId { get; set; }
        public int SegmentId { get; set; }

        public DateTime DecidedAt { get; set; }

        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

        /// <summary>Oransal RL aksiyonu: -0.10 … +0.10</summary>
        public double Action { get; set; }

        /// <summary>Karar anındaki state, "DEMAND|PRICE|TREND|INERTIA|SEASON" formatında.</summary>
        public string StateKey { get; set; }

        // ─── Gecikmeli ödül, pencere kapandığında doldurulur ───
        public bool IsSettled { get; set; }
        public DateTime? SettledAt { get; set; }

        /// <summary>Karar penceresinde gerçekleşen kiralama sayısı.</summary>
        public int? RealizedRentals { get; set; }

        /// <summary>Karar penceresinde gerçekleşen NET ciro (brüt - iade).</summary>
        public decimal? RealizedRevenue { get; set; }

        /// <summary>Brüt ciro — net ile farkı iptal maliyetini gösterir.</summary>
        public decimal? GrossRevenue { get; set; }

        /// <summary>Pencerede iade edilen tutar.</summary>
        public decimal? RefundedAmount { get; set; }

        /// <summary>Gerçekleşen sonuçtan hesaplanan ödül.</summary>
        public double? Reward { get; set; }

        /// <summary>Kontrol grubu araçları RL kararı almaz; A/B ölçümü için işaretlenir.</summary>
        public bool IsControlGroup { get; set; }
    }
}
