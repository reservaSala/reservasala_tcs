var clipboard = new Clipboard('.btn');
clipboard.on('success', function (e) {
    console.log('Senha copiada: ' + e.text);
});
clipboard.on('error', function (e) {
    console.log(e);
});
function copyText() {

}

swal({
    title: senhaNova,
    text: mensagem,
    type: "warning",
    confirmButtonColor: "#DD6B55",
    confirmButtonText: "Copiar senha!",
    showCancelButton: false,
    closeOnConfirm: false
},
    function () {
        $("#copyBtn").click(); 
        swal("Copiado", "Senha copiada com sucesso", "success");
    }
);