using System;
using System.IO;
using System.Threading.Tasks;
using Serilog;
using WDPS.Core.Data;
using WDPS.Core.Models;
using System.Collections.Generic;

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

    public class FeatureFlagConfig
    {
        public Dictionary<string, bool> FeatureFlags { get; set; } = new();
        public Dictionary<string, string> ABTestGroups { get; set; } = new(); // e.g., { "DashboardLayout": "A" }
    }
}

namespace WDPS.Core.Services
{
    public class CodeActivityService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private FileSystemWatcher _watcher;

        public CodeActivityService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public void StartWatching(string directory)
        {
            _watcher = new FileSystemWatcher(directory)
            {
                IncludeSubdirectories = true,
                EnableRaisingEvents = true
            };
            _watcher.Changed += OnChanged;
            _watcher.Created += OnChanged;
            _watcher.Renamed += OnRenamed;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            LogEvent("FileEdit", $"File {e.ChangeType}: {e.FullPath}", e.FullPath, null, null);
        }

        private void OnRenamed(object sender, RenamedEventArgs e)
        {
            LogEvent("FileEdit", $"File Renamed: {e.OldFullPath} -> {e.FullPath}", e.FullPath, null, null);
        }

        public void LogProjectSwitch(string projectName)
        {
            LogEvent("ProjectSwitch", $"Switched to project: {projectName}", null, projectName, null);
        }

        public void LogToolUsage(string toolName)
        {
            LogEvent("ToolUsage", $"Used tool: {toolName}", null, null, toolName);
        }

        private void LogEvent(string type, string desc, string file, string project, string tool)
        {
            var evt = new CodeActivityEvent
            {
                Timestamp = DateTime.UtcNow,
                EventType = type,
                Description = desc,
                FilePath = file,
                ProjectName = project,
                ToolName = tool
            };
            _context.CodeActivityEvents.Add(evt);
            _context.SaveChanges();
            _logger.Information($"Code activity logged: {desc}");
        }
    }
}
