using System.ComponentModel.DataAnnotations;

namespace PublishSystem.Domain.Models
{
    public abstract class PublishJobBaseModel
    {
        [Required]
        public int VersionId { get; set; }

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
    }
}
