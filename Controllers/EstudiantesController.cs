using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCursos.Data;
using GestionCursos.DTOs;
using GestionCursos.Models;

namespace GestionCursos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly AppDbContext _db;
    public EstudiantesController(AppDbContext db) => _db = db;

    // GET api/estudiantes
    [HttpGet]
    public async Task<IEnumerable<EstudianteDto>> GetAll() =>
        await _db.Estudiantes
                 .Select(e => new EstudianteDto(e.EstudianteId, e.Nombre, e.Apellido,
                                                e.Email, e.FechaNacimiento))
                 .ToListAsync();

    // GET api/estudiantes/1
    [HttpGet("{id}")]
    public async Task<ActionResult<EstudianteDto>> GetById(int id)
    {
        var e = await _db.Estudiantes.FindAsync(id);
        if (e is null) return NotFound();
        return new EstudianteDto(e.EstudianteId, e.Nombre, e.Apellido, e.Email, e.FechaNacimiento);
    }

    // POST api/estudiantes
    [HttpPost]
    public async Task<ActionResult<EstudianteDto>> Create(CrearEstudianteDto dto)
    {
        var estudiante = new Estudiante
        {
            Nombre = dto.Nombre,
            Apellido = dto.Apellido,
            Email = dto.Email,
            FechaNacimiento = dto.FechaNacimiento
        };
        _db.Estudiantes.Add(estudiante);
        await _db.SaveChangesAsync();

        var result = new EstudianteDto(estudiante.EstudianteId, estudiante.Nombre,
                                       estudiante.Apellido, estudiante.Email,
                                       estudiante.FechaNacimiento);
        return CreatedAtAction(nameof(GetById), new { id = estudiante.EstudianteId }, result);
    }

    // PUT api/estudiantes/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CrearEstudianteDto dto)
    {
        var estudiante = await _db.Estudiantes.FindAsync(id);
        if (estudiante is null) return NotFound();

        estudiante.Nombre = dto.Nombre;
        estudiante.Apellido = dto.Apellido;
        estudiante.Email = dto.Email;
        estudiante.FechaNacimiento = dto.FechaNacimiento;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/estudiantes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var estudiante = await _db.Estudiantes.FindAsync(id);
        if (estudiante is null) return NotFound();

        _db.Estudiantes.Remove(estudiante);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
