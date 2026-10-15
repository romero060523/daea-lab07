# Laboratorio 07 - CRUD en capas de Persona

Aplicación WPF (.NET 8) con arquitectura en capas, ADO.NET y eliminación lógica.

## Estructura

| Proyecto | Capa | Contenido |
|---|---|---|
| `Entidades` | Entidades | Clase `Persona` |
| `Datos` | Acceso a datos (ADO.NET) | `Conexion`, `DPersona` (procedimientos almacenados) |
| `Negocio` | Lógica de negocio | `NPersona` (validaciones y reglas) |
| `laboratorio7` | Presentación (WPF) | `MainWindow` (CRUD + buscador) |

## Base de datos

Script: `Script/BD_Laboratorio7.sql`. Crea la BD `BD_Laboratorio7`, la tabla `Persona`, los procedimientos almacenados y datos de prueba.

Tabla `Persona`: `IdPersona`, `Nombres`, `Apellidos`, `DNI`, `Email` y `Eliminado` (bit para la eliminación lógica).

```
sqlcmd -S "LAB1506-11\SQLEXPRESS01" -E -C -i Script\BD_Laboratorio7.sql
```

O ábrelo en SSMS y ejecútalo (F5).

## Ejecución

1. Ajustar la cadena de conexión en `laboratorio7/Datos/Conexion.cs` si el servidor es distinto.
2. Abrir `laboratorio7/laboratorio7.sln` en Visual Studio y ejecutar (F5).
