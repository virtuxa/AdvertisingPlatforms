using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using AdvertisingPlatforms.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AdvertisingPlatforms.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdvertisingPlatformsController : ControllerBase
    {
        private readonly IAdvertisingPlatformService _platformService;
        private readonly ILogger<AdvertisingPlatformsController> _logger;

        public AdvertisingPlatformsController(
            IAdvertisingPlatformService platformService, 
            ILogger<AdvertisingPlatformsController> logger)
        {
            _platformService = platformService;
            _logger = logger;
        }

        [HttpPost("upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return BadRequest("Файл не был предоставлен или пуст");
                }

                string fileContent;
                using (var streamReader = new StreamReader(file.OpenReadStream(), Encoding.UTF8))
                {
                    fileContent = await streamReader.ReadToEndAsync();
                }

                var count = await _platformService.LoadFromFileAsync(fileContent);
                return Ok(new { message = $"Загружено {count} рекламных площадок" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке файла");
                return BadRequest($"Ошибка при загрузке файла: {ex.Message}");
            }
        }

        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SearchPlatforms([FromQuery] string location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    return BadRequest("Параметр 'location' обязателен");
                }

                var platforms = _platformService.GetPlatformsForLocation(location);
                return Ok(platforms);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при поиске площадок для локации: {location}");
                return BadRequest($"Ошибка при поиске: {ex.Message}");
            }
        }
    }
}