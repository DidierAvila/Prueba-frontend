using Microsoft.AspNetCore.Mvc;
using Prueba.Web.Services;
using System.Text.Json;

namespace Prueba.Web.Controllers
{
    public class DiagnosticoController : Controller
    {
        private readonly ITareaApiService _tareaApiService;
        private readonly ILogger<DiagnosticoController> _logger;
        private readonly HttpClient _httpClient;

        public DiagnosticoController(ITareaApiService tareaApiService, ILogger<DiagnosticoController> logger, IHttpClientFactory httpClientFactory)
        {
            _tareaApiService = tareaApiService;
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<IActionResult> TestConexion()
        {
            var diagnostico = new
            {
                FechaHora = DateTime.Now,
                Tests = new List<object>()
            };

            // Test 1: Conexión directa con HttpClient básico
            try
            {
                // Ignorar certificados SSL en desarrollo
                var handler = new HttpClientHandler()
                {
                    ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
                };
                using var testClient = new HttpClient(handler);
                
                var response = await testClient.GetAsync("https://localhost:7192/api/Task");
                var content = await response.Content.ReadAsStringAsync();
                
                diagnostico.Tests.Add(new
                {
                    Test = "HttpClient Directo",
                    Status = response.StatusCode.ToString(),
                    Success = response.IsSuccessStatusCode,
                    Content = content,
                    Headers = response.Headers.ToString()
                });
            }
            catch (Exception ex)
            {
                diagnostico.Tests.Add(new
                {
                    Test = "HttpClient Directo",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            // Test 2: Usar el servicio configurado
            try
            {
                var tareas = await _tareaApiService.GetAllTareasAsync();
                diagnostico.Tests.Add(new
                {
                    Test = "TareaApiService",
                    Success = true,
                    TareasCount = tareas.Count(),
                    Tareas = tareas.Take(3) // Solo las primeras 3 para no sobrecargar
                });
            }
            catch (Exception ex)
            {
                diagnostico.Tests.Add(new
                {
                    Test = "TareaApiService",
                    Success = false,
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }

            ViewBag.Diagnostico = JsonSerializer.Serialize(diagnostico, new JsonSerializerOptions 
            { 
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return View();
        }
    }
}