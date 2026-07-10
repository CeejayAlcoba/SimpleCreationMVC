
using Utilities.Interfaces;

namespace Utilities.Classes
{
     public class FileUtility : IFileUtility
    {
        private readonly string _baseDirectory;

        public FileUtility(string baseDirectory = "src/images", bool isAbsolutePath = false)
        {
            _baseDirectory = isAbsolutePath ? baseDirectory : Path.Combine(Directory.GetCurrentDirectory(), baseDirectory);

            if (!Directory.Exists(_baseDirectory))
                Directory.CreateDirectory(_baseDirectory);
        }

        public async Task<string> CreateAsync(string fileName, Stream fileStream)
        {
            var filePath = GetFullPath(fileName);

            using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.CopyToAsync(stream);
            }

            return filePath;
        }

        public async Task<string> UpdateAsync(string oldFileName, string newFileName, Stream newFileStream)
        {
            Delete(oldFileName); 
            return await CreateAsync(newFileName, newFileStream);
        }

        public void Delete(string fileName)
        {
            var filePath = GetFullPath(fileName);

            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        public bool Exists(string fileName)
        {
            var filePath = GetFullPath(fileName);
            return File.Exists(filePath);
        }

        public async Task<byte[]> GetAsync(string fileName)
        {
            var filePath = GetFullPath(fileName);

            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found", fileName);

            return await File.ReadAllBytesAsync(filePath);
        }

        public string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
        
            return extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".pdf" => "application/pdf",
                ".txt" => "text/plain",
                ".mp4" => "video/mp4",
                _ => "application/octet-stream" // default
            };
        }

        private string GetFullPath(string fileName)
        {
            return Path.Combine(_baseDirectory, fileName);
        }
    }
}
