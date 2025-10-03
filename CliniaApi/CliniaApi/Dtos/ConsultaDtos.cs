using System;
using System.ComponentModel.DataAnnotations;

namespace CliniaApi.Dtos;

public class RegistrarConsultaRequest
{
    [Required]
    public int IdMedico { get; set; }

    [Required]
    public int IdPaciente { get; set; }

    public string? Sintomas { get; set; }
    public string? Recomendaciones { get; set; }
    public string? Diagnostico { get; set; }
}

public class ConsultaHistorialResponse
{
    public int Id { get; set; }
    public int IdMedico { get; set; }
    public int IdPaciente { get; set; }
    public string? Sintomas { get; set; }
    public string? Recomendaciones { get; set; }
    public string? Diagnostico { get; set; }
    public DateTime FechaCreacion { get; set; }
    public string MedicoNombreCompleto { get; set; } = string.Empty;
    public string PacienteNombreCompleto { get; set; } = string.Empty;
}
