namespace SCML.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.Entity.ModelConfiguration.Conventions;

    public partial class SCMLModel : DbContext
    {
        public SCMLModel()
            : base("name=SCMLConnectionString")
        {
        }

        public virtual DbSet<Content> Contents { get; set; }
        public virtual DbSet<Type> Types { get; set; }
        public virtual DbSet<JobDetail> JobDetails { get; set; }
        public virtual DbSet<EmailFormModel> EmailFormModels { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Content>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.summary)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.large_image_path)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.thambnail_image_path)
                .IsUnicode(false);

            modelBuilder.Entity<Content>()
                .Property(e => e.content_file_path)
                .IsUnicode(false);

            modelBuilder.Entity<Type>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Type>()
                .HasMany(e => e.Contents)
                .WithRequired(e => e.Type)
                .HasForeignKey(e => e.type_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.details)
                .IsUnicode(false);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.qualification)
                .IsUnicode(false);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.experience)
                .IsUnicode(false);

            modelBuilder.Entity<JobDetail>()
                .Property(e => e.create_by)
                .IsUnicode(false);

            modelBuilder.Entity<EmailFormModel>()
                .Property(e => e.full_name)
                .IsUnicode(false);

            modelBuilder.Entity<EmailFormModel>()
                .Property(e => e.applied_for)
                .IsUnicode(false);

            modelBuilder.Entity<EmailFormModel>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<EmailFormModel>()
                .Property(e => e.file_path)
                .IsUnicode(false);
        }
    }
}

