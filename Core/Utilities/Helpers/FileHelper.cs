using Core.Utilities.Results;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;

namespace Core.Utilities.Helpers
{
    public class FileHelper
    {
        private static string ImageDirectory => Path.Combine(Environment.CurrentDirectory, "wwwroot", "Uploads", "Images");

        public static string Add(IFormFile file)
        {
            if (!Directory.Exists(ImageDirectory))
            {
                Directory.CreateDirectory(ImageDirectory);
            }

            var extension = Path.GetExtension(file.FileName);
            var newFileName = Guid.NewGuid().ToString("N") + extension;
            var fullPath = Path.Combine(ImageDirectory, newFileName);

            try
            {
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
            catch (Exception exception)
            {
                return exception.Message;
            }

            return newFileName;
        }

        public static string Update(string oldFileName, IFormFile file)
        {
            if (!string.IsNullOrEmpty(oldFileName))
            {
                var oldFullPath = Path.Combine(ImageDirectory, oldFileName);
                if (File.Exists(oldFullPath))
                {
                    try
                    {
                        File.Delete(oldFullPath);
                    }
                    catch { }
                }
            }
            return Add(file);
        }

        public Core.Utilities.Results.IResult Delete(string fileName)
        {
            if (!string.IsNullOrEmpty(fileName))
            {
                var fullPath = Path.Combine(ImageDirectory, fileName);
                if (File.Exists(fullPath))
                {
                    try
                    {
                        File.Delete(fullPath);
                    }
                    catch (Exception exception)
                    {
                        return new ErrorResult(exception.Message);
                    }
                }
            }

            return new SuccessResult();
        }

        private static bool IsValidImage(IFormFile file)
        {
            string[] allowedTypes = { "image/jpeg", "image/png", "image/jpg" };

            if (Array.IndexOf(allowedTypes, file.ContentType) < 0)
            {
                return false;
            }

            return true;
        }
    }
}
