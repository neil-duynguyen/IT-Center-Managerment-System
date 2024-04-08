using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using KidProEdu.Application.Interfaces;
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

        public async Task<List<TransactionViewModel>> GetTransactionDetailByTransactionId(Guid id)
        {
            var listTransaction = _unitOfWork.TransactionRepository.GetAllAsync().Result.Where(x => x.ParentsTransaction == id);
            return _mapper.Map<List<TransactionViewModel>>(listTransaction);
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
                throw new Exception("Failed to retrieve transaction summary. " + ex.Message);
            }
        }

        public async Task<TransactionSummaryByMonthInYearViewModel> TransactionsSummariseByMonthInYear(DateTime monthInYear)
        {
            try
            {
                // Lấy danh sách các giao dịch từ Repository
                var transactions = await _unitOfWork.TransactionRepository.GetAllAsync();
                //var mapper = _mapper.Map<List<TransactionViewModel>>(transactions);

                // Tính tổng số tiền của các giao dịch
                double totalAmount = transactions.Sum(t => t.TotalAmount ?? 0);

                var transactionsByMonthInYear = await _unitOfWork.TransactionRepository.GetTransactionByMonthInYear(monthInYear);
                double totalAmountMonthInYear = transactionsByMonthInYear.Sum(t => t.TotalAmount ?? 0);
                var mapper = _mapper.Map<List<TransactionViewModel>>(transactionsByMonthInYear);

                // Tạo view model chứa thông tin tổng hợp
                var transactionsSummarise = new TransactionSummaryByMonthInYearViewModel
                {
                    Transactions = mapper,
                    TotalAmount = totalAmount,
                    TotalAmountOfMonthInYear = totalAmountMonthInYear
                };

                return transactionsSummarise;
            }
            catch (Exception ex)
            {
                // Nếu có lỗi xảy ra, trả về null hoặc xử lý tùy thuộc vào yêu cầu của bạn
                throw new Exception("Failed to retrieve transaction summary. " + ex.Message);
            }
        }
    }
}
