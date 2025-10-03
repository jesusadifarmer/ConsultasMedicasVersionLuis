using CliniaApi.Data;
using CliniaApi.Dtos;
using CliniaApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CliniaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ClinicaIaContext _context;

    public AuthController(ClinicaIaContext context)
    {
        _context = context;
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
    {
        if (!ModelState.IsValid)
        {
            return ValidationProblem(ModelState);
        }

        var user = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Correo == request.Email && u.Activo);

        if (user is null)
        {
            return Unauthorized(new { message = "Credenciales incorrectas." });
        }

        if (!PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Unauthorized(new { message = "Credenciales incorrectas." });
        }

        var response = new LoginResponse
        {
            Id = user.Id,
            Email = user.Correo,
            FullName = user.NombreCompleto
        };

        return Ok(response);
    }
}
