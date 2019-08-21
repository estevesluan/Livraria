var LivroModulo = (function () {

    var _novoCadastro = function () {

        $("#form-livro")[0].reset();
        $("#input-id").val("0");
        $("#btn-excluir").hide();
        $("#img-capa").removeAttr("src");
        $("#input-titulo").val("");
        $("#input-subtitulo").val("");
        $("#input-autor").val("");
        $("#textarea-resumo").html("");
       
        var campos = $("input.form-control, textarea");
        $.each(campos, function (index, campo) {
            $(campo).removeClass("is-valid");
            $(campo).removeClass("is-invalid");
        });

    }

    var _salvar = function (e) {

        var data = new FormData();

        data.append("id", $("#input-id").val());
        data.append("titulo", $("#input-titulo").val());
        data.append("subtitulo", $("#input-subtitulo").val());
        data.append("autor", $("#input-autor").val());
        data.append("resumo", $("#textarea-resumo").val());
        data.append("capa", $('#input-capa')[0].files[0]);
        
        //Validações
        var campos = [
            {
                obj: $("#input-titulo"),
                valor: data.get("titulo"),
                mensagem: "Preencha o título"
            },
            {
                obj: $("#input-autor"),
                valor: data.get("autor"),
                mensagem: "Preencha o autor"
            },
            {
                obj: $("#textarea-resumo"),
                valor: data.get("resumo"),
                mensagem: "Preencha o resumo"
            }
        ];

        let validacao = true;

        $.each(campos, function (index, campo) {

            if (isNullOrEmpty(campo.valor) || campo.valor == 0) {

                campo.obj.focus();

                campo.obj.removeClass("is-valid");
                campo.obj.addClass("is-invalid");

                validacao = false;
                return;
            }

            campo.obj.removeClass("is-invalid");
            campo.obj.addClass("is-valid");
        });

        if (!validacao) {
            PNotify.alert({
                text: "Preencha os campos obrigatórios",
                type: 'notice'
            });
            return;
        }

        $.ajax({

            url: caminhoBase + "Livro/Cadastro",
            type: "POST",
            data: data,
            processData: false,
            contentType: false

        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    PNotify.alert({
                        text: "Cadastro realizado!",
                        type: 'success'
                    });
                    _novoCadastro();

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

    var _excluir = function () {

        var id = $("#input-id").val();

        $.ajax({
            url: caminhoBase + "Livro/Remover/" + id,
            type: "POST",
        })
            .done(function (data) {

                if (data.hasOwnProperty("erro")) {

                    PNotify.alert({
                        text: data.erro,
                        type: 'error'
                    });

                } else {

                    PNotify.alert({
                        text: "Removido com sucesso!",
                        type: 'success'
                    });
                    _novoCadastro();

                }

            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });

            });

    }

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

                    $("#btn-excluir").hide();

                } else {

                    $("#img-capa").attr("src", "data:image/jpg;base64, " + data.imagemCapa);
                    $("#input-titulo").val(data.titulo);
                    $("#input-subtitulo").val(data.subtitulo);
                    $("#input-autor").val(data.autor);
                    $("#textarea-resumo").html(data.resumo);

                    $("#btn-excluir").show();

                }
            })
            .fail(function () {

                PNotify.alert({
                    text: "Erro ao realizar a comunicação com o servidor",
                    type: 'error'
                });
                $("#btn-excluir").hide();

            });

    }

    var _atualizarCapa = function (e) {

        var input = e;
        var url = $(e).val();
        var ext = url.substring(url.lastIndexOf('.') + 1).toLowerCase();
        if (input.files && input.files[0] && (ext == "png")) {

            var reader = new FileReader();
            reader.onload = function (e) {

                $('#img-capa').attr('src', e.target.result);

            }
            reader.readAsDataURL(input.files[0]);

        }

    }

    var inicializar = function () {

        //Inicializa os eventos
        $("form").on("submit", function (e) { e.preventDefault() });
        $("body").on("click", "#btn-novo", _novoCadastro);
        $("body").on("click", "#btn-salvar", _salvar);
        $("body").on("click", "#btn-excluir", _excluir);
        $('#input-capa').on("change", function (e) { _atualizarCapa(e.currentTarget) });

        //Verifica edição do livro
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

    var modulo = new LivroModulo();
    modulo.inicializar();

});
