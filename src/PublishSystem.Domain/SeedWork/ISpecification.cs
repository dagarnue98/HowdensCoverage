using System.Linq.Expressions;

namespace PublishSystem.Domain.SeedWork
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>> Predicate { get; }
    }
}
