using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using FastNet.TaskManager.Models;
using FastNet.TaskManager.QuartzNet;
using FastNet.TaskManager.Repository;

namespace FastNet.TaskManager
{
    public class QuartzService : IHostedService
    {
        private readonly ILogger<QuartzService> _logger;
        /// <summary>
        /// Quartzπ‹¿Ì∆˜
        /// </summary>
        private QuartzManager _quartzManager;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="quartzManager"></param>
        public QuartzService(ILogger<QuartzService> logger, QuartzManager quartzManager)
        {
            _logger = logger;
            _quartzManager = quartzManager;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzService start");
            await _quartzManager.InitQuartzService();
            await _quartzManager.Start();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("QuartzService stop");
            await _quartzManager.Stop();
        }
    }
}
