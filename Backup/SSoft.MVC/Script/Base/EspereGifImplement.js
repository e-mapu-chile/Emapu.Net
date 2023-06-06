var flag1 = 0;
var flag2 = 0;
$(document).ready(function () {
    flag1 = 0;
    flag2 = 0;
    //$('.rutInput').Rut({
    //    on_error: function () {
    //        $('#rutComprador').val('');
    //    }
    //});
    $(':input[type="number"]').blur(function () {
        var id = $(this).attr('id');
        var cantidad = $('#' + id).val();
        if (cantidad.toString() > 0) {
            var coma = cantidad.toString().indexOf(",");
            var punto = cantidad.toString().indexOf(".");
            var menos = cantidad.toString().indexOf("-");
            var mas = cantidad.toString().indexOf("+");
            if (coma > 0 || punto > 0 || menos > 0 || mas > 0) {
                $('#' + id).val(0);
            }
        }
        else {
            $('#' + id).val(0);
        }
    });
});
function ShowEspere(obj) {
    $('#' + obj).addClass('loadinggif');
    $('#' + obj).attr('disabled', 'disabled');
}

function HideEspere(obj) {
    $('#' + obj).removeClass('loadinggif');
    $('#' + obj).removeAttr('disabled');
}


function callBackgroundVenta(tiempo) {
    NProgress.done();
    $.ajax({
        type: "GET",
        url: "../Anabolico/ObtenerCantidadSolicitudesaVender",
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            NProgress.done();
            if (data == null) {
                alertModal("Su session a caducado", 'Alerta');
                return "";
            } else {
                NProgress.done();
                //alert(data)
                setInterval(function () {
                    NProgress.done();

                    callBackgroundVenta2(100, data);
                }, tiempo * 3);

            }
            NProgress.done();
        },
        error: function (data) {
            NProgress.done();
            return "";
        }
    });
}

function callBackgroundVenta2(tiempo, data) {
    setInterval(function () {
        if (data > 0) {
            $('#avisoLink').show();
        } else {
            $('#avisoLink').hide();
        }
    }, tiempo);

}


function callBackgroundVentaPerfil(tiempo) {
    NProgress.done();
    $.ajax({
        type: "GET",
        url: "/Anabolico/ObtenerCantidadSolicitudesaVender",
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            NProgress.done();
            if (data == null) {
                alertModal("Su session a caducado", 'Alerta');
                return "";
            } else {
                NProgress.done();
                //alert(data)
                setInterval(function () {
                    NProgress.done();
                    callBackgroundVentaPerfil2(100, data);
                }, 500);

            }
            NProgress.done();
        },
        error: function (data) {
            NProgress.done();
            return "";
        }
    });
}

function callBackgroundVentaPerfil2(tiempo, data) {
    setInterval(function () {
        if (data > 0) {
            $('#avisoLink').show();
        } else {
            $('#avisoLink').hide();
        }
    }, tiempo);
}



function callBackgroundVencido(tiempo) {
    NProgress.done();
    $.ajax({
        type: "GET",
        url: "../Anabolico/ObtenerCantidadProductoVencidos",
        dataType: 'json',
        contentType: 'application/json',
        success: function (data) {
            NProgress.done();
            if (data == null) {
                alertModal("Su session a caducado", 'Alerta');
                return "";
            } else {
                NProgress.done();
                //alert(data)
                setInterval(function () {
                    NProgress.done();

                    callBackgroundVencido2(100, data);
                }, tiempo * 3);

            }
            NProgress.done();
        },
        error: function (data) {
            NProgress.done();
            return "";
        }
    });
}

function callBackgroundVencido2(tiempo, data) {
    setInterval(function () {
        if (data > 0) {
            $('#vencidoLink').show();
        } else {
            $('#vencidoLink').hide();
        }
    }, tiempo);

}


function alertModal(mensaje, titulo) {
    $('#alertModal').modal('show');
    $('#tituloAlert').text(titulo);
    $('#mensajeAlert').text(mensaje);
}


function alertModalPost(mensaje, titulo, js) {
    $('#alertModal').modal('show');
    $('#tituloAlert').text(titulo);
    $('#mensajeAlert').text(mensaje);

    $('#okBtn').click(function () {
        eval(js);
    });
    
}


function ConfirmModal(mensaje, titulo, js) {
    $('#siBtn').show();
    $('#confirmModal').modal({ keyboard: false });
    $('#tituloConfirm').text(titulo);
    $('#mensajeConfirm').text(mensaje);
    $('.noBtn').click(function () {
        $('#confirmModal').modal('hide');
    });
    $('#siBtn').click(function () {
        $('#siBtn').hide();
        eval(js);
    });
}