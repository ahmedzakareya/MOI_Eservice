using Business.ViewModel.Dynamic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel
{
    public class ConditionVM
    {
      
      public List<SelectListItem>? LicencesType { get; set; }

        
        public string? Condition { get; set; }
    }
}
