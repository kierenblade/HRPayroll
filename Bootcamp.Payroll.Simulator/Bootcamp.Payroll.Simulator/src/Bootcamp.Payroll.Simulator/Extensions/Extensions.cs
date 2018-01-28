using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using Bootcamp.Payroll.Simulator.Classes;
using Bootcamp.Payroll.Simulator.Enums;

namespace Bootcamp.Payroll.Simulator.Extensions
{
    public static class Extensions
    {
        public static bool IsNullOrEmpty(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return true;
            return false;
        }
        public static bool IsNullOrWhiteSpace(this string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return true;
            return false;
        }

        public static bool InEnumRange(this string value, Type useEnum)
        {
            if(Enum.IsDefined(useEnum, value))
                return true;
            return false;
        }

        public static object GetEnum(this string value, Type useEnum)
        {
            if (value.InEnumRange(useEnum))
            {
                return Enum.Parse(useEnum, "0");
            }
            return Enum.Parse(useEnum, value);
        }

    }
}
