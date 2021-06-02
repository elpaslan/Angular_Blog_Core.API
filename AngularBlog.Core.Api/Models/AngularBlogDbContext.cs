using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AngularBlog.Core.Api.Models
{
    public partial class AngularBlogDbContext : DbContext
    {
        public AngularBlogDbContext()
        {
        }

        public AngularBlogDbContext(DbContextOptions<AngularBlogDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<BlogCategory> BlogCategories { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }

       

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Turkish_CI_AS");

            modelBuilder.Entity<Article>(entity =>
            {
                entity.ToTable("Article");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentMain)
                    .IsRequired()
                    .HasColumnName("Content_Main");

                entity.Property(e => e.ContentSummary)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Content_Summary");

                entity.Property(e => e.Picture).HasMaxLength(300);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Publish_Date");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Article_BlogCategory");
            });

            modelBuilder.Entity<BlogCategory>(entity =>
            {
                entity.ToTable("BlogCategory");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CategoryName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ContentMain)
                    .IsRequired()
                    .HasColumnName("Content_Main");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PublishDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Publish_Date");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_Comment_Article");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
