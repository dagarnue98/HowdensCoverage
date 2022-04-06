using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublishSystem.Domain.Entities;

namespace PublishSystem.Domain.Configurations
{
    internal class PublishJobConfiguration : IEntityTypeConfiguration<PublishJob>
    {
        public void Configure(EntityTypeBuilder<PublishJob> builder)
        {
            builder.ToTable(nameof(PublishJob));

            builder.HasKey(b => b.Id);

            builder.Property(b => b.VersionId);
            builder.Property(b => b.State);
            builder.Property(b => b.DepotNo);
            builder.Property(b => b.BuilderId);
            builder.Property(b => b.QuoteNMBR);
            builder.Property(b => b.Name);
            builder.Property(b => b.HdVersion);
            builder.Property(b => b.PlanType);
            builder.Property(b => b.SenderEmail);
            builder.Property(b => b.ReceipientEmail1);
            builder.Property(b => b.ReceipientEmail2);
            builder.Property(b => b.Comments);
            builder.Property(b => b.JobId);
            builder.Property(b => b.BatchJobId);

            builder.HasAlternateKey(b => b.JobId);
        }
    }
}
