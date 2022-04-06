namespace PublishSystem.Domain.Models
{
    public class PublishJobModel : PublishJobBaseModel
    {
        public Guid JobId { get; set; }
        public string State { get; set; }
    }
}
