using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PruebaTecnicaBackend.Models;
using PruebaTecnicaBackend.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly PruebaTecnicaDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthController(PruebaTecnicaDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.NombreUsuario == loginModel.NombreUsuario && u.Pass == loginModel.Pass);

        if (user == null)
            return Unauthorized();

        var token = GenerateJwtToken(user);

        return Ok(new { Token = token });
    }

    private string GenerateJwtToken(Usuario user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.NombreUsuario),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddDays(Convert.ToDouble(_configuration["Jwt:ExpireDays"])),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}

public class LoginModel
{
    public string NombreUsuario { get; set; }
    public string Pass { get; set; }
}
