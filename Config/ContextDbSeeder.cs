using Jornadas_Metalurgia_2026.Models.Role;
using Jornadas_Metalurgia_2026.Models.User;
using Jornadas_Metalurgia_2026.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace Jornadas_Metalurgia_2026.Config
{
    public static class ContextDbSeeder
    {
        public static async Task SeedAdminUser(ApplicationDbContext context, IConfiguration config, IEncoderServices encoderService)
        {

            var userName = config["AdminData:UserName"];
            var email = config["AdminData:Email"];
            var password = config["AdminData:Password"];

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return;
            }

            var adminRole = await context.Roles.FirstOrDefaultAsync(r => r.Name == "Admin");
            if (adminRole == null) return;

            var adminExists =  await context.Set<UserRoles>().AnyAsync(ur => ur.RoleId == adminRole.Id);
          

            if (!adminExists)
            {
                
                    var hashedPassword = encoderService.Encode(password);


                    var defaultAdmin = new User
                    {
                        UserName = userName,
                        Email = email,
                        Password = hashedPassword,

                    };
                    await context.Users.AddAsync(defaultAdmin);
                    await context.SaveChangesAsync();
                
              
              
                        await context.Set<UserRoles>().AddAsync(new UserRoles
                        {
                            UserId = defaultAdmin.Id,
                            RoleId = adminRole.Id
                        });
                        await context.SaveChangesAsync();
            }
                

        }
    }
}
