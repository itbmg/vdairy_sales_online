using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CargoManagementSystem
{
    class DateConverter
    {
       static public DateTime GetLowDate(DateTime dt)
        {
            double Hour, Min, Sec;
            DateTime DT = DateTime.Now;
            DT = dt;
            Hour = -dt.Hour;
            Min = -dt.Minute;
            Sec = -dt.Second;
            DT = DT.AddHours(Hour);
            DT = DT.AddMinutes(Min);
            DT = DT.AddSeconds(Sec);
            return DT;

        }

     static   public DateTime GetHighDate(DateTime dt)
        {
            double Hour, Min, Sec;
            DateTime DT = DateTime.Now;
            Hour = 23 - dt.Hour;
            Min = 59 - dt.Minute;
            Sec = 59 - dt.Second;
            DT = dt;
            DT = DT.AddHours(Hour);
            DT = DT.AddMinutes(Min);
            DT = DT.AddSeconds(Sec);
            return DT;

        }
    }
}
