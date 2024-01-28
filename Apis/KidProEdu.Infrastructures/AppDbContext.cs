using KidProEdu.Application.Utils;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace KidProEdu.Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Role { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"), Name = "Admin", CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A51"), Name = "Staff", CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A52"), Name = "Manager", CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A53"), Name = "Parent", CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A54"), Name = "Children", CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false }
               );

            //434d275c-ff7d-48fa-84e3-bed5ecadca82

            var hashPasswordAdmin = "Admin@123";
            var hashPasswordStaff = "Staff@123";
            var hashPasswordManager = "Manager@123";
            var hashPasswordParent = "Parent@123";
            var hashPasswordChildren = "Children@123";

            builder.Entity<User>().HasData(
                new User { Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82"), RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"), UserName = "DuyNguyen", PasswordHash = hashPasswordAdmin.Hash(), FullName = "Nguyen Van Duy", 
                                GenderType =  "Name", Email = "duynguyen@gmail.com", Phone = "0975844775", DateOfBirth = new DateTime(2010 - 01 - 20), CreationDate = new DateTime(2024 - 01 - 15), IsDeleted = false }
                );

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("Development");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        
    }
}
