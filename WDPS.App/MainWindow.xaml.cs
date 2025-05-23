using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using WDPS.Core.Services;
using WDPS.Core.Models;
using WDPS.Core.Data;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace WDPS.App
{
    public partial class MainWindow : Window
    {
        private FeatureFlagService _featureFlagService;
        private SystemMetricsService _metricsService;
        private AnalyticsService _analyticsService;
        private ApplicationDbContext _dbContext;
        private DispatcherTimer _refreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            LoadFeatureFlags();
            InitializeCoreServices();
            StartRefreshTimer();
        }

        private void LoadFeatureFlags()
        {
            var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FeatureFlagConfig.sample.json");
            _featureFlagService = new FeatureFlagService(configPath);

            // Example: Set theme based on feature flag
            if (_featureFlagService.IsFeatureEnabled("EnableDarkTheme"))
            {
                this.Background = new SolidColorBrush(Color.FromRgb(34, 34, 34));
            }
            else
            {
                this.Background = Brushes.White;
            }

            // Example: Show which A/B test group is active
            var abGroup = _featureFlagService.GetABTestGroup("DashboardLayout");
            this.Title = $"WDPS Dashboard - Layout {abGroup}";
        }

        private void InitializeCoreServices()
        {
            var dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wdps.db");
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlite($"Data Source={dbPath}")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            var logger = new LoggerConfiguration().WriteTo.Console().CreateLogger();
            _metricsService = new SystemMetricsService(_dbContext, logger);
            _analyticsService = new AnalyticsService(_dbContext, logger);
        }

        private void StartRefreshTimer()
        {
            _refreshTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
            _refreshTimer.Tick += async (s, e) =>
            {
                // Collect and display latest metrics
                await _metricsService.CollectAndSaveMetricsAsync();
                var latest = await _dbContext.SystemMetrics.OrderByDescending(m => m.Timestamp).FirstOrDefaultAsync();
                if (latest != null)
                {
                    CpuUsageText.Text = $"CPU Usage: {latest.CpuUsage:F1}%";
                    MemoryUsageText.Text = $"Memory Usage: {latest.MemoryUsage:F1} MB";
                    ActiveWindowText.Text = $"Active Window: {latest.ActiveWindowTitle}";
                    SessionDurationText.Text = $"Session Duration: {latest.SessionDuration:hh\\:mm\\:ss}";
                }

                // Generate and display analytics summary
                await _analyticsService.GenerateDailySummaryAsync(DateTime.Now);
                var summary = await _dbContext.AnalyticsSummaries.OrderByDescending(a => a.Date).FirstOrDefaultAsync();
                if (summary != null)
                {
                    DailyActiveTimeText.Text = $"Daily Active Time: {summary.DailyActiveTimeMinutes:F1} min";
                    FeatureUsageText.Text = $"Feature Usage: {summary.FeatureUsageCount}";
                    RetentionRateText.Text = $"Retention Rate: {summary.RetentionRate * 100:F1}%";
                    ConversionFunnelText.Text = $"Conversion Funnel: Step {summary.ConversionFunnelStep}";
                }

                // Onboarding wizard progress
                var onboarding = await _dbContext.OnboardingProgresses.OrderByDescending(o => o.StartedAt).FirstOrDefaultAsync();
                if (onboarding != null && !onboarding.IsCompleted)
                {
                    OnboardingText.Text = $"Step {onboarding.CurrentStep} of {onboarding.TotalSteps} - In Progress";
                }
                else if (onboarding != null && onboarding.IsCompleted)
                {
                    OnboardingText.Text = $"Onboarding Completed!";
                }
                else
                {
                    OnboardingText.Text = $"Onboarding not started.";
                }

                // Feature suggestion logic (simple example)
                var usageCount = await _dbContext.CodeActivityEvents.CountAsync();
                if (usageCount < 5)
                {
                    FeatureSuggestionsText.Text = "Tip: Try using the code activity tracker!";
                }
                else
                {
                    FeatureSuggestionsText.Text = "Explore advanced analytics in the dashboard.";
                }

                // Premium feature simulation
                if (_featureFlagService.IsFeatureEnabled("EnablePremiumFeatures"))
                {
                    PremiumFeaturesText.Text = "Premium Feature: Advanced Cohort Analysis Enabled!";
                }
                else
                {
                    PremiumFeaturesText.Text = "Upgrade to unlock premium analytics.";
                }
            };
            _refreshTimer.Start();
        }
    }
} 