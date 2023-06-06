$(document).ready(function () {
    $('.sistemaOpc').click(function () {
        var id = $(this).attr('id');
        Direccionar(id);
    });
});

function Direccionar(id) {
    $.ajax({
        type: "POST",
        url: "../Perfil/DireccionarSistema",
        dataType: 'json',
        contentType: 'application/json',
        data: JSON.stringify({ id: id }),
        success: function (data) {
            if (data.id < 1) {
                alertModal(data.mensaje, 'Alerta');
            } else {
                window.location.href = data.url;
                //$('#generarRecetaBtn').attr('disabled', 'disabled');

                //var js = "EjecutarUrl('MonitorPrescripciones')";
                //alertModalPost('La Prescripción Numero ' + data.id + ' fue creada', 'Alerta', js);
            }
            //reporte
        },
        error: function (data) {
            //alert(data.Mensaje);
            alertModal(data.Mensaje, 'Alerta');
        }
    });
}