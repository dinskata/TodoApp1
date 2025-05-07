using Microsoft.EntityFrameworkCore;
using TodoApp.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<ToDoList> ToDoLists { get; set; }
    public DbSet<TaskItem> TaskItems { get; set; }
    public DbSet<TaskAssignment> TaskAssignments { get; set; }
    public DbSet<ListShare> ListShares { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure many-to-many relationships
        modelBuilder.Entity<TaskAssignment>()
            .HasKey(ta => new { ta.TaskItemId, ta.UserId });
            
        modelBuilder.Entity<ListShare>()
            .HasKey(ls => new { ls.ToDoListId, ls.UserId });
            
        // Configure relationships
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.LastModifiedBy)
            .WithMany()
            .HasForeignKey(t => t.LastModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<ToDoList>()
            .HasOne(t => t.CreatedBy)
            .WithMany()
            .HasForeignKey(t => t.CreatedById)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<ToDoList>()
            .HasOne(t => t.LastModifiedBy)
            .WithMany()
            .HasForeignKey(t => t.LastModifiedById)
            .OnDelete(DeleteBehavior.Restrict);
            
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "admin",
                Password = "adminpass", 
                FirstName = "Admin",
                LastName = "User",
                CreatedAt = DateTime.Now,
                CreatedBy = 1,
                LastModified = DateTime.Now,
                LastModifiedBy = 1
            }
        );
    }
}