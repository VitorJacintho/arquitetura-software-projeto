namespace EmprestimosService.DTO
{
    public class EmprestimoCreateDto
    {
        public int AlunoId { get; set; }
        public int LivroId { get; set; }
    }

    public class EmprestimoReadDto
    {
        public int Id { get; set; }
        public int AlunoId { get; set; }
        public string NomeAluno { get; set; } = string.Empty;
        public int LivroId { get; set; }
        public string TituloLivro { get; set; } = string.Empty;
        public DateTime DataEmprestimo { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public bool Devolvido { get; set; }
    }
}
