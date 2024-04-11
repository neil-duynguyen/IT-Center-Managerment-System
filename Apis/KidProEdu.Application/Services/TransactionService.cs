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
                //get InCome
                var getTransactionsTotalAmount = await _unitOfWork.TransactionRepository.GetTransactionsTotalAmount(startDate, endDate);
                //getCommission
                var getCommissionsTotalAmount = await _unitOfWork.EnrollmentRepository.GetCommissionEnrollmentsTotalAmount(startDate, endDate);
                //get total chidren
                var getTotalChildrens = await _unitOfWork.ChildrenRepository.GetTotalChildrens(startDate, endDate);
                //totalParent
                var getTotalParents = await _unitOfWork.UserRepository.GetTotalParents(startDate, endDate);
                //totalManager
                var getTotalManagers = await _unitOfWork.UserRepository.GetTotalManagers(startDate, endDate);
                //totalParent
                var getTotalStaffs = await _unitOfWork.UserRepository.GetTotalStaffs(startDate, endDate);
                //totalParent
                var getTotalTeachers = await _unitOfWork.UserRepository.GetTotalTeachers(startDate, endDate);
                //totalCourse
                var getTotalCourses = await _unitOfWork.CourseRepository.GetTotalCourses(startDate, endDate);



                //dashboard course 
                var transactionByCourses = new List<DashBoardTransactionSummariseByCourseViewModel>();
                var transactionsByCourses = await _unitOfWork.TransactionRepository.GetTransactionsByCourse(startDate, endDate);
                double totalAmountForCourses = transactionsByCourses.Sum(t => t.TotalAmount ?? 0);
                // Lấy danh sách các khóa học
                var courses = await _unitOfWork.CourseRepository.GetAllAsync();
                // Lặp qua từng khóa học
                foreach (var course in courses)
                {
                    // Lọc các giao dịch của khóa học trong năm
                    var transactionsForCourse = transactionsByCourses.Where(t => t.OrderDetail.CourseId == course.Id).ToList();

                    // Tính tổng số tiền của các giao dịch của khóa học trong năm
                    double totalAmountForCourse = transactionsForCourse.Sum(t => t.TotalAmount ?? 0);
                    double percent = totalAmountForCourse == 0 ? 0 : Math.Round(totalAmountForCourse / totalAmountForCourses * 100, 2);
                    // Tạo view model chứa thông tin giao dịch cho khóa học
                    var transactionByCourse = new DashBoardTransactionSummariseByCourseViewModel
                    {
                        CourseName = course.Name,
                        TotalAmountCourse = totalAmountForCourse,
                        Percent = percent
                    };

                    transactionByCourses.Add(transactionByCourse);
                }

                //transactionByMonths
                var transactionByMonths = new List<DashBoardTransactionSummariseByMonthViewModel>();
                /*var transactionsByMonths = await _unitOfWork.TransactionRepository.GetTransactionsByMonth(startDate, endDate);
                double totalAmountForMonths = transactionsByMonths.Sum(t => t.TotalAmount ?? 0);*/

                var currentMonth = new DateTime(startDate.Year, startDate.Month, startDate.Day);
                var endMonth = new DateTime(endDate.Year, endDate.Month, 1);
                while (currentMonth <= endMonth)
                {
                    // Tạo một thời điểm cuối tháng
                    var endOfMonth = currentMonth.AddMonths(1).AddDays(-1);
                    // Lấy thông tin giao dịch cho tháng hiện tại
                    var transactionsForMonth = await _unitOfWork.TransactionRepository.GetTransactionsByMonth(currentMonth, endOfMonth);
                    // Tính tổng số tiền giao dịch cho tháng này
                    var totalAmountForMonth = transactionsForMonth.Sum(t => t.TotalAmount ?? 0);
                    // Thêm thông tin vào danh sách transactionByMonths
                    transactionByMonths.Add(new DashBoardTransactionSummariseByMonthViewModel
                    {
                        MonthAndYear = $"{currentMonth.Month}/{currentMonth.Year}",
                        TotalAmountOfMonthInYear = totalAmountForMonth
                    });
                    // Chuyển sang tháng tiếp theo
                    currentMonth = currentMonth.AddMonths(1);
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
            var listTransaction = _unitOfWork.TransactionRepository.GetAllAsync().Result.Where(x => x.ParentsTransaction == id);
            return _mapper.Map<List<TransactionViewModel>>(listTransaction);
        }

        public async Task<List<TransactionByCoursesViewModel>> TransactionByCoursesInYear(DateTime monthInYear)
        {
            try
            {
                var transactionByCourses = new List<TransactionByCoursesViewModel>();

                // Lấy danh sách các giao dịch trong năm
                var transactionsInYear = await _unitOfWork.TransactionRepository.GetTransactionByYear(monthInYear);
                double totalAmountForYear = transactionsInYear.Sum(t => t.TotalAmount ?? 0);
                // Lấy danh sách các khóa học
                var courses = await _unitOfWork.CourseRepository.GetAllAsync();

                // Lặp qua từng khóa học
                foreach (var course in courses)
                {
                    // Lọc các giao dịch của khóa học trong năm
                    var transactionsForCourse = transactionsInYear.Where(t => t.OrderDetail.CourseId == course.Id).ToList();

                    // Tính tổng số tiền của các giao dịch của khóa học trong năm
                    double totalAmountForCourse = transactionsForCourse.Sum(t => t.TotalAmount ?? 0);
                    double percent = totalAmountForCourse == 0 ? 0 : Math.Round(totalAmountForCourse / totalAmountForYear * 100, 2);
                    // Tạo view model chứa thông tin giao dịch cho khóa học
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
                // Lấy danh sách các giao dịch từ Repository
                var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
                var mapper = _mapper.Map<List<TransactionViewModel>>(transactions);

                // Tính tổng số tiền của các giao dịch
                double totalAmount = transactions.Sum(t => t.TotalAmount ?? 0);

                // Tạo view model chứa thông tin tổng hợp
                var transactionsSummarise = new TransactionSummaryViewModel
                {
                    Transactions = mapper,
                    TotalAmount = totalAmount
                };

                return transactionsSummarise;
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, trả về null hoặc xử lý tùy thuộc vào yêu cầu của bạn
                throw new Exception("Lỗi lấy danh sách thống kê" + ex.Message);
            }
        }

        public async Task<List<TransactionSummaryByMonthInYearViewModel>> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            try
            {
                var transactionSummaries = new List<TransactionSummaryByMonthInYearViewModel>();

                // Lặp qua từ tháng 1 đến tháng trước tháng hiện tại
                for (int month = 1; month <= monthInYear.Month; month++)
                {
                    var monthStart = new DateTime(monthInYear.Year, month, 1);

                    // Lấy danh sách các giao dịch trong tháng
                    var transactionsInMonth = await _unitOfWork.TransactionRepository.GetTransactionByMonthInYear(monthStart);

                    // Tính tổng số tiền của các giao dịch trong tháng
                    double totalAmountMonth = transactionsInMonth.Sum(t => t.TotalAmount ?? 0);

                    // Tính tổng số tiền của tất cả các giao dịch từ đầu năm đến thời điểm hiện tại
                    var transactionsInYear = await _unitOfWork.TransactionRepository.GetTransactionByYear(monthStart);

                    double totalAmountYear = transactionsInYear.Sum(t => t.TotalAmount ?? 0);

                    // Tính phần trăm số tiền trong tháng so với tổng số tiền của tất cả các giao dịch
                    double percent = totalAmountYear == 0 ? 0 : Math.Round(totalAmountMonth / totalAmountYear * 100, 2);

                    // Tạo view model chứa thông tin thống kê của tháng
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
