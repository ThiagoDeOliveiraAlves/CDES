namespace CompartilhamentoDeDicasSustentaveis.Models
{
    /*Essa classe foi criada com o intuito de armazenar o Id da postagem a qual a imagem pertence e seu formato em base64
     para ser enviado para a parte de cliente*/
    public class ImagemBase64
    {
        public int PostagemId { get; set; }
        public string ImgBase64 { get; set; }

        public ImagemBase64(int postagemId, string imgPath)
        {
            Byte[] imageBytes = File.ReadAllBytes(imgPath);
            ImgBase64 = Convert.ToBase64String(imageBytes);
        }

    }
}
