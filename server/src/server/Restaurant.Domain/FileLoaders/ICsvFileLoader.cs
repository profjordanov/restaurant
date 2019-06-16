using System.Threading.Tasks;

namespace Restaurant.Domain.FileLoaders
{
    public interface ICsvFileLoader
    {
        Task<string> LoadFileAsync();
    }
}