using Core.DataAccess;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface IPriceDecisionDal : IEntityRepository<PriceDecision>
    {
        Task AddRangeAsync(List<PriceDecision> decisions);

        /// <summary>Penceresi dolmuş ve henüz ödülü hesaplanmamış kararlar.</summary>
        Task<List<PriceDecision>> GetPendingSettlementsAsync(DateTime decidedBefore, int maxCount);

        /// <summary>
        /// Karar penceresinde gerçekleşen talep ve gelir.
        /// Bookings: iptal edilenler DAHİL tüm rezervasyonlar (fiyat tepkisi sinyali).
        /// Gross/Refunds: net gelir = Gross - Refunds (ödül bunu kullanır).
        /// </summary>
        Task<(int Bookings, decimal Gross, decimal Refunds)> GetRealizedDemandAsync(
            int carId, DateTime from, DateTime to);

        Task UpdateRangeAsync(List<PriceDecision> decisions);

        /// <summary>A/B karşılaştırması: RL grubu vs kontrol grubu, araç-gün başına ciro.</summary>
        Task<List<PriceDecision>> GetSettledSinceAsync(DateTime since);
    }
}
