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
        public DbSet<UserAccount> Users { get; set; }
        public DbSet<AdviseRequest> AdviseRequests { get; set; }
        public DbSet<AnnualWorkingDay> AnnualWorkingDays { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<CategoryEquipment> CategoryEquipment { get; set; }
        public DbSet<Certificate> Certificates { get; set; }
        public DbSet<ChildrenProfile> Childrens { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<ConfigDay> ConfigDays { get; set; }
        public DbSet<ConfigJobType> ConfigJobTypes { get; set; }
        public DbSet<ConfigSystem> ConfigSystems { get; set; }
        public DbSet<ConfigTheme> ConfigThemes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }
        public DbSet<Equipment> Equipment { get; set; }
        public DbSet<Feedback> Feedbacks { get;set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<Lesson> Lessons { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<LocationTrainingProgram> LocationTrainingPrograms { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<NotificationUser> NotificationUsers { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<ScheduleRoom> ScheduleRooms { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<Semester> Semesters { get; set; }
        public DbSet<SemesterCourse> SemesterCourses { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TestTime> TestTimes { get; set; }
        public DbSet<TrainingProgram> TrainingPrograms { get; set; }
        public DbSet<TrainingProgramCategory> TrainingProgramCategories { get; set; }
        public DbSet<TrainingProgramCourse> TrainingProgramCourses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Installment> Installments { get; set; }

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

            builder.Entity<Course>()
               .HasOne(p => p.Certificate)
               .WithOne(x => x.Course)
               .HasForeignKey<Certificate>(x => x.CourseId);

            builder.Entity<TrainingProgram>()
               .HasOne(p => p.Certificate)
               .WithOne(x => x.TrainingProgram)
               .HasForeignKey<Certificate>(x => x.TrainingProgramId);

            builder.Entity<Blog>()
                .HasMany(p => p.Tags)
                .WithMany(x => x.Blogs);

            builder.Entity<Tag>()
                .HasMany(p => p.Blogs)
                .WithMany(x => x.Tags);
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
