using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.ViewModels.TagViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{

    public class TagService : ITagService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;

        public TagService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
        }

        public async Task<List<Tag>> GetTags()
        {
            var tags = await _unitOfWork.TagRepository.GetAllAsync();
            return tags;
        }
    }
}
