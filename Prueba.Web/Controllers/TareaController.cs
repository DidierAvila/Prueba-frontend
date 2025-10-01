using Microsoft.AspNetCore.Mvc;
using Prueba.Web.Models;
using Prueba.Web.Services;

namespace Prueba.Web.Controllers
{
    public class TareaController : Controller
    {
        private readonly ITareaApiService _tareaApiService;
        private readonly ILogger<TareaController> _logger;

        public TareaController(ITareaApiService tareaApiService, ILogger<TareaController> logger)
        {
            _tareaApiService = tareaApiService;
            _logger = logger;
        }

        // GET: Tarea
        public async Task<IActionResult> Index()
        {
            try
            {
                var tareas = await _tareaApiService.GetAllTareasAsync();
                return View(tareas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tareas");
                TempData["Error"] = "Error al cargar las tareas. Por favor, intente nuevamente.";
                return View(new List<Tarea>());
            }
        }

        // GET: Tarea/Details/5
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var tarea = await _tareaApiService.GetTareaByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }
                return View(tarea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea con ID {Id}", id);
                TempData["Error"] = "Error al cargar la tarea.";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Tarea/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tarea/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTareaDto createTareaDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _tareaApiService.CreateTareaAsync(createTareaDto);
                    TempData["Success"] = "Tarea creada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al crear la tarea");
                    TempData["Error"] = "Error al crear la tarea. Por favor, intente nuevamente.";
                }
            }
            return View(createTareaDto);
        }

        // GET: Tarea/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var tarea = await _tareaApiService.GetTareaByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }

                var updateDto = new UpdateTareaDto
                {
                    Id = tarea.Id,
                    Title = tarea.Title ?? string.Empty,
                    Description = tarea.Description,
                    IsCompleted = tarea.IsCompleted
                };

                return View(updateDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea para editar con ID {Id}", id);
                TempData["Error"] = "Error al cargar la tarea para editar.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tarea/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateTareaDto updateTareaDto)
        {
            if (id != updateTareaDto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tareaApiService.UpdateTareaAsync(id, updateTareaDto);
                    TempData["Success"] = "Tarea actualizada exitosamente.";
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al actualizar la tarea con ID {Id}", id);
                    TempData["Error"] = "Error al actualizar la tarea. Por favor, intente nuevamente.";
                }
            }
            return View(updateTareaDto);
        }

        // GET: Tarea/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var tarea = await _tareaApiService.GetTareaByIdAsync(id);
                if (tarea == null)
                {
                    return NotFound();
                }
                return View(tarea);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la tarea para eliminar con ID {Id}", id);
                TempData["Error"] = "Error al cargar la tarea.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Tarea/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _tareaApiService.DeleteTareaAsync(id);
                if (success)
                {
                    TempData["Success"] = "Tarea eliminada exitosamente.";
                }
                else
                {
                    TempData["Error"] = "No se pudo eliminar la tarea. Es posible que ya no exista.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar la tarea con ID {Id}", id);
                TempData["Error"] = "Error al eliminar la tarea.";
            }
            return RedirectToAction(nameof(Index));
        }

        // POST: Tarea/ToggleComplete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleComplete(int id)
        {
            try
            {
                var tarea = await _tareaApiService.GetTareaByIdAsync(id);
                if (tarea != null)
                {
                    var updateDto = new UpdateTareaDto
                    {
                        Id = tarea.Id,
                        Title = tarea.Title ?? string.Empty,
                        Description = tarea.Description,
                        IsCompleted = !tarea.IsCompleted
                    };

                    await _tareaApiService.UpdateTareaAsync(id, updateDto);
                    TempData["Success"] = $"Tarea marcada como {(updateDto.IsCompleted ? "completada" : "pendiente")}.";
                }
                else
                {
                    TempData["Error"] = "No se encontró la tarea.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cambiar el estado de la tarea con ID {Id}", id);
                TempData["Error"] = "Error al actualizar el estado de la tarea.";
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Tarea/Completed
        public async Task<IActionResult> Completed()
        {
            try
            {
                var tareasCompletadas = await _tareaApiService.GetCompletedTareasAsync();
                return View("Index", tareasCompletadas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener las tareas completadas");
                TempData["Error"] = "Error al cargar las tareas completadas.";
                return View("Index", new List<Tarea>());
            }
        }

        // GET: Tarea/TestApi - Método de prueba para verificar la conexión con la API
        public async Task<IActionResult> TestApi()
        {
            try
            {
                _logger.LogInformation("Iniciando prueba de conexión con la API");
                var tareas = await _tareaApiService.GetAllTareasAsync();
                
                ViewBag.ApiTest = true;
                ViewBag.TareasCount = tareas.Count();
                ViewBag.ApiUrl = "https://localhost:7192/api/Task";
                
                return View("Index", tareas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la prueba de API");
                TempData["Error"] = $"Error en la prueba de API: {ex.Message}";
                return View("Index", new List<Tarea>());
            }
        }
    }
}