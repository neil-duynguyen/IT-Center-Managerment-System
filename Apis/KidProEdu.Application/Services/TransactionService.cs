using AutoMapper;
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
    }
}
