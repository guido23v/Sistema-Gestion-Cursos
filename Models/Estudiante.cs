using System.ComponentModel.DataAnnotations;

namespace GestionCursos.Models;

public class Estudiante
{
    public int EstudianteId { get; set; }

    [Required, MaxLength(80)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(80)]
    public string Apellido { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    public DateTime FechaNacimiento { get; set; }

    // Navegación
    public ICollection<Inscripcion> Inscripciones { get; set; } = new List<Inscripcion>();
}
