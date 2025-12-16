using Business.ViewModel;

namespace MOI_Eservice.Areas.Admin.Service
{
    public class SaveAttachment
    {
        private readonly IWebHostEnvironment _env;

        public SaveAttachment(IWebHostEnvironment env)
        {
                _env = env;
        }

        public async Task<FileSaveResponseVM> SaveFileToDiskAsync(IFormFile file, string fileNameFromFile, string relativePath, string? reqNo)
        {
            string filepath = Path.Combine(_env.WebRootPath, relativePath);
            string uploadsFolder;
            if (!string.IsNullOrEmpty(reqNo))
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath, reqNo);
            }
            else
            {
                uploadsFolder = Path.Combine(_env.WebRootPath, relativePath);
            }

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder); // Create directory if it doesn't exist
            }
            string fileName;

            // Generate the sequence number and file name
            if (!string.IsNullOrEmpty(reqNo))
            {
                string _Reqno = reqNo + "/AttachNo-";
                Random random = new Random();
                int sequenceNumber = random.Next(100, 1000); // Generating a random number for sequence
                fileName = $"{_Reqno}{sequenceNumber}.pdf"; // AttachNo-{sequenceNumber}.pdf
            }
            else
            {
                fileName = $"{fileNameFromFile}.pdf";
            }
            string filePath = Path.Combine(filepath, fileName);
            string filePathWithoutSlash = filePath.Replace("/", "\\"); // Replace / with backslash for Windows compatibility

            try
            {
                // Save the file asynchronously
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream); // Copy file to the disk
                }

                return new FileSaveResponseVM
                {
                    FilePath = fileName,
                    FileName = fileNameFromFile
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
