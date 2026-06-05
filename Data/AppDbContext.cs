using Microsoft.EntityFrameworkCore;
using GestionCursos.Models;

namespace GestionCursos.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Inscripcion> Inscripciones => Set<Inscripcion>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Unicidad: un estudiante no puede inscribirse dos veces al mismo curso
        modelBuilder.Entity<Inscripcion>()
            .HasIndex(i => new { i.EstudianteId, i.CursoId })
            .IsUnique();

        // Seed de datos de prueba
        modelBuilder.Entity<Curso>().HasData(
            new Curso { CursoId = 1, Nombre = "Matemáticas I", Descripcion = "Álgebra y cálculo básico",
                        FechaInicio = new DateTime(2025, 1, 15), FechaFin = new DateTime(2025, 6, 15) },
            new Curso { CursoId = 2, Nombre = "Programación .NET", Descripcion = "Desarrollo con C# y ASP.NET Core",
                        FechaInicio = new DateTime(2025, 2, 1),  FechaFin = new DateTime(2025, 7, 1) }
        );

        modelBuilder.Entity<Estudiante>().HasData(
            new Estudiante { EstudianteId = 1, Nombre = "Ana", Apellido = "García",
                             Email = "ana@mail.com", FechaNacimiento = new DateTime(2000, 5, 20) },
            new Estudiante { EstudianteId = 2, Nombre = "Luis", Apellido = "Pérez",
                             Email = "luis@mail.com", FechaNacimiento = new DateTime(1999, 11, 3) }
        );
    }
}
