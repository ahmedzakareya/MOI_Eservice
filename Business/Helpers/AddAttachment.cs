using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.ViewModel;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace Business.Helpers
{
    public  class AddAttachment
    {
        private readonly IConfiguration _configuration;

        public AddAttachment(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async static Task<FileSaveResponseVM> SaveFileToDiskAsync(IFormFile file, string relativePath, string reqNo)
        {
           // string currentDirectory = AppContext.BaseDirectory;
            //string mainProjectPath = Path.Combine(Directory.GetParent(currentDirectory).FullName,"~/", "MOI_Eservice");
            //Console.WriteLine(mainProjectPath);
            //var DirectoryPath =  Path.GetFullPath(mainProjectPath);
            //Console.WriteLine(DirectoryPath);
            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Admin", "Files", "Request_Documents", "Tourism");

            //string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath);
            //string uploadsFolder = Path.Combine(@"F:\MOI_Eservice\wwwroot\Admin\Files\Request_Documents\Tourism", relativePath);
           // string uploadsFolder = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('~', '/'));

            // Log the full directory path for debugging
            Console.WriteLine("Full Path to Upload Folder: " + uploadsFolder);

            try
            {
                // Check if the directory exists, if not create it
                if (!Directory.Exists(uploadsFolder))
                {
                    Console.WriteLine("Directory does not exist. Creating directory...");
                    Directory.CreateDirectory(uploadsFolder);
                }

                Random random = new Random();
                int threeDigitNumber = random.Next(100, 1000);
                string filename = reqNo + "/PA-AttachNo-" + threeDigitNumber + ".pdf";
                // Construct the file path
                string filePath = Path.Combine(uploadsFolder, filename);

                // Log the full file path for debugging
                Console.WriteLine("Full File Path: " + filePath);

                // Save the file to disk
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return new FileSaveResponseVM
                {
                    FilePath = filePath,
                    FileName = filename
                };
            }
            catch (Exception ex)
            {
                // Log the exception details
                Console.WriteLine("Error: " + ex.Message);
                throw; // Rethrow exception or handle accordingly
            }


        }

    }
}



