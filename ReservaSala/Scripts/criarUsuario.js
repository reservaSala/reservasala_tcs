
////inserção de tcs.com no email
//var email = $("#emailTexto");
//$(document).ready(function () {
//    email.val("\u0040tcs.com");
//});

//validação de email
var password = document.getElementById("password")
    , confirm_password = document.getElementById("confirm_password");

function validatePassword() {
    if (password.value != confirm_password.value) {
        confirm_password.setCustomValidity("Senhas diferentes!");
    } else {
        confirm_password.setCustomValidity('');
    }
}

password.onchange = validatePassword;
confirm_password.onkeyup = validatePassword;

function submit() {
    //pegando os valores do email, digitado e o @tcs.com
    var emailtexto = $("#emailTexto");
    var email = $("#email").val();
    var restoEmail = $("#emailResto").text();

    //juntando os dois e jogando para model 
    var textoEmail = email + restoEmail;
    emailtexto.val() = textoEmail;

    if (email == "" || email == null) {

    }


}