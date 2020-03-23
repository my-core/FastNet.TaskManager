using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace FastNet.TaskManager.QuartzNet
{
    /// <summary>
    /// 实现Job的注入(quartz默认情况下，它仅支持无参数构造函数)
    /// </summary>
    public class ServiceCollectionJobFactory : IJobFactory
    {
        protected readonly IServiceProvider _serviceProvider;
        private ConcurrentDictionary<IJob, IServiceScope> _createdJob = new ConcurrentDictionary<IJob, IServiceScope>();

        public ServiceCollectionJobFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 实现 Newjob  接口
        /// </summary>
        /// <param name="bundle"></param>
        /// <param name="scheduler"></param>
        /// <returns></returns>
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            var scoped = _serviceProvider.CreateScope();
            var result = scoped.ServiceProvider.GetService(bundle.JobDetail.JobType) as IJob;
            _createdJob.AddOrUpdate(result, scoped, (j, s) => scoped);
            return result;
        }

        public void ReturnJob(IJob job)
        {
            if (_createdJob.TryRemove(job, out var scope))
            {
                scope.Dispose();
            }

            var disposable = job as IDisposable;
            disposable?.Dispose();
        }
    }
}
