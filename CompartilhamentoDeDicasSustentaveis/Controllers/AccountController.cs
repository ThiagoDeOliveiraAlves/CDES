using CompartilhamentoDeDicasSustentaveis.Data;
using CompartilhamentoDeDicasSustentaveis.Entities;
using CompartilhamentoDeDicasSustentaveis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace CompartilhamentoDeDicasSustentaveis.Controllers
{
    public class AccountController : Controller
    {
        //tenta muder de IdentityUser para UsuarioIdentity
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly SignInManager<UsuarioIdentity> _signInManager;

        //readonly ApplicationDbContext _db;

        //mudei aqui também para UsuarioIdentity
        public AccountController(UserManager<UsuarioIdentity> userManager,
            SignInManager<UsuarioIdentity> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_db = db;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(CadastrarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new UsuarioIdentity
                {
                    UserName = model.Nome,
                    Email = model.Email,
                };
                /*Armazena dados do usuário na tabela AspNetUsers (o código abaixo). Perceba que eu não enviei a model.Senha dentro do bloco da
                 instância UsuarioIdentity. Tive que enviar só agora, no código abaixo. Pelo o que vi com o chat gpt, tem que fazer dessa forma
                 por questões de segurança. O hash de senha é um valor criptograficamente seguro que é derivado da senha do usuário usando algoritmos hash
                fazer dessa forma garamte que o processo seja feito de forma segura e correta.
                Além disso, tentar colocar a senha ali direto pode gerar erros.
                Daí, essa variável result vai receber o resultado dessa operação, se deu certo ou não*/
                var result = await _userManager.CreateAsync(user, model.Senha);

                //se o usuário foi criado com sucesso, faz o login do usuário
                //usando o serviço SignInManager e redireciona para o método Action Postagem no controller Postagem
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Postagem", "Postagem");
                }
                //se houver erros, então inclui no ModelState
                //que será exibida pela tag helper summary na validação

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                Console.WriteLine("model state Valid");

                UsuarioIdentity signedUser = await _userManager.FindByEmailAsync(model.Email);
                var result = await _signInManager.PasswordSignInAsync(signedUser.UserName, model.Senha, model.LembrarMe, false);

                Console.WriteLine("result.Succeeded: " + result.Succeeded);

                if (result.Succeeded)
                {
                    Console.WriteLine("Operação bem sucedida");

                    return RedirectToAction("Postagem", "Postagem");
                }
                
                Console.WriteLine("model state invalid");
                ModelState.AddModelError(string.Empty, "Login Inválido");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            if (!_signInManager.IsSignedIn(User))
            {
                Console.WriteLine("Teste Logout: O logout foi feito com sucesso");
            }
            else
            {
                Console.WriteLine("Teste Logout: O logout foi feito com sucesso");
            }
            return RedirectToAction("Index", "Home");
        }
    
    }
}
