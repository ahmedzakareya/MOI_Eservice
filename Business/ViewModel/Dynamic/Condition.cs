using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class Condition
    {
        public string? Field { get; set; }
        public string? Operator { get; set; }
        public object? Value { get; set; }
    }
}
