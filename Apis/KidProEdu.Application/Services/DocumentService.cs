using AutoMapper;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Divisions;
using KidProEdu.Application.Validations.Documents;
using KidProEdu.Application.Validations.Lessons;
using KidProEdu.Application.ViewModels.DocumentViewModels;
using KidProEdu.Application.ViewModels.LessonViewModels;
using KidProEdu.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public DocumentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateDocument(CreateDocumentViewModel createDocumentViewModel)
        {
            var validator = new CreateDocumentViewModelValidator();
            var validationResult = validator.Validate(createDocumentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var documents = await _unitOfWork.DocumentRepository.GetDocumentsByClassId(createDocumentViewModel.ClassId);
            if (documents.Any(d => d.LessonId == createDocumentViewModel.LessonId))
            {
                throw new Exception("Lớp học đã có tài liệu bài học này rồi");
            }
            var mapper = _mapper.Map<Document>(createDocumentViewModel);
            await _unitOfWork.DocumentRepository.AddAsync(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo tài liệu thất bại");
        }

        public async Task<bool> DeleteDocument(Guid id)
        {
            var result = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy tài liệu này");
            else
            {
                _unitOfWork.DocumentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa tài liệu thất bại");
            }
        }

        public async Task<DocumentViewModel> GetDocumentById(Guid id)
        {
            var results = await _unitOfWork.DocumentRepository.GetByIdAsync(id);

            var mapper = _mapper.Map<DocumentViewModel>(results);

            return mapper;
        }

        public async Task<List<DocumentViewModel>> GetDocuments()
        {
            var results = _unitOfWork.DocumentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();

            var mapper = _mapper.Map<List<DocumentViewModel>>(results);

            return mapper;
        }

        public async Task<List<DocumentViewModel>> GetDocumentsByClassId(Guid classId)
        {
            var results = await _unitOfWork.DocumentRepository.GetDocumentsByClassId(classId);

            var mapper = _mapper.Map<List<DocumentViewModel>>(results);

            return mapper;
        }

        public async Task<List<DocumentViewModel>> GetDocumentsByLessonId(Guid lessonId)
        {
            var results = await _unitOfWork.DocumentRepository.GetDocumentsByLessonId(lessonId);

            var mapper = _mapper.Map<List<DocumentViewModel>>(results);

            return mapper;
        }

        public async Task<bool> UpdateDocument(UpdateDocumentViewModel updateDocumentViewModel)
        {
            var validator = new UpdateDocumentViewModelValidator();
            var validationResult = validator.Validate(updateDocumentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var document = await _unitOfWork.DocumentRepository.GetByIdAsync(updateDocumentViewModel.Id);
            if (document == null)
            {
                throw new Exception("Không tìm thấy tài liệu");
            }

            var documents = await _unitOfWork.DocumentRepository.GetDocumentsByClassId(updateDocumentViewModel.ClassId);
            if(document.LessonId != updateDocumentViewModel.LessonId)
            {
                if (documents.Any(d => d.LessonId == updateDocumentViewModel.LessonId))
                {
                    throw new Exception("Lớp học đã có tài liệu bài học này rồi");
                }
            }
            var mapper = _mapper.Map<Document>(document);
            mapper.ClassId = updateDocumentViewModel.ClassId;
            mapper.LessonId = updateDocumentViewModel.LessonId;
            mapper.Url = updateDocumentViewModel.Url;
            _unitOfWork.DocumentRepository.Update(mapper);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật tài liệu thất bại");
        }
    }
}
