using Microsoft.AspNetCore.Mvc;
using Template.DTOs;
using Template.Entities;
using Template.Servicos;

namespace Template.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlunosController : ControllerBase
    {
        private readonly AlunosDomain _domain;

        public AlunosController(AlunosDomain domain)
        {
            _domain = domain;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var alunos = await _domain.ListarAsync();
            var result = alunos.Select(a => new AlunoReadDto { Id = a.Id, Nome = a.Nome, Qnt_Emprestimo = a.Qnt_Emprestimo }).ToList();
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aluno = await _domain.BuscarPorIdAsync(id);
            if (aluno == null) return NotFound();
            var dto = new AlunoReadDto { Id = aluno.Id, Nome = aluno.Nome, Qnt_Emprestimo = aluno.Qnt_Emprestimo };
            return Ok(dto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AlunoCreateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var aluno = new Aluno { Nome = dto.Nome, Qnt_Emprestimo = dto.Qnt_Emprestimo };
            var created = await _domain.InserirAsync(aluno);
            var readDto = new AlunoReadDto { Id = created.Id, Nome = created.Nome, Qnt_Emprestimo = created.Qnt_Emprestimo };
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, readDto);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] AlunoUpdateDto dto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var alunoAtualizado = new Aluno { Nome = dto.Nome, Qnt_Emprestimo = dto.Qnt_Emprestimo };
            var ok = await _domain.EditarAsync(id, alunoAtualizado);
            if (!ok) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _domain.RemoverAsync(id);
            if (!ok) return NotFound();
            return NoContent();
        }
    }
}
