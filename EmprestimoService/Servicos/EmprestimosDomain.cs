using EmprestimosService.DTO;
using EmprestimosService.Entities;
using Microsoft.EntityFrameworkCore;
using EmprestimosService.Infra;

namespace EmprestimosService.Servicos
{
    public class EmprestimosDomain
    {
        private readonly DataContext _ctx;
        private readonly IHttpClientFactory _httpClientFactory;

        public EmprestimosDomain(DataContext ctx, IHttpClientFactory httpClientFactory)
        {
            _ctx = ctx;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<EmprestimoReadDto>> GetAllAsync()
        {
            // Só dados locais (sem chamada remota)
            var emprestimos = await _ctx.Emprestimos.ToListAsync();

            return emprestimos.Select(e => new EmprestimoReadDto
            {
                Id = e.Id,
                AlunoId = e.AlunoId,
                LivroId = e.LivroId,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucao = e.DataDevolucao,
                Devolvido = e.Devolvido
            }).ToList();
        }

        public async Task<EmprestimoReadDto?> GetByIdAsync(int id)
        {
            var emp = await _ctx.Emprestimos.FindAsync(id);
            if (emp == null) return null;

            return new EmprestimoReadDto
            {
                Id = emp.Id,
                AlunoId = emp.AlunoId,
                LivroId = emp.LivroId,
                DataEmprestimo = emp.DataEmprestimo,
                DataDevolucao = emp.DataDevolucao,
                Devolvido = emp.Devolvido
            };
        }

        public async Task<EmprestimoReadDto?> CriarEmprestimoAsync(EmprestimoCreateDto dto)
        {
            var httpAlunos = _httpClientFactory.CreateClient("AlunosService");
            var httpLivros = _httpClientFactory.CreateClient("LivrosService");

            // 1ª integração: buscar aluno
            var aluno = await httpAlunos.GetFromJsonAsync<AlunoRemotoDto>($"alunos/{dto.AlunoId}");
            if (aluno == null || !aluno.Ativo)
                return null;

            // 2ª integração: buscar livro
            var livro = await httpLivros.GetFromJsonAsync<LivroRemotoDto>($"livros/{dto.LivroId}");
            if (livro == null || !livro.Disponivel)
                return null;

            // grava local
            var emp = new Emprestimo
            {
                AlunoId = dto.AlunoId,
                LivroId = dto.LivroId,
                DataEmprestimo = DateTime.UtcNow
            };

            _ctx.Emprestimos.Add(emp);
            await _ctx.SaveChangesAsync();

            // 3ª integração: alteração – incrementa empréstimos do aluno
            await httpAlunos.PutAsync($"alunos/{dto.AlunoId}/incrementa", null);

            // 4ª integração (extra): marca livro como emprestado
            await httpLivros.PutAsync($"livros/{dto.LivroId}/emprestar", null);

            return new EmprestimoReadDto
            {
                Id = emp.Id,
                AlunoId = emp.AlunoId,
                LivroId = emp.LivroId,
                NomeAluno = aluno.Nome,
                TituloLivro = livro.Titulo,
                DataEmprestimo = emp.DataEmprestimo,
                DataDevolucao = emp.DataDevolucao,
                Devolvido = emp.Devolvido
            };
        }

        public async Task<bool> RegistrarDevolucaoAsync(int id)
        {
            var emp = await _ctx.Emprestimos.FindAsync(id);
            if (emp == null || emp.Devolvido) return false;

            emp.DataDevolucao = DateTime.UtcNow;
            await _ctx.SaveChangesAsync();

            var httpLivros = _httpClientFactory.CreateClient("LivrosService");
            await httpLivros.PutAsync($"livros/{emp.LivroId}/devolver", null);

            return true;
        }
    }
}
