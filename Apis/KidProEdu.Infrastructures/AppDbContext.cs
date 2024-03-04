using KidProEdu.Application.Utils;
using KidProEdu.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Reflection.Emit;

namespace KidProEdu.Infrastructures
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() { }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Role> Role { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<SkillTag> SkillTag { get; set; }
        public DbSet<AdviseRequest> AdviseRequest { get; set; }
        public DbSet<AnnualWorkingDay> AnnualWorkingDay { get; set; }
        public DbSet<Attendance> Attendance { get; set; }
        public DbSet<Blog> Blog { get; set; }
        public DbSet<CategoryEquipment> CategoryEquipment { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<ChildrenProfile> ChildrenProfile { get; set; }
        public DbSet<Class> Class { get; set; }
        public DbSet<ConfigDay> ConfigDay { get; set; }
        public DbSet<ConfigJobType> ConfigJobType { get; set; }
        public DbSet<ConfigSystem> ConfigSystem { get; set; }
        public DbSet<ConfigTheme> ConfigTheme { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Course> Course { get; set; }
        public DbSet<Enrollment> Enrollment { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<LogEquipment> LogEquipment { get; set; }
        public DbSet<Feedback> Feedback { get;set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Division> Division { get; set; }
        public DbSet<Lesson> Lesson { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationUser> NotificationUser { get; set; }
        public DbSet<Question> Question { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<Room> Room { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<ScheduleRoom> ScheduleRoom { get; set; }
        public DbSet<Score> Score { get; set; }
        public DbSet<Semester> Semester { get; set; }
        public DbSet<SemesterCourse> SemesterCourse { get; set; }
        public DbSet<Tag> Tag { get; set; }
        public DbSet<TestTime> TestTime { get; set; }   
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<RequestUserAccount> RequestUserAccount { get; set; }
        public DbSet<DivisionUserAccount> DivisionUserAccount { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"), Name = "Admin", CreationDate = new DateTime(2024 - 01 - 15)},
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A51"), Name = "Manager", CreationDate = new DateTime(2024 - 01 - 15)},
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A52"), Name = "Staff", CreationDate = new DateTime(2024 - 01 - 15) },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A53"), Name = "Teacher", CreationDate = new DateTime(2024 - 01 - 15) },
               new Role { Id = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A54"), Name = "Parent", CreationDate = new DateTime(2024 - 01 - 15) });


            var userAdmin = "Admin";
            var userManager = "Manager";
            var userStaff = "Staff";

            var hashPasswordAdmin = "Admin@123";
            var hashPasswordManager = "Manager@123";
            var hashPasswordStaff = "Staff@123";
            var hashPasswordTeacher = "Teacher@123";                     
            var hashPasswordParent = "Parent@123";

            builder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca82"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"),
                    UserName = userAdmin,
                    PasswordHash = hashPasswordAdmin.Hash(),
                    FullName = "NguyenVanDuy",
                    Email = "duynguyen@gmail.com",
                    Phone = "0975844775",
                    DateOfBirth = new DateTime(1997 - 01 - 20),
                    Status = Domain.Enums.StatusUser.Enable,
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

            builder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca83"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A51"),
                    UserName = userManager,
                    PasswordHash = hashPasswordManager.Hash(),
                    FullName = "HoangQuocDung",
                    Email = "dunghoang@gmail.com",
                    Phone = "0975844796",
                    DateOfBirth = new DateTime(1999 - 04 - 10),
                    Status = Domain.Enums.StatusUser.Enable,
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

            builder.Entity<UserAccount>().HasData(
                new UserAccount
                {
                    Id = new Guid("434d275c-ff7d-48fa-84e3-bed5ecadca84"),
                    RoleId = new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A52"),
                    UserName = userStaff,
                    PasswordHash = hashPasswordStaff.Hash(),
                    FullName = "LinhChi",
                    Email = "linhchi@gmail.com",
                    Phone = "0356724796",
                    DateOfBirth = new DateTime(2001 - 11 - 11),
                    Status = Domain.Enums.StatusUser.Enable,
                    CreationDate = new DateTime(2024 - 01 - 15),
                });

            builder.Entity<Slot>().HasData(
                new Slot
                {
                    Id = new Guid("85dd1d22-518b-4949-bf99-46d173caf3fe"),
                    Name = "Slot1",
                    StartTime = new TimeSpan(7, 0, 0),
                    EndTime = new TimeSpan(9, 15, 0)
                },
                new Slot
                {
                    Id = new Guid("dd5e5665-6522-4625-8d87-a807d856a318"),
                    Name = "Slot2",
                    StartTime = new TimeSpan(9, 30, 0),
                    EndTime = new TimeSpan(11, 45, 0)
                },
                new Slot
                {
                    Id = new Guid("d42e7c30-ffd4-4a09-ac73-1d175f89a200"),
                    Name = "Slot3",
                    StartTime = new TimeSpan(12, 30, 0),
                    EndTime = new TimeSpan(14, 45, 0)
                },
                new Slot
                {
                    Id = new Guid("2758033d-7eef-41d6-a6ca-b9586fe3749d"),
                    Name = "Slot4",
                    StartTime = new TimeSpan(15, 0, 0),
                    EndTime = new TimeSpan(17, 15, 0)
                },
                new Slot
                {
                    Id = new Guid("2c22f784-57ee-476c-a630-7c080b721db5"),
                    Name = "Slot5",
                    StartTime = new TimeSpan(19, 00, 0),
                    EndTime = new TimeSpan(21, 15, 0)
                }
                );

            builder.Entity<Enrollment>()
                .HasOne(x => x.Class)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.ClassId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            
            builder.Entity<Enrollment>()
                .HasOne(x => x.ChildrenProfile)
                .WithMany(x => x.Enrollments)
                .HasForeignKey(x => x.ChildrenProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ScheduleRoom>()
                .HasOne(x => x.Room)
                .WithMany(x => x.ScheduleRooms)
                .HasForeignKey(x => x.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ScheduleRoom>()
                .HasOne(x => x.Schedule)
                .WithMany(x => x.ScheduleRooms)
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull);        

            builder.Entity<ChildrenCertificate>()
                .HasOne(x => x.ChildrenProfile)
                .WithMany(x => x.ChildrenCertificates)
                .HasForeignKey(x => x.ChildrenProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<ChildrenCertificate>()
                .HasOne(x => x.Certificate)
                .WithMany(x => x.ChildrenCertificates)
                .HasForeignKey(x => x.CertificateId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Attendance>()
                .HasOne(x => x.Schedule)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.ScheduleId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Attendance>()
                .HasOne(x => x.ChildrenProfile)
                .WithMany(x => x.Attendances)
                .HasForeignKey(x => x.ChildrenProfileId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<RequestUserAccount>()
                .HasOne(x => x.UserAccount)
                .WithMany(x => x.RequestUserAccounts)
                .HasForeignKey(x => x.RecieverId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<RequestUserAccount>()
                .HasOne(x => x.Request)
                .WithMany(x => x.RequestUserAccounts)
                .HasForeignKey(x => x.RequestId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Course>()
               .HasOne(p => p.Certificate)
               .WithOne(x => x.Course)
               .HasForeignKey<Certificate>(x => x.CourseId);

            builder.Entity<Slot>()
               .HasOne(p => p.Schedule)
               .WithOne(x => x.Slot)
               .HasForeignKey<Schedule>(x => x.SlotId);

            builder.Entity<Blog>()
                .HasMany(p => p.Tags)
                .WithMany(x => x.Blogs);

            builder.Entity<Tag>()
                .HasMany(p => p.Blogs)
                .WithMany(x => x.Tags);

            /*builder.Entity<UserAccount>()
                .HasMany(p => p.Divisions)
                .WithMany(x => x.UserAccounts);

            builder.Entity<Division>()
                .HasMany(p => p.UserAccounts)
                .WithMany(x => x.Divisions);*/

            /*builder.Entity<Course>()
                .Property(c => c.ParentCourse)
                .HasConversion(
                v => JsonConvert.SerializeObject(v), // Chuyển đổi ICollection<Guid>? thành chuỗi JSON khi lưu vào cơ sở dữ liệu
                v => JsonConvert.DeserializeObject<ICollection<Guid>>(v)); // Chuyển đổi chuỗi JSON thành ICollection<Guid>? khi đọc từ cơ sở dữ liệu
                
            builder.Entity<Transaction>()
               .Property(c => c.ParentTransaction)
               .HasConversion(
               v => JsonConvert.SerializeObject(v), // Chuyển đổi ICollection<Guid>? thành chuỗi JSON khi lưu vào cơ sở dữ liệu
               v => JsonConvert.DeserializeObject<ICollection<Guid>>(v)); // Chuyển đổi chuỗi JSON thành ICollection<Guid>? khi đọc từ cơ sở dữ liệu*/
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "..", "KidProEdu.API"))
                    .AddJsonFile("appsettings.json")
                    .Build();

                string connectionString = configuration.GetConnectionString("Development");
                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        
    }
}
