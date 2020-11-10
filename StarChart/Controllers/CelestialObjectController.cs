using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var celestialObject = await _context.CelestialObjects.FirstOrDefaultAsync(c => c.Id == id);

            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = await _context.CelestialObjects.Where(c => c.Id != id).ToListAsync();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            var celestialObject = await _context.CelestialObjects.FirstOrDefaultAsync(c => c.Name == name);

            if (celestialObject == null)
            {
                return NotFound();
            }

            celestialObject.Satellites = await _context.CelestialObjects
                .Where(c => c.Id == celestialObject.Id)
                .ToListAsync();

            return Ok(celestialObject);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var celestialObjects = await _context.CelestialObjects.ToListAsync();

            foreach (var celestialObject in celestialObjects)
            {
                celestialObject.Satellites = await _context.CelestialObjects.Where(c => c.Id != celestialObject.Id).ToListAsync();
            }

            return Ok(celestialObjects);
        }
    }
}
