using EManagementVSA.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EManagementVSA.Data;

public class EmployeeDbContext : IdentityDbContext<Employee>
{
    public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
    {

    }

    public DbSet<Organization> Organizations { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Department> Departments { get; set; }
    public Address Address { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>()
                .HasIndex(b => b.Email)
                .IsUnique();

        modelBuilder.Entity<Employee>()
            .HasOne(d => d.Department)
            .WithMany(e => e.Employees)
            .HasForeignKey(e => e.DepartmentId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Department>()
            .HasMany(d => d.SubDepartments)
            .WithOne(d => d.ParentDepartment)
            .HasForeignKey(d => d.ParentDepartmentId)
            .OnDelete(DeleteBehavior.Restrict);

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
    }
}