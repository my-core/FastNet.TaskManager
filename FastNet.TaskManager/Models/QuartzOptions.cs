using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastNet.TaskManager.Models
{
    public class QuartzOptions
    {
        /// <summary>
        /// 开启集群
        /// </summary>
        public string Clustered { get; set; }
        /// <summary>
        /// 调度标识名,集群中每一个实例都必须使用相同的名称
        /// </summary>
        public string InstanceName { get; set; }
        /// <summary>
        /// ID设置为自动获取 每一个必须不同
        /// </summary>
        public string InstanceId { get; set; }
        /// <summary>
        /// 线程池类型
        /// </summary>
        public string ThreadPoolType { get; set; }
        /// <summary>
        /// 线程池个数
        /// </summary>
        public string ThreadCount { get; set; }
        /// <summary>
        /// 存储类型(数据保存方式为持久化)
        /// </summary>
        public string JobStoreType { get; set; }
        /// <summary>
        /// /表明前缀
        /// </summary>
        public string TablePrefix { get; set; }
        /// <summary>
        /// 使用Sqlserver的Ado操作代理类
        /// </summary>
        public string DriverDelegateType { get; set; }
        /// <summary>
        /// 连接字符串
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// sqlserver版本
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// 序列化类型(json)
        /// </summary>
        public string SerializerType { get; set; }
    }
}
