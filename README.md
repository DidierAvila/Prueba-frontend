# Prueba Frontend - Gestión de Tareas

Este es el frontend del sistema de gestión de tareas que interactúa con la API RESTful backend.

## Características Implementadas

✅ **Integración completa con API REST**
- Conexión a API en `https://localhost:7192`
- Manejo de todos los endpoints CRUD
- Gestión de errores y mensajes de estado

✅ **Funcionalidades CRUD**
- **Create**: Crear nuevas tareas
- **Read**: Listar todas las tareas y ver detalles
- **Update**: Editar tareas existentes
- **Delete**: Eliminar tareas

✅ **Funcionalidades Adicionales**
- **Toggle Complete**: Cambiar estado de completado rápidamente
- **Filter**: Ver solo tareas completadas
- **Responsive Design**: Interfaz adaptable con Bootstrap

## Arquitectura Implementada

### Servicios
- `TareaApiService`: Servicio para manejar todas las llamadas HTTP a la API
- Inyección de dependencias configurada en `Program.cs`
- Manejo de errores y logging

### Modelos/DTOs
- `Tarea`: Modelo principal para mostrar datos
- `CreateTareaDto`: DTO para crear nuevas tareas
- `UpdateTareaDto`: DTO para actualizar tareas existentes

### Controlador
- `TareaController`: Controlador MVC completamente asíncrono
- Manejo de errores con mensajes informativos
- Integración con TempData para notificaciones

### Vistas
- `Index`: Lista de tareas con acciones
- `Create`: Formulario de creación
- `Edit`: Formulario de edición
- `Details`: Vista de detalles
- `Delete`: Confirmación de eliminación

## Configuración

### API Base URL
Configurada en `appsettings.json`:
```json
{
  "ApiSettings": {
    "BaseUrl": "https://localhost:7192"
  }
}
```

### Requisitos
1. **API Backend** debe estar ejecutándose en `https://localhost:7192`
2. **.NET 9.0** instalado
3. **Visual Studio 2022** o **VS Code**

## Cómo usar

### 1. Ejecutar la aplicación
```bash
dotnet run
```

### 2. Navegar a la sección de Tareas
- Hacer clic en "Tareas" en el menú de navegación
- La aplicación se conectará automáticamente a la API

### 3. Funcionalidades disponibles
- **Crear Tarea**: Botón "Crear Nueva Tarea"
- **Ver Todas**: Botón "Todas las Tareas"
- **Ver Completadas**: Botón "Tareas Completadas"
- **Editar**: Botón "Editar" en cada tarea
- **Eliminar**: Botón "Eliminar" con confirmación
- **Toggle Estado**: Botón para marcar como completada/pendiente

## Estructura de Proyecto

```
Prueba.Web/
├── Controllers/
│   ├── HomeController.cs
│   └── TareaController.cs          # Controlador principal de tareas
├── Models/
│   ├── ErrorViewModel.cs
│   ├── Tarea.cs                    # Modelo principal
│   └── TareaDtos.cs                # DTOs para API
├── Services/
│   └── TareaApiService.cs          # Servicio de API
├── Views/
│   ├── Home/
│   ├── Shared/
│   └── Tarea/                      # Vistas de tareas
│       ├── Index.cshtml
│       ├── Create.cshtml
│       ├── Edit.cshtml
│       ├── Details.cshtml
│       └── Delete.cshtml
├── Program.cs                      # Configuración de servicios
└── appsettings.json               # Configuración de API
```

## Manejo de Errores

- **Conectividad API**: Mensajes informativos si la API no está disponible
- **Validación**: Validación de formularios en cliente y servidor
- **Estados HTTP**: Manejo apropiado de 404, 400, 500, etc.
- **UX**: Mensajes de éxito/error visibles para el usuario

## Tecnologías Utilizadas

- **ASP.NET Core MVC 9.0**
- **HttpClient** para llamadas a API
- **Bootstrap 5** para UI responsive
- **Razor Views** para templating
- **Dependency Injection** para servicios
- **TempData** para mensajes flash