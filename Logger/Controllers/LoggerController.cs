using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace Logger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoggerController(ILogger<LoggerController> logger) : ControllerBase
    {
        private readonly ILogger<LoggerController> _logger = logger;

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] JsonElement payload)
        {
            var now = DateTime.Now;
            var day = now.Day.ToString("D2");
            var hour = now.Hour.ToString("D2");
            var timestamp = now.ToString("yyyyMMddHHmm", CultureInfo.InvariantCulture);
            var shortTimestamp = now.ToString("yyyyMMdd", CultureInfo.InvariantCulture);

            // Log the request payload
            var requestLogPath = Path.Combine("Logs", "Request", shortTimestamp, day, hour);
            Directory.CreateDirectory(requestLogPath);
            var requestFile = Path.Combine(requestLogPath, $"{timestamp}.json");
            await AppendJsonToFileAsync(requestFile, payload);

            // Prepare and log the response payload
            var responsePayload = new
            {
                code = "ok",
                message = "success"
            };
            var responseLogPath = Path.Combine("Logs", "Response", shortTimestamp, day, hour);
            Directory.CreateDirectory(responseLogPath);
            var responseFile = Path.Combine(responseLogPath, $"{timestamp}.json");
            await AppendJsonToFileAsync(responseFile, responsePayload);

            return Ok(responsePayload);
        }

        private static async Task AppendJsonToFileAsync<T>(string filePath, T newEntry)
        {
            List<JsonElement> entries = new();
            var options = new JsonSerializerOptions { WriteIndented = true };

            if (System.IO.File.Exists(filePath))
            {
                var existingContent = await System.IO.File.ReadAllTextAsync(filePath, Encoding.UTF8);
                try
                {
                    var doc = JsonDocument.Parse(existingContent);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        entries.AddRange(doc.RootElement.EnumerateArray());
                    }
                    else
                    {
                        entries.Add(doc.RootElement);
                    }
                }
                catch
                {
                    // If file is corrupted or not valid JSON, ignore and start fresh
                }
            }

            // Convert newEntry to JsonElement
            JsonElement newElement;
            if (newEntry is JsonElement je)
            {
                newElement = je;
            }
            else
            {
                var json = JsonSerializer.Serialize(newEntry, options);
                newElement = JsonDocument.Parse(json).RootElement.Clone();
            }
            entries.Add(newElement);

            var arrayJson = JsonSerializer.Serialize(entries, options);
            await System.IO.File.WriteAllTextAsync(filePath, arrayJson, Encoding.UTF8);
        }
    }
}
