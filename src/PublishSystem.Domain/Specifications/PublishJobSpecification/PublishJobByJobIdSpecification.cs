using PublishSystem.Domain.Entities;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Specifications.PublishJobSpecification
{
    public class PublishJobByJobIdSpecification : Specification<PublishJob>
    {
        public PublishJobByJobIdSpecification(Guid jobId)
            : base(x => x.JobId == jobId)
        {
        }
    }
}
