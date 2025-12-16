using Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface ISpecification<T> where T : class
    {
        public List<Expression<Func<T, object>>> Includes { get; set; }
        public Expression<Func<T, bool>> Conditions { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public List<Expression<Func<T,object>>> Select { get; set; }
        public Expression<Func<T, int>> Count { get; set; }   
        public int? Take { get; set; }

    }
}
