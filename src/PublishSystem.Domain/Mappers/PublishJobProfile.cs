using AutoMapper;
using PublishSystem.Domain.Entities;
using PublishSystem.Domain.Models;

namespace PublishSystem.Domain.Mappers
{
    public class PublishJobProfile : ProfileBase<PublishJobModel, PublishJob>
    {
        protected override IMappingExpression<PublishJobModel, PublishJob> Map()
        {
            return base.Map()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
        }

        protected override IMappingExpression<PublishJob, PublishJobModel> ReverseMap()
        {
            return base.ReverseMap();
        }
    }
}
