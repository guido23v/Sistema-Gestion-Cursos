using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionCursos.Data;
using GestionCursos.DTOs;
using GestionCursos.Models;

namespace GestionCursos.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CursosController : ControllerBase
{
    private readonly AppDbContext _db;
    public CursosController(AppDbContext db) => _db = db;

    // GET api/cursos
    [HttpGet]
    public async Task<IEnumerable<CursoDto>> GetAll() =>
        await _db.Cursos
                 .Select(c => new CursoDto(c.CursoId, c.Nombre, c.Descripcion,
                                           c.FechaInicio, c.FechaFin))
                 .ToListAsync();

    // GET api/cursos/1
    [HttpGet("{id}")]
    public async Task<ActionResult<CursoDto>> GetById(int id)
    {
        var c = await _db.Cursos.FindAsync(id);
        if (c is null) return NotFound();
        return new CursoDto(c.CursoId, c.Nombre, c.Descripcion, c.FechaInicio, c.FechaFin);
    }

    // POST api/cursos
    [HttpPost]
    public async Task<ActionResult<CursoDto>> Create(CrearCursoDto dto)
    {
        var curso = new Curso
        {
            Nombre = dto.Nombre,
            Descripcion = dto.Descripcion,
            FechaInicio = dto.FechaInicio,
            FechaFin = dto.FechaFin
        };
        _db.Cursos.Add(curso);
        await _db.SaveChangesAsync();

        var result = new CursoDto(curso.CursoId, curso.Nombre, curso.Descripcion,
                                   curso.FechaInicio, curso.FechaFin);
        return CreatedAtAction(nameof(GetById), new { id = curso.CursoId }, result);
    }

    // PUT api/cursos/1
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, CrearCursoDto dto)
    {
        var curso = await _db.Cursos.FindAsync(id);
        if (curso is null) return NotFound();

        curso.Nombre = dto.Nombre;
        curso.Descripcion = dto.Descripcion;
        curso.FechaInicio = dto.FechaInicio;
        curso.FechaFin = dto.FechaFin;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE api/cursos/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var curso = await _db.Cursos.FindAsync(id);
        if (curso is null) return NotFound();

        _db.Cursos.Remove(curso);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
