using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Demo.PL.Helpers
{
    public class DocumentSettings
    {
        public static async Task<string> UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Located Folder Path 
            //string folderPaht = @"D:\\.Net C40\\Videos\\07 MVC\\Session 04\\Demo Solution\\Demo.PL\\wwwroot\\files\\" + folderName;
            //string folderPath = Directory.GetCurrentDirectory() + @"\wwwroot\files\" + folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);


            // 2. Get File Name And Make It UNIQUE
            string fileName = $"{Guid.NewGuid()}{file.FileName}";

            // 3. Get File Path 
            string filePath = Path.Combine(folderPath, fileName);


            // 4. Save File as Streams : 
            using var fileStream = new FileStream(filePath, FileMode.Create);

            await file.CopyToAsync(fileStream);

            return fileName;

        }


        public static void DeleteFile(string fileName, string folderName)
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);

            if (File.Exists(filePath))
            {
                using (var fileStream = File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                    fileStream.Close();
                }

                // Attempt to delete the file

                File.Delete(filePath);

            }
        }
    }
}
