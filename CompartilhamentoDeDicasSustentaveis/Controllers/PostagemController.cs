using CompartilhamentoDeDicasSustentaveis.Data;
using CompartilhamentoDeDicasSustentaveis.Entities;
using CompartilhamentoDeDicasSustentaveis.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace CompartilhamentoDeDicasSustentaveis.Controllers
{
    [Authorize]//pode ser usado tanto em controladores uanto em métodos Actions
    /*Estamos dizendo que agora, somento usuários autenticados têm permissão para acessar esse controlador.
     * Nesse caso, o middleware de autorização vai verificar se o usuário está autenticado antes de permitir o acessoa ao recurso correspondente,
     se o usuário não estiver autenticado, o usuário será redirecionado para o login*/
    public class PostagemController : Controller
    {
        private string ServidorPath { get; set; }
        private readonly UserManager<UsuarioIdentity> _userManager;
        private readonly ApplicationDbContext _db;

        public PostagemController(IWebHostEnvironment sistema, UserManager<UsuarioIdentity> userManager, ApplicationDbContext db) //essa é a variável do ambiente do host, onde estamos hospedando
        {
            ServidorPath = sistema.WebRootPath; /*esse método serve para pegar o nome da pasta que está o nosso wwroot.
            Então ele já tras o C:/Usuario... etc*/
            _userManager = userManager;
            _db = db;
        }

        [HttpGet]
        public IActionResult Postagem()
        {
            List<PostagemViewModel> postagens = ObterPostagens();
            return View(postagens);
        }

        [HttpPost]
        public async Task <IActionResult> Postagem(string titulo,string texto, List <IFormFile> imagens, string categoria) //Essa interface trabalha com imagens. Obs o nome que dei "foto" é o mesmo da input do formulário
        {
            //OBS: Os nomes das variáveis de parâmetro devem ser iguais ao nomes dos campos do formData que criamos

            string caminhoparaSalvarImagem = ServidorPath + "\\ImgPostagem\\";

            var userId = _userManager.GetUserId(User);

            var usuario = await _userManager.FindByIdAsync(userId);

            Postagem postagem = new Postagem(titulo, texto, categoria);

            if (imagens.Count > 0) //Para caso não tenha imagem
            {

                if (!Directory.Exists(caminhoparaSalvarImagem))
                {
                    Directory.CreateDirectory(caminhoparaSalvarImagem);
                }

                for (int i = 0; i < imagens.Count; i++)
                {

                    string novoNomeImagem = Guid.NewGuid().ToString() + imagens[i].FileName;
                    /*Essa biblioteca Guid serve para gerar um hash criptografado. Acho que um conjunto de caracteres aleatórios. Daí o prof adicionou o 
                    nome da foto acho que para "garantir" que o nome seja diferente de uma outra imagem. Já que existe a possibilidade (embor baixissima) de
                    ser gerado dois hashsets iguais*/

                    postagem.ImagemPath.Add(new ImagemPath { ImgPath = (caminhoparaSalvarImagem + novoNomeImagem)});

                    using (var stream = System.IO.File.Create(caminhoparaSalvarImagem + novoNomeImagem)) //serve para criar o arquivo onde a imagem ficara
                    {
                        //Aqui estamos copiando a imagem e colocando no locar que criamos
                        imagens[i].CopyTo(stream);
                    }
                }
            }

            //atribuindo nova postagem ao usuario
            usuario.Postagens.Add(postagem);

            //atualizando o usuario. Subindo a nova postagem no banco
            await _userManager.UpdateAsync(usuario);
            
            return RedirectToAction("Postagem");   
        }

        [HttpGet]
        public IActionResult Filtrar()
        {
            return View();

        }

        public IActionResult PostagensFiltradas(DateTime dataInicio, DateTime dataFim, String categoria)
        {
           
            List<PostagemViewModel> postagemView = FiltrarPostagem(dataInicio, dataFim, categoria);

            return View(postagemView);
        }

        public List<PostagemViewModel> ObterPostagens()
        {
            Console.WriteLine("Teste inicio");
            int total = _db.Postagem.Count();

            List<Postagem> postagem = new List<Postagem>();
            List<PostagemViewModel> postagemView = new List<PostagemViewModel>();

            if (total >= 25)
            {
                postagem = _db.Postagem
                    .OrderByDescending(p => p.DataPost)
                    .Take(25)
                    .Include(p => p.ImagemPath)
                    .ToList();
                //Esse .Include(p=> p.ImagemPath) serve para carregar as imagens. Porque como o ImagemPath é representado em outra tabela, ocorre o Lazy loading

            }
            else if (total != 0)
            {
                postagem = _db.Postagem
                    .OrderByDescending(p => p.DataPost)
                    .Include(p => p.ImagemPath)
                    .ToList();
            }
            else
            {
                postagem = null;
            }
            if (postagem != null)
            {
                for (int i = 0; i < postagem.Count; i++)
                {
                    List<ImagemBase64> imgBase64 = new List<ImagemBase64>();
                    int teste = 0;
                    foreach (ImagemPath img in postagem[i].ImagemPath)
                    {

                        //Console.WriteLine("Caminho da imagem: " + img.ImgPath);

                        imgBase64.Add(new ImagemBase64(img.PostagemId, img.ImgPath));
                        //Console.WriteLine("Imagem convertida para base64: [" + teste + "] " + imgBase64[teste].ImgBase64);
                        teste++;
                    }
                    postagemView.Add(new PostagemViewModel(postagem[i], imgBase64));
                }

            }

            return postagemView;
        }
        
        public List<PostagemViewModel> FiltrarPostagem(DateTime? dataInicio, DateTime dataFim, String categoria)
        {
            Console.WriteLine("O método foi aberto");
            List<Postagem> postagem = new List<Postagem>();
            List<PostagemViewModel> postagemView = new List<PostagemViewModel>();

            //tive que fazer isso, porque a consulta considera somente ate meia noite. Então se a dataFim for 28 e a postagem ter sido feita em dia 28 00:02, ele não pegaria ela
            dataFim = dataFim.AddDays(1);

            //Um dateTime não fica com valor null, quando não possui valor atribuído, se valor fica 01/01/0001 que podemos adquirir ao fazer DateTime.MinValue
            if (dataInicio != DateTime.MinValue && dataFim != DateTime.MinValue.AddDays(1))
            {
                postagem = _db.Postagem
                    .Where(p => p.DataPost >= dataInicio && p.DataPost <= dataFim && p.Categoria == categoria)
                    .Include(p => p.ImagemPath)
                    .ToList();

            }
            else if (dataInicio != DateTime.MinValue)
            {
                postagem = _db.Postagem
                    .Where(p => p.DataPost >= dataInicio && p.Categoria == categoria)
                    .Include(p => p.ImagemPath)
                    .ToList();
            }
            else if (dataFim != DateTime.MinValue.AddDays(1))
            {
                postagem = _db.Postagem
                    .Where(p => p.DataPost <= dataFim && p.Categoria == categoria)
                    .Include(p => p.ImagemPath)
                    .ToList();
            }
            else
            {
                postagem = _db.Postagem
                    .Where(p => p.Categoria == categoria)
                    .Include(p => p.ImagemPath)
                    .ToList();
            }

            if (postagem != null)
            {
                for (int i = 0; i < postagem.Count; i++)
                {
                    List<ImagemBase64> imgBase64 = new List<ImagemBase64>();
                    int teste = 0;
                    foreach (ImagemPath img in postagem[i].ImagemPath)
                    {

                        imgBase64.Add(new ImagemBase64(img.PostagemId, img.ImgPath));
                        teste++;
                    }
                    postagemView.Add(new PostagemViewModel(postagem[i], imgBase64));
                }
            }
            return postagemView;
        }
    }
}
