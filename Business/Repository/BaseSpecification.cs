using Business.Interfaces;
using Business.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public static class BaseSpecification<T> where T : class
    {
        public static IQueryable<T> GetQuery(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;


            if (spec.Conditions != null)
                query = query.Where(spec.Conditions);


            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);


            if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);


            if (spec.Includes != null)
            {
                query = spec.Includes.Where(i => i != null).Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
            }
                
                ///if (spec.Select.Any())
            ///{
            ///    var param = Expression.Parameter(typeof(T), "x");
            ///    var bindings = spec.Select.Select(sel =>
            ///    {
            ///        var invokedExp = Expression.Invoke(sel, param);
            ///        return Expression.Bind(invokedExp.Type.GetProperty("PropertyName"), invokedExp);
            ///    }).ToList();
            ///    var body = Expression.MemberInit(Expression.New(typeof(T)), bindings);
            ///    var combinedSelector = Expression.Lambda<Func<T, T>>(body, param);
            ///    query = query.Select(combinedSelector);
            ///}
            ///query = query.Select(spec.Select);

            foreach (var include in spec.Includes)
            {
                Console.WriteLine(include);
            }
            if(spec.Take.HasValue)
            {
                query = query.Take(spec.Take.Value);
            }
            return query;
        }

        public static int GetQueryCount(IQueryable<T> inputQuery, ISpecification<T> spec)
        {
            var query = inputQuery;


            if (spec.Conditions != null)
                query = query.Where(spec.Conditions);


            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);


            if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);



            query = spec.Includes.Aggregate(query, (currentQuery, includeExpression) => currentQuery.Include(includeExpression));
           


            return query.Count();
        }
    }
}
