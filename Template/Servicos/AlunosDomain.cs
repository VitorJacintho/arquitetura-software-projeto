using Microsoft.EntityFrameworkCore;
using Template.Entities;

namespace Template.Servicos
{
    public class AlunosDomain
    {
        private readonly Template.Infra.DataContext _context;

        public AlunosDomain(Template.Infra.DataContext context)
        {
            _context = context;
        }

        public async Task<Aluno> InserirAsync(Aluno aluno)
        {
            _context.Alunos.Add(aluno);
            await _context.SaveChangesAsync();
            return aluno;
        }

        public async Task<List<Aluno>> ListarAsync()
        {
            return await _context.Alunos.AsNoTracking().ToListAsync();
        }

        public async Task<Aluno?> BuscarPorIdAsync(int id)
        {
            return await _context.Alunos.FindAsync(id);
        }

        public async Task<bool> EditarAsync(int id, Aluno alunoAtualizado)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return false;

            aluno.Nome = alunoAtualizado.Nome;
            aluno.Qnt_Emprestimo = alunoAtualizado.Qnt_Emprestimo;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoverAsync(int id)
        {
            var aluno = await _context.Alunos.FindAsync(id);
            if (aluno == null) return false;

            _context.Alunos.Remove(aluno);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
