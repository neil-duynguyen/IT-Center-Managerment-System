using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.DashBoardViewModel
{
    public class DashBoardTransactionSummariseByCourseViewModel
    {
        public string CourseCode {  get; set; }
        public double TotalAmountCourse {  get; set; }
        public double Percent {  get; set; }
        
    }
}
