using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Business.Helpers
{
    public static class ExceptionLog
    {
        //private static string exceptionPath = System.Configuration.Configuration.AppSettings["LogExceptionPath"].ToString();


        private static IConfiguration _configuration;

        // This method can be called to set the IConfiguration in a static context
        public static void SetConfiguration(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public static string LogException(object inputItemModel, string fileStartName)
        {
            string fn = Guid.NewGuid().ToString();
            //fn.Replace("-", "");

            string fileName = fileStartName + fn + ".json";

            //string path = exceptionPath;
            string path = _configuration.GetSection("AppSettings:LogExceptionPath").Value;

            path = path + "\\" + DateTime.Now.Year.ToString() + "\\" + DateTime.Now.Month.ToString() + "\\" + DateTime.Now.Day.ToString() + "\\";
            if (Directory.Exists(path) == false)
            {
                Directory.CreateDirectory(path);
            }

            string output = JsonConvert.SerializeObject(inputItemModel, Formatting.Indented);
            string fName = path + fileName;
            System.IO.File.WriteAllText(fName, output);
            return fileName;
        }

        //private static void LogToDatabase(object inputItemModel, string fileStartName, string fn)
        //{
        //    try
        //    {
        //        // Assuming you have your DbContext available (replace YourDbContext with actual context class)
        //        using (var context = new YourDbContext())
        //        {
        //            var log = new ExceptionLogEntry
        //            {
        //                ExceptionId = Guid.NewGuid(),
        //                Message = inputItemModel.ToString(),
        //                StackTrace = "Stack trace here",  // Replace this with the actual stack trace if needed
        //                ActionName = fileStartName,
        //                ControllerName = "YourControllerName", // Get dynamically from context if necessary
        //                FileName = fileStartName + fn + ".json",
        //                Timestamp = DateTime.Now,
        //                UserId = "SomeUserId" // Optionally, get user ID dynamically
        //            };

        //            context.ExceptionLogs.Add(log);
        //            context.SaveChanges();  // Save to database
        //        }
        //    }
        //    catch (Exception dbEx)
        //    {
        //        // If database logging fails, log the exception to the file system for persistence
        //        string path = _configuration.GetSection("AppSettings:LogExceptionPath").Value;
        //        string errorFileName = "DatabaseLogError_" + Guid.NewGuid() + ".txt";
        //        string errorMessage = "Error logging to database: " + dbEx.Message + "\n" + dbEx.StackTrace;

        //        System.IO.File.WriteAllText(path + "\\" + errorFileName, errorMessage);  // Fallback to file logging
        //    }
        //}
//        CREATE TABLE ExceptionLogs
//(
//    Id INT PRIMARY KEY IDENTITY,
//    ExceptionId UNIQUEIDENTIFIER NOT NULL,
//    Message NVARCHAR(MAX),
//    StackTrace NVARCHAR(MAX),
//    ActionName NVARCHAR(255),
//    ControllerName NVARCHAR(255),
//    FileName NVARCHAR(255),
//    Timestamp DATETIME NOT NULL DEFAULT GETDATE(),
//    UserId NVARCHAR(255) NULL
//);
    }
}
