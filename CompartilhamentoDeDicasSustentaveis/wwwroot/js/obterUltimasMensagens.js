
var teste = @Html.Raw(Newtonsoft.Json.JsonConvert.SerializeObject(Model));

teste.forEach(function (p) {
    var postagemTitulo = p.Titulo;
    var postagemTexto = p.Texto;
    var titulo = document.createElement("h3");
    var texto = document.createElement("p");
    titulo.innerHTML = postagemTitulo;
    texto.innerHTML = postagemTexto;

    var li = document.createElement("li");

    li.appendChild(titulo);
    li.appendChild(texto);
    console.log("Testezinho");

    p.ImagemBase64.forEach(function (imgBase64) {
        var img = document.createElement("img");
        img.style.width = "600px";
        img.style.margin = "auto";
        img.style.marginBottom = "20px";
        img.src = "data:image/jpeg;base64," + imgBase64.ImgBase64;

        li.appendChild(img);
    });

    var ul = document.getElementById("postagens");
    ul.appendChild(li);
});