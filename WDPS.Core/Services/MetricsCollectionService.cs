using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace WDPS.Core.Services
{
    public class MetricsCollectionService : BackgroundService
    {
        private readonly SystemMetricsService _metricsService;
        private readonly ILogger _logger;
        private readonly TimeSpan _collectionInterval = TimeSpan.FromMinutes(1);

        public MetricsCollectionService(SystemMetricsService metricsService, ILogger logger)
        {
            _metricsService = metricsService;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.Information("Metrics collection service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _metricsService.CollectAndSaveMetricsAsync();
                    await Task.Delay(_collectionInterval, stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.Error(ex, "Error occurred while collecting metrics");
                    await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                }
            }

            _logger.Information("Metrics collection service stopped");
        }
    }
} 