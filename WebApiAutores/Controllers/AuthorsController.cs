using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiAutores.Entities;

namespace WebApiAutores.Controllers
{
    [ApiController]
    [Route("api/authors")]
    public class AuthorsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        public AuthorsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Author>>> Get()
        {
            return await context.Authors.Include(x => x.Books).ToListAsync();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> Get(int id)
        {
            var author = await context.Authors.Include(x=>x.Books).FirstOrDefaultAsync(x => x.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<Author>> Get([FromRoute] string name)
        {
            var author = await context.Authors.Include(x => x.Books).FirstOrDefaultAsync(x => x.Name.Contains(name));
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }

        [HttpPost]
        public async Task<ActionResult> Post(Author author)
        {
            var existsAuthorWithSameName = await context.Authors.AnyAsync(x => x.Name == author.Name);
            if (existsAuthorWithSameName)
            {
                return BadRequest($"This author with name {author.Name} already exists");
            }
            context.Add(author);
            await context.SaveChangesAsync();
            return Ok(author);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(Author author, int id)
        {
            if (author.Id != id)
            {
                return BadRequest("Id no exists");
            }
            var exist = await context.Authors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Id no exists");
            }
            context.Update(author);
            await context.SaveChangesAsync();
            return Ok(author);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var exist = await context.Authors.AnyAsync(x => x.Id == id);
            if (!exist)
            {
                return NotFound("Id no exists");
            }
            context.Remove(new Author { Id = id });
            await context.SaveChangesAsync();
            return Ok("Author removed");
        }
    }
}
