var textarea = document.getElementById('medidor');
var info = document.getElementById('carResTxt');
var limite = pageLimite;
textarea.addEventListener('keyup', verificar);

function verificar(e) {
    var qtdcaracteres = this.value.length;
    var restantes = limite - qtdcaracteres;
    if (restantes < 1) {
        this.value = this.value.slice(0, limite);
        return info.innerHTML = 0;
    }
    info.innerHTML = restantes + " Caracteres restantes";
}