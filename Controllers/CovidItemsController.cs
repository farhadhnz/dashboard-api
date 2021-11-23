using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dashboard_api.Models;

namespace dashboard_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CovidItemsController : ControllerBase
    {
        private readonly CovidContext _context;

        public CovidItemsController(CovidContext context)
        {
            _context = context;
        }

        // GET: api/CovidItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CovidItem>>> GetCovidItems()
        {
            return await _context.CovidItems.ToListAsync();
        }

        // GET: api/CovidItems/5
        // [HttpGet("{id}")]
        // public async Task<ActionResult<CovidItem>> GetCovidItem(long id)
        // {
        //     var covidItem = await _context.CovidItems.FindAsync(id);

        //     if (covidItem == null)
        //     {
        //         return NotFound();
        //     }

        //     return covidItem;
        // }

        [HttpGet("location/{id}")]
        public async Task<ActionResult<IEnumerable<CovidItem>>> GetLocationData(string id)
        {
            var covidItems = await _context.CovidItems
                            .Where(x => x.IsoCode == id.ToUpper())
                            .ToListAsync();

            return covidItems;
        }

        // PUT: api/CovidItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCovidItem(long id, CovidItem covidItem)
        {
            if (id != covidItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(covidItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CovidItemExists(id))
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

        // POST: api/CovidItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CovidItem>> PostCovidItem(CovidItem covidItem)
        {
            _context.CovidItems.Add(covidItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCovidItem", new { id = covidItem.Id }, covidItem);
        }

        // DELETE: api/CovidItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCovidItem(long id)
        {
            var covidItem = await _context.CovidItems.FindAsync(id);
            if (covidItem == null)
            {
                return NotFound();
            }

            _context.CovidItems.Remove(covidItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CovidItemExists(long id)
        {
            return _context.CovidItems.Any(e => e.Id == id);
        }
    }
}
