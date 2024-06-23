using AspMVC.Models;

namespace AspMVC.Services
{
    public class HandleUploadFileService
    {
        
        public static async Task<string> SaveFile(IFormFile file, string dir)
        {
            if (file == null)
            {
                return null;
            }
            //var username = User.Identity.Name;
            //var FileStoragePath = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", "ProjectsFiles");
            //var userDir = Path.Combine(FileStoragePath, username);
            //var userProjectFileDir = Path.Combine(userDir, projectPageModel.Title);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string uploadedFile = Path.Combine(dir, file.FileName);
            using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return uploadedFile;
        }
        public static async Task SaveMultipleFiles(ICollection<IFormFile> files, string dir)
        {
            if (files != null)
            {

                //var username = User.Identity.Name;
                //var FileStoragePath = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", "ProjectsFiles");
                //var userDir = Path.Combine(FileStoragePath, username);
                //var userProjectFileDir = Path.Combine(userDir, projectPageModel.Title);
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                foreach (IFormFile file in files)
                {
                    string uploadedFile = Path.Combine(dir, file.FileName);
                    using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }

            }
        }
        public static async Task<string> SaveImage(IFormFile file, string dir)
        {
            if (file == null)
            {
                return null;
            }
            //var username = User.Identity.Name;
            //var FileStoragePath = Path.Combine(_environment.ContentRootPath, "Areas", "Blog", "Data", "ProjectsFiles");
            //var userDir = Path.Combine(FileStoragePath, username);
            //var userProjectFileDir = Path.Combine(userDir, projectPageModel.Title);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
            string uploadedFile = Path.Combine(dir, file.FileName);
            using (var fileStream = new FileStream(uploadedFile, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return uploadedFile;
        }


    }
}
