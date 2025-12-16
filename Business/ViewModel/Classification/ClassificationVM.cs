using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.ViewModel.ClassificationVM
{
    public class ClassificationVM
    {
    }
        public class EvaluationDetail
        {
            public int? EvaluationId { get; set; }
            public string EvaluationName { get; set; }
            public bool IsSelected { get; set; }
        }

        public class HotelClassDetail
        {
            public int HotelClassId { get; set; }
            public string HotelClassName { get; set; }
        public int? ClassificationId { get; set; }
            public int? CategoryId { get; set; }
            public bool? Status { get; set; }
            public ClassTypeDetail ClassType { get; set; }
            public List<EvaluationDetail> Evaluations { get; set; }
        }

        public class ClassTypeDetail
        {
            public int ClassTypeId { get; set; }
            public string ClassTypeName { get; set; }
        }

        public class ClassificationBranchDetail
        {
            public int BranchId { get; set; }
            public string BranchName { get; set; }
            public List<HotelClassDetail> HotelClasses { get; set; }
        }
    public class ClassificationResponse
    {
        public string ClassificationName { get; set; }
        public int ClassificationId { get; set; }

        public List<ClassificationBranchDetail> Branches { get; set; }
    }

    public class HotelClassViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public int? ClassBranchId { get; set; }
        public string? BranchName { get; set; }

        public int? ClassTypeId { get; set; }
        public string? TypeName { get; set; }

        public int? CategoryId { get; set; }
        public bool? Status { get; set; }

        public string? ClassificationName { get; set; }
    }

}
