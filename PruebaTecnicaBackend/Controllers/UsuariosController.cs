using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackend.Models;
using PruebaTecnicaBackend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly PruebaTecnicaDbContext _context;

    public UsuariosController(PruebaTecnicaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
    {
        return await _context.Usuarios.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Usuario>> GetUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);

        if (usuario == null)
        {
            return NotFound();
        }

        return usuario;
    }

    [HttpPost]
    public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
    {
        usuario.FechaCreacion = DateTime.Now;
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Identificador }, usuario);
    }


    [HttpPut("{id}")]
    public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
    {
        if (id != usuario.Identificador)
        {
            return BadRequest();
        }

        _context.Entry(usuario).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!UsuarioExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUsuario(int id)
    {
        var usuario = await _context.Usuarios.FindAsync(id);
        if (usuario == null)
        {
            return NotFound();
        }

        _context.Usuarios.Remove(usuario);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool UsuarioExists(int id)
    {
        return _context.Usuarios.Any(e => e.Identificador == id);
    }
}
