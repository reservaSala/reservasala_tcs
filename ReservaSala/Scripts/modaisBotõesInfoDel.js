function btnAbrirModal(id) {
    $("#modal_Sala").load(DetailsSalas + '/' + id, function () {
        $("#modal_Sala").modal('toggle');
    });
};

function btnAbrirModalDelete(id) {
    $("#modal_SalaDelete").load(DeleteSalas + '/' + id, function () {
        $("#modal_SalaDelete").modal('toggle');
    });
};


function btnAbrirModalUsuarioDelete(id) {
    $("#modal_Delete").load(DeleteUsuarioSala + '/' + id, function () {
        $("#modal_Delete").modal('toggle');
    });
};

function btnAbrirDeletarUser(id) {
    $("#modal_DeleteUser").load(DeleteUsuarioAcesso + '/' + id, function () {
        $("#modal_DeleteUser").modal('toggle');
    });
}