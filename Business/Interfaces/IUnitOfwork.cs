using Business.ViewModel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Interfaces
{
    public interface IUnitOfwork:IDisposable
    {
        public IGenericRepo<T> genericRepository<T>() where T : class;

        public Task<int> Complete();

        public Task<List<T>> FetchFromApiAsync<T>(string endpoint);
        public Task<T> FetchByIdFromApiAsync<T>(string endpoint);
        void Rollback();

        public IDbContextTransaction BeginTransaction();
        public void RollbackTransaction();
        public void CommitTransaction();

        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();

    }
}
