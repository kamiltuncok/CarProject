using System.Collections.Generic;

namespace Core.Entities.Requests
{
    /// <summary>
    /// Talep eşiklerinin filo verisinden yeniden hesaplanmasının sonucu.
    /// </summary>
    public class DemandCalibrationDto
    {
        public int WindowDays { get; set; }
        public List<SegmentCalibration> Segments { get; set; } = new List<SegmentCalibration>();
        public List<string> Notes { get; set; } = new List<string>();
    }

    public class SegmentCalibration
    {
        public int SegmentId { get; set; }
        public string SegmentName { get; set; }
        public int CarCount { get; set; }

        public double OldLow { get; set; }
        public double OldHigh { get; set; }
        public double NewLow { get; set; }
        public double NewHigh { get; set; }

        /// <summary>Yeni eşiklerle LOW / MID / HIGH kovalarına düşen araç sayısı.</summary>
        public int LowCount { get; set; }
        public int MidCount { get; set; }
        public int HighCount { get; set; }
    }
}
