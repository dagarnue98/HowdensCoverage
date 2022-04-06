using AutoMapper;

namespace PublishSystem.Domain.Mappers
{
    public abstract class ProfileBase<TSource, TDestination> : Profile
        where TSource : class
        where TDestination : class
    {
        public ProfileBase()
        {
            _ = Map();
            _ = ReverseMap();
        }

        protected virtual IMappingExpression<TSource, TDestination> Map()
        {
            return CreateMap<TSource, TDestination>();
        }

        protected virtual IMappingExpression<TDestination, TSource> ReverseMap()
        {
            return CreateMap<TDestination, TSource>();
        }
    }
}
