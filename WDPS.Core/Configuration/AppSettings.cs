namespace WDPS.Core.Configuration
{
    public class AppSettings
    {
        public DatabaseSettings Database { get; set; }
        public LoggingSettings Logging { get; set; }
        public MetricsSettings Metrics { get; set; }

        public class DatabaseSettings
        {
            public string ConnectionString { get; set; }
        }

        public class LoggingSettings
        {
            public string LogFilePath { get; set; }
            public string MinimumLevel { get; set; }
        }

        public class MetricsSettings
        {
            public int CollectionIntervalMinutes { get; set; }
            public bool EnableCpuTracking { get; set; }
            public bool EnableMemoryTracking { get; set; }
            public bool EnableWindowTracking { get; set; }
        }
    }
} 