using System;

namespace CliniaApi.Models;

public class Consulta
{
    public int Id { get; set; }
    public int IdMedico { get; set; }
    public int IdPaciente { get; set; }
    public string? Sintomas { get; set; }
    public string? Recomendaciones { get; set; }
    public string? Diagnostico { get; set; }
    public DateTime FechaCreacion { get; set; }

    public Medico? Medico { get; set; }
    public Paciente? Paciente { get; set; }
}
