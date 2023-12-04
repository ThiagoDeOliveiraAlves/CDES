namespace CompartilhamentoDeDicasSustentaveis.Entities
{
    public class Postagem
    {
        public int PostagemId { get; set; }
        public string Titulo { get; set; }
        public string Texto { get; set; }
        public ICollection<ImagemPath> ImagemPath { get; set; }
        // Antes estava usando Ienumerable<string> poré o Entity Framework nõ estava reconhecendo, e parece que isso é normal.
        //Daí, a solução foi criar uma classe chamada ImagemPath e colocar no lugar da string.
        //public string VideoPath { get; set; }
        public DateTime DataPost { get; set; } = DateTime.Now;
        public UsuarioIdentity Remetente { get; set; }
        //Tive que deixar como string porque por padrao, a chave primária da tabela AspNetUsers é uma string
        public string RemetenteId { get; set; }
        public string Categoria { get; set; }
        //public int Curtidas { get; set; }
        
        public Postagem(string titulo, string texto,string categoria)
        {
            Titulo = titulo;
            Texto = texto;
            Categoria = categoria;
            ImagemPath = new List<ImagemPath>();
        }
    }
}
