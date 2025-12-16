using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class EnumHelper
    {
        public static string GetDisplayName(Enum value)
        {
            var attr = value.GetType()
                            .GetField(value.ToString())
                            .GetCustomAttributes(typeof(DisplayAttribute), false)
                            .FirstOrDefault() as DisplayAttribute;

            return attr?.Name ?? value.ToString();
        }
    }
}
