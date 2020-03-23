using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Quartz.Impl;
using FastNet.TaskManager.Models;
using FastNet.TaskManager.Repository;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;

namespace FastNet.TaskManager.QuartzNet
{
    public class QuartzManager
    {
        private readonly ILogger<QuartzService> _logger;
        /// <summary>
        /// 运营库仓储
        /// </summary>
        private QuartzRepository _yunYingRepository;
        private QuartzOptions _quartzOptions;
        /// <summary>
        /// 作业调度池
        /// </summary>
        public IScheduler _scheduler;
        public QuartzManager(ILogger<QuartzService> logger, IServiceProvider serviceProvider, QuartzRepository yunYingRepository, IOptions<QuartzOptions> options)
        {
            _logger = logger;
            _yunYingRepository = yunYingRepository;
            _quartzOptions = options.Value;
            //配置AdoJobStore的集群功能
            //1-所有实例都必须使用不同的属性文件，因为它们的实例ID必须不同，但是所有其他属性都应该相同。
            //2-您可能应该从一组“新”表开始（假设您由于其他测试混合了数据，具有集群的非集群设置可能很糟糕。)
            //3-尝试在它们运行时杀死其中一个集群实例，然后看看其余实例将恢复正在进行的作业。请注意，默认情况下，检测到故障可能需要15秒钟左右(最少7.5秒)的时间设置
            //4-也可以尝试使用/不使用已注册的shutdown-hook插件来运行它调度程序。（quartz.plugins.management.ShutdownHookPlugin）。
            //5-注意：切勿在单独的计算机上运行群集，除非它们在不同的计算机上运行使用某种形式的时间同步服务（守护程序）来同步时钟。

            //配置持久化-集群
            NameValueCollection properties = new NameValueCollection();
            //开启集群
            properties["quartz.jobStore.clustered"] = string.IsNullOrEmpty(_quartzOptions.Clustered) ? "false" : _quartzOptions.Clustered;
            //调度标识名,集群中每一个实例都必须使用相同的名称
            properties["quartz.scheduler.instanceName"] = string.IsNullOrEmpty(_quartzOptions.InstanceName) ? "TaskScheduler" : _quartzOptions.InstanceName;
            //ID设置为自动获取 每一个必须不同
            properties["quartz.scheduler.instanceId"] = string.IsNullOrEmpty(_quartzOptions.InstanceId) ? "AUTO" : _quartzOptions.InstanceId;
            //这个时间大于10000（10秒）会导致WithMisfireHandlingInstructionDoNothing不生效。
            properties["quartz.jobStore.misfireThreshold"] = "5000";
            //properties["quartz.threadPool.type"] = "Quartz.Simpl.SimpleThreadPool, Quartz";
            //线程池个数
            properties["quartz.threadPool.threadCount"] = string.IsNullOrEmpty(_quartzOptions.ThreadCount) ? "20" : _quartzOptions.ThreadCount;
            //存储类型(数据保存方式为持久化)
            properties["quartz.jobStore.type"] = string.IsNullOrEmpty(_quartzOptions.JobStoreType) ? "Quartz.Impl.AdoJobStore.JobStoreTX, Quartz" : _quartzOptions.JobStoreType;
            //设置为TRUE不会出现序列化非字符串类到 BLOB 时产生的类版本问题
            //properties["quartz.jobStore.useProperties"] = "true";
            //数据库别名(任意)
            properties["quartz.jobStore.dataSource"] = "myDS";
            //表明前缀
            properties["quartz.jobStore.tablePrefix"] = string.IsNullOrEmpty(_quartzOptions.TablePrefix) ? "QRTZ_" : _quartzOptions.TablePrefix;
            //使用Sqlserver的Ado操作代理类
            properties["quartz.jobStore.driverDelegateType"] = string.IsNullOrEmpty(_quartzOptions.DriverDelegateType) ? "Quartz.Impl.AdoJobStore.SqlServerDelegate, Quartz" : _quartzOptions.DriverDelegateType;
            //连接字符串
            properties["quartz.dataSource.myDS.connectionString"] = _quartzOptions.ConnectionString;
            //sqlserver版本
            properties["quartz.dataSource.myDS.provider"] = string.IsNullOrEmpty(_quartzOptions.Provider) ? "SqlServer" : _quartzOptions.Provider;
            //序列化类型(json)
            properties["quartz.serializer.type"] = string.IsNullOrEmpty(_quartzOptions.SerializerType) ? "json" : _quartzOptions.SerializerType;

            //创建作业调度池
            _scheduler = new StdSchedulerFactory(properties).GetScheduler().Result;
            //实现Job的注入(quartz默认情况下，它仅支持无参数构造函数)
            _scheduler.JobFactory = new ServiceCollectionJobFactory(serviceProvider);
        }

        /// <summary>
        /// 初始化quartz服务
        /// </summary>
        public async Task InitQuartzService()
        {
            try
            {
                _logger.LogInformation("start InitQuartzService");
                List<TaskJobInfo> taskJobs = _yunYingRepository.GetList<TaskJobInfo>();
                if (taskJobs != null && taskJobs.Count > 0)
                {
                    foreach (TaskJobInfo taskJob in taskJobs)
                    {
                        await CreateJob(taskJob);
                    }
                }
                await _scheduler.Start();
                _logger.LogInformation("end InitQuartzService -> success");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "QuartzService init failed!");
            }
        }

        /// <summary>
        /// 创建Job
        /// </summary>
        /// <param name="jobItem"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public async Task CreateJob(TaskJobInfo taskJob)
        {
            var isExist = await ExistJob(taskJob.JobName);
            if (isExist)
                return;
            Type[] types = GetJobClass();
            var jobType = types.Where(t => t.Name == taskJob.JobClassName).FirstOrDefault();
            if (jobType == null)
            {
                _logger.LogWarning($"Can not find JobType[{taskJob.JobClassName}],JobID[{taskJob.JobID}],JobName[{taskJob.JobName}]!");
                return;
            }
            var jobDetail = new JobDetailImpl(taskJob.JobName, jobType);
            jobDetail.JobDataMap.Put(JobParamKey.JobID, taskJob.JobID);
            ICronTrigger trigger = CreateCronTrigger(taskJob);
            await _scheduler.ScheduleJob(jobDetail, trigger);
        }

        /// <summary>
        /// 创建CronTrigger
        /// </summary>
        /// <returns></returns>
        private ICronTrigger CreateCronTrigger(TaskJobInfo taskJob)
        {
            var scheduleBuilder = CronScheduleBuilder.CronSchedule(taskJob.CronExpression);
            ICronTrigger trigger = (ICronTrigger)TriggerBuilder.Create()
                .WithSchedule(scheduleBuilder.WithMisfireHandlingInstructionDoNothing())
                .WithIdentity(taskJob.JobName)
                .Build();
            return trigger;
        }

        /// <summary>  
        /// 反射获取程序集中的继承了IJob接口的类
        /// </summary>  
        /// <param name="assemblyName">程序集</param>  
        private static Type[] GetJobClass()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IJob))))
                .ToArray();
            return types;
        }

        #region 作业调度管理
        /// <summary>
        /// 开始执行
        /// </summary>
        public async Task Start()
        {
            await _scheduler.Start();
        }
        /// <summary>
        /// 停止执行
        /// </summary>
        public async Task Pause()
        {
            await _scheduler.PauseAll();
        }
        /// <summary>
        /// 恢复执行
        /// </summary>
        public async Task Resume()
        {
            await _scheduler.ResumeAll();
        }
        /// <summary>
        /// 线束执行
        /// </summary>
        public async Task Stop()
        {
            await _scheduler.Shutdown();
        }
        /// <summary>
        /// 获取job的状态
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<bool> ExistJob(string jobName)
        {
            return await _scheduler.CheckExists(new JobKey(jobName));
        }
        /// <summary>
        /// 获取job的状态
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task<int> GetState(string jobName)
        {
            var isExist = await ExistJob(jobName);
            if (isExist)
            {
                var result = await _scheduler.GetTriggerState(new TriggerKey(jobName));
                return (int)result;
            }
            else
            {
                return -1;
            }

        }
        /// <summary>
        /// 暂时任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task PauseJob(string jobName)
        {
            var isExist = await ExistJob(jobName);
            if (isExist)
                await _scheduler.PauseJob(new JobKey(jobName));
        }
        /// <summary>
        /// 恢复任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task ResumeJob(string jobName)
        {
            var isExist = await ExistJob(jobName);
            if (isExist)
                await _scheduler.ResumeJob(new JobKey(jobName));
        }
        /// <summary>
        /// 删除任务
        /// </summary>
        /// <param name="jobName"></param>
        /// <returns></returns>
        public async Task DeleteJob(string jobName)
        {
            var isExist = await ExistJob(jobName);
            if (isExist)
                await _scheduler.DeleteJob(new JobKey(jobName));
        }
        #endregion
    }
}
