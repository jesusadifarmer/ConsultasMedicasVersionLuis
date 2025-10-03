using System;
using System.ComponentModel.DataAnnotations;

namespace CliniaApi.Dtos;

public class UsuarioRequest
{
    [Required]
    [EmailAddress]
    [StringLength(256)]
    public string Correo { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string NombreCompleto { get; set; } = string.Empty;

    [StringLength(200, MinimumLength = 6)]
    public string? Password { get; set; }

    public int? IdMedico { get; set; }

    public bool Activo { get; set; } = true;
}

public class UsuarioResponse
{
    public int Id { get; set; }
    public string Correo { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public int? IdMedico { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
}
