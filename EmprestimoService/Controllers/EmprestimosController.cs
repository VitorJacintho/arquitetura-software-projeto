using EmprestimosService.DTO;
using EmprestimosService.Servicos;
using Microsoft.AspNetCore.Mvc;

namespace EmprestimosService.Controllers
{
    [ApiController]
    [Route("emprestimos")]
    public class EmprestimosController : ControllerBase
    {
        private readonly EmprestimosDomain _domain;

        public EmprestimosController(EmprestimosDomain domain)
        {
            _domain = domain;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmprestimoReadDto>>> GetAll()
        {
            var lista = await _domain.GetAllAsync();
            return Ok(lista);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<EmprestimoReadDto>> GetById(int id)
        {
            var emp = await _domain.GetByIdAsync(id);
            if (emp == null) return NotFound();
            return Ok(emp);
        }

        [HttpPost]
        public async Task<ActionResult<EmprestimoReadDto>> Create(EmprestimoCreateDto dto)
        {
            var criado = await _domain.CriarEmprestimoAsync(dto);
            if (criado == null)
                return BadRequest("Aluno ou livro inválido/indisponível.");

            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
        }

        [HttpPut("{id:int}/devolver")]
        public async Task<IActionResult> Devolver(int id)
        {
            var ok = await _domain.RegistrarDevolucaoAsync(id);
            if (!ok) return BadRequest("Empréstimo não encontrado ou já devolvido.");
            return NoContent();
        }
    }
}
