using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Template.Infra
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilderAlunos = new DbContextOptionsBuilder<DataContext>();
            optionsBuilderAlunos.UseSqlite("Data Source=alunos.db");
            return new DataContext(optionsBuilderAlunos.Options);

            var optionsBuilderLivros = new DbContextOptionsBuilder<DataContext>();
            optionsBuilderLivros.UseSqlite("Data Source=livros.db");
            return new DataContext(optionsBuilderLivros.Options);
        }
    }
}
