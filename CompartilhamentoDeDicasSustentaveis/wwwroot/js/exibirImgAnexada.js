//Esse bloco de código serve para pegar a imagem da input e mostrar ela na tela na tag img
document.getElementById('imagens').addEventListener('click', function () {
    const imagem = document.getElementById('imagens');
    const imagens = document.getElementById("imagens").files;
    var local = document.getElementById("imgs-carregadas");
    //const imgTag = [];
    if (imagens) {
        for (let i = 0; i < imagens.length; i++) {

            const reader = new FileReader();

            const imgTag = document.createElement("img")

            reader.onload = function (e) {
                imgTag[i].src = e.target.result;

                local.appendChild(imgTag);
            };

            reader.readAsDataURL(imagens[i]); //lê o arquivo como uma url de dados
        }
    }
});
        //bloco acaba aqui