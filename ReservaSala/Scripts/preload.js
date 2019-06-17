
jQuery(window).ready(function () {
    setTimeout('$("#preload").fadeOut(100)', 1500);

});

function preLoad() {
    $("#myform").on("submit", function () {
        $("#preload").fadeIn();
    });
}