using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

namespace ApiServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("print")]
        public IActionResult DownloadPdf()
        {
            string fileName = "sample.pdf";
            string path = Assembly.GetEntryAssembly()?.Location ?? string.Empty;
            path = Path.GetDirectoryName(path) ?? string.Empty;
            IFileProvider provider = new PhysicalFileProvider(Path.Combine(path, "wwwroot/files"));
            IFileInfo fileInfo = provider.GetFileInfo(fileName);
            Stream readStream = fileInfo.CreateReadStream();
            string mimeType = "application/pdf";
            return File(readStream, mimeType, fileName);
        }
    }
}