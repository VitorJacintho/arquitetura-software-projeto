using LivrosService.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LivrosService.Infra
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        public DbSet<Livro> Livros => Set<Livro>();
    }
}
