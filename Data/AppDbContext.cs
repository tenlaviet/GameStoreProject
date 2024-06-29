using AspMVC.Models;
using AspMVC.Models.EF;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Reflection.Metadata;

namespace AspMVC.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            base.OnConfiguring(builder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasMany(p => p.Projects).WithOne(c => c.Creator).HasForeignKey(c => c.CreatorId).OnDelete(DeleteBehavior.Restrict);
                entity.HasMany(c => c.Comments).WithOne(a => a.Author).HasForeignKey(a => a.AuthorId).OnDelete(DeleteBehavior.Restrict);

            });
            modelBuilder.Entity<ProjectPageModel>(entity =>
            {
                entity.HasIndex(p => p.Slug);
                entity.HasMany(c => c.Comments).WithOne(p => p.ProjectPage).HasForeignKey(p => p.ProjectPageId).OnDelete(DeleteBehavior.Cascade);
            });



        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Platform> Platform { get; set; }
        public DbSet<ProjectPageModel> ProjectPages { get; set; }
        public DbSet<ProjectUploadedPicture> ProjectUploadedPicture { get; set; }
        public DbSet<ProjectUploadedFile> ProjectUploadedFile { get; set; }
        public DbSet<ProjectUploadedCoverImage> ProjectUploadedCoverImage { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Comment> Comments { get; set; }
        //public DbSet<ProjectModel> ProjectModels { get; set; }

        //public async Task<IActionResult> SeedDataAsync;
        //Add-Migration ""
        //Update-Database ""
        //Remove-Migration ""

    }
}
