$('#somenteNumeros').keypress(function (num) {
    //var er = /[^0-9.]/;
    //er.lastIndex = 0;
    //var campo = num;
    //if (er.test(campo.value)) {
    //    campo.value = "";
    //}

    var keyCode = (num.keyCode ? num.keyCode : num.which);

    if (!(keyCode > 47 && keyCode < 58)) {
        num.preventDefault();
    }   
});

$(document).ready(function () {
    $('#somenteNumeros').bind('cut copy paste', function (event) {
        event.preventDefault();
    });
});