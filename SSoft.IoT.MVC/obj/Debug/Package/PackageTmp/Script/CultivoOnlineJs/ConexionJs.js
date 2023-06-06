$(document).ready(function () {

    $('#descargar').click(function () {
        Descargar();
    });
});

function Descargar() {
    if (ValidarDescargar()) {
        var nombreRed = $('#nombreRed').val();
        var claveRed = $('#claveRed').val();
        var token = $('#token').val();

        $.ajax({
            type: "GET",
            url: "ValidarToken",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                token: token
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.CodigoMensaje > 0) {
                    alert(data.Mensaje);
                } else {
                    var url = "DescargarConexion?nombreRed=" + nombreRed + "&claveRed=" + claveRed + "&token=" + token + "";
                    $('#nombreRed').val('');
                    $('#claveRed').val('');
                    $('#token').val('');
                    EjecutarUrl(url);

                }
            },
            error: function (data) {
                ObtenerConfiguracionPlaca(tokenPlacaId, modeloPlacaId);
            }
        });
    }
}

function ValidarDescargar() {
    var countError = 0;
    var mensajeError = "";


    var nombreRed = $('#nombreRed').val();
    var claveRed = $('#claveRed').val();
    var token = $('#token').val();

    if (nombreRed.length < 1) {
        countError++;
        mensajeError += "* Debe Ingresar el nombre de su RED. </br>";
    }
    if (claveRed.length < 1) {
        countError++;
        mensajeError += "* Debe Ingresar una clave de su RED. </br>";
    }
    if (token.length < 1) {
        countError++;
        mensajeError += "* Debe Ingresar su Key. </br>";
    }

    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}