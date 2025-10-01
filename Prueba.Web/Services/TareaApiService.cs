using Prueba.Web.Models;

namespace Prueba.Web.Services
{
    public interface ITareaApiService
    {
        Task<IEnumerable<Tarea>> GetAllTareasAsync();
        Task<IEnumerable<Tarea>> GetCompletedTareasAsync();
        Task<Tarea?> GetTareaByIdAsync(int id);
        Task<Tarea> CreateTareaAsync(CreateTareaDto createTareaDto);
        Task<Tarea> UpdateTareaAsync(int id, UpdateTareaDto updateTareaDto);
        Task<bool> DeleteTareaAsync(int id);
    }

    public class TareaApiService : ITareaApiService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<TareaApiService> _logger;
        private const string ApiBaseUrl = "/api/Task";
        private const string FallbackBaseUrl = "https://localhost:7192";

        public TareaApiService(HttpClient httpClient, ILogger<TareaApiService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
            
            // Verificar y configurar BaseAddress si no está establecido
            if (_httpClient.BaseAddress == null)
            {
                _logger.LogWarning("HttpClient BaseAddress es null, configurando fallback: {FallbackUrl}", FallbackBaseUrl);
                _httpClient.BaseAddress = new Uri(FallbackBaseUrl);
            }
            
            _logger.LogInformation("TareaApiService inicializado con BaseAddress: {BaseUrl}", _httpClient.BaseAddress);
        }

        public async Task<IEnumerable<Tarea>> GetAllTareasAsync()
        {
            try
            {
                _logger.LogInformation("Llamando a la API: {BaseUrl}{ApiUrl}", _httpClient.BaseAddress, ApiBaseUrl);
                
                var response = await _httpClient.GetAsync(ApiBaseUrl);
                
                _logger.LogInformation("Respuesta de la API: {StatusCode}", response.StatusCode);
                
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Contenido de la respuesta: {Content}", content);
                    
                    var tareas = await response.Content.ReadFromJsonAsync<IEnumerable<Tarea>>();
                    _logger.LogInformation("Tareas deserializadas: {Count}", tareas?.Count() ?? 0);
                    
                    return tareas ?? new List<Tarea>();
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    _logger.LogError("Error en la API: {StatusCode}, Content: {Content}", response.StatusCode, errorContent);
                    return new List<Tarea>();
                }
            }
            catch (HttpRequestException httpEx)
            {
                _logger.LogError(httpEx, "Error de conexión HTTP al obtener todas las tareas. BaseUrl: {BaseUrl}", _httpClient.BaseAddress);
                return new List<Tarea>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error general al obtener todas las tareas");
                return new List<Tarea>();
            }
        }

        public async Task<IEnumerable<Tarea>> GetCompletedTareasAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/completed");
                response.EnsureSuccessStatusCode();
                
                var tareas = await response.Content.ReadFromJsonAsync<IEnumerable<Tarea>>();
                return tareas ?? new List<Tarea>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tareas completadas");
                return new List<Tarea>();
            }
        }

        public async Task<Tarea?> GetTareaByIdAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{ApiBaseUrl}/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Tarea>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea con ID {Id}", id);
                return null;
            }
        }

        public async Task<Tarea> CreateTareaAsync(CreateTareaDto createTareaDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ApiBaseUrl, createTareaDto);
                response.EnsureSuccessStatusCode();
                
                var createdTarea = await response.Content.ReadFromJsonAsync<Tarea>();
                return createdTarea ?? throw new InvalidOperationException("No se pudo crear la tarea");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear la tarea");
                throw;
            }
        }

        public async Task<Tarea> UpdateTareaAsync(int id, UpdateTareaDto updateTareaDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"{ApiBaseUrl}/{id}", updateTareaDto);
                response.EnsureSuccessStatusCode();
                
                var updatedTarea = await response.Content.ReadFromJsonAsync<Tarea>();
                return updatedTarea ?? throw new InvalidOperationException("No se pudo actualizar la tarea");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la tarea con ID {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteTareaAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{ApiBaseUrl}/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return false;
                }
                
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarea con ID {Id}", id);
                return false;
            }
        }
    }
}