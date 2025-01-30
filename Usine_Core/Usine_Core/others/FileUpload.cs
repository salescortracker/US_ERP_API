using Microsoft.AspNetCore.Http;
using System.IO;
using System;

namespace Usine_Core.others
{
    public class FileUpload
    {
        #region File Upload
        public static string UploadFile(IFormFile file, string fileUploadPath, bool IsGuidName = false)
        {
            bool isUploaded = false;
            string uploadedFileName = string.Empty;
            string originalFileName = string.Empty;

            try
            {
                originalFileName = file.FileName;
                if (IsGuidName)
                {
                    Guid guid = Guid.NewGuid();
                    var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                    uploadedFileName = guid + extension;
                }
                else
                    uploadedFileName = originalFileName;

                var pathBuilt = Path.Combine(Directory.GetCurrentDirectory(), fileUploadPath);
                if (!Directory.Exists(pathBuilt))
                {
                    Directory.CreateDirectory(pathBuilt);
                }
                var path = Path.Combine(pathBuilt, uploadedFileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                isUploaded = true;
            }
            catch (Exception ex)
            {
            }

            return uploadedFileName; 
        }

        public static string GetFileUrl(string path, string fileName)
        {
            return fileName != null ? (path.Replace("\\", "/") + "/" + fileName) : "";
        }
        public static string GetFilePathFormat(string path)
        {
            return path.Replace("/", "\\");
        }
        public class FileUploadPath
        {
            public static string uploadimg { get { return "Uploads\\inventory\\itemdetails"; } }
        }
            #endregion
        }


}
