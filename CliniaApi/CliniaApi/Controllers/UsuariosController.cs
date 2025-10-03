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
public class UsuariosController : ControllerBase
{
    private readonly ClinicaIaContext _context;

    public UsuariosController(ClinicaIaContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<UsuarioResponse>>> GetUsuarios()
    {
        var usuarios = await _context.Usuarios
            .FromSqlRaw("EXEC dbo.spObtenerUsuarios")
            .AsNoTracking()
            .ToListAsync();

        var response = usuarios.Select(ToResponse);
        return Ok(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponse>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id);

        if (usuario is null)
        {
            return NotFound(new { message = "No se encontró el usuario solicitado." });
        }

        return Ok(ToResponse(usuario));
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioResponse>> CreateUsuario([FromBody] UsuarioRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        if (string.IsNullOrWhiteSpace(request.Password))
        {
            ModelState.AddModelError(nameof(request.Password), "La contraseña es obligatoria para nuevos usuarios.");
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _context.Usuarios
                .FromSqlInterpolated($"EXEC dbo.spGuardarUsuario @Id={(int?)null}, @Correo={request.Correo}, @Password={request.Password}, @NombreCompleto={request.NombreCompleto}, @IdMedico={request.IdMedico}, @Activo={request.Activo}")
                .AsNoTracking()
                .ToListAsync();

            var usuario = result.FirstOrDefault();
            if (usuario is null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = "No fue posible registrar al usuario." });
            }

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, ToResponse(usuario));
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioResponse>> UpdateUsuario(int id, [FromBody] UsuarioRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        try
        {
            var result = await _context.Usuarios
                .FromSqlInterpolated($"EXEC dbo.spGuardarUsuario @Id={id}, @Correo={request.Correo}, @Password={request.Password}, @NombreCompleto={request.NombreCompleto}, @IdMedico={request.IdMedico}, @Activo={request.Activo}")
                .AsNoTracking()
                .ToListAsync();

            var usuario = result.FirstOrDefault();
            if (usuario is null)
            {
                return NotFound(new { message = "No se encontró el usuario a actualizar." });
            }

            return Ok(ToResponse(usuario));
        }
        catch (SqlException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario is null)
        {
            return NotFound(new { message = "No se encontró el usuario a eliminar." });
        }

        if (!usuario.Activo)
        {
            return NoContent();
        }

        usuario.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private static UsuarioResponse ToResponse(Usuario usuario) => new()
    {
        Id = usuario.Id,
        Correo = usuario.Correo,
        NombreCompleto = usuario.NombreCompleto,
        IdMedico = usuario.IdMedico,
        Activo = usuario.Activo,
        FechaCreacion = usuario.FechaCreacion
    };
}
