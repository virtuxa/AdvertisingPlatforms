using System.Collections.Generic;

namespace AdvertisingPlatforms.Models
{
    public class AdvertisingPlatform
    {
        public string Name { get; set; } = string.Empty;
        public List<string> Locations { get; set; } = new List<string>();
    }
}