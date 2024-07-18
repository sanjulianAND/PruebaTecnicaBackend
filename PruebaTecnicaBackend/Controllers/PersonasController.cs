using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PruebaTecnicaBackend.Models;
using PruebaTecnicaBackend.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly PruebaTecnicaDbContext _context;

    public PersonasController(PruebaTecnicaDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
    {
        return await _context.Personas.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Persona>> GetPersona(int id)
    {
        var persona = await _context.Personas.FindAsync(id);

        if (persona == null)
        {
            return NotFound();
        }

        return persona;
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> PostPersona(Persona persona)
    {
        _context.Personas.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersona), new { id = persona.Identificador }, persona);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> PutPersona(int id, Persona persona)
    {
        if (id != persona.Identificador)
        {
            return BadRequest();
        }

        _context.Entry(persona).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PersonaExists(id))
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
    public async Task<IActionResult> DeletePersona(int id)
    {
        var persona = await _context.Personas.FindAsync(id);
        if (persona == null)
        {
            return NotFound();
        }

        _context.Personas.Remove(persona);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private bool PersonaExists(int id)
    {
        return _context.Personas.Any(e => e.Identificador == id);
    }
}
