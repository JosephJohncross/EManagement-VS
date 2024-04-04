using EManagementVSA.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public DbSet<Address> Address { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Employee>()
                .HasIndex(b => b.Email)
                .IsUnique();
        modelBuilder.Entity<Organization>()
                .HasIndex(b => b.Email)
                .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasOne<Employee>()
            .WithOne()
            .HasForeignKey<ApplicationUser>(e => e.EmployeeId);

        modelBuilder.Entity<ApplicationUser>()
            .HasOne<Organization>()
            .WithMany()
            .HasForeignKey(e => e.OrganizationId);

        modelBuilder.Entity<Employee>()
            .HasOne(d => d.Department)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.SubDepartments)
            .WithOne(d => d.ParentDepartment)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Organization>()
            .HasMany(d => d.Departments)
            .WithOne(d => d.Organization)
            .HasForeignKey(k => k.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Organization>()
            .HasMany(d => d.Employees)
            .WithOne(d => d.Organization)
            .HasForeignKey(k => k.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Organization>()
            .HasOne(d => d.Address)
            .WithOne(a => a.Organization)
            .HasForeignKey<Address>(a => a.OrganizationId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Employee>()
            .HasOne(d => d.Address)
            .WithOne(a => a.Employee)
            .HasForeignKey<Address>(d => d.EmployeeId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}