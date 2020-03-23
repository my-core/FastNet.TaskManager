using FastNet.TaskManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastNet.TaskManager.DataContract
{
    public class TaskJobListResponse : TaskJobInfo
    {
        public int JobStatus { get; set; }
    }

}
