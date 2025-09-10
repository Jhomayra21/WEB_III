# CHANGELOG

## [0.1.2] - 2025-08-29
### Changed
- Registrar el Seeder en el program.

### Added
- Agregamos la clase abstacta IDbInitializer (explicación en la misma clase)
- Agregamos la clase Seeder para la siembre de datos, la misma contiene la lógica de la siembra de datos para la tabla Tarea

## [0.1.1] - 2025-08-28
### Changed
- Registrar el DBContext en el program.

### Added
- Crear páginas en Razor con Scaffolding (Agregar -> Nuevo elemento con Scaffolding -> Páginas Razor que usan EF CRUD -> )
- Paquete Bogus (para crear datos falsos)

## [0.1.0] - 2025-08-26
### Added
- Versión inicial del proyecto.
- Se creó el modelo inicial de Tarea.
- Se creó la estructura base de las carpetas y archivos del proyecto.
- Se instalaron las librerias necesarias para el funcionamiento.
- Se configuró la cadena de conexión para la base de datos TareaDB
- Se creó el Context para la migración
- Se realizó la migración (Add-Migration InitialCreate)
- Se realizó la actualización a TareaDB (Update-Database)

### Fixed
- Se corrigió un error en el modelo.
- Se resolvió el problema con la cadena de conexión (error certificación).