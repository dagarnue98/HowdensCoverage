namespace PublishSystem.Domain.Entities
{
    using PublishSystem.Domain.SeedWork;

    public class EmailTemplate : Entity
    {
        public int Id { get; set; }

        public string Code { get; set; }

        public string Name { get; set; }

        public string Translate { get; set; }

        public string CustomTemplate { get; set; }
    }
}
