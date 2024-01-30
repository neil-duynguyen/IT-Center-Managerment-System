using KidProEdu.Application.Interfaces;

namespace KidProEdu.Application.Services
{
    public class CurrentTime : ICurrentTime
    {
         public DateTime GetCurrentTime()
        {
            TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
            return vietnamTime;
        }
    }
}
