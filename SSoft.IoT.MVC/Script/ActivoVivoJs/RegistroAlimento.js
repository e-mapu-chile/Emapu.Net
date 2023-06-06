
$(document).ready(function () {
    $("#smartwizard").on("showStep", function (e, anchorObject, stepNumber, stepDirection, stepPosition) {
        //alert("You are on step "+stepNumber+" now");
        if (stepPosition === 'first') {
            $("#prev-btn").addClass('disabled');
        } else if (stepPosition === 'final') {
            $("#next-btn").addClass('disabled');
        } else {
            $("#prev-btn").removeClass('disabled');
            $("#next-btn").removeClass('disabled');
        }

    });

    $("#smartwizard").on("leaveStep", function (e, anchorObject, stepNumber, stepDirection) {
        ////var elmForm = $("#form-step-" + stepNumber);
        ////// stepDirection === 'forward' :- this condition allows to do the form validation
        ////// only on forward navigation, that makes easy navigation on backwards still do the validation when going next
        ////if (stepDirection === 'forward' && elmForm) {
        ////    elmForm.validator('validate');
        ////    var elmErr = elmForm.children('.has-error');
        ////    if (elmErr && elmErr.length > 0) {
        ////        // Form validation failed
        ////        return false;
        ////    }
        ////}
        ////return true;


        return ValidarFormulario(stepNumber);
    });

    // Toolbar extra buttons
    ////////var btnFinish = $('<button></button>').text('Finalizar')
    ////////                                 .addClass('btn btn-info')
    ////////                                 .on('click', function () {
    ////////                                     Finalizar();
    ////////                                 });
    var btnCancel = $('<button></button>').text('Cancelar')
                                     .addClass('btn btn-danger')
                                     .on('click', function () { $('#smartwizard').smartWizard("reset"); });

    // Please note enabling option "showStepURLhash" will make navigation conflict for multiple wizard in a page.
    // so that option is disabling => showStepURLhash: false


    $('#smartwizard').smartWizard({
        selected: 0,
        theme: 'arrows',
        transitionEffect: 'fade',
        showStepURLhash: false,
        toolbarSettings: {
            toolbarPosition: 'both',
            toolbarExtraButtons: [btnCancel]
        }
    });

    $('#establecimientoSelect').change(function () {
        var id = $(this).val();
        $('#establecimientoHidden').val(id);
        //$('#EstablecimientoIdHiddenForm').val(id);
        ObtenerEstablecimiento(id);
    });
});
function ValidarFormulario(paso) {
    var countError = 0;
    var mensajeError = "";

    if (paso == 0) {
        //PASO1
        var establecimientoId = $('#establecimientoSelect').val();

        if (establecimientoId == 0) {
            countError++;
            mensajeError += "* Debe Seleccionar un Establecimiento. </br>";
        }
    }
    if (paso == 1) {
        var alimentoId = $('#alimentoSelect').val();
        //var nombreLote = $('#lote').val();

        if (alimentoId == 0) {
            countError++;
            mensajeError += "* Debe Seleccionar un Alimento. </br>";
        }
        //if (nombreLote.length < 4) {
        //    countError++;
        //    mensajeError += "* Debe Ingresar un Lote, al menos 3 caracteres. </br>";
        //}
    }
    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function ObtenerEstablecimiento(id) {
    $('#establecimientoSelect').attr('disabled', 'disabled');
    $.ajax({
        type: "GET",
        url: "../Animal/ObtenerEstablecimiento",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            id: id
        },
        success: function (data) {
            //$('#especieSelect').removeAttr('disabled', 'disabled');
            if (data.Mensaje == "OK") {
                $('#nombreEstablecimiento').val(data.Establecimiento.Nombre);
                $('#nombreTitularEstablecimiento').val(data.Establecimiento.Titular);
                $('#direccion').val(data.Establecimiento.Direccion);
                //$('#enfermedadSelect option').remove();
                //$('#enfermedadSelect').append('<option value="0">Seleccione </option>');
                //if (data.Enfermedades.length > 0) {
                //    for (var i = 0; i < data.Enfermedades.length; i++) {
                //        $('#enfermedadSelect').append('<option value="' + data.Enfermedades[i].Id + '">' + data.Enfermedades[i].Descripcion + '</option>');
                //    }
                //}
            } else {
                $('#nombreEstablecimiento').val('');
                $('#nombreTitularEstablecimiento').val('');
                $('#direccion').val('');
            }
        },
        error: function (data) {
            $('#establecimientoSelect').removeAttr('disabled', 'disabled');
            jAlert(data.Mensaje, 'Alerta');
        }
    });
}