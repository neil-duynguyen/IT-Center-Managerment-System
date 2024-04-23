using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Schedules;
using KidProEdu.Application.ViewModels.ScheduleViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class SlotService : ISlotService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SlotService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }
        public async Task<List<Slot>> GetSlots()
        {
            var results = _unitOfWork.SlotRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false)
                .OrderBy(x => x.SlotType).ThenBy(x => x.StartTime).ToList();
            return results;
        }
    }
}
