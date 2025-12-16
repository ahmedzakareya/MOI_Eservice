using Business.Interfaces;
using Business.ViewModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
//using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Business.Repository
{
    public class GenericRepo<T> : IGenericRepo<T> where T : class
    {
        private readonly EServiceDbContext _Context;

        public GenericRepo(EServiceDbContext context)
        {
            _Context = context;
        }

        public async Task Delete(T entity)
        => _Context.Set<T>().Remove(entity);

        public async Task DeleteRange(Expression<Func<T, bool>> filter)
        {
            var entities = await _Context.Set<T>().Where(filter).ToListAsync();
            _Context.Set<T>().RemoveRange(entities);
        }
        public async Task AddRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

            await _Context.Set<T>().AddRangeAsync(entities);
        }
        public async Task UpdateRange(IEnumerable<T> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities));

             _Context.Set<T>().UpdateRange(entities);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _Context.Set<T>().ToListAsync();
        }

        public async Task<object> Create(T entity)
        {
            var existingEntity = _Context.ChangeTracker.Entries<T>()
        .FirstOrDefault(e => e.Entity == entity);

            if (existingEntity != null)
            {
                existingEntity.State = EntityState.Detached;
            }

            return await _Context.Set<T>().AddAsync(entity);
        }


        public async Task<int> Count(Expression<Func<T, bool>> expression)
        {
            return await _Context.Set<T>().CountAsync(expression);
        }



        public async Task<IEnumerable<T>> GetAll()
        => await _Context.Set<T>().ToListAsync();



        public async Task<T> GetbyId(object id)
        => await _Context.Set<T>().FindAsync(id);

        public async Task<T> GetByIdObject(Expression<Func<T, bool>> expression)
            => await _Context.Set<T>().FirstOrDefaultAsync(expression);

        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression)
        {
            return _Context.Set<T>().Where(expression);
        }
        public IQueryable<TResult> GetFilteredWithProjection<TResult>(
    Expression<Func<T, bool>>? filter = null,
    Expression<Func<T, TResult>>? selector = null,
    params Expression<Func<T, object>>[] includes // Accept multiple include expressions
)
        {
            var query = _Context.Set<T>().AsQueryable();

            // Apply the includes
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            // Apply the filter if provided
            if (filter != null)
            {
                query = query.Where(filter);
            }

            // Apply the projection (selector)
            if (selector != null)
            {
                return query.Select(selector);
            }

            throw new ArgumentNullException(nameof(selector), "A selector is required for projection.");
        }



        public async Task<T> GetByIdWithSpec(ISpecification<T> spec)
        {
            return await EvaluateExpression(spec).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<T>> GetTableWithSpecService(ISpecification<T> spec)
        {
            return await EvaluateExpression(spec).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetTableWithSpec(ISpecification<T> spec)
        {
            return await EvaluateExpression(spec).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            var trackedEntity = _Context.ChangeTracker.Entries<T>()
         .FirstOrDefault(e => e.Entity != null && e.Entity.GetType() == typeof(T) && e.Property("Id").CurrentValue.Equals(entity.GetType().GetProperty("Id")?.GetValue(entity)));

            if (trackedEntity != null)
            {
                trackedEntity.State = EntityState.Detached; // Detach it to avoid tracking conflict
            }

            _Context.Attach(entity);
            _Context.Entry(entity).State = EntityState.Modified;

        }
        public async Task<object> Update(T entity)
        => _Context.Set<T>().Update(entity);


        //public async Task<object> Save(T entity)
        //{
        //    try
        //    {
        //        _Context.Set<T>().Add(entity);
        //        _Context.SaveChanges();
        //        return entity;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}
        public async Task<int> GetTableWithSpecCountService(ISpecification<T> spec)
        {
            return EvaluateExpressionWithCount(spec);
        }

        public IQueryable<T> EvaluateExpression(ISpecification<T> spec)
        {
            return BaseSpecification<T>.GetQuery(_Context.Set<T>(), spec);
        }
        public int EvaluateExpressionWithCount(ISpecification<T> spec)
        {
            return BaseSpecification<T>.GetQueryCount(_Context.Set<T>(), spec);
        }



    }
}
