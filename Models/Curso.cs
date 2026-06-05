using System.ComponentModel.DataAnnotations;

namespace GestionCursos.Models;

public class Curso
{
    public int CursoId { get; set; }

    [Required, MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    public string? Descripcion { get; set; }

    public DateTime FechaInicio { get; set; }

    public DateTime FechaFin { get; set; }

    // Navegación
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
