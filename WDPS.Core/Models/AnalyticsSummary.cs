using System;

namespace WDPS.Core.Models
{
    public class AnalyticsSummary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public double DailyActiveTimeMinutes { get; set; }
        public int FeatureUsageCount { get; set; }
        public double SessionDurationAverageMinutes { get; set; }
        public double RetentionRate { get; set; } // Simulated or calculated
        public int ConversionFunnelStep { get; set; } // For mock premium features
    }
} 