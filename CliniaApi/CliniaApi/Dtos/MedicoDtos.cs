using System;
using System.ComponentModel.DataAnnotations;

namespace CliniaApi.Dtos;

public class MedicoRequest
{
    [Required]
    [StringLength(100)]
    public string PrimerNombre { get; set; } = string.Empty;

    [StringLength(100)]
    public string? SegundoNombre { get; set; }

    [Required]
    [StringLength(100)]
    public string ApellidoPaterno { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ApellidoMaterno { get; set; }

    [Required]
    [StringLength(50)]
    public string Cedula { get; set; } = string.Empty;

    [StringLength(30)]
    public string? Telefono { get; set; }

    [StringLength(150)]
    public string? Especialidad { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}

public class MedicoResponse
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
}
