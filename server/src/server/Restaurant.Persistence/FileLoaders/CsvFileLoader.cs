using Restaurant.Domain.FileLoaders;
using System.IO;
using System.Threading.Tasks;

namespace Restaurant.Persistence.FileLoaders
{
    public class CsvFileLoader : ICsvFileLoader
    {
        private readonly string _filePath;

        public CsvFileLoader(string filePath)
        {
            _filePath = filePath;
        }

        public Task<string> LoadFileAsync()
        {
            using (var reader = new StreamReader(_filePath))
            {
                return reader.ReadToEndAsync();
            }
        }
    }
}