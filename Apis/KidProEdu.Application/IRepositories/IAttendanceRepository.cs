﻿using KidProEdu.Application.Repositories;
using KidProEdu.Application.ViewModels.AttendanceViewModels;
using KidProEdu.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.IRepositories
{

    public interface IAttendanceRepository : IGenericRepository<Attendance>
    {
        Task<List<Attendance>> GetAttendanceByScheduleId(Guid id);
    }
}