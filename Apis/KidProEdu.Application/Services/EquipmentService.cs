using AutoMapper;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Wordprocessing;
using FluentValidation;
using KidProEdu.Application.Interfaces;
using KidProEdu.Application.Utils;
using KidProEdu.Application.Validations.CategoryEquipments;
using KidProEdu.Application.Validations.Equipments;
using KidProEdu.Application.ViewModels.AdviseRequestViewModels;
using KidProEdu.Application.ViewModels.CategoryEquipmentViewModels;
using KidProEdu.Application.ViewModels.EquipmentViewModels;
using KidProEdu.Domain.Entities;
using KidProEdu.Domain.Enums;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace KidProEdu.Application.Services
{
    public class EquipmentService : IEquipmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentTime _currentTime;
        private readonly IClaimsService _claimsService;
        private readonly IMapper _mapper;
        private readonly QRCodeUtility _qrCodeUtility;
        private readonly IAttendanceService _attendanceService;

        public EquipmentService(IUnitOfWork unitOfWork, ICurrentTime currentTime, IClaimsService claimsService, IMapper mapper, QRCodeUtility qrCodeUtility, IAttendanceService attendanceService)
        {
            _unitOfWork = unitOfWork;
            _currentTime = currentTime;
            _claimsService = claimsService;
            _mapper = mapper;
            _qrCodeUtility = qrCodeUtility;
            _attendanceService = attendanceService;
        }

        public async Task<bool> CreateEquipment(CreateEquipmentViewModel createEquipmentViewModel)
        {
            var validator = new CreateEquipmentViewModelValidator();
            var validationResult = validator.Validate(createEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var mapper = _mapper.Map<Equipment>(createEquipmentViewModel);
            await _unitOfWork.EquipmentRepository.AddAsync(mapper);
            mapper.Code = _qrCodeUtility.GenerateQRCode($"{mapper.Id}");
            mapper.Status = StatusOfEquipment.Returned;
            var cate = await _unitOfWork.CategoryEquipmentRepository.GetByIdAsync(createEquipmentViewModel.CategoryEquipmentId);
            cate.Quantity = cate.Quantity + 1;
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Tạo thiết bị thất bại");

        }

        //Tạo list Equipment
        public async Task CreateListEquipment(CreateEquipmentViewModel createEquipmentViewModel, int quantity)
        {
            for (int i = 0; i < quantity; i++)
            {
                await CreateEquipment(createEquipmentViewModel);
            }
        }

        public async Task<bool> DeleteEquipment(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);

            if (result == null)
                throw new Exception("Không tìm thấy thiết bị");
            else
            {
                _unitOfWork.EquipmentRepository.SoftRemove(result);
                return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Xóa thiết bị thất bại");
            }

        }

        public async Task<bool> EquipmentBorrowedManagement(EquipmentBorrowedManagementViewModel equipmentBorrowedManagementViewModel)
        {
            var validator = new EquipmentBorrowedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentBorrowedManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentBorrowedManagementViewModel.EquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if (equipment.Status == StatusOfEquipment.Borrowed)
            {
                throw new Exception("Thiết bị đang được cho mượn");
            }
            else if (equipment.Status == StatusOfEquipment.Repair)
            {
                throw new Exception("Thiết bị đang được bảo dưỡng nên không thể cho mượn");
            }

            // Cập nhật trạng thái thiết bị

            equipment.Status = StatusOfEquipment.Borrowed;
            equipment.RoomId = equipmentBorrowedManagementViewModel.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentBorrowedManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Borrowed;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = _currentTime.GetCurrentTime();
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = equipmentBorrowedManagementViewModel.ReturnedDealine;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = equipmentBorrowedManagementViewModel.RoomId;
                logEquipment.Note = null;

                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)equipmentBorrowedManagementViewModel.UserAccountId);
                    await SendEmailUtil.SendEmail(teacher.Email, "Xác nhận yêu cầu mượn thiết bị",
                    "KidProEdu thông báo, \n\n" +
                    "Yêu cầu mượn thiết bị của bạn đã được xác nhận, \n" +
                    "   Thông tin:, \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + equipment.Code + "\n" +
                    "         Tên thiết bị: " + equipment.Name + "\n" +
                    //"         Loại thiết bị: " + equipment.CategoryEquipment.Name + "\n" +
                    "         Ngày hẹn trả: " + equipmentBorrowedManagementViewModel.ReturnedDealine + "\n" +
                    "Vui lòng trả thiết bị đúng trong thời hạn đã hẹn, xin cảm ơn!. \n\n" +
                    "Trân trọng, \n" +
                    "KidPro Education!");
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> EquipmentRepairManagement(EquipmentRepairManagementViewModel equipmentRepairManagementViewModel)
        {
            var validator = new EquipmentRepairManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentRepairManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentRepairManagementViewModel.EquipmentId);
            //var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if (equipment.Status == StatusOfEquipment.Repair)
            {
                throw new Exception("Thiết bị đang được bảo dưỡng rồi");
            }

            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Repair;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {

                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentRepairManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Repair;
                logEquipment.RepairDate = _currentTime.GetCurrentTime();
                logEquipment.BorrowedDate = null;
                logEquipment.ReturnedDate = null;
                logEquipment.ReturnedDealine = null;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = null;
                logEquipment.CategoryEquipmentId = null;
                logEquipment.Quantity = 1;
                logEquipment.Note = null;
                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var updateCategoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetCategoryEquipmentByEquipmentId(equipment.Id);
                    if (updateCategoryEquipment != null)
                    {
                        updateCategoryEquipment.Quantity = updateCategoryEquipment.Quantity - 1;
                        _unitOfWork.CategoryEquipmentRepository.Update(updateCategoryEquipment);
                        var result3 = await _unitOfWork.SaveChangeAsync();
                        if (result3 > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public async Task<bool> EquipmentReturnedManagement(EquipmentReturnedManagementViewModel equipmentReturnedManagementViewModel)
        {
            var validator = new EquipmentReturnedManagementViewModelValidator();
            var validationResult = validator.Validate(equipmentReturnedManagementViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentReturnedManagementViewModel.EquipmentId);
            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }
            else if (equipment.Status == StatusOfEquipment.Returned)
            {
                throw new Exception("Thiết bị đã được trả rồi");
            }
            // Cập nhật trạng thái thiết bị
            equipment.Status = StatusOfEquipment.Returned;
            _unitOfWork.EquipmentRepository.Update(equipment);
            var result = await _unitOfWork.SaveChangeAsync();
            if (result > 0)
            {
                var logEquipment = new LogEquipment();
                logEquipment.EquipmentId = equipment.Id;
                logEquipment.UserAccountId = equipmentReturnedManagementViewModel.UserAccountId;
                logEquipment.Name = equipment.Name;
                logEquipment.Code = equipment.Code;
                logEquipment.Price = equipment.Price;
                logEquipment.Status = StatusOfEquipment.Returned;
                logEquipment.RepairDate = null;
                logEquipment.BorrowedDate = null;
                logEquipment.ReturnedDate = _currentTime.GetCurrentTime();
                logEquipment.ReturnedDealine = null;
                logEquipment.WarrantyPeriod = equipment.WarrantyPeriod;
                logEquipment.PurchaseDate = equipment.PurchaseDate;
                logEquipment.RoomId = null;
                logEquipment.CategoryEquipmentId = null;
                logEquipment.Quantity = 1;
                logEquipment.Note = null;

                await _unitOfWork.LogEquipmentRepository.AddAsync(logEquipment);
                var result2 = await _unitOfWork.SaveChangeAsync();
                if (result2 > 0)
                {
                    var updateCategoryEquipment = await _unitOfWork.CategoryEquipmentRepository.GetCategoryEquipmentByEquipmentId(equipment.Id);
                    if (updateCategoryEquipment != null)
                    {
                        updateCategoryEquipment.Quantity = updateCategoryEquipment.Quantity + 1;
                        _unitOfWork.CategoryEquipmentRepository.Update(updateCategoryEquipment);
                        var result3 = await _unitOfWork.SaveChangeAsync();
                        if (result3 > 0)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        /*public async Task<bool> EquipmentManagement(EquipmentWithLogEquipmentBorrowedViewModel equipmentWithLogEquipmentView)
        {
            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(equipmentWithLogEquipmentView.Equipment.Id);
            var logEquipment = _unitOfWork.LogEquipmentRepository.GetAllAsync().Result.OrderByDescending(x => x.CreationDate).FirstOrDefault(x => x.EquipmentId == equipment.Id);

            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }else{
                if (equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được cho mượn");
                }
                else if(equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đang được cho mượn nên không thể bảo dưỡng");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Borrowed)
                {
                    throw new Exception("Thiết bị đang được bảo dưỡng nên không thể cho mượn");
                }
                else if (equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned)
                {
                    throw new Exception("Thiết bị đã được trả lại rồi");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Repair)
                {
                    throw new Exception("Thiết bị đã được mang đi bảo dưỡng rồi");
                }
                else if (equipment.Status == StatusOfEquipment.Repair && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người mang thiết bị đi bảo dưỡng khác với người nhận lại thiết bị");
                }
                else if (equipment.Status == StatusOfEquipment.Borrowed && equipmentWithLogEquipmentView.Equipment.Status == StatusOfEquipment.Returned && equipmentWithLogEquipmentView.Log.UserAccountId != logEquipment.UserAccountId)
                {
                    throw new Exception("Người muợn thiết bị khác với người trả thiết bị");
                }

            }
            // Cập nhật trạng thái thiết bị
            var mapper = _mapper.Map<Equipment>(equipment);
            mapper.Status = equipmentWithLogEquipmentView.Equipment.Status;
            mapper.RoomId = equipmentWithLogEquipmentView.Equipment.RoomId;
            _unitOfWork.EquipmentRepository.Update(equipment);

            var mapper2 = _mapper.Map<LogEquipment>(equipmentWithLogEquipmentView.Log);            
            mapper2.EquipmentId = equipment.Id;
            mapper2.UserAccountId = equipmentWithLogEquipmentView.Log.UserAccountId;
            mapper2.Name = equipment.Name;
            mapper2.Code = equipment.Code;
            mapper2.Price = equipment.Price;
            mapper2.Status = equipmentWithLogEquipmentView.Equipment.Status;
            mapper2.RepairDate = equipmentWithLogEquipmentView.Log.RepairDate;
            mapper2.BorrowedDate = equipmentWithLogEquipmentView.Log.BorrowedDate;
            mapper2.ReturnedDate = equipmentWithLogEquipmentView.Log.ReturnedDate;
            mapper2.ReturnedDealine = equipmentWithLogEquipmentView.Log.ReturnedDealine;
            mapper2.WarrantyPeriod = equipment.WarrantyPeriod;
            mapper2.PurchaseDate = equipment.PurchaseDate;
            mapper2.RoomId = equipmentWithLogEquipmentView.Equipment.RoomId;
            _unitOfWork.LogEquipmentRepository.AddAsync(mapper2);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thất bại");
        }*/

        public async Task<EquipmentByIdViewModel> GetEquipmentById(Guid id)
        {
            var result = await _unitOfWork.EquipmentRepository.GetByIdAsync(id);
            var mapper = _mapper.Map<EquipmentByIdViewModel>(result);
            return mapper;

        }

        public async Task<List<EquipmentViewModel>> GetEquipments()
        {
            var results = _unitOfWork.EquipmentRepository.GetAllAsync().Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
        }

        public async Task<List<EquipmentViewModel>> GetListEquipmentByName(string name)
        {
            var results = _unitOfWork.EquipmentRepository.GetListEquipmentByName(name).Result.Where(x => x.IsDeleted == false).OrderByDescending(x => x.CreationDate).ToList();
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
        }

        public async Task<bool> UpdateEquipment(UpdateEquipmentViewModel updateEquipmentViewModel)
        {
            var validator = new UpdateEquipmentViewModelValidator();
            var validationResult = validator.Validate(updateEquipmentViewModel);
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    throw new Exception(error.ErrorMessage);
                }
            }

            var equipment = await _unitOfWork.EquipmentRepository.GetByIdAsync(updateEquipmentViewModel.Id);
            if (equipment == null)
            {
                throw new Exception("Không tìm thấy thiết bị");
            }

            equipment.CategoryEquipmentId = updateEquipmentViewModel.CategoryEquipmentId;
            equipment.RoomId = updateEquipmentViewModel.RoomId;
            equipment.Name = updateEquipmentViewModel.Name;
            equipment.Price = updateEquipmentViewModel.Price;
            equipment.Status = updateEquipmentViewModel.Status;
            equipment.WarrantyPeriod = updateEquipmentViewModel.WarrantyPeriod;
            equipment.PurchaseDate = updateEquipmentViewModel.PurchaseDate;
            _unitOfWork.EquipmentRepository.Update(equipment);
            return await _unitOfWork.SaveChangeAsync() > 0 ? true : throw new Exception("Cập nhật thiết bị thất bại");
        }

        public async Task AutoCheckReturn()
        {
            var equipments = await _unitOfWork.LogEquipmentRepository.GetLogEquipmentByReturnDeadline(_currentTime.GetCurrentTime().AddDays(1));
            foreach (var item in equipments)
            {
                var teacher = await _unitOfWork.UserRepository.GetByIdAsync((Guid)item.UserAccountId);
                await SendEmailUtil.SendEmail(teacher.Email, "Nhắc nhở sắp tới hạn trả thiết bị",
                    "KidPro thông báo, \n\n" +
                    "Nhắc nhở sắp tới hạn trả thiết bị \n" +
                    "  Thông tin: \n" +
                    "         Người mượn: " + teacher.FullName + "\n" +
                    "         Email: " + teacher.Email + "\n" +
                    "         Sđt: " + teacher.Phone + "\n" +
                    //"         Mã thiết bị: " + item.Code + "\n" +
                    "         Tên thiết bị: " + item.Name + "\n" +
                    "         Ngày hẹn trả: " + item.ReturnedDealine + "\n" +
                    "Vui lòng bỏ qua email nếu bạn đã trả thiết bị rồi. \n\n" +
                    "Xin cảm ơn, \n" +
                    "KidPro Education!");
            }
        }

        public async Task<List<EquipmentViewModel>> GetEquipmentByStatus(StatusOfEquipment status)
        {
            var results = await _unitOfWork.EquipmentRepository.GetListEquipmentByStatus(status);
            var mapper = _mapper.Map<List<EquipmentViewModel>>(results);
            return mapper;
        }

        //Hàm này dùng để lấy ra các lớp theo ngày để xử lý phần chuẩn bị equipment
        public async Task<List<LearningProgress>> GetEquipmentByDate(DateOnly date)
        {
            //Lấy ra các lớp đang có status là Started
            var listClass = _unitOfWork.ClassRepository.GetAllAsync().Result.Where(x => x.StatusOfClass == StatusOfClass.Started).ToList();

            //Lấy ra children ở mỗi lớp để xem tiến độ học
            var listEnrollment = listClass.Select(x => x.Enrollments.FirstOrDefault()).Where(enrollment => enrollment != null).ToList();

            List<LearningProgress> learningProgress = new List<LearningProgress>();

            foreach (var item in listEnrollment)
            {
                var listAttendance = await _attendanceService.GetAttendanceDetailsByCourseIdAndChildrenId(item.Class.CourseId, item.ChildrenProfileId);

                //getTeacher
                var teacher = item.Class.TeachingClassHistories.OrderByDescending(x => x.CreationDate).FirstOrDefault().UserAccount;

                if (listAttendance.Any(x => DateOnly.FromDateTime(x.Date) == date))
                {
                    //getRoom
                    var roomName = item.Class.Schedules.FirstOrDefault(x => x.DayInWeek.Equals(date.DayOfWeek.ToString())).ScheduleRooms.OrderByDescending(x => x.CreationDate).FirstOrDefault().Room.Name;

                    int daysStudied = listAttendance.Count(x => DateOnly.FromDateTime(x.Date) <= date);

                    // Tạo một ánh xạ giữa tên slot và thời gian tương ứng
                    Dictionary<string, string> slotTimeMap = new Dictionary<string, string>
                        {
                            { "Slot1", "7:00" },
                            { "Slot2", "9:30" },
                            { "Slot3", "12:30" },
                            { "Slot4", "15:00" },
                            { "Slot5", "19:00" }
                        };

                    // Lấy tên slot
                    var getSlot = item.Class.Schedules.FirstOrDefault()?.Slot?.Name;

                    // Kiểm tra xem slot có tồn tại trong ánh xạ không và gán thời gian tương ứng
                    var slot = "";
                    if (getSlot != null && slotTimeMap.ContainsKey(getSlot))
                    {
                        slot = getSlot + " (" + slotTimeMap[getSlot] + ")";
                    }

                    learningProgress.Add(new LearningProgress
                    {
                        ClassId = item.ClassId,
                        ClassCode = item.Class.ClassCode,
                        Progress = daysStudied,
                        TeacherId = teacher.Id,
                        NameTeacher = teacher.FullName,
                        Slot = slot,
                        RoomName = roomName
                    });
                }
            }

            List<LearningProgress> itemsToAdd = new List<LearningProgress>();

            foreach (var item in learningProgress)
            {
                var listEquipment = await GetEquipmentByProgress(item.ClassId, item.Progress);

                if (listEquipment.Count() != 0)
                {
                    itemsToAdd.Add(item);
                }
            }

            //learningProgress.AddRange(itemsToAdd);

            return itemsToAdd;
        }

        //Get equipment theo ngày
        public async Task<List<PrepareEquipmentViewModel>> GetEquipmentByProgress(Guid classId, int progress)
        {
            List<PrepareEquipmentViewModel> listPrepareEquipmentView = new List<PrepareEquipmentViewModel>();

            var getClass = await _unitOfWork.ClassRepository.GetByIdAsync(classId);
            var getCourse = await _unitOfWork.CourseRepository.GetByIdAsync(getClass.CourseId);
            var filterLesson = getCourse.Lessons.Where(x => !x.IsDeleted).ToList();
            int? duration = 0;

            foreach (var item in filterLesson)
            {
                if (progress != 1)
                {
                    if (item.Duration + duration < progress)
                    {
                        duration += item.Duration;
                    }
                    else
                    {
                        var getLesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.Id);
                        if (getLesson.TypeOfPractice != null)
                        {
                            await AddEquipmentToList(getLesson, getClass.Enrollments.Count, listPrepareEquipmentView, (TypeOfPractice)getLesson.TypeOfPractice);
                            return listPrepareEquipmentView;
                        }
                        else
                        {
                            return listPrepareEquipmentView;
                        }
                    }
                }
                else
                {
                    var getLesson = await _unitOfWork.LessonRepository.GetByIdAsync(item.Id);

                    if (getLesson.TypeOfPractice != null)
                    {
                        await AddEquipmentToList(getLesson, getClass.Enrollments.Count, listPrepareEquipmentView, (TypeOfPractice)getLesson.TypeOfPractice);
                        return listPrepareEquipmentView;
                    }
                    else
                    {
                        return listPrepareEquipmentView;
                    }
                }

            }
            return listPrepareEquipmentView;
        }
        private async Task AddEquipmentToList(Lesson getLesson, int enrollmentCount, List<PrepareEquipmentViewModel> listPrepareEquipmentView, TypeOfPractice typeOfPractice)
        {
            if (getLesson.TypeOfPractice == TypeOfPractice.Individual)
            {
                foreach (var equipment in getLesson.CategoryEquipments)
                {
                    listPrepareEquipmentView.Add(new PrepareEquipmentViewModel { Id = equipment.Id, Name = equipment.Name, Quantity = enrollmentCount });
                }
            }
            else if (getLesson.TypeOfPractice == TypeOfPractice.Group)
            {
                foreach (var equipment in getLesson.CategoryEquipments)
                {
                    var a = (int)Math.Ceiling((double)((double)(enrollmentCount) / getLesson.GroupSize));
                    listPrepareEquipmentView.Add(new PrepareEquipmentViewModel { Id = equipment.Id, Name = equipment.Name, Quantity = a });
                }
            }
        }

        //xuất excel equipment theo ngày
        public async Task<Stream> ExportExcelFileAsync(DateOnly date)
        {
            string[] columnNames = new string[] { "Equpment", "Quantity" };
            string header = string.Join(",", columnNames);

            var stream = new MemoryStream();

            var listClass = await GetEquipmentByDate(date);

            using (var package = new ExcelPackage(stream))
            {
                foreach (var item in listClass)
                {
                    var listEquipment = await GetEquipmentByProgress(item.ClassId, item.Progress);
                    var worksheet = package.Workbook.Worksheets.Add(item.ClassCode);
                    worksheet.Cells.LoadFromText(header);

                    for (int i = 0; i < columnNames.Length; i++)
                    {
                        worksheet.Cells[1, i + 1].Value = columnNames[i];
                    }

                    //Add data
                    for (int i = 0; i < listEquipment.Count; i++)
                    {
                        worksheet.Cells[i + 2, 1].Value = listEquipment[i].Name;
                        worksheet.Cells[i + 2, 2].Value = listEquipment[i].Quantity;
                    }
                }
                await package.SaveAsync();
            }
            stream.Position = 0;

            return stream;
        }

        public async Task<List<EquipmentViewModel>> GetListEquipmentByCateId(Guid cateId)
        {
            var result = await _unitOfWork.EquipmentRepository.GetListEquipmentByCateId(cateId);
            var mapper = _mapper.Map<List<EquipmentViewModel>>(result);
            return mapper;
        }

        public async Task<List<EquipmentViewModel>> GetListEquipmentByCateIdAndStatus(Guid cateId, StatusOfEquipment status)
        {
            var result = await _unitOfWork.EquipmentRepository.GetListEquipmentByCateIdAndStatus(cateId, status);
            var mapper = _mapper.Map<List<EquipmentViewModel>>(result);
            return mapper;
        }
    }

    public class LearningProgress
    {
        public Guid ClassId { get; set; }
        public string ClassCode { get; set; }
        public int Progress { get; set; }
        public Guid TeacherId { get; set; }
        public string NameTeacher { get; set; }
        public string Slot { get; set; }
        public string RoomName { get; set; }
    }
}
