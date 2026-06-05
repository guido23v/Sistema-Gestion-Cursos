# Sistema de Gestión de Cursos — .NET 8

## Requisitos
- .NET SDK 8.0  →  https://dotnet.microsoft.com/download

## Ejecutar el proyecto

```bash
# 1. Restaurar paquetes y crear la base de datos (SQLite)
cd GestionCursos
dotnet ef migrations add InitialCreate
dotnet ef database update

# 2. Iniciar la API
dotnet run
```

> La app también aplica migraciones automáticamente al arrancar  
> gracias a `db.Database.Migrate()` en `Program.cs`.

Swagger UI disponible en: **http://localhost:5000/swagger**

---

## Endpoints

### Cursos  `/api/cursos`
| Método | Ruta              | Descripción          |
|--------|-------------------|----------------------|
| GET    | /api/cursos        | Listar todos         |
| GET    | /api/cursos/{id}   | Obtener por ID       |
| POST   | /api/cursos        | Crear curso          |
| PUT    | /api/cursos/{id}   | Actualizar curso     |
| DELETE | /api/cursos/{id}   | Eliminar curso       |

### Estudiantes  `/api/estudiantes`
| Método | Ruta                    | Descripción            |
|--------|-------------------------|------------------------|
| GET    | /api/estudiantes         | Listar todos           |
| GET    | /api/estudiantes/{id}    | Obtener por ID         |
| POST   | /api/estudiantes         | Crear estudiante       |
| PUT    | /api/estudiantes/{id}    | Actualizar estudiante  |
| DELETE | /api/estudiantes/{id}    | Eliminar estudiante    |

### Inscripciones  `/api/inscripciones`
| Método | Ruta                                  | Descripción                     |
|--------|---------------------------------------|---------------------------------|
| GET    | /api/inscripciones                    | Listar todas                    |
| GET    | /api/inscripciones/estudiante/{id}    | Cursos de un estudiante         |
| GET    | /api/inscripciones/curso/{id}         | Estudiantes de un curso         |
| POST   | /api/inscripciones                    | Inscribir estudiante en curso   |
| DELETE | /api/inscripciones/{id}               | Eliminar inscripción            |

---

## Estructura del proyecto

```
GestionCursos/
├── Controllers/
│   ├── CursosController.cs
│   ├── EstudiantesController.cs
│   └── InscripcionesController.cs
├── Data/
│   └── AppDbContext.cs
├── DTOs/
│   └── Dtos.cs
├── Models/
│   ├── Curso.cs
│   ├── Estudiante.cs
│   └── Inscripcion.cs
├── Program.cs
└── GestionCursos.csproj
```

## Tablas en la base de datos

| Tabla         | Columnas principales                                              |
|---------------|-------------------------------------------------------------------|
| Cursos        | curso_id, nombre, descripcion, fecha_inicio, fecha_fin            |
| Estudiantes   | estudiante_id, nombre, apellido, email, fecha_nacimiento          |
| Inscripciones | inscripcion_id, estudiante_id (FK), curso_id (FK), fecha_inscripcion |
