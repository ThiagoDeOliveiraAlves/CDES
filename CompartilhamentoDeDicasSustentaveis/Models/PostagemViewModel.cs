using CompartilhamentoDeDicasSustentaveis.Entities;

namespace CompartilhamentoDeDicasSustentaveis.Models
{
    //modelo que será usado para imprimir as postagens na View
    public class PostagemViewModel
    {
        public int PostagemId { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public ICollection<ImagemBase64> ImagemBase64 { get; set; } = new List<ImagemBase64>();
        // Antes estava usando Ienumerable<string> poré o Entity Framework nõ estava reconhecendo, e parece que isso é normal.
        //Daí, a solução foi criar uma classe chamada ImagemPath e colocar no lugar da string.
        //public string VideoPath { get; set; }
        public DateTime Data = DateTime.Now;
        public UsuarioIdentity Remetente { get; set; }
        public string Categoria { get; set; }
        //public int Curtidas { get; set; }

        public PostagemViewModel(Postagem postagem, List <ImagemBase64> img)
        {
            PostagemId = postagem.PostagemId;
            Titulo = postagem.Titulo;
            Texto = postagem.Texto;
            Data = postagem.DataPost;
            Remetente = postagem.Remetente;
            Categoria = postagem.Categoria;
            ImagemBase64 = img;
        }
    }
}
