var LivroListaModulo = (function () {
    var _numeroPagina;
    var _paginacao;

    var _novaPesquisa = function () {

        _numeroPagina = 1;
        _carregarLivros();

    }

    var _editarLivro = function (e) {
        var id = $(e).attr("data-id-livro");
        window.location.href = caminhoBase + "Livro/Cadastro/" + id;
    }

    var _detalhesLivro = function (e) {
        var id = $(e).attr("data-id-livro");
        window.location.href = caminhoBase + "Livro/Detalhes/" + id;
    }

    var _carregarLivros = function () {

        //lista de livros
        var lista = $("#div-lista-livros");
        //Texto da pesquisa
        var pesquisa = $("#input-pesquisa-livro").val();
        //Número de livros por página
        var livrosPorPagina = $("#select-livros-por-pagina option:selected").val();

        //Exibe o loading no botão
        var botao = $("#btn-pesquisar-livro");
        botao.prop("disabled", true);
        botao.find(".spinner-border").show();

        $.ajax({
            url: caminhoBase + "Livro/ListaLivrosPagina/",
            data: { pesquisa: pesquisa, numeroPagina: _numeroPagina, numeroItensPorPagina: livrosPorPagina },
            type: "GET"
        })
            .done(function (data) {
                //limpar a lista de livros
                lista.empty();

                if (data.livros !== undefined && data.livros !== null && data.livros.length > 0) {
                    
                    //Atualizar a paginação com os novos livros
                    _paginacao.twbsPagination('destroy');
                    _paginacao.twbsPagination({
                        startPage: _numeroPagina,
                        totalPages: data.total,
                        first: 'Início',
                        prev: '<',
                        next: '>',
                        last: 'Fim'
                    }).on('page', function (evt, page) {
                        _numeroPagina = page;
                        _carregarLivros();
                    });
                    //Adicionar ao html todos livros retornados
                    $.each(data.livros, (index, valor) => {

                        //Cria elementos da pagina para adicionar ao html
                        var img = $(document.createElement("img"));
                        var divLivro = $(document.createElement("div"));
                        var divCard = $(document.createElement("div"));
                        var divCardBody = $(document.createElement("div"));
                        var pCard = $(document.createElement("p"));
                        var divCardBtn = $(document.createElement("div"));
                        var divCardBtnGroup = $(document.createElement("div"));

                        var btnDetalhes = $(document.createElement("button"));
                        var btnEditar = $(document.createElement("button"));

                        if (valor.imagemCapa == null || valor.imagemCapa == undefined) {
                            img.attr("src", "/images/img-livro.png");
                        } else {
                            img.attr("src", "data:image/jpg;base64, " + valor.imagemCapa);
                        }

                        img.addClass("card-img-top");
                        divLivro.addClass("col-md-4");
                        divCard.addClass("card mb-4 shadow-sm");
                        divCardBody.addClass("card-body");
                        pCard.html(valor.titulo + " - " + valor.autor).addClass("card-text");
                        divCardBtn.addClass("d-flex justify-content-between align-items-center");
                        divCardBtnGroup.addClass("btn-group");

                        btnDetalhes.html("Detalhes").attr("type", "button").attr("data-id-livro", valor.id).addClass("btn btn-sm btn-outline-secondary btn-detalhes-livro");
                        btnEditar.html("Editar").attr("type", "button").attr("data-id-livro", valor.id).addClass("btn btn-sm btn-outline-secondary btn-editar-livro");

                        divCardBtnGroup.append(btnEditar);
                        divCardBtnGroup.append(btnDetalhes);
                        divCardBtn.append(divCardBtnGroup);
                        divCardBody.append(pCard);
                        divCardBody.append(divCardBtnGroup);
                        divCard.append(img);
                        divCard.append(divCardBody);
                        divLivro.append(divCard);

                        //Adicionar o livro na lista da página
                        lista.append(divLivro);

                    });
                }
                //Esconde o loading do botão
                botao.prop("disabled", false);
                botao.find(".spinner-border").hide();
            })
            .fail(function () {
                //Esconde o loading do botão
                botao.prop("disabled", false);
                botao.find(".spinner-border").hide();
            });

    }

    var _iniciarHub = function () {

        //Configura conexao com o hub de livros
        var connection = new signalR.HubConnectionBuilder()
            .withUrl("/livroHub")
            .configureLogging(signalR.LogLevel.Information)
            .build();

        connection.start().then(function () { });

        connection.on("AtualizarLista", function () {
            _carregarLivros();
        });

    }

    var inicializar = function () {

        //iniciar contagem
        _numerooPagina = 1
        _paginacao = $('#ul-paginacao-livros');

        //Eventos
        $("body").on("click", "#btn-pesquisar-livro", _novaPesquisa);
        $("body").on("click", ".btn-editar-livro", function (e) { _editarLivro(e.currentTarget) });
        $("body").on("click", ".btn-detalhes-livro", function (e) { _detalhesLivro(e.currentTarget) });
        $('#select-livros-por-pagina').on("change", _novaPesquisa);

        //iniciar hub
        _iniciarHub();

        //iniciar página
        _novaPesquisa();

    }

    return {
        inicializar: inicializar
    }

});

$(function () {

    var modulo = new LivroListaModulo();
    modulo.inicializar();

});