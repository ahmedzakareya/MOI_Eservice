using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.Dynamic
{
    public class WorkFlowVM
    {
        public int Id { get; set; }
        public string? ServiceName { get; set; }
        public int? ServiceId { get; set; }
      //  public string? ActivityTypeName { get; set; }
       // public int?ActivityTypeId { get; set; }
        public string? RequestTypeName { get; set; }
        public int? RequestTypeId { get; set; }
        public string? CurrentStatusName { get; set; }
        public int? CurrentStatusId { get; set; }
        public string? NextStatusName { get; set; }
        public int? NextStatusId { get; set; }
        public string? Conditions { get; set; }
       public bool IsPermissionRequired { get; set; }
        public string? FlagRequestStatus { get; set; }
        public string? FlagRequestType { get; set; }

        public string? TransactionTypeName { get; set; }
        public int? TransactionTypeId { get; set; }
        public int? SortOrder { get; set; }
        public IEnumerable<SelectListItem>? Services { get; set; }
        
       // public IEnumerable<SelectListItem>? ActivityTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestTypes { get; set; }
        public IEnumerable<SelectListItem>? RequestStatus { get; set; }
        public IEnumerable<SelectListItem>? TransactionTypes { get; set; }
        //[ForeignKey("ActivityTypeId")]
        //public virtual ActivityTypesLookup ActivityTypesLookup { get; set; }
        //[ForeignKey("CurrentStatusId")]
        //public virtual RequestStatusLookup RequestStatusCurrent { get; set; }
        //[ForeignKey("NextStatusId")]
        //public virtual RequestStatusLookup RequestStatusNext { get; set; }


    }
}
