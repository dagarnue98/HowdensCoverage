using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSystem.Integration.Rendering.AzureBatch.Events
{
    public  class ExecutionInfo
    {
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int? ExitCode { get; set; }
        public int RetryCount { get; set; }
        public int? RequeueCount { get; set; }
    }
}
