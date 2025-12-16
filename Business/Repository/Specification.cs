using Business.Interfaces;
using Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class Specification<T>:ISpecification<T> where T : class
    {
        public List<Expression<Func<T, object>>> Includes { get; set; } = new List<Expression<Func<T, object>>>();
        public Expression<Func<T, bool>> Conditions { get; set; }
        public Expression<Func<T, object>> OrderBy { get; set; }
        public Expression<Func<T, object>> OrderByDesc { get; set; }
        public Expression<Func<T, int>> Count { get; set; }
        public int? Take { get; set; }

        public List<Expression<Func<T, object>>> Select { get; set; } = new List<Expression<Func<T, object>>>();
        public Specification()
        {

        }
        public virtual void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }
        public virtual void AddSelect(Expression<Func<T, object>> selectExpression)
        {
            Select.Add(selectExpression);
        }

        public Specification(Expression<Func<T, bool>> conditions)
        {
            Conditions = conditions;
        }
        public void ApplyTake(int count)
        {
            Take = count;
        }
        public void OrderByAsc(Expression<Func<T, object>> _OrderBy)
        {
            OrderBy = _OrderBy;
        }
        public void OrderByDescSpec(Expression<Func<T, object>> _OrderByDesc)
        {
            OrderByDesc = _OrderByDesc;
        }

        public void CountSpec(Expression<Func<T, int>> Spec)
        {
            Count = Spec;   
        }


    }
}
