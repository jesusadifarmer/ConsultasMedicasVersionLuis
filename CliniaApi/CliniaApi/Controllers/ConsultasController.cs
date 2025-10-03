using System.Linq;
using CliniaApi.Data;
using CliniaApi.Dtos;
using CliniaApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CliniaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ConsultasController : ControllerBase
{
    private readonly ClinicaIaContext _context;

    public ConsultasController(ClinicaIaContext context)
    {
        _context = context;
    }

    [HttpGet("historial")]
    public async Task<ActionResult<IEnumerable<ConsultaHistorialResponse>>> ObtenerHistorial()
    {
        var historial = await _context.ConsultasHistorial
            .FromSqlRaw("EXEC dbo.spObtenerConsultasHistorial")
            .AsNoTracking()
            .ToListAsync();

        var response = historial.Select(ToResponse);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<ConsultaHistorialResponse>> RegistrarConsulta([FromBody] RegistrarConsultaRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _context.ConsultasHistorial
                .FromSqlInterpolated($"EXEC dbo.spRegistrarConsulta @IdMedico={request.IdMedico}, @IdPaciente={request.IdPaciente}, @Sintomas={request.Sintomas}, @Recomendaciones={request.Recomendaciones}, @Diagnostico={request.Diagnostico}")
                .AsNoTracking()
                .ToListAsync();

            var consulta = result.FirstOrDefault();
            if (consulta is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "No fue posible registrar la consulta." });
            }

            return Ok(ToResponse(consulta));
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static ConsultaHistorialResponse ToResponse(ConsultaHistorialItem consulta) => new()
    {
        Id = consulta.Id,
        IdMedico = consulta.IdMedico,
        IdPaciente = consulta.IdPaciente,
        Sintomas = consulta.Sintomas,
        Recomendaciones = consulta.Recomendaciones,
        Diagnostico = consulta.Diagnostico,
        FechaCreacion = consulta.FechaCreacion,
        MedicoNombreCompleto = consulta.MedicoNombreCompleto.Trim(),
        PacienteNombreCompleto = consulta.PacienteNombreCompleto.Trim()
    };
}
