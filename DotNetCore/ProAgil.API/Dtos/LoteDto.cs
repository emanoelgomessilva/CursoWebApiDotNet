using System.ComponentModel.DataAnnotations;

namespace ProAgil.API.Dtos
{
    public class LoteDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome deve ser preenchido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo preço deve ser preenchido ")]
        public decimal Preco { get; set; }

        [Required(ErrorMessage = "O campo data de início deve ser preenchido")]
        public string DataInicio { get; set; }

        [Required(ErrorMessage = "O campo data de fim deve ser preenchido")]
        public string DataFim { get; set; }

        [Range(2, 120000, ErrorMessage = "A quantidade de estar entre 2 e 120000")]
        public int Quantidade { get; set; }
    }
}