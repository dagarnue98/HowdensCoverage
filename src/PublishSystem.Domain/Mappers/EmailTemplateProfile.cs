namespace PublishSystem.Domain.Mappers
{
    using AutoMapper;
    using PublishSystem.Domain.Entities;
    using PublishSystem.Domain.Models;

    public class EmailTemplateProfile : ProfileBase<EmailTemplateModel, EmailTemplate>
    {
        protected override IMappingExpression<EmailTemplateModel, EmailTemplate> Map()
        {
            return base.Map();
        }

        protected override IMappingExpression<EmailTemplate, EmailTemplateModel> ReverseMap()
        {
            return base.ReverseMap();
        }
    }
}
