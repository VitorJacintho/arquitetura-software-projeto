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
            var emprestimos = await _ctx.Emprestimos.ToListAsync();

            var dtos = emprestimos.Select(e => new EmprestimoReadDto
            {
                Id = e.Id,
                AlunoId = e.AlunoId,
                LivroId = e.LivroId,
                DataEmprestimo = e.DataEmprestimo,
                DataDevolucao = e.DataDevolucao,
                Devolvido = e.Devolvido,
                NomeAluno = null,
                TituloLivro = null
            }).ToList();

            if (!dtos.Any()) return dtos;

            var alunoClient = _httpClientFactory.CreateClient("AlunosService");
            var livroClient = _httpClientFactory.CreateClient("LivrosService");

            var alunoCache = new Dictionary<int, string?>();
            var livroCache = new Dictionary<int, string?>();

            var tasks = dtos.Select(async dto =>
            {
                if (!alunoCache.TryGetValue(dto.AlunoId, out var nomeAluno))
                {
                    try
                    {
                        var aluno = await alunoClient.GetFromJsonAsync<AlunoRemotoDto>($"alunos/{dto.AlunoId}");
                        nomeAluno = aluno?.Nome ?? "Desconhecido";
                    }
                    catch
                    {
                        nomeAluno = "Desconhecido";
                    }
                    alunoCache[dto.AlunoId] = nomeAluno;
                }
                dto.NomeAluno = nomeAluno;

                if (!livroCache.TryGetValue(dto.LivroId, out var titulo))
                {
                    try
                    {
                        var livro = await livroClient.GetFromJsonAsync<LivroRemotoDto>($"livros/{dto.LivroId}");
                        titulo = livro?.Titulo ?? "Desconhecido";
                    }
                    catch
                    {
                        titulo = "Desconhecido";
                    }
                    livroCache[dto.LivroId] = titulo;
                }
                dto.TituloLivro = titulo;
            });

            await Task.WhenAll(tasks);

            return dtos;
        }

        public async Task<EmprestimoReadDto?> GetByIdAsync(int id)
        {
            var emp = await _ctx.Emprestimos.FindAsync(id);
            if (emp == null) return null;

            var dto = new EmprestimoReadDto
            {
                Id = emp.Id,
                AlunoId = emp.AlunoId,
                LivroId = emp.LivroId,
                DataEmprestimo = emp.DataEmprestimo,
                DataDevolucao = emp.DataDevolucao,
                Devolvido = emp.Devolvido
            };

            var alunoClient = _httpClientFactory.CreateClient("AlunosService");
            var livroClient = _httpClientFactory.CreateClient("LivrosService");

            try
            {
                var aluno = await alunoClient.GetFromJsonAsync<AlunoRemotoDto>($"alunos/{dto.AlunoId}");
                dto.NomeAluno = aluno?.Nome ?? "Desconhecido";
            }
            catch
            {
                dto.NomeAluno = "Desconhecido";
            }

            try
            {
                var livro = await livroClient.GetFromJsonAsync<LivroRemotoDto>($"livros/{dto.LivroId}");
                dto.TituloLivro = livro?.Titulo ?? "Desconhecido";
            }
            catch
            {
                dto.TituloLivro = "Desconhecido";
            }

            return dto;
        }

        public async Task<EmprestimoReadDto?> CriarEmprestimoAsync(EmprestimoCreateDto dto)
        {
            var httpAlunos = _httpClientFactory.CreateClient("AlunosService");
            var httpLivros = _httpClientFactory.CreateClient("LivrosService");

            var aluno = await httpAlunos.GetFromJsonAsync<AlunoRemotoDto>($"alunos/{dto.AlunoId}");
            if (aluno == null)
                return null;

            var livro = await httpLivros.GetFromJsonAsync<LivroRemotoDto>($"livros/{dto.LivroId}");
            if (livro == null || !livro.Disponivel)
                return null;

            var emp = new Emprestimo
            {
                AlunoId = dto.AlunoId,
                LivroId = dto.LivroId,
                DataEmprestimo = DateTime.UtcNow
            };

            _ctx.Emprestimos.Add(emp);
            await _ctx.SaveChangesAsync();

            await httpAlunos.PutAsync($"alunos/{dto.AlunoId}/incrementa", null);

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
