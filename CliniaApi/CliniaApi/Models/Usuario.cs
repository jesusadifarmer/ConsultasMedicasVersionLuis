using System;

namespace CliniaApi.Models;

public class Usuario
{
    public int Id { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int? IdMedico { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Medico? Medico { get; set; }
}
