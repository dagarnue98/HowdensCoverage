using PublishSystem.Domain.SeedWork;
using System.ComponentModel.DataAnnotations;

namespace PublishSystem.Domain.Entities
{
    public class PublishJob : Entity
    {
        public int Id { get; set; }

        [Required]
        public Guid JobId { get; set; }

        public int VersionId { get; set; }
        public string State { get; set; }
        public string DepotNo { get; set; }
        public string BuilderId { get; set; }
        public string QuoteNMBR { get; set; }

        [Required]
        public string Name { get; set; }

        public char HdVersion { get; set; }
        public int PlanType { get; set; }
        public string SenderEmail { get; set; }
        public string ReceipientEmail1 { get; set; }
        public string ReceipientEmail2 { get; set; }
        public string Comments { get; set; }
        public string? BatchJobId { get; set; }
    }
}
