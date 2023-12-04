using Microsoft.AspNetCore.Identity;

namespace CompartilhamentoDeDicasSustentaveis.Entities
{

     /*Herdei de IdentityUser porque é a classe que o Identity usa para salvar os usuários.
      No caso, eu estendi a classe IdentityUser para colocar os atributos adicionais*/

    public class UsuarioIdentity : IdentityUser
        //Obs: por padrão, a chave primária do identity é uma string
    {
        public ICollection<Postagem> Postagens { get; set; } = new List<Postagem>();
    }
}
