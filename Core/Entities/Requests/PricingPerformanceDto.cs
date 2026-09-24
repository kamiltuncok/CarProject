using System.Collections.Generic;

namespace Core.Entities.Requests
{
    /// <summary>
    /// A/B ölçümü: RL fiyatlandırılan araçlar ile sabit fiyatlı kontrol grubunun
    /// karşılaştırması. Bu ölçüm olmadan RL'in para kazandırdığına dair kanıt yoktur.
    /// </summary>
    public class PricingPerformanceDto
    {
        public int WindowDays { get; set; }

        public GroupPerformance RlGroup { get; set; }
        public GroupPerformance ControlGroup { get; set; }

        /// <summary>RL grubunun kontrol grubuna göre araç-gün başına ciro farkı, yüzde.</summary>
        public double? UpliftPercent { get; set; }

        public List<string> Notes { get; set; } = new List<string>();
    }

    public class GroupPerformance
    {
        public int Decisions { get; set; }
        public int Cars { get; set; }
        public int RealizedRentals { get; set; }
        public decimal RealizedRevenue { get; set; }

        /// <summary>Karar başına ortalama ciro — grupları karşılaştırmanın temel metriği.</summary>
        public decimal RevenuePerDecision { get; set; }

        public double AverageReward { get; set; }
    }
}
