using Business.Interfaces;
using Business.ViewModel;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Business.Repository
{
    public class UnitOfWork:IUnitOfwork
    {
        private readonly EServiceDbContext _context;
        private readonly HttpClient _httpClient;
        private IDbContextTransaction _dbTransaction;

        public Hashtable _repository { get; set; }
        // private Dictionary<Type, object> _repository;
        private TransactionScope _transactionScope;

        public UnitOfWork(EServiceDbContext context,HttpClient httpClient)
        {
            _context = context;
            _httpClient = httpClient;
        }

        public async Task<int> Complete()
        {
            _context.ChangeTracker.Entries().ToList();
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepo<T> genericRepository<T>() where T : class
         {
            if (_repository == null)
                _repository = new Hashtable();

            var type = typeof(T).Name;

            if (!_repository.ContainsKey(type))
            {
                var repo = new GenericRepo<T>(_context);
                _repository.Add(type, repo);

            }

            return (IGenericRepo<T>)_repository[type];

        }
        #region New method to fetch data from an external API

        public async Task<List<T>> FetchFromApiAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync(endpoint);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<T>>(jsonResponse);
            }
            return new List<T>();
        }

        public async Task<T> FetchByIdFromApiAsync<T>(string endpoint)
        {
            var response = await _httpClient.GetAsync($"{endpoint}");
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            return default;
        }
        #endregion
        public void Rollback()
        {
            _transactionScope.Dispose();
           
        }
        public void RollbackTransaction()
        {
            _dbTransaction.Rollback();
        }

        public IDbContextTransaction BeginTransaction()
        {
           return _context.Database.BeginTransaction();
        }
        public void CommitTransaction()
        {
         _dbTransaction.Commit();
        }
        public async Task BeginTransactionAsync()
        {
            _dbTransaction = await _context.Database.BeginTransactionAsync(System.Data.IsolationLevel.ReadCommitted);
        }
        public async Task CommitTransactionAsync()
        {
            if (_dbTransaction != null)
            {
                await _dbTransaction.CommitAsync();
                await _dbTransaction.DisposeAsync();
                _dbTransaction = null;
            }
        }
        public async Task RollbackTransactionAsync()
        {
            if (_dbTransaction != null)
            {
                await _dbTransaction.RollbackAsync();
                await _dbTransaction.DisposeAsync();
                _dbTransaction = null;
            }
        }
    }
}
