using Microsoft.Extensions.Logging;
using Quartz;
using Quartz.Impl;
using FastNet.TaskManager.Models;
using FastNet.TaskManager.Repository;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FastNet.TaskManager.QuartzNet.Jobs
{
    /// <summary>
    /// 公共资讯抓取作业
    /// </summary>
    public class TaskJob : IJob
    {
        private ILogger<TaskJob> _logger;
        /// <summary>
        /// 运营库仓储
        /// </summary>
        private QuartzRepository _yunYingRepository;
        public TaskJob(ILogger<TaskJob> logger, QuartzRepository yunYingRepository)
        {
            _logger = logger;
            _yunYingRepository = yunYingRepository;
        }
        public async Task Execute(IJobExecutionContext context)
        {
            int jobID = 0;
            try
            {
                JobDetailImpl jobDetail = (JobDetailImpl)context.JobDetail;
                jobID = jobDetail.JobDataMap.GetInt(JobParamKey.JobID);
                _logger.LogInformation($"satrt execute->JobID[{jobID}]");               
                await CallTaskApi(jobID);
                _logger.LogInformation($"end execute->JobID[{jobID}]");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"execute failed->JobID[{jobID}]");
            }
        }

        /// <summary>
        /// 调用任务API,触发任务调度
        /// </summary>
        private async Task CallTaskApi(int jobID)
        {
            await Task.Run(() =>
           {
               TaskJobInfo taskJob = _yunYingRepository.GetModel<TaskJobInfo>(new { JobID = jobID });
               if (taskJob == null)
               {
                   _logger.LogWarning($"Can not find the taskjob -> JobID[{jobID}]");
                   return;
               }
               if (string.IsNullOrEmpty(taskJob.ApiUrl))
               {
                   _logger.LogWarning($"the call apiUrl of taskjob is null-> JobID[{jobID}]");
                   return;
               }
               _logger.LogInformation($"start CallTaskApi -> ApiUrl[{taskJob.ApiUrl}]");
               try
               {
                   //call api url
                   using (var httpClient = new HttpClient())
                   {
                       var result = httpClient.GetStringAsync(taskJob.ApiUrl).Result;
                       _logger.LogInformation($"CallTaskApi success->JobID[{jobID}],Result[{result}]");
                       taskJob.LastExecuteTime = DateTime.Now;
                       _yunYingRepository.Update(taskJob);
                   }
               }
               catch (Exception ex)
               {
                   _logger.LogError(ex, $"CallTaskApi failed -> ApiUrl[{taskJob.ApiUrl}]");
               }
           });      
        }
    }
}
