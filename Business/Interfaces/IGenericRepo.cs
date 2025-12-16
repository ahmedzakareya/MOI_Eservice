using Business.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IGenericRepo<T> where T : class
    {

        public Task<object> Update(T entity);
        //public Task<object> Save(T entity);

        public Task Delete(T entity);
        public Task<IEnumerable<T>> GetAllAsync();
        public Task<IEnumerable<T>> GetAll();
        public Task<T> GetbyId(object id);
        public Task<object> Create(T entity);
        public  Task<T> GetByIdObject(Expression<Func<T, bool>> expression);
        public  Task UpdateAsync(T entity);
        public IQueryable<T> GetByCondition(Expression<Func<T, bool>> expression);
        public IQueryable<TResult> GetFilteredWithProjection<TResult>(
                                        Expression<Func<T, bool>>? filter = null,
                                        Expression<Func<T, TResult>>? selector = null,
                                    params Expression<Func<T, object>>[] includes);
        public  Task DeleteRange(Expression<Func<T, bool>> filter);
        public Task AddRange(IEnumerable<T> entities);
        public Task UpdateRange(IEnumerable<T> entities);

        public Task<IEnumerable<T>> GetTableWithSpec(ISpecification<T> spec);

        public Task<IEnumerable<T>> GetTableWithSpecService(ISpecification<T> spec);

        public Task<int> GetTableWithSpecCountService(ISpecification<T> spec);

        public Task<int> Count(Expression<Func<T, bool>> expression);


		public Task<T> GetByIdWithSpec(ISpecification<T> spec);
    }
}
