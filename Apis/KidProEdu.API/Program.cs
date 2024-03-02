using Microsoft.EntityFrameworkCore;
using KidProEdu.Infrastructures;
using KidProEdu.Application.Repositories;
using KidProEdu.Infrastructures.Repositories;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Services;
using Infrastructures.Repositories;
using KidProEdu.Application;
using Microsoft.OpenApi.Models;
using Infrastructures;
using KidProEdu.Application.IRepositories;
using KidProEdu.Application.Hubs;

namespace KidProEdu.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddSignalR();

            //CORS
            var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
            builder.Services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  policy =>
                                  {
                                      policy.WithOrigins("http://localhost:5173",
                                                        "http://127.0.0.1:5173",
                                                        "https://kid-pro-edu.netlify.app"
                                                          )
                                                            //.AllowAnyOrigin()
                                                            .AllowAnyHeader()
                                                            .AllowAnyMethod()
                                                            .AllowCredentials();

                                  });
            });

            #region Configtoken
            var secretKey = builder.Configuration["AppSettings:SecretKey"];
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            builder.Services.AddAuthentication
                (JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        //tu cap token 
                        ValidateIssuer = false,
                        ValidateAudience = false,

                        //ky vao token
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),
                        ClockSkew = TimeSpan.Zero
                    };
                });


            //Config Swagger
            builder.Services.AddSwaggerGen(options =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Type = SecuritySchemeType.ApiKey,
                    Description = "Put \"Bearer {token}\" your JWT Bearer token on textbox below!",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    },
                };
                options.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, jwtSecurityScheme);
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
            {
                {
                    jwtSecurityScheme,
                    new List<string>()
                }
            });
            });
            #endregion

            //Connection DB
            builder.Services.AddDbContext<AppDbContext>(options =>
                    options.UseSqlServer(builder.Configuration.GetConnectionString("Development"),
                        builder => builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName)));


            #region DIRepository
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ITagRepository, TagRepository>();
            builder.Services.AddScoped<ILocationRepository, LocationRepository>();
            builder.Services.AddScoped<ICategoryEquipmentRepository, CategoryEquipmentRepository>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<IRoomRepository, RoomRepository>();
            builder.Services.AddScoped<IEquipmentRepository, EquipmentRepository>();
            builder.Services.AddScoped<IBlogRepository, BlogRepository>();
            builder.Services.AddScoped<IChildrenRepository, ChildrenRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();
            builder.Services.AddScoped<INotificationUserRepository, NotificationUserRepository>();
            builder.Services.AddScoped<IRatingRepository, RatingRepository>();
            builder.Services.AddScoped<IDivisionRepository, DivisionRepository>();
            builder.Services.AddScoped<ILessonRepository, LessonRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<ICourseRepository, CourseRepository>();
            builder.Services.AddScoped<ISemesterCourseRepository, SemesterCourseRepository>();
            builder.Services.AddScoped<IRequestRepository, RequestRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();
            builder.Services.AddScoped<ILogEquipmentRepository, LogEquipmentRepository>();
            #endregion

            #region DIService
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IClaimsService, ClaimsService>();
            builder.Services.AddSingleton<ICurrentTime, CurrentTime>();
            builder.Services.AddScoped<ITagService, TagService>();
            builder.Services.AddScoped<ILocationService, LocationService>();
            builder.Services.AddScoped<ICategoryEquipmentService, CategoryEquipmentService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<IRoomService, RoomService>();
            builder.Services.AddScoped<IEquipmentService, EquipmentService>();
            builder.Services.AddScoped<IBlogService, BlogService>();
            builder.Services.AddScoped<IChildrenService, ChildrenService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<INotificationUserService, NotificationUserService>();
            builder.Services.AddScoped<IRatingService, RatingService>();
            builder.Services.AddScoped<IDivisionService, DivisionService>();
            builder.Services.AddScoped<ILessonService, LessonService>();
            builder.Services.AddScoped<ICourseService, CourseService>();
            builder.Services.AddScoped<IQuestionService, QuestionService>();
            builder.Services.AddScoped<ISemesterCourseService, SemesterCourseService>();
            builder.Services.AddScoped<IRequestService, RequestService>();
            builder.Services.AddScoped<IClassService, ClassService>();
            builder.Services.AddScoped<IDocumentService, DocumentService>();
            builder.Services.AddScoped<ILogEquipmentService, LogEquipmentService>();
            #endregion

            builder.Services.AddAutoMapper(typeof(Program));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    //c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
                    c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                });
            }

            if (app.Environment.IsProduction())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "KidProEdu API v1");
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseCors(MyAllowSpecificOrigins);

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseStaticFiles();

            app.MapHub<NotificationHub>("/notificationHub");
            /*app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<NotificationHub>("/notificationHub");
            });*/

            app.MapControllers();

            app.Run();
        }
    }
}