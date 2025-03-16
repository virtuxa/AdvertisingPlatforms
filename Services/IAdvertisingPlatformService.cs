using System.Collections.Generic;
using System.Threading.Tasks;
using AdvertisingPlatforms.Models;

namespace AdvertisingPlatforms.Services
{
    public interface IAdvertisingPlatformService
    {
        Task<int> LoadFromFileAsync(string fileContent);
        List<string> GetPlatformsForLocation(string location);
    }
}