using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PublishSystem.Integration.Rendering.AzureBatch.Events
{
    public class TaskEvent
    {
        public string JobId { get; set; }
        public string Id { get; set; }
        public string TaskType { get; set; }
        public int SystemTaskVersion { get; set; }
        public int RequiredSlots { get; set; }
        public NodeInfo NodeInfo { get; set; }
        public MultiInstanceSettings MultiInstanceSettings { get; set; }
        public Constraints Constraints { get; set; }
        public ExecutionInfo? ExecutionInfo { get; set; }
        public SchedulingError? SchedulingError { get; set; }
    }
}
