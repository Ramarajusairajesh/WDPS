using System;

namespace WDPS.Core.Models
{
    public class CodeActivityEvent
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string EventType { get; set; } // e.g., "FileEdit", "ProjectSwitch", "ToolUsage"
        public string Description { get; set; }
        public string FilePath { get; set; }
        public string ProjectName { get; set; }
        public string ToolName { get; set; }
    }
} 