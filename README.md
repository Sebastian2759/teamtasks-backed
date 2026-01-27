# Backend – Team Tasks Dashboard (.NET Core)

API REST en **.NET Core** con arquitectura por capas + CQRS para gestionar **Proyectos, Tareas, Developers** y soportar dashboards.

## Implementado ✅

### Arquitectura
- Flujo: **Controller → UseCase (MediatR) → Repository → EF/SP**
- Validaciones: **FluentValidation**
- Acceso a datos:
  - **EF Core** para consultas simples
  - **Stored Procedures** para paginación/filtros y lógica crítica

### Catálogos (Referencial Data)
- Tablas:
  - `TB_ReferencialData` (master)
  - `TB_ReferencialData_Details` (detail)
- Se usan para **status, priority, etc.** (valores en español).
- Mapeo por GUIDs fijos en `Application.Constants.Constants`.

### Stored Procedure
- `TT_SP_CreateTask`
  - Crea tarea con validaciones de negocio:
    - ProjectId válido
    - AssigneeId válido
    - Status/Priority pertenecen al catálogo correcto
  - Devuelve la tarea creada o error descriptivo.

### Endpoints operativos
- **ReferencialData** (para dropdowns):
  - `GET /api/v1/ReferencialData/state-proyect`
  - `GET /api/v1/ReferencialData/state-area`
  - `GET /api/v1/ReferencialData/task-priorities`
  - (y los demás según `Constants.IdsReferencesData`)
- **Developers**:
  - `GET /api/developers` → `{ developers: [{ developerId, fullName, email }] }`
- **Tasks**:
  - `POST /api/tasks` → crea tarea vía `TT_SP_CreateTask`

### Fixes importantes
- Ajuste para evitar error EF Core: **non-composable SQL** al ejecutar `EXEC` con `FromSqlRaw`.
- Corrección de negocio: envío de **IDs correctos** de catálogo para `IdTaskStatus` y `IdTaskPriority`.

### Endpoints requeridos
- `GET /api/projects` (si no está ya)
- `GET /api/projects/{id}/tasks` con:
  - filtros: `status`, `assigneeId`
  - paginación: `page`, `pageSize`
- Dashboards:
  - `GET /api/dashboard/developer-workload`
  - `GET /api/dashboard/project-health`
  - `GET /api/dashboard/developer-delay-risk`

### SQL/Business
- Consulta/feature: **tareas próximas a vencer (7 días)**

### Opcional
- `PUT /api/tasks/{id}/status`
- Tests de negocio (integration o SP directo)

## Ejecutar
```bash
dotnet restore
dotnet build
dotnet run --project Api/Api
