using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastNet.TaskManager.DataContract
{
    public class SwitchStatusRequest
    {
        /// <summary>
        /// 
        /// </summary>
        public int JobId { get; set; }
        /// <summary>
        /// 1-启动 2-停止
        /// </summary>
        public int Status { get; set; }
    }
}
