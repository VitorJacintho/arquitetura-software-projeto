using System.ComponentModel.DataAnnotations;

namespace Template.DTOs
{
    public class AlunoCreateDto
    {
        [Required]
        public string Nome { get; set; } = null!;

        public int Qnt_Emprestimo { get; set; }
    }
}
