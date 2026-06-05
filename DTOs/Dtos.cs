namespace GestionCursos.DTOs;

// ── Curso ──────────────────────────────────────────────
public record CursoDto(int CursoId, string Nombre, string? Descripcion,
                       DateTime FechaInicio, DateTime FechaFin);

public record CrearCursoDto(string Nombre, string? Descripcion,
                             DateTime FechaInicio, DateTime FechaFin);

// ── Estudiante ─────────────────────────────────────────
public record EstudianteDto(int EstudianteId, string Nombre, string Apellido,
                            string Email, DateTime FechaNacimiento);

public record CrearEstudianteDto(string Nombre, string Apellido,
                                  string Email, DateTime FechaNacimiento);

// ── Inscripción ────────────────────────────────────────
public record InscripcionDto(int InscripcionId, int EstudianteId,
                              string NombreEstudiante, int CursoId,
                              string NombreCurso, DateTime FechaInscripcion);

public record CrearInscripcionDto(int EstudianteId, int CursoId);
