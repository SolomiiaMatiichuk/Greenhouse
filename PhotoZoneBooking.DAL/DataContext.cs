using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PhotoZoneBooking.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PhotoZoneBooking.DAL.Constraints;

namespace PhotoZoneBooking.DAL;

public class DataContext : IdentityDbContext<User>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options) { }
    
    public virtual DbSet<PhotoZoneDetail> PhotoZoneDetails { get; set; }
    public virtual DbSet<PhotoZone> PhotoZones { get; set; }
    public virtual DbSet<Reservation> Reservations { get; set; }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //seed admin role
        builder.Entity<IdentityRole>().HasData(new IdentityRole { 
            Name = SystemRoleConstraints.AdminRole, 
            NormalizedName = SystemRoleConstraints.AdminRole.ToUpper(), 
            Id = SystemRoleConstraints.AdminRoleId,
            ConcurrencyStamp = SystemRoleConstraints.AdminRoleId
        });
        
        //seed user role
        builder.Entity<IdentityRole>().HasData(new IdentityRole { 
            Name = SystemRoleConstraints.UserRole, 
            NormalizedName = SystemRoleConstraints.UserRole.ToUpper(), 
            Id = SystemRoleConstraints.UserRoleId,
            ConcurrencyStamp = SystemRoleConstraints.UserRoleId
        });

        //create user
        var user = new User { 
            Id = SystemRoleConstraints.AdminId,
            Email = "admin@gmail.com",
            NormalizedEmail = "admin@gmail.com".ToUpper(),
            EmailConfirmed = true, 
            FirstName = "admin",
            LastName = "admin",
            UserName = "admin@gmail.com",
            NormalizedUserName = "admin@gmail.com".ToUpper()
        };

        //set user password
        var passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, "Qwerty1-");

        //seed user
        builder.Entity<User>().HasData(user);

        //set user role to admin
        builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string> {
            RoleId = SystemRoleConstraints.AdminRoleId, 
            UserId = SystemRoleConstraints.AdminId 
        });
    }
}