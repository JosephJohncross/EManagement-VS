using Microsoft.AspNetCore.Identity;

namespace EManagementVSA.Data;

public static class SeedData
{
    public static void Initialize (IServiceProvider serviceProvider) {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        AddRole(roleManager, "Admin");
        AddRole(roleManager, "HR");
        AddRole(roleManager, "DepartmentHead");
        AddRole(roleManager, "Employee");
    }

    private static void AddRole(RoleManager<IdentityRole> roleManager, string roleName)
    {
        if (!roleManager.RoleExistsAsync(roleName).Result)
        {
            var role = new IdentityRole { Name = roleName };
            roleManager.CreateAsync(role).Wait();
        }
    }
}