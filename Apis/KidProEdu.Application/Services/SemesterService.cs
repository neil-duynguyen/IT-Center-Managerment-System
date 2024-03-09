using AutoMapper;
using AutoMapper.Execution;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Validations.Semesters;
using KidProEdu.Application.ViewModels.SemesterViewModels;
using KidProEdu.Domain.Entities;
using System.Linq.Expressions;

namespace KidProEdu.Application.Services
{
    public class SemesterService : ISemesterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;

        public SemesterService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
        }

        public async Task<bool> CreateSemester()
        {
            // Xác định ngày bắt đầu của năm học
            DateTime startDate = new(_currentTime.GetCurrentTime().Year, 1, 1);

            if ((await _unitOfWork.SemesterRepository.GetSemesterByStartDate(startDate)) != null)
                throw new Exception("Tạo thất bại, học kỳ trong năm học này đã được tạo");

            // Tính độ dài của mỗi học kỳ (3 tháng)
            int semesterDurationMonths = 3;

            // Tạo và thêm 4 học kỳ vào cơ sở dữ liệu
            for (int i = 0; i < 4; i++)
            {
                // Xác định ngày bắt đầu và ngày kết thúc của học kỳ
                DateTime semesterStartDate = startDate.AddMonths(i * semesterDurationMonths);
                DateTime semesterEndDate = semesterStartDate.AddMonths(semesterDurationMonths).AddDays(-1);

                string semesterPrefix;

                // Xác định phần tiền tố của tên học kỳ dựa trên tháng bắt đầu
                int month = semesterStartDate.Month;
                if (month >= 1 && month <= 3)
                {
                    semesterPrefix = "SP"; // Spring
                }
                else if (month >= 4 && month <= 6)
                {
                    semesterPrefix = "SU"; // Summer
                }
                else if (month >= 7 && month <= 9)
                {
                    semesterPrefix = "FA"; // Fall
                }
                else
                {
                    semesterPrefix = "WI"; // Winter
                }

                // Xác định phần số cuối của tên học kỳ từ năm
                string yearSuffix = (startDate.Year % 100).ToString("00");

                // Xác định tên của học kỳ
                string semesterName = $"{semesterPrefix}{yearSuffix}";

                // Tạo đối tượng Semester
                var semester = new Semester
                {
                    SemesterName = semesterName,
                    StatusSemester = Domain.Enums.StatusSemester.Closed,
                    StartDate = semesterStartDate,
                    EndDate = semesterEndDate
                    // Các thông tin khác nếu cần
                };

                // Thêm đối tượng Semester vào cơ sở dữ liệu
                await _unitOfWork.SemesterRepository.AddAsync(semester);
            }

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo Semester thất bại");
        }

        public async Task<bool> DeleteSemester(Guid semesterId)
        {
            var result = await _unitOfWork.SemesterRepository.GetByIdAsync(semesterId);

            if (result == null)
                throw new Exception("Không tìm thấy Semester này");
            else
            {
                _unitOfWork.SemesterRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa Semester thất bại");
            }
        }

        public async Task<Semester> GetSemesterById(Guid semesterId)
        {
            var semester = await _unitOfWork.SemesterRepository.GetByIdAsync(semesterId);
            return semester;
        }

        public async Task<List<SemesterViewModel>> GetSemesters()
        {
            var semesters = _unitOfWork.SemesterRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            return _mapper.Map<List<SemesterViewModel>>(semesters);
        }

        public async Task<bool> UpdateSemester(UpdateSemesterViewModel updateSemesterViewModel, params Expression<Func<Semester, object>>[] uniqueProperties)
        {
            var validator = new UpdateSemesterViewModelValidator();
            var validationResult = validator.Validate(updateSemesterViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage.ToString());
                }
            }

            foreach (var property in uniqueProperties)
            {
                var semester = await _unitOfWork.SemesterRepository.GetSemesterByProperty(updateSemesterViewModel, property);
                if (semester != null && semester.Id != updateSemesterViewModel.Id)
                {
                    throw new Exception($"{property.GetMember().Name} đã tồn tại");
                }
            }

            /*var semester = await _unitOfWork.SemesterRepository.GetSemesterBySemesterName(updateSemesterViewModel.SemesterName);
            if (semester != null && semester.Id != updateSemesterViewModel.Id)
            {
                throw new Exception("Tên đã tồn tại");
            }

            semester = await _unitOfWork.SemesterRepository.GetSemesterByStartDate(updateSemesterViewModel.StartDate);
            if (semester != null && semester.Id != updateSemesterViewModel.Id)
            {
                throw new Exception("Ngày bắt đầu đã tồn tại");
            }*/

            var existingSemester = await _unitOfWork.SemesterRepository.GetByIdAsync(updateSemesterViewModel.Id) ?? throw new Exception("Không tìm thấy học kỳ");
            //var mapper = _mapper.Map<Semester>(updateSemesterViewModel);
            _unitOfWork.SemesterRepository.Update(_mapper.Map(updateSemesterViewModel, existingSemester));
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật Semester thất bại");
        }

        public async Task<bool> ChangeStatusSemester(Guid id)
        {
            var semester = await _unitOfWork.SemesterRepository.GetByIdAsync(id) ?? throw new Exception("Không tìm thấy kỳ này");
            if (semester.StatusSemester.Equals(Domain.Enums.StatusSemester.Closed))
            {
                semester.StatusSemester = Domain.Enums.StatusSemester.Started;
            }
            else
            {
                semester.StatusSemester = Domain.Enums.StatusSemester.Closed;
            }

            _unitOfWork.SemesterRepository.Update(semester);

            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Thay đổi trạng thái kỳ học thất bại");
        }
    }
}
