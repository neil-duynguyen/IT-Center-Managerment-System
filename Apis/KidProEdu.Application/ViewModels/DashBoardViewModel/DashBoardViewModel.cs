using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.DashBoardViewModel
{
    public class DashBoardViewModel
    {
        public double TotalAmountTransaction { get; set; }
        public double TotalCommission {  get; set; }
        public int TotalStaff {  get; set; }
        public int TotalParent { get; set; }
        public int TotalManager { get; set; }
        public int TotalChildren { get; set; }
        public int TotalCourse {  get; set; }
        public int TotalTeacher { get; set; }
        public List<DashBoardTransactionSummariseByMonthViewModel> dashBoardTransactionSummariseByMonthViewModels { get; set; }
        public List<DashBoardTransactionSummariseByCourseViewModel> dashBoardTransactionSummariseByCourseViewModels { get; set; }
    }
}
