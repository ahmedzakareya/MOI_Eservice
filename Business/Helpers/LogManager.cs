using Business.Interfaces;
using Business.Repository;
using Business.ViewModel;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public class LogManager
    {
        private readonly EServiceDbContext _context;
        private readonly IUnitOfwork _unitOfwork;

        public LogManager()
        {
        }
        public LogManager(EServiceDbContext context,IUnitOfwork unitOfwork)
        {

            _context = context;
            _unitOfwork = unitOfwork;
        }

        private static readonly object padlock = new object();
        private static LogManager instance = null;
        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (padlock)
                    {
                        if (instance == null)
                        {
                            instance = new LogManager();
                        }
                    }
                }
                return instance;
            }
        }
        public void AddErrorLog(Exception ex)
        {
            try
            {
                // Create the error log instance
                var errorLog = new ApierrorLoggings()
                {
                    CreatedOn = DateTime.Now,
                    Message = ex.Message,
                    Source = ex.Source,
                    StackTrace = ex.StackTrace,
                    Details = ex.InnerException == null || ex.InnerException.InnerException == null
                              ? string.Empty
                              : ex.InnerException.InnerException.Message,
                    Type = "error"
                };

                // Add error log to the context via Unit of Work and save
                _unitOfwork.genericRepository<ApierrorLoggings>().Create(errorLog);
                _unitOfwork.Complete();
            }
            catch (Exception e)
            {
                // If logging fails, log the error to the console
                Console.WriteLine("Error logging to the database: " + e.Message);
            }
        }


    }
}
