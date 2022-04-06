using PublishSystem.Domain.SeedWork;

namespace PublishSystem.Domain.Specification.PublishJobSpecification
{
    public class PublishJobByVersionIdSpecification : Specification<Entities.PublishJob>
    {
        public PublishJobByVersionIdSpecification(int id)
            : base(x => x.VersionId == id)
        {
        }

    }
}
