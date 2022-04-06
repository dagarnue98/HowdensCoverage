using System.Linq.Expressions;

namespace PublishSystem.Domain.SeedWork
{
    public class Specification<T> : ISpecification<T>
    {
        public Specification(Expression<Func<T, bool>> predicate)
        {
            this.Predicate = predicate;
        }

        public static ISpecification<T> False { get; } = new Specification<T>(x => false);
        public static ISpecification<T> True { get; } = new Specification<T>(x => true);

        public Expression<Func<T, bool>> Predicate { get; }
    }
}
