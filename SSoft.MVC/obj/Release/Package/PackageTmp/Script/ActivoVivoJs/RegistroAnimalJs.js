$(document).ready(function () {
    $('#verMenuBtn').show();

    $('#verMenuBtn').click(function () {
        var flagVolverMenu = $('#flagVolverMenu').val();
        if (flagVolverMenu == 0) {
            $('.contenido').hide();
            $('#menuDialog').show();
            $('#menuNivel2').show();
            $('.Animales').show();
            $('#flagVolverMenu').val(1);
        } else {
            $('.contenido').show();
            $('.Animales').hide();
            $('#menuDialog').hide();
            $('#flagVolverMenu').val(0);
        }
    });

});
function Siguiente(fichaActual, fichaSiguiente) {
    $('#' + fichaActual).hide();
    $('#' + fichaSiguiente).show();
}
function Atras(fichaActual, fichaAnterior) {
    $('#' + fichaActual).hide();
    $('#' + fichaAnterior).show();
}