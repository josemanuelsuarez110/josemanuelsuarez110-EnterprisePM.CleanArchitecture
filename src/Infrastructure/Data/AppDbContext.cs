using Microsoft.EntityFrameworkCore;
using ProjectManagementERP.Domain.Entities;

namespace ProjectManagementERP.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Project> Projects => Set<Project>();
        public DbSet<TaskItem> Tasks => Set<TaskItem>();
        public DbSet<Milestone> Milestones => Set<Milestone>();
        public DbSet<TimeEntry> TimeEntries => Set<TimeEntry>();
        public DbSet<ResourceAllocation> ResourceAllocations => Set<ResourceAllocation>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => e.Name);
                entity.HasMany(e => e.Tasks).WithOne(t => t.Project).HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.Milestones).WithOne(m => m.Project).HasForeignKey(m => m.ProjectId).OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(e => e.ResourceAllocations).WithOne(r => r.Project).HasForeignKey(r => r.ProjectId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<TaskItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Title).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => new { e.ProjectId, e.Status });
                entity.HasMany(e => e.TimeEntries).WithOne(t => t.Task).HasForeignKey(t => t.TaskId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Milestone>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.HasIndex(e => new { e.ProjectId, e.DueDate });
            });

            modelBuilder.Entity<TimeEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.TaskId, e.Date });
            });

            modelBuilder.Entity<ResourceAllocation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => new { e.ProjectId, e.UserId }).IsUnique();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
