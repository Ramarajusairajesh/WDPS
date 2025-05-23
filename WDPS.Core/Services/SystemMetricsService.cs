using System;
using System.Diagnostics;
using System.Management;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WDPS.Core.Data;
using WDPS.Core.Models;

namespace WDPS.Core.Services
{
    public class SystemMetricsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly PerformanceCounter _cpuCounter;
        private readonly PerformanceCounter _memoryCounter;

        public SystemMetricsService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;

            try
            {
                _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
                _memoryCounter = new PerformanceCounter("Memory", "Available MBytes");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to initialize performance counters");
            }
        }

        public async Task CollectAndSaveMetricsAsync()
        {
            try
            {
                var metrics = new SystemMetrics
                {
                    Timestamp = DateTime.UtcNow,
                    CpuUsage = GetCpuUsage(),
                    MemoryUsage = GetMemoryUsage(),
                    ActiveWindowTitle = GetActiveWindowTitle(),
                    ActiveProcessName = GetActiveProcessName(),
                    SessionDuration = GetSessionDuration()
                };

                _context.SystemMetrics.Add(metrics);
                await _context.SaveChangesAsync();

                _logger.Information("System metrics collected and saved successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to collect and save system metrics");
            }
        }

        private double GetCpuUsage()
        {
            try
            {
                return _cpuCounter?.NextValue() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get CPU usage");
                return 0;
            }
        }

        private double GetMemoryUsage()
        {
            try
            {
                return _memoryCounter?.NextValue() ?? 0;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get memory usage");
                return 0;
            }
        }

        private string GetActiveWindowTitle()
        {
            try
            {
                var foregroundWindow = GetForegroundWindow();
                var length = GetWindowTextLength(foregroundWindow);
                var builder = new System.Text.StringBuilder(length + 1);
                GetWindowText(foregroundWindow, builder, builder.Capacity);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get active window title");
                return string.Empty;
            }
        }

        private string GetActiveProcessName()
        {
            try
            {
                var foregroundWindow = GetForegroundWindow();
                GetWindowThreadProcessId(foregroundWindow, out uint processId);
                var process = Process.GetProcessById((int)processId);
                return process.ProcessName;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get active process name");
                return string.Empty;
            }
        }

        private TimeSpan GetSessionDuration()
        {
            try
            {
                var query = new SelectQuery("SELECT LastBootUpTime FROM Win32_OperatingSystem");
                using var searcher = new ManagementObjectSearcher(query);
                foreach (ManagementObject obj in searcher.Get())
                {
                    var lastBootUpTime = ManagementDateTimeConverter.ToDateTime(obj["LastBootUpTime"].ToString());
                    return DateTime.Now - lastBootUpTime;
                }
                return TimeSpan.Zero;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to get session duration");
                return TimeSpan.Zero;
            }
        }

        // Windows API imports
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, System.Text.StringBuilder text, int count);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int GetWindowTextLength(IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);
    }
} 