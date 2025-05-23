using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WDPS.Core.Data;
using WDPS.Core.Models;

namespace WDPS.Core.Services
{
    public class AnalyticsService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;

        public AnalyticsService(ApplicationDbContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task GenerateDailySummaryAsync(DateTime date)
        {
            try
            {
                var start = date.Date;
                var end = start.AddDays(1);

                var activeTime = await _context.SystemMetrics
                    .Where(m => m.Timestamp >= start && m.Timestamp < end)
                    .SumAsync(m => m.SessionDuration.TotalMinutes);

                var featureUsage = await _context.CodeActivityEvents
                    .Where(e => e.Timestamp >= start && e.Timestamp < end)
                    .CountAsync();

                var avgSession = await _context.SystemMetrics
                    .Where(m => m.Timestamp >= start && m.Timestamp < end)
                    .AverageAsync(m => m.SessionDuration.TotalMinutes);

                // Simulate retention and conversion for demo
                var retention = 0.8; // 80% retention (mock)
                var funnelStep = 2; // e.g., user reached step 2 in conversion funnel

                var summary = new AnalyticsSummary
                {
                    Date = start,
                    DailyActiveTimeMinutes = activeTime,
                    FeatureUsageCount = featureUsage,
                    SessionDurationAverageMinutes = avgSession,
                    RetentionRate = retention,
                    ConversionFunnelStep = funnelStep
                };

                _context.AnalyticsSummaries.Add(summary);
                await _context.SaveChangesAsync();
                _logger.Information($"Analytics summary generated for {date:yyyy-MM-dd}");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Failed to generate analytics summary");
            }
        }
    }
} 