using Jornadas_Metalurgia_2026.Models.Inscription;
using Jornadas_Metalurgia_2026.Models.Role;
using Jornadas_Metalurgia_2026.Models.User;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;

namespace Jornadas_Metalurgia_2026.Config
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Inscription> Inscriptions { get; set; }
        public DbSet<AttendanceInscription> AttendanceInscriptions { get; set; }
        public DbSet<Inscription> PresentationInscriptions{ get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique();
            modelBuilder.Entity<User>().HasIndex(x => x.Email).IsUnique();
            modelBuilder.Entity<Role>().HasIndex(x => x.Name).IsUnique();
            modelBuilder.Entity<Inscription>()
                .HasDiscriminator<string>("Discriminator")
                .HasValue<Inscription>("Inscription")
                .HasValue<AttendanceInscription>("AttendanceInscription")
                .HasValue<PresentationInscription>("PresentationInscription");

            modelBuilder.Entity<User>()
            .HasMany(x => x.Roles)
            .WithMany()
            .UsingEntity<UserRoles>(
                l => l.HasOne<Role>().WithMany().HasForeignKey(x => x.RoleId),
                r => r.HasOne<User>().WithMany().HasForeignKey(x => x.UserId)



                );

            modelBuilder.Entity<Role>().HasData(

                new Role { Id = 1, Name = "Admin" },
                new Role { Id = 2, Name = "User" }

                );
            

        }

    }
}
