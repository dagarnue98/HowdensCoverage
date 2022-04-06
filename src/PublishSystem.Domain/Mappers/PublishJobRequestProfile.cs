using AutoMapper;
using PublishSystem.Domain.Entities;
using PublishSystem.Domain.Models;

namespace PublishSystem.Domain.Mappers
{
    public class PublishRequestProfile : ProfileBase<PublishJobRequestModel, PublishJob>
    {
        protected override IMappingExpression<PublishJobRequestModel, PublishJob> Map()
        {
            return base.Map();
        }

        protected override IMappingExpression<PublishJob, PublishJobRequestModel> ReverseMap()
        {
            return base.ReverseMap();
        }
    }
}
