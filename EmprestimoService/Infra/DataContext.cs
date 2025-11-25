using EmprestimosService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmprestimosService.Infra
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Emprestimo> Emprestimos => Set<Emprestimo>();
    }
}
