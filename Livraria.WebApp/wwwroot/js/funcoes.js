//Funções de utilidade
window.isNullOrEmpty = function (string) {
    return string === undefined || string === null || string === "";
};

//Define padrões de mensagens PNotify
PNotify.defaults.styling = "bootstrap4";
PNotify.defaults.icons = 'fontawesome4';
PNotify.defaults.stack = {
    dir1: 'down',
    dir2: 'right',
    firstpos1: 25,
    firstpos2: 25,
    spacing1: 36,
    spacing2: 36,
    push: 'top',
    context: document.body
};

PNotify.defaults.modules = {
    Buttons: {
        sticker: false
    }
};

window.atualizarMenu = function (menuAtivoId) {
    $(".nav-link").removeClass("active");

    if (menuAtivoId != "") {
        $("#" + menuAtivoId).addClass("active");
    }
}