using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AdvertisingPlatforms.Models;
using Microsoft.Extensions.Logging;

namespace AdvertisingPlatforms.Services
{
    public class AdvertisingPlatformService : IAdvertisingPlatformService
    {
        private readonly ILogger<AdvertisingPlatformService> _logger;
        private List<AdvertisingPlatform> _platforms = new List<AdvertisingPlatform>();

        public AdvertisingPlatformService(ILogger<AdvertisingPlatformService> logger)
        {
            _logger = logger;
        }

        public async Task<int> LoadFromFileAsync(string fileContent)
        {
            try
            {
                var newPlatforms = new List<AdvertisingPlatform>();
                
                using (var reader = new StringReader(fileContent))
                {
                    string line;
                    while ((line = await reader.ReadLineAsync()) != null)
                    {
                        if (string.IsNullOrWhiteSpace(line))
                            continue;

                        var parts = line.Split(':', 2);
                        if (parts.Length != 2)
                        {
                            _logger.LogWarning($"Некорректный формат строки: {line}");
                            continue;
                        }

                        var platformName = parts[0].Trim();
                        var locations = parts[1].Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(l => l.Trim())
                            .ToList();

                        var platform = new AdvertisingPlatform
                        {
                            Name = platformName,
                            Locations = locations
                        };

                        newPlatforms.Add(platform);
                    }
                }

                _platforms = newPlatforms;
                
                return _platforms.Count;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке данных из файла");
                throw;
            }
        }

        public List<string> GetPlatformsForLocation(string location)
        {
            if (string.IsNullOrWhiteSpace(location))
                return new List<string>();

            try
            {
                var normalizedLocation = location.Trim();
                
                var matchingPlatforms = _platforms
                    .Where(p => p.Locations.Any(platformLocation => 
                        normalizedLocation.StartsWith(platformLocation) || normalizedLocation == platformLocation))
                    .Select(p => p.Name)
                    .Distinct()
                    .ToList();

                return matchingPlatforms;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при поиске площадок для локации: {location}");
                return new List<string>();
            }
        }
    }
}