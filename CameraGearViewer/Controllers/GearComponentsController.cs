using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CameraGearViewer.Classes;

namespace CameraGearViewer.Controllers
{
    [Route("api/controller")]
    [ApiController]
    public class GearComponentsController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public GearComponentsController(DatabaseContext context)
        {
            _context = context;
        }

        // GET: api/GearComponents
        [HttpGet]
        public IEnumerable<GearComponent> GetGearComponents()
        {
            return _context.GearComponents;
        }

        // GET: api/GearComponents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGearComponent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gearComponent = await _context.GearComponents.FindAsync(id);

            if (gearComponent == null)
            {
                return NotFound();
            }

            return Ok(gearComponent);
        }

        // PUT: api/GearComponents/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGearComponent([FromRoute] long id, [FromBody] GearComponent gearComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gearComponent.Id)
            {
                return BadRequest();
            }

            _context.Entry(gearComponent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GearComponentExists(id))
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

        // POST: api/GearComponents
        [HttpPost]
        public async Task<IActionResult> PostGearComponent([FromBody] GearComponent gearComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.GearComponents.Add(gearComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGearComponent", new { id = gearComponent.Id }, gearComponent);
        }

        // DELETE: api/GearComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGearComponent([FromRoute] long id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gearComponent = await _context.GearComponents.FindAsync(id);
            if (gearComponent == null)
            {
                return NotFound();
            }

            _context.GearComponents.Remove(gearComponent);
            await _context.SaveChangesAsync();

            return Ok(gearComponent);
        }

        private bool GearComponentExists(long id)
        {
            return _context.GearComponents.Any(e => e.Id == id);
        }
    }
}