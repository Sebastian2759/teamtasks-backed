# Backend – Team Tasks Dashboard (.NET Core)

API REST en **.NET Core** con arquitectura por capas + CQRS para gestionar **Proyectos, Tareas, Developers** y soportar dashboards.

## Implementado 

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
#script sql IMPORTANTE :
USE [TeamTasksSample]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [dbo].[Projects] DROP CONSTRAINT [FK_Projects_ProjectStatus]
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Projects]') AND type in (N'U'))
DROP TABLE [dbo].[Projects]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Projects](
	[ProjectId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](200) NOT NULL,
	[ClientName] [varchar](200) NOT NULL,
	[StartDate] [date] NOT NULL,
	[EndDate] [date] NULL,
	[IdProjectStatus] [uniqueidentifier] NOT NULL,
	[CreatedAts] [datetime] NOT NULL,
 CONSTRAINT [PK_Projects] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
ALTER TABLE [dbo].[Projects]  WITH CHECK ADD  CONSTRAINT [FK_Projects_ProjectStatus] FOREIGN KEY([IdProjectStatus])
REFERENCES [dbo].[TB_ReferencialData_Details] ([Id])
GO

ALTER TABLE [dbo].[Projects] CHECK CONSTRAINT [FK_Projects_ProjectStatus]
GO


IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Developers]') AND type in (N'U'))
DROP TABLE [dbo].[Developers]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Developers](
	[DeveloperId] [uniqueidentifier] NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[Email] [varchar](256) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedAts] [datetime] NOT NULL,
 CONSTRAINT [PK_Developers] PRIMARY KEY CLUSTERED 
(
	[DeveloperId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO


----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_ReferencialData_Details]') AND type in (N'U'))
DROP TABLE [dbo].[TB_ReferencialData_Details]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_ReferencialData_Details](
	[Id] [uniqueidentifier] NOT NULL,
	[IdReferencialData] [uniqueidentifier] NOT NULL,
	[Description] [varchar](200) NOT NULL,
	[AuxiliarData] [varchar](200) NULL,
	[Valid] [bit] NOT NULL,
	[RegisterDate] [datetime] NULL,
	[RegisterUser] [varchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [varchar](100) NULL,
 CONSTRAINT [PK_TB_ReferencialData_Details] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TB_ReferencialData]') AND type in (N'U'))
DROP TABLE [dbo].[TB_ReferencialData]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[TB_ReferencialData](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [varchar](200) NOT NULL,
	[AuxiliarData] [varchar](200) NULL,
	[Valid] [bit] NOT NULL,
	[RegisterDate] [datetime] NOT NULL,
	[RegisterUser] [varchar](100) NULL,
	[UpdateDate] [datetime] NULL,
	[UpdateUser] [varchar](100) NULL,
 CONSTRAINT [PK_TB_ReferencialData] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [dbo].[TT_SP_CreateTask]
(
    @ProjectId           UNIQUEIDENTIFIER,
    @Title               VARCHAR(200),
    @Description         VARCHAR(MAX) = NULL,
    @AssigneeId          UNIQUEIDENTIFIER,
    @IdTaskStatus        UNIQUEIDENTIFIER,
    @IdTaskPriority      UNIQUEIDENTIFIER,
    @EstimatedComplexity INT,
    @DueDate             DATE,
    @CompletionDate      DATE = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Validaciones base
    IF NOT EXISTS (SELECT 1 FROM dbo.Projects WHERE ProjectId = @ProjectId)
        THROW 51000, 'El ProjectId no existe.', 1;

    IF NOT EXISTS (SELECT 1 FROM dbo.Developers WHERE DeveloperId = @AssigneeId AND IsActive = 1)
        THROW 51000, 'El AssigneeId no existe o el desarrollador está inactivo.', 1;

    IF (@EstimatedComplexity < 1 OR @EstimatedComplexity > 5)
        THROW 51000, 'La complejidad estimada debe estar entre 1 y 5.', 1;

    IF (@Title IS NULL OR LTRIM(RTRIM(@Title)) = '')
        THROW 51000, 'El título de la tarea es obligatorio.', 1;

    IF (@DueDate IS NULL)
        THROW 51000, 'La fecha de vencimiento (DueDate) es obligatoria.', 1;

    -- 2) Validar que Status/Priority pertenezcan a los catálogos correctos
    DECLARE @CAT_EstadosTarea UNIQUEIDENTIFIER;
    DECLARE @CAT_Prioridades UNIQUEIDENTIFIER;

    SELECT TOP 1 @CAT_EstadosTarea = Id
    FROM dbo.TB_ReferencialData
    WHERE Description = 'ESTADOS DE TAREA' AND Valid = 1;

    SELECT TOP 1 @CAT_Prioridades = Id
    FROM dbo.TB_ReferencialData
    WHERE Description = 'PRIORIDADES DE TAREA' AND Valid = 1;

    IF (@CAT_EstadosTarea IS NULL)
        THROW 51000, 'No existe el catálogo "ESTADOS DE TAREA" en TB_ReferencialData.', 1;

    IF (@CAT_Prioridades IS NULL)
        THROW 51000, 'No existe el catálogo "PRIORIDADES DE TAREA" en TB_ReferencialData.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TB_ReferencialData_Details
        WHERE Id = @IdTaskStatus AND IdReferencialData = @CAT_EstadosTarea AND Valid = 1
    )
        THROW 51000, 'El estado de tarea (IdTaskStatus) no es válido o no pertenece al catálogo.', 1;

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.TB_ReferencialData_Details
        WHERE Id = @IdTaskPriority AND IdReferencialData = @CAT_Prioridades AND Valid = 1
    )
        THROW 51000, 'La prioridad (IdTaskPriority) no es válida o no pertenece al catálogo.', 1;

    -- 3) Regla CompletionDate según estado "Completada"
    DECLARE @ID_StatusCompletada UNIQUEIDENTIFIER;

    SELECT TOP 1 @ID_StatusCompletada = d.Id
    FROM dbo.TB_ReferencialData_Details d
    WHERE d.IdReferencialData = @CAT_EstadosTarea
      AND d.Description = 'Completada'
      AND d.Valid = 1;

    IF (@ID_StatusCompletada IS NULL)
        THROW 51000, 'No existe el valor "Completada" dentro del catálogo ESTADOS DE TAREA.', 1;

    IF (@IdTaskStatus = @ID_StatusCompletada AND @CompletionDate IS NULL)
        THROW 51000, 'Si el estado es "Completada", CompletionDate es obligatorio.', 1;

    IF (@IdTaskStatus <> @ID_StatusCompletada AND @CompletionDate IS NOT NULL)
        THROW 51000, 'CompletionDate solo debe enviarse cuando el estado sea "Completada".', 1;

    -- 4) Insert
    DECLARE @NewTaskId UNIQUEIDENTIFIER = NEWID();

    INSERT INTO dbo.Tasks
    (
        TaskId, ProjectId, Title, Description, AssigneeId,
        IdTaskStatus, IdTaskPriority,
        EstimatedComplexity, DueDate, CompletionDate, CreatedAts
    )
    VALUES
    (
        @NewTaskId, @ProjectId, @Title, @Description, @AssigneeId,
        @IdTaskStatus, @IdTaskPriority,
        @EstimatedComplexity, @DueDate, @CompletionDate, GETDATE()
    );

    -- 5) Retorno (con textos en español)
    SELECT
        t.TaskId,
        t.ProjectId,
        p.Name AS ProjectName,

        t.Title,
        t.Description,

        t.AssigneeId,
        (d.FirstName + ' ' + d.LastName) AS AssigneeName,

        t.IdTaskStatus,
        st.Description AS EstadoTarea,

        t.IdTaskPriority,
        pr.Description AS Prioridad,

        t.EstimatedComplexity,
        t.DueDate,
        t.CompletionDate,
        t.CreatedAts
    FROM dbo.Tasks t
    INNER JOIN dbo.Projects p ON p.ProjectId = t.ProjectId
    INNER JOIN dbo.Developers d ON d.DeveloperId = t.AssigneeId
    INNER JOIN dbo.TB_ReferencialData_Details st ON st.Id = t.IdTaskStatus
    INNER JOIN dbo.TB_ReferencialData_Details pr ON pr.Id = t.IdTaskPriority
    WHERE t.TaskId = @NewTaskId;
END
GO


----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE OR ALTER   PROCEDURE [dbo].[TT_SP_GetProjectTasksPaged]
(
    @ProjectId      UNIQUEIDENTIFIER,
    @IdTaskStatus   UNIQUEIDENTIFIER = NULL,
    @AssigneeId     UNIQUEIDENTIFIER = NULL,
    @Page           INT = 1,
    @PageSize       INT = 10
)
AS
BEGIN
    SET NOCOUNT ON;

    -- Normalización básica
    IF (@Page IS NULL OR @Page < 1) SET @Page = 1;
    IF (@PageSize IS NULL OR @PageSize < 1) SET @PageSize = 10;
    IF (@PageSize > 100) SET @PageSize = 100;

    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    SELECT
        t.TaskId,
        t.ProjectId,
        p.Name              AS ProjectName,

        t.Title,
        t.Description,

        t.AssigneeId,
        (d.FirstName + ' ' + d.LastName) AS AssigneeName,

        t.IdTaskStatus,
        st.Description      AS EstadoTarea,

        t.IdTaskPriority,
        pr.Description      AS Prioridad,

        t.EstimatedComplexity,
        t.DueDate,
        t.CompletionDate,
        t.CreatedAts,

        COUNT(1) OVER() AS TotalCount
    FROM dbo.Tasks t
    INNER JOIN dbo.Projects p ON p.ProjectId = t.ProjectId
    INNER JOIN dbo.Developers d ON d.DeveloperId = t.AssigneeId
    INNER JOIN dbo.TB_ReferencialData_Details st ON st.Id = t.IdTaskStatus
    INNER JOIN dbo.TB_ReferencialData_Details pr ON pr.Id = t.IdTaskPriority
    WHERE
        t.ProjectId = @ProjectId
        AND (@IdTaskStatus IS NULL OR t.IdTaskStatus = @IdTaskStatus)
        AND (@AssigneeId  IS NULL OR t.AssigneeId  = @AssigneeId)
    ORDER BY
        t.DueDate ASC,
        t.CreatedAts DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
END
GO


----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
DROP VIEW [dbo].[VW_DeveloperDelayRisk]
GO

/****** Object:  View [dbo].[VW_DeveloperDelayRisk]    Script Date: 27/01/2026 12:03:23 a. m. ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


CREATE   VIEW [dbo].[VW_DeveloperDelayRisk]
AS
WITH CompletedStatus AS
(
    SELECT TOP 1 d.Id AS CompletedStatusId
    FROM dbo.TB_ReferencialData m
    INNER JOIN dbo.TB_ReferencialData_Details d
        ON d.IdReferencialData = m.Id
    WHERE m.Description = 'ESTADOS DE TAREA'
      AND d.Description = 'Completada'
      AND m.Valid = 1
      AND d.Valid = 1
),
CompletedDelays AS
(
    SELECT
        t.AssigneeId,
        CASE
            WHEN t.CompletionDate IS NULL THEN 0
            WHEN DATEDIFF(DAY, t.DueDate, t.CompletionDate) > 0
                THEN DATEDIFF(DAY, t.DueDate, t.CompletionDate)
            ELSE 0
        END AS DelayDays
    FROM dbo.Tasks t
    CROSS JOIN CompletedStatus cs
    WHERE t.IdTaskStatus = cs.CompletedStatusId
),
DelayAvg AS
(
    SELECT
        AssigneeId,
        CAST(AVG(CAST(DelayDays AS DECIMAL(10,2))) AS DECIMAL(10,2)) AS AvgDelayDays
    FROM CompletedDelays
    GROUP BY AssigneeId
),
OpenTasks AS
(
    SELECT
        t.AssigneeId,
        COUNT(1) AS OpenTasksCount,
        MIN(t.DueDate) AS NearestDueDate,
        MAX(t.DueDate) AS LatestDueDate
    FROM dbo.Tasks t
    CROSS JOIN CompletedStatus cs
    WHERE t.IdTaskStatus <> cs.CompletedStatusId
    GROUP BY t.AssigneeId
)
SELECT
    d.DeveloperId,
    (d.FirstName + ' ' + d.LastName) AS DeveloperName,

    ISNULL(o.OpenTasksCount, 0) AS OpenTasksCount,
    ISNULL(a.AvgDelayDays, 0)   AS AvgDelayDays,

    o.NearestDueDate,
    o.LatestDueDate,

    CASE
        WHEN o.LatestDueDate IS NULL THEN NULL
        ELSE DATEADD(DAY, CAST(ISNULL(a.AvgDelayDays, 0) AS INT), o.LatestDueDate)
    END AS PredictedCompletionDate,

    CASE
        WHEN o.LatestDueDate IS NULL THEN 0
        WHEN DATEADD(DAY, CAST(ISNULL(a.AvgDelayDays, 0) AS INT), o.LatestDueDate) > o.LatestDueDate THEN 1
        WHEN ISNULL(a.AvgDelayDays, 0) >= 3 THEN 1
        ELSE 0
    END AS HighRiskFlag
FROM dbo.Developers d
LEFT JOIN OpenTasks o ON o.AssigneeId = d.DeveloperId
LEFT JOIN DelayAvg a  ON a.AssigneeId = d.DeveloperId
WHERE d.IsActive = 1;
GO


--------------------------------INSERTS---------------------------------------------------------------------------------------------------------------------------
INSERT INTO dbo.TB_ReferencialData
(
    Id, [Description], AuxiliarData, Valid, RegisterDate, RegisterUser, UpdateDate, UpdateUser
)
VALUES
('10000000-0000-0000-0000-000000000001', 'ESTADOS DE PROYECTO', NULL, 1, '2026-01-24T23:44:30.297', 'SCRIPT', NULL, NULL),
('10000000-0000-0000-0000-000000000002', 'ESTADOS DE TAREA',    NULL, 1, '2026-01-24T23:44:30.297', 'SCRIPT', NULL, NULL),
('10000000-0000-0000-0000-000000000003', 'PRIORIDADES DE TAREA',NULL, 1, '2026-01-24T23:44:30.297', 'SCRIPT', NULL, NULL);
GO

/* ============================================================
   INSERTS – TB_ReferencialData_Details (Detail)
   ============================================================ */

INSERT INTO dbo.TB_ReferencialData_Details
(
    Id, IdReferencialData, [Description], AuxiliarData, Valid, RegisterDate, RegisterUser, UpdateDate, UpdateUser
)
VALUES
-- ESTADOS DE PROYECTO (100...001)
('20000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000001', 'Planeado',    NULL, 1, '2026-01-24T23:46:36.423', 'SCRIPT', NULL, NULL),
('20000000-0000-0000-0000-000000000002', '10000000-0000-0000-0000-000000000001', 'En progreso', NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('20000000-0000-0000-0000-000000000003', '10000000-0000-0000-0000-000000000001', 'Completado',  NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),

-- ESTADOS DE TAREA (100...002)
('30000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000002', 'Por hacer',   NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('30000000-0000-0000-0000-000000000002', '10000000-0000-0000-0000-000000000002', 'En progreso', NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('30000000-0000-0000-0000-000000000003', '10000000-0000-0000-0000-000000000002', 'Bloqueada',   NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('30000000-0000-0000-0000-000000000004', '10000000-0000-0000-0000-000000000002', 'Completada',  NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),

-- PRIORIDADES DE TAREA (100...003)
('40000000-0000-0000-0000-000000000001', '10000000-0000-0000-0000-000000000003', 'Baja',        NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('40000000-0000-0000-0000-000000000002', '10000000-0000-0000-0000-000000000003', 'Media',       NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),
('40000000-0000-0000-0000-000000000003', '10000000-0000-0000-0000-000000000003', 'Alta',        NULL, 1, '2026-01-24T23:46:36.427', 'SCRIPT', NULL, NULL),

-- STATUS metidos en PRIORIDADES.
('40000000-0000-0000-0000-000000000010', '10000000-0000-0000-0000-000000000003', 'Por hacer',   'STATUS', 1, '2026-01-26T20:59:17.230', 'SCRIPT', NULL, NULL),
('40000000-0000-0000-0000-000000000011', '10000000-0000-0000-0000-000000000003', 'En progreso', 'STATUS', 1, '2026-01-26T20:59:17.230', 'SCRIPT', NULL, NULL),
('40000000-0000-0000-0000-000000000012', '10000000-0000-0000-0000-000000000003', 'Bloqueada',   'STATUS', 1, '2026-01-26T20:59:17.230', 'SCRIPT', NULL, NULL),
('40000000-0000-0000-0000-000000000013', '10000000-0000-0000-0000-000000000003', 'Completada',  'STATUS', 1, '2026-01-26T20:59:17.230', 'SCRIPT', NULL, NULL);
GO

--------------------------------INSERTS---------------------------------------------------------------------------------------------------------------------------
INSERT INTO dbo.Developers
(
  DeveloperId,
  FirstName,
  LastName,
  Email,
  IsActive,
  CreatedAts
)
VALUES
('11111111-1111-1111-1111-111111111111', 'Ana',    'Gómez',  'ana.gomez@correo.com',    1, '2026-01-24T23:47:33.783'),
('22222222-2222-2222-2222-222222222222', 'Carlos', 'López',  'carlos.lopez@correo.com', 1, '2026-01-24T23:47:33.783'),
('33333333-3333-3333-3333-333333333333', 'María',  'Ruiz',   'maria.ruiz@correo.com',   1, '2026-01-24T23:47:33.783'),
('44444444-4444-4444-4444-444444444444', 'Andrés', 'Pérez',  'andres.perez@correo.com', 1, '2026-01-24T23:47:33.783'),
('55555555-5555-5555-5555-555555555555', 'Sofía',  'Torres', 'sofia.torres@correo.com', 1, '2026-01-24T23:47:33.783');
GO

--------------------------------INSERTS---------------------------------------------------------------------------------------------------------------------------

INSERT INTO dbo.Projects
(
  ProjectId,
  [Name],
  ClientName,
  StartDate,
  EndDate,
  IdProjectStatus,
  CreatedAts
)
VALUES
('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 'Plataforma CRM',           'NSERIO',    '2025-12-25', NULL, '20000000-0000-0000-0000-000000000002', '2026-01-24T23:48:55.910'),
('BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', 'Actualización Facturación','Cliente X', '2025-11-25', NULL, '20000000-0000-0000-0000-000000000002', '2026-01-24T23:48:55.910'),
('CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC', 'MVP Analítica',            'Cliente Y', '2026-01-09', NULL, '20000000-0000-0000-0000-000000000001', '2026-01-24T23:48:55.910');
GO

--------------------------------INSERTS---------------------------------------------------------------------------------------------------------------------------

INSERT INTO dbo.Tasks
(
  TaskId,
  ProjectId,
  Title,
  [Description],
  AssigneeId,
  IdTaskStatus,
  IdTaskPriority,
  EstimatedComplexity,
  DueDate,
  CompletionDate,
  CreatedAts
)
VALUES
('5492671C-699A-420D-87DB-0A0790C96C8D','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Implementar autenticación',NULL,'11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000003',4,'2026-01-14','2026-01-16','2026-01-24T23:53:30.223'),
('452C2D56-15B6-42FD-B75B-0FFE940DB3BC','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Prueba titulo','Esto es una prueba','11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000003',3,'2026-01-30','2026-01-01','2026-01-26T21:17:59.470'),
('021A3C2B-C848-41C4-92A5-149C1B477A86','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Definir KPIs',NULL,'44444444-4444-4444-4444-444444444444','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000002',2,'2026-01-19','2026-01-19','2026-01-24T23:53:30.223'),
('F4C8D1F9-743A-46EC-9462-19BF06D8B469','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Integración API pagos',NULL,'22222222-2222-2222-2222-222222222222','30000000-0000-0000-0000-000000000002','40000000-0000-0000-0000-000000000003',4,'2026-01-26',NULL,'2026-01-24T23:53:30.223'),
('FDB877FE-5ABD-415C-99D4-1E23F203568E','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Pruebas unitarias',NULL,'55555555-5555-5555-5555-555555555555','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000002',3,'2026-01-12','2026-01-12','2026-01-24T23:53:30.223'),
('70180DA5-4CFB-485C-AEA9-2914DD7076B0','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Diseñar pantallas base',NULL,'11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000002',2,'2026-01-25',NULL,'2026-01-24T23:53:30.223'),
('78481CAB-6CCE-4D5B-85B2-426C33CE04D0','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Nueva tarea de prueba','Creada desde SP','11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000002',3,'2026-01-30',NULL,'2026-01-25T15:24:52.537'),
('F283FBBA-DCDA-4A27-9DA1-468A4B0D3302','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Optimización de rendimiento',NULL,'44444444-4444-4444-4444-444444444444','30000000-0000-0000-0000-000000000002','40000000-0000-0000-0000-000000000003',5,'2026-01-30',NULL,'2026-01-24T23:53:30.223'),
('D5C723C3-60A1-4290-A8B4-4E824C41C9B8','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Documentación técnica',NULL,'33333333-3333-3333-3333-333333333333','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000001',1,'2026-02-07',NULL,'2026-01-24T23:53:30.223'),
('5B3B1035-4CE8-4205-8B6E-52E38BE661B1','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Scripts de migración',NULL,'11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000003',5,'2026-01-04','2026-01-08','2026-01-24T23:53:30.223'),
('31F03385-9221-4D81-BB61-550C5F3621F8','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Ajuste módulo facturas',NULL,'33333333-3333-3333-3333-333333333333','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000002',3,'2026-02-02',NULL,'2026-01-24T23:53:30.223'),
('AD3E0639-DBA4-4456-8959-56365347C211','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Corrección de interfaz',NULL,'22222222-2222-2222-2222-222222222222','30000000-0000-0000-0000-000000000002','40000000-0000-0000-0000-000000000002',2,'2026-01-27',NULL,'2026-01-24T23:53:30.223'),
('C3DADC44-F5DD-4234-B0D2-6F757954E652','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Manejo de reintentos',NULL,'55555555-5555-5555-5555-555555555555','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000002',2,'2026-02-04',NULL,'2026-01-24T23:53:30.223'),
('5C5B722A-A25A-40DA-B23B-7698F80B85D0','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Refactor de servicios',NULL,'44444444-4444-4444-4444-444444444444','30000000-0000-0000-0000-000000000003','40000000-0000-0000-0000-000000000001',3,'2026-02-05',NULL,'2026-01-24T23:53:30.223'),
('8E07D2CB-2351-410F-89FB-7B58DAE9C491','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Revisión final',NULL,'11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000002',3,'2026-02-01',NULL,'2026-01-24T23:53:30.223'),
('1974F265-A50F-4F6E-8E93-86D7B24F9875','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Corrección casos borde',NULL,'44444444-4444-4444-4444-444444444444','30000000-0000-0000-0000-000000000004','40000000-0000-0000-0000-000000000001',2,'2026-01-17','2026-01-18','2026-01-24T23:53:30.223'),
('82B6561F-647D-428B-96EA-8ED0744EA775','BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB','Mejorar logs',NULL,'55555555-5555-5555-5555-555555555555','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000001',1,'2026-01-29',NULL,'2026-01-24T23:53:30.223'),
('8F12092E-D603-46A4-AE39-95A6FD15D2A0','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Crear endpoint dashboard',NULL,'33333333-3333-3333-3333-333333333333','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000003',5,'2026-01-30',NULL,'2026-01-24T23:53:30.223'),
('1A3B6692-4E33-4D70-A62E-A82177922BAE','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Construir primer reporte',NULL,'22222222-2222-2222-2222-222222222222','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000003',4,'2026-01-31',NULL,'2026-01-24T23:53:30.223'),
('CE0868F5-76E2-4D52-87B8-C16AB2D0CD79','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Ajustes UX finales',NULL,'22222222-2222-2222-2222-222222222222','30000000-0000-0000-0000-000000000001','40000000-0000-0000-0000-000000000001',1,'2026-01-30',NULL,'2026-01-24T23:53:30.223'),
('C3C0E09D-9F12-4F24-BC6D-C3670CBAAE8D','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Optimizar consultas',NULL,'55555555-5555-5555-5555-555555555555','30000000-0000-0000-0000-000000000003','40000000-0000-0000-0000-000000000003',5,'2026-02-03',NULL,'2026-01-24T23:53:30.223'),
('9787DE26-94C7-424C-BAF2-D7865B54C485','CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC','Preparar datos iniciales',NULL,'33333333-3333-3333-3333-333333333333','30000000-0000-0000-0000-000000000002','40000000-0000-0000-0000-000000000002',3,'2026-01-28',NULL,'2026-01-24T23:53:30.223'),
('38884354-D96D-49F1-90D5-FC1085E57335','AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA','Titulo2','Esto es un proyecto que esta en progress','11111111-1111-1111-1111-111111111111','30000000-0000-0000-0000-000000000002','40000000-0000-0000-0000-000000000003',3,'2026-01-31',NULL,'2026-01-26T21:19:52.983');
GO
