using System;
using System.Collections.Generic;

namespace CliniaApi.Models;

public class Medico
{
    public int Id { get; set; }
    public string PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; }
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string? ApellidoMaterno { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public string? Telefono { get; set; }
    public string? Especialidad { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
