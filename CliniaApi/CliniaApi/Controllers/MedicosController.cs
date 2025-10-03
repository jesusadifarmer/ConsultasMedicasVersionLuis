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
public class MedicosController : ControllerBase
{
    private readonly ClinicaIaContext _context;

    public MedicosController(ClinicaIaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MedicoResponse>>> GetMedicos()
    {
        var medicos = await _context.Medicos
            .FromSqlRaw("EXEC dbo.spObtenerMedicos")
            .AsNoTracking()
            .ToListAsync();

        var response = medicos.Select(ToResponse);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MedicoResponse>> GetMedico(int id)
    {
        var medico = await _context.Medicos
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);

        if (medico is null)
        {
            return NotFound(new { message = "No se encontró el médico solicitado." });
        }

        return Ok(ToResponse(medico));
    }

    [HttpPost]
    public async Task<ActionResult<MedicoResponse>> CreateMedico([FromBody] MedicoRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _context.Medicos
                .FromSqlInterpolated($"EXEC dbo.spGuardarMedico @Id={(int?)null}, @PrimerNombre={request.PrimerNombre}, @SegundoNombre={request.SegundoNombre}, @ApellidoPaterno={request.ApellidoPaterno}, @ApellidoMaterno={request.ApellidoMaterno}, @Cedula={request.Cedula}, @Telefono={request.Telefono}, @Especialidad={request.Especialidad}, @Email={request.Email}, @Activo={request.Activo}")
                .AsNoTracking()
                .ToListAsync();

            var medico = result.FirstOrDefault();
            if (medico is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "No fue posible registrar al médico." });
            }

            return CreatedAtAction(nameof(GetMedico), new { id = medico.Id }, ToResponse(medico));
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<MedicoResponse>> UpdateMedico(int id, [FromBody] MedicoRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _context.Medicos
                .FromSqlInterpolated($"EXEC dbo.spGuardarMedico @Id={id}, @PrimerNombre={request.PrimerNombre}, @SegundoNombre={request.SegundoNombre}, @ApellidoPaterno={request.ApellidoPaterno}, @ApellidoMaterno={request.ApellidoMaterno}, @Cedula={request.Cedula}, @Telefono={request.Telefono}, @Especialidad={request.Especialidad}, @Email={request.Email}, @Activo={request.Activo}")
                .AsNoTracking()
                .ToListAsync();

            var medico = result.FirstOrDefault();
            if (medico is null)
            {
                return NotFound(new { message = "No se encontró el médico a actualizar." });
            }

            return Ok(ToResponse(medico));
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteMedico(int id)
    {
        var medico = await _context.Medicos.FirstOrDefaultAsync(m => m.Id == id);
        if (medico is null)
        {
            return NotFound(new { message = "No se encontró el médico a eliminar." });
        }

        if (!medico.Activo)
        {
            return NoContent();
        }

        medico.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static MedicoResponse ToResponse(Medico medico) => new()
    {
        Id = medico.Id,
        PrimerNombre = medico.PrimerNombre,
        SegundoNombre = medico.SegundoNombre,
        ApellidoPaterno = medico.ApellidoPaterno,
        ApellidoMaterno = medico.ApellidoMaterno,
        Cedula = medico.Cedula,
        Telefono = medico.Telefono,
        Especialidad = medico.Especialidad,
        Email = medico.Email,
        Activo = medico.Activo,
        FechaCreacion = medico.FechaCreacion
    };
}
