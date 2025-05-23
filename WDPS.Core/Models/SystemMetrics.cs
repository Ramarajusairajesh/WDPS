using System;

namespace WDPS.Core.Models
{
    public class SystemMetrics
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public double CpuUsage { get; set; }
        public double MemoryUsage { get; set; }
        public string ActiveWindowTitle { get; set; }
        public string ActiveProcessName { get; set; }
        public TimeSpan SessionDuration { get; set; }
    }
} 
