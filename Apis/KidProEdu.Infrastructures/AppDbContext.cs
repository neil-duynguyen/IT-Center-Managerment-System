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
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"), Name = "Admin", CreationDate = new DateTime(2024 - 01 - 15)},
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A51"), Name = "Manager", CreationDate = new DateTime(2024 - 01 - 15)},
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A52"), Name = "Staff", CreationDate = new DateTime(2024 - 01 - 15) },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A53"), Name = "Teacher", CreationDate = new DateTime(2024 - 01 - 15) },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A54"), Name = "Parent", CreationDate = new DateTime(2024 - 01 - 15) },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A55"), Name = "Children", CreationDate = new DateTime(2024 - 01 - 15) }
               );


            var userAdmin = "Admin";
            var userManager = "Manager";
            var userStaff = "Staff";

            var hashPasswordAdmin = "Admin@123";
            var hashPasswordManager = "Manager@123";
            var hashPasswordStaff = "Staff@123";
            var hashPasswordTeacher = "Teacher@123";                     
            var hashPasswordParent = "Parent@123";
            var hashPasswordChildren = "Children@123";

            builder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"),
                    UserName = userAdmin,
                    PasswordHash = hashPasswordAdmin.Hash(),
                    FullName = "NguyenVanDuy",
                    Email = "duynguyen@gmail.com",
                    Phone = "0975844775",
                    DateOfBirth = new DateTime(1997 - 01 - 20),
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

            builder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca83"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A51"),
                    UserName = userManager,
                    PasswordHash = hashPasswordManager.Hash(),
                    FullName = "HoangQuocDung",
                    Email = "dunghoang@gmail.com",
                    Phone = "0975844796",
                    DateOfBirth = new DateTime(1999 - 04 - 10),
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

            builder.Entity<User>().HasData(
                new User
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca84"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A52"),
                    UserName = userStaff,
                    PasswordHash = hashPasswordStaff.Hash(),
                    FullName = "LinhChi",
                    Email = "linhchi@gmail.com",
                    Phone = "0356724796",
                    DateOfBirth = new DateTime(2001 - 11 - 11),
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

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
