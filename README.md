# Task Manager – Backend (.NET)

## Pasos para ejecutar el proyecto

### Requisitos
- .NET SDK 8 (o el que esté configurado en el `global.json`/solution)
- Haber Hido al repositorio ResourceProjectTask donde se encontrará scripts y sp's para este proceso.
- SQL Server (LocalDB o instancia local/remota)
- (Opcional) SSMS / Azure Data Studio

### 1) Base de datos
1. Abrir el script de base de datos (por ejemplo `database.sql` / `TaskManagerDb.sql`).
2. Ejecutarlo en SQL Server (crea `TaskManagerDb`, tablas, triggers, SPs y seeds).

> Nota: el proyecto usa catálogos en **MasterData / MasterDataDetail** (TaskStatus y TaskPriority) con GUIDs fijos para mantener consistencia.

### 2) Configurar Connection String
En `Api/appsettings.json` (o el proyecto que aplique) actualiza:
- `ConnectionStrings:DefaultConnection` (o el nombre usado en tu `DbContext`)

Ejemplo (ajusta servidor/credenciales):
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=TaskManagerDb;Trusted_Connection=True;TrustServerCertificate=True"
  }
}
```

### 3) Restaurar y ejecutar
Desde la carpeta de la solución:
```bash
dotnet restore
dotnet build
dotnet run --project Api
```

- API por defecto: `http://localhost:7777` (según `launchSettings.json`)
- Base path: `/api/v1/`

### 4) Endpoints principales
- **Users**
  - `GET /api/v1/users?PageNumber=1&PageSize=10&Search=...`
  - `POST /api/v1/users`
- **Tasks**
  - `GET /api/v1/tasks?PageNumber=1&PageSize=10&Status=...&AssignedUserId=...&PriorityId=...&Search=...`
  - `POST /api/v1/tasks`
  - `PUT /api/v1/tasks/{id}/status` body: `{ "status": "InProgress" }`
- **Values (catálogos para combos)**
  - `GET /api/v1/values/task-statuses`
  - `GET /api/v1/values/task-priorities`

### 5) Stored Procedures (paginación/filtros)
- `sp_users_get_paged`
- `sp_tasks_get_paged`

> Recomendación: usar **SqlParameter** y evitar concatenación de strings.

---

## Decisiones técnicas

### Arquitectura
- **Clean Architecture** por capas:
  - `Domain`: entidades, contratos, query models
  - `Application`: CQRS (Commands/Queries), Validators (FluentValidation), DTOs, constantes/enums
  - `Persistence`: EF Core + repositorios, ejecución de SPs y QueryModels
  - `Api`: Controllers versionados + MediatR
- **CQRS + MediatR**
  - Controladores “thin”: delegan en Requests/Handlers
  - Validación con **FluentValidation** (validator por request + behavior/pipeline)
- **Catálogos (MasterData)**
  - `MasterData` (Master) + `MasterDataDetail` (Detail)
  - Status/Priority viven en **MasterDataDetail**
  - Enums + diccionario de GUIDs fijos para estabilidad.
- **Paginación & filtros**
  - Listados implementados con SPs (`sp_*_get_paged`) por performance y control de filtros.
- **Triggers SQL Server**
  - Se declaró `HasTrigger("TR_Tasks_SetUpdatedAtUtc")` en el mapping EF para evitar el error
    de `OUTPUT clause` al guardar cambios.

---
---

## Notas
- Status permitido: `Pending`, `InProgress`, `Done`
- Regla de negocio: transición `Pending -> Done` **no permitida**.
