using KidProEdu.Application.Interfaces;

namespace KidProEdu.Application.Services
{
    public class CurrentTime : ICurrentTime
    {
        public DateTime GetCurrentTime() => DateTime.UtcNow;
    }
}
