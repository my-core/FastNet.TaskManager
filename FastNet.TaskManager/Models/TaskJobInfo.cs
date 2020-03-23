using System;
using System.Collections.Generic;
using System.Text;
using FastNet.Framework.Dapper.Mapper;

namespace FastNet.TaskManager.Models
{
    /// <summary>
    /// 调度任务信息
    /// </summary>
    [Table("task_job", "JobID")]
    public class TaskJobInfo
    {
        /// <summary>
        /// 作业id
        /// </summary>
        public int JobID { get; set; }
        /// <summary>
        /// 作业名称
        /// </summary>
        public string JobName { get; set; }
        /// <summary>
        /// 作业描述
        /// </summary>
        public string JobDesc { get; set; }
        /// <summary>
        /// 任务执行的类名
        /// </summary>
        public string JobClassName { get; set; }
        /// <summary>
        /// 调度任务Cron表达式
        /// </summary>
        public string CronExpression { get; set; }
        /// <summary>
        /// 作业调用api
        /// </summary>
        public string ApiUrl { get; set; }
        /// <summary>
        /// 扩展参数
        /// </summary>
        public string ExtendData { get; set; }
        /// <summary>
        /// 最新执行时间
        /// </summary>
        public DateTime LastExecuteTime { get; set; }
        /// <summary>
        /// 0-不可用 1-可用
        /// </summary>
        public bool Enabled { get; set; }


    }
}
