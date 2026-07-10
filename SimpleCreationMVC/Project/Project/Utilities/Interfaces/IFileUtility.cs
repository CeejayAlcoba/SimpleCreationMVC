
using Models;

namespace Utilities.Interfaces
{
    public interface IFileUtility
    {
        Task<string> CreateAsync(string fileName, Stream fileStream);
        Task<string> UpdateAsync(string oldFileName, string newFileName, Stream newFileStream);
        void Delete(string fileName);
        bool Exists(string fileName);
        Task<byte[]> GetAsync(string fileName);
        string GetContentType(string fileName);
    }
}


