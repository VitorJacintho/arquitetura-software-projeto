namespace EmprestimosService.DTO
{
    public class AlunoRemotoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public int QtdEmprestimos { get; set; }
        public bool Ativo { get; set; }
    }

    public class LivroRemotoDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public bool Disponivel { get; set; }
    }
}
