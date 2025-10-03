using System.Linq;
using CliniaApi.Data;
using CliniaApi.Dtos;
using CliniaApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly ClinicaIaContext _context;

    public PacientesController(ClinicaIaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PacienteResponse>>> GetPacientes()
    {
        var pacientes = await _context.Pacientes
            .AsNoTracking()
            .Where(p => p.Activo)
            .OrderBy(p => p.PrimerNombre)
            .ThenBy(p => p.ApellidoPaterno)
            .ThenBy(p => p.ApellidoMaterno)
            .ToListAsync();

        var response = pacientes.Select(ToResponse);
        return Ok(response);
    }

    private static PacienteResponse ToResponse(Paciente paciente) => new()
    {
        Id = paciente.Id,
        PrimerNombre = paciente.PrimerNombre,
        SegundoNombre = paciente.SegundoNombre,
        ApellidoPaterno = paciente.ApellidoPaterno,
        ApellidoMaterno = paciente.ApellidoMaterno,
        Telefono = paciente.Telefono,
        Activo = paciente.Activo,
        FechaCreacion = paciente.FechaCreacion
    };
}
