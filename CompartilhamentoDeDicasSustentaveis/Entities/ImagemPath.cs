using System.ComponentModel.DataAnnotations;

namespace CompartilhamentoDeDicasSustentaveis.Entities
{
    public class ImagemPath
    {
        [Key]
        public int Id { get; set; }
        public string ImgPath { get; set; }
        public Postagem Postagem { get; set; }
        public int PostagemId { get; set; }
    }
}
