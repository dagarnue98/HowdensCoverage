namespace PublishSystem.Domain.Specification.EmailTEmplateSpecification
{
    using PublishSystem.Domain.Entities;
    using PublishSystem.Domain.SeedWork;

    public class EmailTemplateByCodeSpecification : Specification<EmailTemplate>
    {
        public EmailTemplateByCodeSpecification(string code)
            : base(x => x.Code == code)
        {
        }
    }
}
