namespace EmprestimosService.Entities
{
    public class Emprestimo
    {
        public int Id { get; set; }

        public int AlunoId { get; set; }

        public int LivroId { get; set; }

        public DateTime DataEmprestimo { get; set; } = DateTime.UtcNow;

        public DateTime? DataDevolucao { get; set; }

        public bool Devolvido => DataDevolucao != null;
    }
}
