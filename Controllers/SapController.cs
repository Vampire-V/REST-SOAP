using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace DemoSAP.Controllers
{
    // Model สำหรับ Data
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    [ApiController]
    [Route("api/[controller]")]
    public class SapController : ControllerBase
    {
        private readonly ILogger<SapController> _logger;

        public SapController(ILogger<SapController> logger)
        {
            _logger = logger;
        }

        private void LogResult(object result)
        {
            var logMessage = System.Text.Json.JsonSerializer.Serialize(
                result,
                new System.Text.Json.JsonSerializerOptions { WriteIndented = true }
            );
            _logger.LogInformation(logMessage);
        }

        private async Task<object> GetRequestDetailsAsync()
        {
            // ดึง Headers
            var headers = HttpContext.Request.Headers.ToDictionary(
                h => h.Key,
                h => h.Value.ToString()
            );

            // ดึง Query Parameters
            var queryParams = HttpContext.Request.Query.ToDictionary(
                q => q.Key,
                q => q.Value.ToString()
            );

            // ดึง Body (เฉพาะ Method ที่รองรับ Body)
            string body = null;
            if (
                HttpContext.Request.Method == HttpMethods.Post
                || HttpContext.Request.Method == HttpMethods.Put
            )
            {
                HttpContext.Request.EnableBuffering(); // ทำให้ Body อ่านได้หลายครั้ง
                using (
                    var reader = new StreamReader(
                        HttpContext.Request.Body,
                        Encoding.UTF8,
                        true,
                        1024,
                        leaveOpen: true
                    )
                )
                {
                    body = await reader.ReadToEndAsync();
                }
                HttpContext.Request.Body.Position = 0; // รีเซ็ตตำแหน่งของ Stream
            }

            // สร้าง Response Object
            return new
            {
                Method = HttpContext.Request.Method,
                Headers = headers,
                QueryParams = queryParams,
                Body = body,
            };
        }

        // GET: api/test
        [HttpGet("GetItems")] // ใช้ชื่อ Method เป็น Path
        public async Task<IActionResult> GetItems()
        {
            var result = await GetRequestDetailsAsync();
            LogResult(result);
            return Ok(result);
        }

        // GET: api/test/1
        [HttpGet("GetItem/{id}")]
        public async Task<IActionResult> GetItem(int id)
        {
            var result = await GetRequestDetailsAsync();
            LogResult(result);
            return Ok(result);
        }

        // POST: api/test
        [HttpPost("CreateItem")]
        public async Task<IActionResult> CreateItem()
        {
            var result = await GetRequestDetailsAsync();
            LogResult(result);
            return Ok(result);
        }

        // PUT: api/test/1
        [HttpPut("UpdateItem/{id}")]
        public async Task<IActionResult> UpdateItem(int id)
        {
            var result = await GetRequestDetailsAsync();
            LogResult(result);
            return Ok(result);
        }

        // DELETE: api/test/1
        [HttpDelete("DeleteItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            var result = await GetRequestDetailsAsync();
            LogResult(result);
            return Ok(result);
        }
    }
}
