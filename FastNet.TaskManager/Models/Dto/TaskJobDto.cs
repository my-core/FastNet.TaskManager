using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastNet.TaskManager.Models
{ 
    public class TaskJobDto : TaskJobInfo
    {
        public int Status { get; set; }
        public string StatusName { get; set; }
    }

}
