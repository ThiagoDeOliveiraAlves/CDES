using System.ComponentModel.DataAnnotations;

namespace CompartilhamentoDeDicasSustentaveis.Models
{
    public class LoginViewModel
    {
        /*OBS: Essa lógica será usada apenas para a nossa view, perceba que essa classe não é criada no banco de dados,
         não fizemos DbSet nessa classe. Quando usamos o Identty e fazemos uma nova migração, é criada uma tabela no banco de dados
        onde ficam as informações dos usuários. */

        [Required(ErrorMessage ="O email é obrigratório")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }

        [Display(Name = "Lembrar-me")]
        public bool LembrarMe { get; set; }
    }
}
