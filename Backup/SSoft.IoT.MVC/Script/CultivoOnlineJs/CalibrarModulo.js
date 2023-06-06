$(document).ready(function () {
    ObtenerModelos();
    $('#guardarCalibracion').click(function () {
        GuardarCalibracion();
    });
    $('#obtenerValor').click(function () {
        GuardarCalibracionParaObtener();
    });
    $('#guardarCalibracionManual').click(function () {
        GuardarCalibracionManual();
    });
    $('#calibrarAuto').click(function () {
        GuardarCalibracionAutomatica();
    });
});
function Calibrar(id, e) {
    $("#porcentajeValueClient").slider({
        min: 0,
        max: 100,
        focus: false,
        range: false

    });
    $("#porcentajeValueClient2").slider({
        min: 0,
        max: 100,
        focus: false,
        range: false

    });

    $('#valorSensorEstimativo').slider({
        min: 0,
        max: 1024,
        focus: false,
        range: false
    });
    $('#myTabContent li:first-child a').tab('show') // Select first tab
    $('#idToken').val(id);
}
function Acciones(cellvalue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModal" style="background-color: yellow;color:black;border:none;" title="Planificar" onclick="Calibrar(' + rowObject[0] + ',' + rowObject[3] + ');">Calibrar Modulo</button>';
    return htmls;
}
function ObtenerModelos() {
    var mypostData = $("#modeloTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Identificacion, s.Categoria, s.FechaNacimiento, s.FechaCompra, s.KGIngreso, s.PrecioCompra
    $("#modeloTable").jqGrid({
        url: "ObtenerModelos", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Identificación", "Modelo", "", "Acciones"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Identificacion", index: "Identificacion", align: "center", width: 80, sorttype: "int", hidden: false },
        { name: "Modelo", index: "Modelo", width: 100, align: "center", sorttype: "string", sortable: true, search: true },
        { name: "IdModelo", index: "IdModelo", sorttype: "int", hidden: true },
        { name: 'acciones', width: 80, index: status, align: "center", editable: true, formatter: Acciones }],
        width: '55%',
        height: 500,
        toppager: true,
        pager: $("#modeloPager"),
        rowNum: 200,
        rowList: [200],
        viewrecords: true, // Specify if "total number of records" is displayed
        sortname: "Id",
        sortorder: "asc",
        caption: "",
        loadComplete: function () {
            //1349 mi notebook
            //1920 otra pantalla mas grand
            //alert($(window).width());
            if ($(window).width() > 1600) {
                $("#modeloTable").jqGrid('setGridWidth', $(window).width() - 870, true);
            } else {
                if ($(window).width() < 500) {
                    $("#modeloTable").jqGrid('setGridWidth', $(window).width(), true);
                }
                else {
                    $("#modeloTable").jqGrid('setGridWidth', $(window).width() - 270, true);
                }
            }
        }
    }).navGrid("#modeloPager",
    { refresh: true, add: false, edit: false, del: false },
        {}, // settings for edit
        {}, // settings for add
        {}, // settings for delete
        { sopt: ['cn'] }, // Search options. Some options can be set on column level
        { closeAfterSearch: true }
     );
    jQuery("#modeloTable").jqGrid('setGridParam', {
        url: "ObtenerModelos", page: 1
    }).trigger("reloadGrid");
}
function GuardarCalibracion() {
    if (ValidarCalibrar()) {
        var porcentaje = $('#porcentajeValueClient').val();
        var idToken = $('#idToken').val();
        var valorSensor = $('#valorSensorTxt').val();
        parseInt
        $.ajax({
            type: "GET",
            url: "GuardarCalibracion",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                tokenplacaId: idToken,
                porcentaje: porcentaje,
                valorSensor: parseInt(valorSensor)
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.CodigoMensaje > 0) {
                    alert(data.Mensaje);
                } else {
                    alert("Calibración Registrada.");
                    $('#myModal').modal('hide');
                    ObtenerModelos();
                }
            },
            error: function (data) {
                ObtenerModelos();
            }
        });
    }
}
function GuardarCalibracionManual() {
    if (ValidarCalibrarManual()) {
        var porcentaje = $('#porcentajeValueClient2').val();
        var idToken = $('#idToken').val();
        var valorSensor = $('#valorSensorEstimativo').val();
        parseInt
        $.ajax({
            type: "GET",
            url: "GuardarCalibracion",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                tokenplacaId: idToken,
                porcentaje: porcentaje,
                valorSensor: parseInt(valorSensor)
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.CodigoMensaje > 0) {
                    alert(data.Mensaje);
                } else {
                    alert("Calibración Registrada.");
                    $('#myModal').modal('hide');
                    ObtenerModelos();
                }
            },
            error: function (data) {
                ObtenerModelos();
            }
        });
    }
}
function GuardarCalibracionAutomatica() {

    var porcentaje = 100;
    var idToken = $('#idToken').val();
    var valorSensor = -2;
    $.ajax({
        type: "GET",
        url: "GuardarCalibracion",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            tokenplacaId: idToken,
            porcentaje: porcentaje,
            valorSensor: valorSensor
        },
        success: function (data) {
            //$('#especieSelect').removeAttr('disabled', 'disabled');
            if (data.CodigoMensaje > 0) {
                alert(data.Mensaje);
            } else {
                alert("Calibración Registrada.");
                $('#myModal').modal('hide');
                ObtenerModelos();
            }
        },
        error: function (data) {
            ObtenerModelos();
        }
    });

}
function ValidarCalibrar() {
    var countError = 0;
    var mensajeError = "";

    var token = $('#idToken').val();
    var valor = $('#valorSensorTxt').val();


    if (token < 1) {
        countError++;
        mensajeError += "* No tiene token seleccionado </br>";
    }
    if (valor < 1) {
        countError++;
        mensajeError += "* Debe Obtener el valor del sensor </br>";
    }

    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function ValidarCalibrarManual() {
    var countError = 0;
    var mensajeError = "";

    var token = $('#idToken').val();
    var valor = $('#valorSensorEstimativo').val();


    if (token < 1) {
        countError++;
        mensajeError += "* No tiene token seleccionado </br>";
    }
    if (valor < 1) {
        countError++;
        mensajeError += "* Debe Obtener el valor del sensor </br>";
    }

    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function GuardarCalibracionParaObtener() {
    if (ValidarObtenerValor()) {
        var idToken = $('#idToken').val();
        $.ajax({
            type: "GET",
            url: "GuardarCalibracion",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                tokenplacaId: idToken,
                porcentaje: 0,
                valorSensor: -1
            },
            success: function (data) {
                if (data.CodigoMensaje > 0) {
                    alert(data.Mensaje);
                } else {
                    alert("Espere 1 minuto aprox para ver el ultimo valor.");
                    ObtenerValorSensor();
                    setInterval(function () { ObtenerValorSensor(); }, 60000);
                }
            },
            error: function (data) {
                alert("Error contacte con su administrador.");
                ObtenerModelos();
            }
        });
    }
}
function ValidarObtenerValor() {
    var countError = 0;
    var mensajeError = "";

    var token = $('#idToken').val();

    if (token < 1) {
        countError++;
        mensajeError += "* No tiene token seleccionado </br>";
    }

    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function ObtenerValorSensor() {
    if (ValidarObtenerValor()) {
        var idToken = $('#idToken').val();
        $.ajax({
            type: "GET",
            url: "ObtenerValorSensor",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                tokenplacaId: idToken
            },
            success: function (data) {
                if (data.length > 0) {

                    var spl = data.split('&');
                    var vl = spl[0];
                    $('#valorSensorTxt').val(vl);

                    var htmls = vl + " con fecha: " + spl[1];
                    $('#valorS').html(htmls);

                } else {
                    $('#valorS').html("<b>espere...</b>");
                }
            },
            error: function (data) {
                alert("Error contacte con su administrador.");
                ObtenerModelos();
            }
        });
    }
}