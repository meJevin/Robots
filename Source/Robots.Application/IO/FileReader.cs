using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robots.Application.IO
{
    public interface IFileReader
    {
        Task<string> ReadAllTextAsync(string filePath);
    }

    public class FileReader : IFileReader
    {
        public async Task<string> ReadAllTextAsync(string filePath)
        {
            return await File.ReadAllTextAsync(filePath);
        }
    }
}
