namespace LivrosService.DTO
{
    public class LivroCreateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
    }

    public class LivroUpdateDto
    {
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public bool Disponivel { get; set; }
    }

    public class LivroReadDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Autor { get; set; } = string.Empty;
        public int AnoPublicacao { get; set; }
        public bool Disponivel { get; set; }
    }
}
