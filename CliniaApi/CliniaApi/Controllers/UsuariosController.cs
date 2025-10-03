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

    /// <summary>
    /// Obtiene la lista completa de usuarios registrados en el sistema.
    /// </summary>
    /// <returns>
    /// Una respuesta HTTP 200 con la colección de usuarios encontrados.
    /// </returns>
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

    /// <summary>
    /// Obtiene la información de un usuario específico.
    /// </summary>
    /// <param name="id">Identificador del usuario a consultar.</param>
    /// <returns>
    /// Una respuesta HTTP 200 con la información del usuario, o 404 si no existe.
    /// </returns>
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

    /// <summary>
    /// Crea un nuevo usuario con la información proporcionada.
    /// </summary>
    /// <param name="request">Datos del usuario a registrar.</param>
    /// <returns>
    /// Una respuesta HTTP 201 con el usuario creado o un mensaje de error si la operación falla.
    /// </returns>
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

    /// <summary>
    /// Actualiza la información de un usuario existente.
    /// </summary>
    /// <param name="id">Identificador del usuario a actualizar.</param>
    /// <param name="request">Datos actualizados para el usuario.</param>
    /// <returns>
    /// Una respuesta HTTP 200 con el usuario actualizado o un mensaje de error si no se encuentra.
    /// </returns>
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

    /// <summary>
    /// Desactiva lógicamente a un usuario existente.
    /// </summary>
    /// <param name="id">Identificador del usuario a desactivar.</param>
    /// <returns>
    /// Una respuesta HTTP 204 si la operación se ejecuta correctamente o 404 si no existe.
    /// </returns>
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

    /// <summary>
    /// Convierte un modelo de dominio <see cref="Usuario"/> en su representación de respuesta.
    /// </summary>
    /// <param name="usuario">Entidad de usuario a transformar.</param>
    /// <returns>Un objeto listo para ser devuelto al cliente.</returns>
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
