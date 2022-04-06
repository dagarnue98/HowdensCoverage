using Microsoft.EntityFrameworkCore;
using PublishSystem.Domain.Configurations;
using PublishSystem.Domain.Entities;

namespace PublishSystem.Domain
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<PublishJob> PublishJobs { get; set; }

        public DbSet<EmailTemplate> EmailTemplate { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new PublishJobConfiguration());
            modelBuilder.ApplyConfiguration(new EmailTemplateConfiguration());
        }
    }
}
