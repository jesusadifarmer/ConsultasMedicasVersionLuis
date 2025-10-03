using System;
using System.Collections.Generic;

namespace CliniaApi.Models;

public class Paciente
{
    public int Id { get; set; }
    public string PrimerNombre { get; set; } = string.Empty;
    public string? SegundoNombre { get; set; }
    public string ApellidoPaterno { get; set; } = string.Empty;
    public string? ApellidoMaterno { get; set; }
    public string? Telefono { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }

    public ICollection<Consulta> Consultas { get; set; } = new List<Consulta>();
}
