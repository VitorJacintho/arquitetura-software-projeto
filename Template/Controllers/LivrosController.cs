using LivrosService.DTO;
using LivrosService.Services;
using Microsoft.AspNetCore.Mvc;

namespace LivrosService.Controllers
{
    [ApiController]
    [Route("livros")]
    public class LivrosController : ControllerBase
    {
        private readonly LivrosDomain _domain;

        public LivrosController(LivrosDomain domain)
        {
            _domain = domain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroReadDto>>> GetAll()
        {
            var livros = await _domain.GetAllAsync();
            return Ok(livros);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<LivroReadDto>> GetById(int id)
        {
            var livro = await _domain.GetByIdAsync(id);
            if (livro == null) return NotFound();
            return Ok(livro);
        }

        [HttpPost]
        public async Task<ActionResult<LivroReadDto>> Create(LivroCreateDto dto)
        {
            var created = await _domain.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, LivroUpdateDto dto)
        {
            var ok = await _domain.UpdateAsync(id, dto);
            if (!ok) return NotFound();
            return NoContent();
        }

        // 👇 usados pelo EmprestimosService
        [HttpPut("{id:int}/emprestar")]
        public async Task<IActionResult> Emprestar(int id)
        {
            var ok = await _domain.EmprestarAsync(id);
            if (!ok) return BadRequest("Livro indisponível ou não encontrado.");
            return NoContent();
        }

        [HttpPut("{id:int}/devolver")]
        public async Task<IActionResult> Devolver(int id)
        {
            var ok = await _domain.DevolverAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
