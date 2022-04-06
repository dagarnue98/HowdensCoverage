namespace PublishSystem.Domain.SeedWork
{
    public abstract class Entity : IEntity
    {
        public virtual int Id { get; private set; }
    }
}
