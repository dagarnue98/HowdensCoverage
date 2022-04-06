using PublishSystem.Domain.Entities;
using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Specifications.PublishJobSpecification
{
    public class PublishJobByIdSpecification : Specification<PublishJob>
    {
        public PublishJobByIdSpecification(int id)
            : base(x => x.Id == id)
        {
        }
    }
}
