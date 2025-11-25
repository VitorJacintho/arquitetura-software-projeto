using LivrosService.DTO;
using LivrosService.Entities;
using LivrosService.Infra;
using Microsoft.EntityFrameworkCore;

namespace LivrosService.Services
{
    public class LivrosDomain
    {
        private readonly DataContext _ctx;

        public LivrosDomain(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<LivroReadDto>> GetAllAsync()
        {
            return await _ctx.Livros
                .Select(l => new LivroReadDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    AnoPublicacao = l.AnoPublicacao,
                    Disponivel = l.Disponivel
                }).ToListAsync();
        }

        public async Task<LivroReadDto?> GetByIdAsync(int id)
        {
            return await _ctx.Livros
                .Where(l => l.Id == id)
                .Select(l => new LivroReadDto
                {
                    Id = l.Id,
                    Titulo = l.Titulo,
                    Autor = l.Autor,
                    AnoPublicacao = l.AnoPublicacao,
                    Disponivel = l.Disponivel
                }).FirstOrDefaultAsync();
        }

        public async Task<LivroReadDto> CreateAsync(LivroCreateDto dto)
        {
            var livro = new Livro
            {
                Titulo = dto.Titulo,
                Autor = dto.Autor,
                AnoPublicacao = dto.AnoPublicacao,
                Disponivel = true
            };

            _ctx.Livros.Add(livro);
            await _ctx.SaveChangesAsync();

            return new LivroReadDto
            {
                Id = livro.Id,
                Titulo = livro.Titulo,
                Autor = livro.Autor,
                AnoPublicacao = livro.AnoPublicacao,
                Disponivel = livro.Disponivel
            };
        }

        public async Task<bool> UpdateAsync(int id, LivroUpdateDto dto)
        {
            var livro = await _ctx.Livros.FindAsync(id);
            if (livro == null) return false;

            livro.Titulo = dto.Titulo;
            livro.Autor = dto.Autor;
            livro.AnoPublicacao = dto.AnoPublicacao;
            livro.Disponivel = dto.Disponivel;

            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmprestarAsync(int id)
        {
            var livro = await _ctx.Livros.FindAsync(id);
            if (livro == null || !livro.Disponivel) return false;

            livro.Disponivel = false;
            await _ctx.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DevolverAsync(int id)
        {
            var livro = await _ctx.Livros.FindAsync(id);
            if (livro == null) return false;

            livro.Disponivel = true;
            await _ctx.SaveChangesAsync();
            return true;
        }
    }
}
