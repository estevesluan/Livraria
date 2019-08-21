var LivroDetalhesModulo = (function () {

    var _carregarLivro = function (id) {

        $.ajax({

            url: caminhoBase + "Livro/Dados/" + id,
            type: "GET"

        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    if (data.imagemCapa == null || data.imagemCapa == undefined) {
                        $("#img-capa").attr("src", "/images/img-livro.png");
                    } else {
                        $("#img-capa").attr("src", "data:image/jpg;base64, " + data.imagemCapa);
                    }

                    var subtitulo = "";
                    if (!isNullOrEmpty(data.subtitulo)){
                        subtitulo = " - " + data.subtitulo;
                    }
                    $("#span-titulo").html(data.titulo + subtitulo);
                    $("#span-autor").html(data.autor);
                    $("#span-resumo").html(data.resumo);

                    $("#a-cadastro").attr("href", caminhoBase + "Livro/Cadastro/" + data.id);

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

    var inicializar = function () {

        //Carregar dados do livro
        var id = $("#input-id").val();

        if (id != null && id > 0) {
            _carregarLivro(id);
        }

    }

    return {
        inicializar: inicializar
    }

});

$(function () {

    var modulo = new LivroDetalhesModulo();
    modulo.inicializar();

});
