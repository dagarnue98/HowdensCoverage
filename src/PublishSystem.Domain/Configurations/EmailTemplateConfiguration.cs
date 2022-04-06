using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PublishSystem.Domain.Entities;

namespace PublishSystem.Domain.Configurations
{
    internal class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
    {
        public void Configure(EntityTypeBuilder<EmailTemplate> builder)
        {
            builder.ToTable(nameof(EmailTemplate));

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Code);
            builder.Property(b => b.Name);
            builder.Property(b => b.Translate);
            builder.Property(b => b.CustomTemplate);
        }
    }
}
