﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Ostoslista.Utils
{
    public class TimeConverter
    {
        public static DateTime ConvertToEetTime(DateTime utcDt)
        {
            var eet = TimeZoneInfo.FindSystemTimeZoneById("E. Europe Standard Time");
            utcDt = DateTime.SpecifyKind(utcDt, DateTimeKind.Utc);
            var convertedTime = TimeZoneInfo.ConvertTime(utcDt, eet);

            return convertedTime;
        }
    }
}