using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Serilog;
using WDPS.Core.Data;
using WDPS.Core.Models;
using WDPS.Core.Services;
using Xunit;

namespace WDPS.Tests
{
    public class SystemMetricsServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly SystemMetricsService _service;

        public SystemMetricsServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();

            _service = new SystemMetricsService(_context, _logger);
        }

        [Fact]
        public async Task CollectAndSaveMetricsAsync_ShouldSaveMetricsToDatabase()
        {
            // Act
            await _service.CollectAndSaveMetricsAsync();

            // Assert
            var savedMetrics = await _context.SystemMetrics.FirstOrDefaultAsync();
            Assert.NotNull(savedMetrics);
            Assert.NotEqual(default, savedMetrics.Timestamp);
            Assert.True(savedMetrics.CpuUsage >= 0);
            Assert.True(savedMetrics.MemoryUsage >= 0);
        }

        [Fact]
        public async Task CollectAndSaveMetricsAsync_ShouldHandleErrorsGracefully()
        {
            // Arrange
            _context.Dispose(); // Force an error by disposing the context

            // Act & Assert
            await _service.CollectAndSaveMetricsAsync(); // Should not throw
        }
    }
} 