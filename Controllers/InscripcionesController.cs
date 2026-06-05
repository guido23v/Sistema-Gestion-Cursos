using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCursos.Data;
using GestionCursos.DTOs;
using GestionCursos.Models;

namespace GestionCursos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InscripcionesController : ControllerBase
{
    private readonly AppDbContext _db;
    public InscripcionesController(AppDbContext db) => _db = db;

    // GET api/inscripciones
    [HttpGet]
    public async Task<IEnumerable<InscripcionDto>> GetAll() =>
        await _db.Inscripciones
                 .Include(i => i.Estudiante)
                 .Include(i => i.Curso)
                 .Select(i => new InscripcionDto(
                     i.InscripcionId,
                     i.EstudianteId,
                     i.Estudiante.Nombre + " " + i.Estudiante.Apellido,
                     i.CursoId,
                     i.Curso.Nombre,
                     i.FechaInscripcion))
                 .ToListAsync();

    // GET api/inscripciones/estudiante/1  → cursos de un estudiante
    [HttpGet("estudiante/{estudianteId}")]
    public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetByEstudiante(int estudianteId)
    {
        var existe = await _db.Estudiantes.AnyAsync(e => e.EstudianteId == estudianteId);
        if (!existe) return NotFound("Estudiante no encontrado.");

        var lista = await _db.Inscripciones
                             .Include(i => i.Estudiante)
                             .Include(i => i.Curso)
                             .Where(i => i.EstudianteId == estudianteId)
                             .Select(i => new InscripcionDto(
                                 i.InscripcionId, i.EstudianteId,
                                 i.Estudiante.Nombre + " " + i.Estudiante.Apellido,
                                 i.CursoId, i.Curso.Nombre, i.FechaInscripcion))
                             .ToListAsync();
        return lista;
    }

    // GET api/inscripciones/curso/1  → estudiantes de un curso
    [HttpGet("curso/{cursoId}")]
    public async Task<ActionResult<IEnumerable<InscripcionDto>>> GetByCurso(int cursoId)
    {
        var existe = await _db.Cursos.AnyAsync(c => c.CursoId == cursoId);
        if (!existe) return NotFound("Curso no encontrado.");

        var lista = await _db.Inscripciones
                             .Include(i => i.Estudiante)
                             .Include(i => i.Curso)
                             .Where(i => i.CursoId == cursoId)
                             .Select(i => new InscripcionDto(
                                 i.InscripcionId, i.EstudianteId,
                                 i.Estudiante.Nombre + " " + i.Estudiante.Apellido,
                                 i.CursoId, i.Curso.Nombre, i.FechaInscripcion))
                             .ToListAsync();
        return lista;
    }

    // POST api/inscripciones
    [HttpPost]
    public async Task<ActionResult<InscripcionDto>> Create(CrearInscripcionDto dto)
    {
        // Validaciones: existencia de estudiante y curso
        var estudianteExiste = await _db.Estudiantes.AnyAsync(e => e.EstudianteId == dto.EstudianteId);
        if (!estudianteExiste) return BadRequest("Estudiante no encontrado.");

        var cursoExiste = await _db.Cursos.AnyAsync(c => c.CursoId == dto.CursoId);
        if (!cursoExiste) return BadRequest("Curso no encontrado.");

        // Evitar duplicados (un estudiante por curso)
        var yaInscrito = await _db.Inscripciones
                                  .AnyAsync(i => i.EstudianteId == dto.EstudianteId
                                              && i.CursoId == dto.CursoId);
        if (yaInscrito) return Conflict("El estudiante ya está inscrito en este curso.");

        // Crear inscripción
        var nuevaInscripcion = new Inscripcion
        {
            EstudianteId = dto.EstudianteId,
            CursoId = dto.CursoId,
            FechaInscripcion = DateTime.UtcNow
        };
        _db.Inscripciones.Add(nuevaInscripcion);
        await _db.SaveChangesAsync();

        // Cargar relaciones para armar el DTO de respuesta
        await _db.Entry(nuevaInscripcion).Reference(i => i.Estudiante).LoadAsync();
        await _db.Entry(nuevaInscripcion).Reference(i => i.Curso).LoadAsync();

        var result = new InscripcionDto(
            nuevaInscripcion.InscripcionId,
            nuevaInscripcion.EstudianteId,
            nuevaInscripcion.Estudiante.Nombre + " " + nuevaInscripcion.Estudiante.Apellido,
            nuevaInscripcion.CursoId,
            nuevaInscripcion.Curso.Nombre,
            nuevaInscripcion.FechaInscripcion);

        // Devolver 201 apuntando a la lista de inscripciones del estudiante
        return CreatedAtAction(nameof(GetByEstudiante), new { estudianteId = nuevaInscripcion.EstudianteId }, result);
    }

    // DELETE api/inscripciones/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var inscripcion = await _db.Inscripciones.FindAsync(id);
        if (inscripcion is null) return NotFound();

        _db.Inscripciones.Remove(inscripcion);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
