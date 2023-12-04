//Subir a postagem

document.getElementById("Enviar").addEventListener("click", function () {
    try {
        const titulo = document.getElementById("titulo").value;
        const texto = document.getElementById("texto").value;
        const imagens = document.getElementById("imagens").files
        const categoria = document.getElementById("categoria").value;

        //Tratamento de exceçoes. Não deve permitir campos nulos ou vazios
        if (titulo == null || titulo == "") {
            throw new Error("Erro: Informe um título");
        }
        else if (texto == null || texto == "") {
            throw new Error("Erro: Escreva uma descrição")
        }

        const formData = new FormData();

        formData.append("titulo", titulo);
        formData.append("texto", texto);
        formData.append("categoria", categoria);

        if (imagens) {
            //testando se todos os arquivos são imagens
            for (let i = 0; i < imagens.length; i++) {
                if (!isImagem(imagens[i])) {
                    throw new Error("Erro: Somente arquivos de extensão png ou jpeg");
                }
            }
            //adicionando as imagens ao formData. Obs: quando criamos campos com o mesmo nome no formData, o asp entende que se trata de uma coleção. Daí ele consegue mandar certinho para a nossa lista
            for (let i = 0; i < imagens.length; i++) {
                formData.append("imagens", imagens[i]);
            }
        }

        const xhr = new XMLHttpRequest();
        xhr.open("POST", "/Postagem/Postagem", true);

        //aqui estamos recebendo o retorno do Postagem2, no caso, o endereço da imagem (onde ele salvou)
        xhr.onload = function () {
            if (xhr.status == 200) {
                /*var response = JSON.parse(xhr.responseText);
                var imagemPath = response.path;
                const imagem = document.getElementById("imagem2");
                console.log(imagemPath);
                imagem.src = imagemPath;
                 */
            }
        };
        // Adiciona um pequeno atraso (500 milissegundos) antes de enviar a próxima solicitação
        setTimeout(function () {
            // Limpa as inputs para permitir o envio de novas postagens
            document.getElementById("titulo").value = "";
            document.getElementById("texto").value = "";
            document.getElementById("imagens").value = "";
        }, 500);

        xhr.send(formData);
        window.alert("A postagem foi realizada com sucesso!");
    }
    catch (e) {
        window.alert(e);
    }
});


//função para testar se é uma imagem
function isImagem(file) {
    const tiposPermitidos = ["image/png", "image/jpeg"];

    // Verifica se a extensão do tipo do arquivo está na lista de tipos de imagem permitidos
    return tiposPermitidos.includes(file.type);
}
