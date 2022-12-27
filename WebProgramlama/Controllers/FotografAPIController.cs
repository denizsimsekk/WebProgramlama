using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebProgramlama.Data;
using WebProgramlama.Models;

namespace WebProgramlama.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FotografAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public FotografAPIController(ApplicationDbContext dbContext)
        {
            _context = dbContext;
        }
        [HttpGet]
        public async Task<ActionResult<List<Fotograf>>> Get()
        {
            var y = await _context.Fotograflar.ToListAsync();
            if (y is null)
            {
                return NoContent();
            }
            return y;

        }

        // GET api/<ValuesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Fotograf>> Get(int id)
        {
            var y = await _context.Fotograflar.FirstOrDefaultAsync(x => x.FotografId == id);
            if (y is null)
            {
                return NoContent();
            }
            return y;
        }

        // POST api/<ValuesController>
        [HttpPost]
        public IActionResult Post([FromBody] Fotograf y)
        {
            _context.Fotograflar.Add(y);
            _context.SaveChanges();
            return Ok();
        }

        // PUT api/<ValuesController>/5
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Fotograf y)
        {
            var y1 = _context.Fotograflar.FirstOrDefault(x => x.FotografId == id);
            if (y1 is null)
            {
                return NotFound();
            }
            y1.FotografAciklamasi = y.FotografAciklamasi;
            y1.KategoriID = y.KategoriID;
            _context.Update(y1);
            _context.SaveChanges();
            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var y1 = _context.Fotograflar.FirstOrDefault(x => x.FotografId == id);
            if (y1 is null)
            {
                return NotFound();
            }
            if (_context.Fotograflar.Any(x => x.FotografId == id))
            {
                return NotFound("Yazara ait Kitaplar var");
            }
            _context.Fotograflar.Remove(y1);
            _context.SaveChanges();
            return Ok();
        }
    }
}
