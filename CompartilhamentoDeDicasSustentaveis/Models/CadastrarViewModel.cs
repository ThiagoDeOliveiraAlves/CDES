using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CompartilhamentoDeDicasSustentaveis.Models
{
    public class CadastrarViewModel
    {
        [Required(ErrorMessage = "Insira um nome")]
        public string? Nome { get; set; }

        [Required(ErrorMessage = "Insira um e-mail")]
        [EmailAddress(ErrorMessage = "O e-mail é inválido")]
        public string? Email { get; set; }
        [Required(ErrorMessage = "Insira uma senha")]
        [DataType(DataType.Password)]
        public string? Senha { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Confirme a senha")]
        [Compare("Senha", ErrorMessage = "As senhas não coincidem")]
        public string? ConfirmarSenha { get; set; }
    }
}
