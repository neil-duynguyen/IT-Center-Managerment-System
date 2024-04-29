using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.DashBoardViewModel;
using KidProEdu.Application.ViewModels.TransactionViewModels;
using KidProEdu.Application.ViewModels.UserViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public TransactionService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<List<TransactionViewModel>> GetAllTransaction()
        {
            var listTransaction = _unitOfWork.TransactionRepository.GetAllAsync().Result.Where(x => x.OrderDetail is not null);

            var getCurrentUserId = _unitOfWork.UserRepository.GetByIdAsync(_claimsService.GetCurrentUserId).Result.Role.Name;

            List<Transaction> transaction = new List<Transaction>();

            if (getCurrentUserId.Equals("Admin") || getCurrentUserId.Equals("Manager"))
            {
                transaction = listTransaction.Where(x => x.ParentsTransaction is null).ToList();
            }

            if (getCurrentUserId.Equals("Staff"))
            {
                transaction = listTransaction.Where(x => x.OrderDetail.Order.CreatedBy == _claimsService.GetCurrentUserId && x.ParentsTransaction is null).ToList();
            }

            if (getCurrentUserId.Equals("Parent"))
            {
                transaction = listTransaction.Where(x => x.OrderDetail.Order.UserId == _claimsService.GetCurrentUserId && x.ParentsTransaction is null).ToList();
            }

            return _mapper.Map<List<TransactionViewModel>>(transaction);
        }

        public async Task<DashBoardViewModel> GetDashBoards(DateTime startDate, DateTime endDate)
        {
            try
            {
              
                var getTransactionsTotalAmount = await _unitOfWork.TransactionRepository.GetTransactionsTotalAmount(startDate, endDate);

                var getCommissionsTotalAmount = await _unitOfWork.EnrollmentRepository.GetCommissionEnrollmentsTotalAmount(startDate, endDate);
               
                var getTotalChildrens = await _unitOfWork.ChildrenRepository.GetTotalChildrens(startDate, endDate);
              
                var getTotalParents = await _unitOfWork.UserRepository.GetTotalParents(startDate, endDate);
              
                var getTotalManagers = await _unitOfWork.UserRepository.GetTotalManagers(startDate, endDate);
               
                var getTotalStaffs = await _unitOfWork.UserRepository.GetTotalStaffs(startDate, endDate);
                
                var getTotalTeachers = await _unitOfWork.UserRepository.GetTotalTeachers(startDate, endDate);
              
                var getTotalCourses = await _unitOfWork.CourseRepository.GetTotalCourses(startDate, endDate);      
                var transactionByCourses = new List<DashBoardTransactionSummariseByCourseViewModel>();
                var transactionsByCourses = await _unitOfWork.TransactionRepository.GetTransactionsByCourse(startDate, endDate);
                double totalAmountForCourses = transactionsByCourses.Sum(t => t.TotalAmount ?? 0);
             
                var courses = await _unitOfWork.CourseRepository.GetAllAsync();
               
                foreach (var course in courses)
                {
                    var transactionsForCourse = transactionsByCourses.Where(t => t.OrderDetail != null &&  t.OrderDetail.CourseId == course.Id).ToList();

             
                    double totalAmountForCourse = transactionsForCourse.Sum(t => t.TotalAmount ?? 0);
                    double percent = totalAmountForCourse == 0 ? 0 : Math.Round(totalAmountForCourse / totalAmountForCourses * 100, 2);
               
                    var transactionByCourse = new DashBoardTransactionSummariseByCourseViewModel
                    {
                        CourseCode = course.CourseCode,
                        TotalAmountCourse = totalAmountForCourse,
                        Percent = percent
                    };

                    transactionByCourses.Add(transactionByCourse);
                }

          
                var transactionByMonths = new List<DashBoardTransactionSummariseByMonthViewModel>();
              

                var dateStart = new DateTime(startDate.Year, startDate.Month, startDate.Day);
                var dateEnd = new DateTime(endDate.Year, endDate.Month, endDate.Day);
                while (dateStart <= dateEnd)
                {

                    var endOfMonth = new DateTime(dateStart.Year, dateStart.Month, DateTime.DaysInMonth(dateStart.Year, dateStart.Month));

                    if (dateStart.Year == endDate.Year && dateStart.Month == endDate.Month)
                    {
                        endOfMonth = endDate; 
                    }

                    var transactionsForMonth = await _unitOfWork.TransactionRepository.GetTransactionsByMonth(dateStart, endOfMonth);
                   
                    var totalAmountForMonth = transactionsForMonth.Sum(t => t.TotalAmount ?? 0);
                   
                    transactionByMonths.Add(new DashBoardTransactionSummariseByMonthViewModel
                    {
                        MonthAndYear = $"{dateStart.Month}/{dateStart.Year}",
                        TotalAmountOfMonthInYear = totalAmountForMonth
                    });

                    dateStart = endOfMonth.AddDays(1);
                }


                var dashboard = new DashBoardViewModel
                {
                    TotalAmountTransaction = getTransactionsTotalAmount,
                    TotalCommission = getCommissionsTotalAmount,
                    TotalChildren = getTotalChildrens,
                    TotalCourse = getTotalCourses,
                    TotalManager = getTotalManagers,
                    TotalParent = getTotalParents,
                    TotalStaff = getTotalStaffs,
                    TotalTeacher = getTotalTeachers,
                    dashBoardTransactionSummariseByCourseViewModels = transactionByCourses,
                    dashBoardTransactionSummariseByMonthViewModels = transactionByMonths,
                };
                return dashboard;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin thống kê: " + ex.Message);
            }
        }

        public async Task<List<TransactionViewModel>> GetTransactionDetailByTransactionId(Guid id)
        {
            var listTransaction = _unitOfWork.TransactionRepository.GetAllAsync().Result.Where(x => x.ParentsTransaction == id).OrderBy(x => x.InstallmentPeriod); ;
            return _mapper.Map<List<TransactionViewModel>>(listTransaction);
        }

        public async Task<List<TransactionByCoursesViewModel>> TransactionByCoursesInYear(DateTime monthInYear)
        {
            try
            {
                var transactionByCourses = new List<TransactionByCoursesViewModel>();

            
                var transactionsInYear = await _unitOfWork.TransactionRepository.GetTransactionByYear(monthInYear);
                double totalAmountForYear = transactionsInYear.Sum(t => t.TotalAmount ?? 0);
           
                var courses = await _unitOfWork.CourseRepository.GetAllAsync();

          
                foreach (var course in courses)
                {
                 
                    var transactionsForCourse = transactionsInYear.Where(t => t.OrderDetail.CourseId == course.Id).ToList();

                   
                    double totalAmountForCourse = transactionsForCourse.Sum(t => t.TotalAmount ?? 0);
                    double percent = totalAmountForCourse == 0 ? 0 : Math.Round(totalAmountForCourse / totalAmountForYear * 100, 2);
            
                    var transactionByCourse = new TransactionByCoursesViewModel
                    {
                        CourseName = course.Name,
                        TotalAmountByCourse = totalAmountForCourse,
                        TotalAmountByYear = totalAmountForYear,
                        Transactions = _mapper.Map<List<TransactionViewModel>>(transactionsForCourse),
                        Percent = percent
                    };

                    transactionByCourses.Add(transactionByCourse);
                }
                return transactionByCourses;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin thống kê: " + ex.Message);
            }
        }

        public async Task<TransactionSummaryViewModel> TransactionsSummarise()
        {
            try
            {
                
                var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
                var mapper = _mapper.Map<List<TransactionViewModel>>(transactions);
                double totalAmount = transactions.Sum(t => t.TotalAmount ?? 0);
                var transactionsSummarise = new TransactionSummaryViewModel
                {
                    Transactions = mapper,
                    TotalAmount = totalAmount
                };

                return transactionsSummarise;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Lỗi lấy danh sách thống kê" + ex.Message);
            }
        }

        public async Task<List<TransactionSummaryByMonthInYearViewModel>> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            try
            {
                var transactionSummaries = new List<TransactionSummaryByMonthInYearViewModel>();

               
                for (int month = 1; month <= monthInYear.Month; month++)
                {
                    var monthStart = new DateTime(monthInYear.Year, month, 1);                 
                    var transactionsInMonth = await _unitOfWork.TransactionRepository.GetTransactionByMonthInYear(monthStart);                   
                    double totalAmountMonth = transactionsInMonth.Sum(t => t.TotalAmount ?? 0);
                    var transactionsInYear = await _unitOfWork.TransactionRepository.GetTransactionByYear(monthStart);

                    double totalAmountYear = transactionsInYear.Sum(t => t.TotalAmount ?? 0);
                 
                    double percent = totalAmountYear == 0 ? 0 : Math.Round(totalAmountMonth / totalAmountYear * 100, 2);
                  
                    var transactionSummary = new TransactionSummaryByMonthInYearViewModel
                    {
                        Month = monthStart.ToString("MMMM yyyy"),
                        Transactions = _mapper.Map<List<TransactionViewModel>>(transactionsInMonth),
                        TotalAmountByYear = totalAmountYear,
                        TotalAmountOfMonthInYear = totalAmountMonth,
                        Percent = percent
                    };

                    transactionSummaries.Add(transactionSummary);
                }

                return transactionSummaries;
            }
            catch (Exception ex)
            {
                throw new Exception("Lỗi khi lấy thông tin thống kê: " + ex.Message);
            }
        }


    }
}
