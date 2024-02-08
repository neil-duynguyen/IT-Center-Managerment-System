using KidProEdu.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Application.ViewModels.RoomViewModels
{
    public class UpdateRoomViewModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public StatusOfRoom? Status { get; set; }
    }
}
