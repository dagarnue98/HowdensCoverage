using PublishSystem.Domain.Entities;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Specification.PublishRequestSpecification
{
    public class PublishJobByBatchJobIdSpecification : Specification<PublishJob>
    {
        public PublishJobByBatchJobIdSpecification(string batchJobId)
            : base(x => x.BatchJobId == batchJobId)
        {
        }
    }
}
