﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidProEdu.Domain.Enums
{
    public enum TestType
    {
        Entrance = 1, //kiểm tra đầu vào
        Progress = 2, //kiểm tra quá trình 2 bài mỗi bài 15%
        //FortyFiveMinuteExam = 3, //kiểm tra 45p
        MidTerm = 4, //kiểm tra giữa kì 1 bài 30%
        Final = 5 //kiểm tra cuối kì 1 bài 40%
    }
}
