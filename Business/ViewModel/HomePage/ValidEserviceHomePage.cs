using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.HomePage
{
    public class ValidEserviceHomePage
    {
        public ValidEserviceCombinations ValidEserviceCombinations { get; set; }

        public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestTypes { get; set; }

        public IEnumerable<SelectListItem>? LicenceTypes { get; set; }
        public List<ActivityTypesLookup>? ActivityTypesModel { get; set; }

        public List<RequestsTypesLookup>? RequestTypesModel { get; set; }
        public List<LicenceTypesLookup>? LicenceTypesLookup { get; set; }

    }
    public class ValidEserviceEditViewModel
    {
        public ValidEserviceCombinations License { get; set; }
  
        public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestTypes { get; set; }
       
        public IEnumerable<SelectListItem>? LicenceTypes { get; set; }
        public List<ActivityTypesLookup>? ActivityTypesModel { get; set; }
      
        public List<RequestsTypesLookup>? RequestTypesModel { get; set; }
        public List<LicenceTypesLookup>? LicenceTypesLookup { get; set; }
    }
}
