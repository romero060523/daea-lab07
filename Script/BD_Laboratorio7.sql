/* =========================================================
   Laboratorio 07 - CRUD en capas de Persona
   Base de datos : BD_Laboratorio7
   Servidor      : LAB1506-11\SQLEXPRESS01
   ========================================================= */

-- Requerido por el índice filtrado (y por los SP que modifican la tabla)
SET ANSI_NULLS ON;
SET QUOTED_IDENTIFIER ON;
GO

IF DB_ID('BD_Laboratorio7') IS NULL
    CREATE DATABASE BD_Laboratorio7;
GO

USE BD_Laboratorio7;
GO

/* ---------------------------------------------------------
   Tabla Persona (5 campos + flag de eliminación lógica)
   --------------------------------------------------------- */
IF OBJECT_ID('dbo.Persona', 'U') IS NOT NULL
    DROP TABLE dbo.Persona;
GO

CREATE TABLE dbo.Persona
(
    IdPersona  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Persona PRIMARY KEY,
    Nombres    NVARCHAR(100)     NOT NULL,
    Apellidos  NVARCHAR(100)     NOT NULL,
    DNI        CHAR(8)           NOT NULL,
    Email      NVARCHAR(150)     NULL,
    Eliminado  BIT               NOT NULL CONSTRAINT DF_Persona_Eliminado DEFAULT (0)
);
GO

-- El DNI es único solo entre los registros activos
CREATE UNIQUE INDEX UX_Persona_DNI_Activo
    ON dbo.Persona (DNI)
    WHERE Eliminado = 0;
GO

/* ---------------------------------------------------------
   Procedimientos almacenados
   --------------------------------------------------------- */
CREATE OR ALTER PROCEDURE dbo.sp_Persona_Listar
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPersona, Nombres, Apellidos, DNI, Email, Eliminado
    FROM dbo.Persona
    WHERE Eliminado = 0
    ORDER BY Apellidos, Nombres;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Persona_Buscar
    @Texto NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT IdPersona, Nombres, Apellidos, DNI, Email, Eliminado
    FROM dbo.Persona
    WHERE Eliminado = 0
      AND (    Nombres   LIKE '%' + @Texto + '%'
            OR Apellidos LIKE '%' + @Texto + '%'
            OR DNI       LIKE '%' + @Texto + '%')
    ORDER BY Apellidos, Nombres;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Persona_Insertar
    @Nombres   NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @DNI       CHAR(8),
    @Email     NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO dbo.Persona (Nombres, Apellidos, DNI, Email)
    VALUES (@Nombres, @Apellidos, @DNI, @Email);

    SELECT CAST(SCOPE_IDENTITY() AS INT) AS IdPersona;
END
GO

CREATE OR ALTER PROCEDURE dbo.sp_Persona_Actualizar
    @IdPersona INT,
    @Nombres   NVARCHAR(100),
    @Apellidos NVARCHAR(100),
    @DNI       CHAR(8),
    @Email     NVARCHAR(150) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Persona
    SET Nombres   = @Nombres,
        Apellidos = @Apellidos,
        DNI       = @DNI,
        Email     = @Email
    WHERE IdPersona = @IdPersona
      AND Eliminado = 0;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

-- Eliminación lógica: no se borra el registro, solo se marca
CREATE OR ALTER PROCEDURE dbo.sp_Persona_Eliminar
    @IdPersona INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE dbo.Persona
    SET Eliminado = 1
    WHERE IdPersona = @IdPersona
      AND Eliminado = 0;

    SELECT @@ROWCOUNT AS FilasAfectadas;
END
GO

/* ---------------------------------------------------------
   Datos de prueba
   --------------------------------------------------------- */
INSERT INTO dbo.Persona (Nombres, Apellidos, DNI, Email) VALUES
(N'Juan Carlos', N'Pérez García',     '45871236', N'jperez@gmail.com'),
(N'María Elena', N'Quispe Mamani',    '70125489', N'mquispe@hotmail.com'),
(N'Luis Alberto', N'Torres Huamán',   '41236985', N'ltorres@outlook.com'),
(N'Ana Lucía',   N'Flores Rojas',     '72589641', N'aflores@gmail.com'),
(N'Pedro José',  N'Ramírez Castillo', '46987123', NULL);
GO
