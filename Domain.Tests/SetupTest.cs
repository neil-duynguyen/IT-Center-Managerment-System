using AutoFixture;
using AutoMapper;
using KidProEdu.API.Mappers;
using KidProEdu.Application;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Repositories;
using KidProEdu.Infrastructures;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Domain.Tests
{
    public class SetupTest : IDisposable
    {
        protected readonly IMapper _mapperConfig;
        protected readonly Fixture _fixture;
        protected readonly Mock<IUnitOfWork> _unitOfWorkMock;
        protected readonly Mock<IClaimsService> _claimsServiceMock;
        protected readonly Mock<ICurrentTime> _currentTimeMock;

        protected readonly Mock<IRoleRepository> _roleRepositoryMock;
        protected readonly Mock<IUserRepository> _userRepositoryMock;

        protected readonly AppDbContext _dbContext;

        public SetupTest()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MapperConfigurationsProfile());
            });
            _mapperConfig = mappingConfig.CreateMapper();
            _fixture = new Fixture();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _claimsServiceMock = new Mock<IClaimsService>();
            _currentTimeMock = new Mock<ICurrentTime>();

            _roleRepositoryMock = new Mock<IRoleRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _dbContext = new AppDbContext(options);

            _currentTimeMock.Setup(x => x.GetCurrentTime()).Returns(DateTime.UtcNow);
            _claimsServiceMock.Setup(x => x.GetCurrentUserId).Returns(new Guid("D5FA55C7-315D-4634-9C73-08DBBC3F3A50"));
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}