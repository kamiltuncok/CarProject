using Core.DataAccess.EntityFramework;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccess.Concrete.EntityFramework
{
    public class EfPriceDecisionDal
        : EfEntityRepositoryBase<PriceDecision, RentACarContext>, IPriceDecisionDal
    {
        public async Task AddRangeAsync(List<PriceDecision> decisions)
        {
            using var context = new RentACarContext();
            await context.PriceDecisions.AddRangeAsync(decisions);
            await context.SaveChangesAsync();
        }

        public async Task<List<PriceDecision>> GetPendingSettlementsAsync(
            DateTime decidedBefore, int maxCount)
        {
            using var context = new RentACarContext();
            return await context.PriceDecisions
                .Where(p => !p.IsSettled && p.DecidedAt <= decidedBefore)
                .OrderBy(p => p.DecidedAt)
                .Take(maxCount)
                .ToListAsync();
        }

        public async Task<(int Bookings, decimal Gross, decimal Refunds)> GetRealizedDemandAsync(
            int carId, DateTime from, DateTime to)
        {
            using var context = new RentACarContext();

            // TALEP ile GELİR ayrı ölçülür:
            //
            //   Talep  = pencerede YAPILAN tüm rezervasyonlar, sonradan iptal
            //            edilenler DAHİL. Müşteri o fiyata rezervasyon yaptı;
            //            öğrenmek istediğimiz fiyat tepkisi budur. İptalleri
            //            saymamak, yüksek fiyatta talebi sistematik olarak
            //            düşük ölçer (iptal fiyatla pozitif ilişkili).
            //
            //   Gelir  = brüt tutar eksi iadeler. Ödül bunu kullanır; brüt ciro
            //            tahsil edilmeyen parayı ödüllendiriyordu (ölçüldü: %30).
            var query = context.Rentals
                .Where(r => r.CarId == carId
                            && r.StartDate >= from
                            && r.StartDate < to);

            var bookings = await query.CountAsync();
            if (bookings == 0)
                return (0, 0m, 0m);

            var gross = await query.SumAsync(r => r.TotalPrice);
            var refunds = await query.SumAsync(r => r.RefundAmount ?? 0m);
            return (bookings, gross, refunds);
        }

        public async Task UpdateRangeAsync(List<PriceDecision> decisions)
        {
            using var context = new RentACarContext();
            context.PriceDecisions.UpdateRange(decisions);
            await context.SaveChangesAsync();
        }

        public async Task<List<PriceDecision>> GetSettledSinceAsync(DateTime since)
        {
            using var context = new RentACarContext();
            return await context.PriceDecisions
                .Where(p => p.IsSettled && p.DecidedAt >= since)
                .ToListAsync();
        }
    }
}
