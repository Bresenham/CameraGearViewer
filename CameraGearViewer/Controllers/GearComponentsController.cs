using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CameraGearViewer.Classes;
using System.Reflection;

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

        // "/orderBy=price&direction=asc"
        [HttpGet("sorted")]
        public IEnumerable<GearComponent> GetGearComponentsSorted([FromQuery] string orderBy, [FromQuery] string direction)
        {
            if (orderBy == null) return _context.GearComponents;

            PropertyInfo prop = typeof(GearComponent).GetProperty(orderBy);

            if (direction.Equals("asc"))
                return _context.GearComponents.OrderBy(x => prop.GetValue(x, null));
            else
                return _context.GearComponents.OrderByDescending(x => prop.GetValue(x, null));
        }

        // "/filterText=ABCD
        [HttpGet("filtered")]
        public IEnumerable<GearComponent> GetGearComponentsFiltered([FromQuery] string filterText)
        {
            var filter = filterText.ToLower();
            return _context.GearComponents.Where(x => x.Name.ToLower().Contains(filter));
        }

        // GET: api/GearComponents/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetGearComponent([FromRoute] string id)
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
        public async Task<IActionResult> PutGearComponent([FromRoute] string id, [FromBody] GearComponent gearComponent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gearComponent.ForumLink)
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
            if (!ModelState.IsValid || GearComponentExists(gearComponent.ForumLink))
            {
                return BadRequest(ModelState);
            }

            _context.GearComponents.Add(gearComponent);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGearComponent", new { id = gearComponent.ForumLink }, gearComponent);
        }

        // DELETE: api/GearComponents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGearComponent([FromRoute] string id)
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

        private bool GearComponentExists(string id)
        {
            return _context.GearComponents.Any(e => e.ForumLink == id);
        }
    }
}