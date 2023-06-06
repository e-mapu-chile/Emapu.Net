$(document).ready(function () {
    ObtenerModelos();

    $('#guardarConfig').click(function () {
        GuardarTarea();
    });
});
function Configurar(id, modeloId) {
    $('#tokenPlacaIdHidden').val(id);
    $('#modeloIdHidden').val(modeloId);
    ObtenerConfiguracionPlaca(id, modeloId);
}
function Acciones(cellvalue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModal" style="background-color: green;color:white;border:none;" title="Planificar" onclick="Configurar(' + rowObject[0] + ',' + rowObject[3] + ');">Planificar cultivo</button>';
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
function QuitarTarea(id) {
    var tokenPlacaId = $('#tokenPlacaIdHidden').val();
    var modeloPlacaId = $('#modeloIdHidden').val();
    var r = confirm("¿Esta seguro de quitar la tarea?");
    if (r == true) {
        $.ajax({
            type: "GET",
            url: "EliminarTarea",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                id: id
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                ObtenerConfiguracionPlaca(tokenPlacaId, modeloPlacaId);
            },
            error: function (data) {
                ObtenerConfiguracionPlaca(tokenPlacaId, modeloPlacaId);
            }
        });
    }
}
function Acciones2(cellvalue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModalPlani" style="background-color: green;color:white;border:none;" title="Editar" onclick="Editar(' + rowObject[0] + ');">Editar</button>';
    htmls += ' <button style="background-color: red;color:white;border:none;" title="Quitar" onclick="QuitarTarea(' + rowObject[0] + ');">Quitar</button>';
    return htmls;
}
function ObtenerConfiguracionPlaca(id, modeloId) {

    $('#modeloIdCliente').val(modeloId);
    $('#modeloIdHidden').val(modeloId);

    var mypostData = $("#configTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Identificacion, s.Categoria, s.FechaNacimiento, s.FechaCompra, s.KGIngreso, s.PrecioCompra
    $("#configTable").jqGrid({
        url: "ObtenerConfiguraciones?tokenPlacaId=" + id + "", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Identificación", "Fecha Inicio", "Fecha Fin", "", "Acciones"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Nombre", index: "Nombre", align: "center", width: 180, sorttype: "int", hidden: false },
        { name: "FechaInicio", index: "FechaInicio", width: 100, align: "center", sorttype: "string", sortable: true, search: true },
        { name: "FechaFin", index: "FechaFin", width: 100, align: "center", sorttype: "string", sortable: true, search: true },
        { name: "Vigente", index: "Vigente", sorttype: "int", hidden: true },
        { name: 'acciones', width: 80, index: status, align: "center", editable: true, formatter: Acciones2 }],
        width: '95%',
        height: 500,
        toppager: true,
        pager: $("#configPager"),
        rowNum: 200,
        rowList: [200],
        viewrecords: true, // Specify if "total number of records" is displayed
        sortname: "Id",
        sortorder: "asc",
        caption: "",
        loadComplete: function () {
            if ($(window).width() < 500) {
                $("#configTable").jqGrid('setGridWidth', $(window).width(), true);
            }
            else {
                $("#configTable").jqGrid('setGridWidth', $(window).width() - 270, true);
            }
        }
    }).navGrid("#configPager",
    { refresh: true, add: false, edit: false, del: false },
        {}, // settings for edit
        {}, // settings for add
        {}, // settings for delete
        { sopt: ['cn'] }, // Search options. Some options can be set on column level
        { closeAfterSearch: true }
     );
    jQuery("#configTable").jqGrid('setGridParam', {
        url: "ObtenerConfiguraciones?tokenPlacaId=" + id + "", page: 1
    }).trigger("reloadGrid");
}
function Editar(id) {
    var modeloId = $('#modeloIdCliente').val();
    $('#idConfHidden').val(id);
    $.ajax({
        type: "GET",
        url: "ObtenerModeloComponenteFrm",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            idConfig: id,
            modeloId: modeloId
        },
        success: function (data) {
            //$('#especieSelect').removeAttr('disabled', 'disabled');
            $('#formularioDinamico').html('');
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    $('#fechaInicio').val(data[i].FechaInicio);
                    $('#fechaFin').val(data[i].FechaFin);
                    var htmls = '<div style="margin-left:0%;">';
                    htmls += '</div><div style="margin-left:1%;">';
                    htmls += '<table class="tablaFormulario4Cols">';
                    htmls += '<tr>';
                    htmls += '<td style="padding-left:0%;"><b>-' + data[i].Descripcion + ':</b></td>';
                    htmls += '<td style="padding-left:20%;"><b>H</b></td>';
                    htmls += '<td><input class="span2 confvalue" valorCienPor="' + data[i].Valor100Porciento + '"  pindigital="' + data[i].PinDigital + '" pinanalogo="' + data[i].PinAnalogo + '"  id="accion_' + data[i].PinAnalogo + '" type="text" style="width:100%;"/><td>';
                    htmls += '<b>S</b></td>';
                    htmls += '</tr>';
                    htmls += '</table>';
                    htmls += '</div></br>';
                    $('#formularioDinamico').append(htmls);
                    $('#nombreTarea').val(data[i].Nombre);

                    var op = data[i].ValorOptimo;
                    var ac = data[i].ValorAccion;

                    //var pOp = (parseFloat(op) / data[i].Valor100Porciento) * 100;
                    //var totalOp = 100 - pOp;
                    //var pAc = (parseFloat(ac) / data[i].Valor100Porciento) * 100;
                    //var totalAc = 100 - pAc;

                    //var opI = parseInt(totalOp);
                    //var acI = parseInt(totalAc);
                    var opI = parseInt(op);
                    var acI = parseInt(ac);
                    var min = data[i].Valor100Porciento;
                    var htmlss = Regla(opI, acI, min);
                    $('#nivelRango').html(htmlss);
                    $("#accion_" + data[i].PinAnalogo + "").slider({
                        min: min,
                        max: 1024,
                        value: [opI, acI],
                        //value: [acI, opI],
                        focus: false,
                        range: true

                    });
                    $(".span2").slider().on('slideStart', function (ev) {
                        originalVal = $(".span2").data('slider').getValue();
                    });

                    $(".span2").slider().on('slideStop', function (ev) {
                        //var newVal = $(".span2").data('slider').getValue();
                        //if (originalVal != newVal) {
                        //    alert('Value Changed!');
                        //}
                        var configuracion;
                        var valorAccion;
                        var valorOptimo;
                        var valor100Porciento;
                        $(".confvalue").each(function () {
                            var value = $(this).val();
                            var pinAnalogo = $(this).attr('pinanalogo');
                            var pinDigital = $(this).attr('pindigital');
                            valor100Porciento = $(this).attr('valorCienPor');
                            var values = value.split(',');
                            valorAccion = values[1];
                            valorOptimo = values[0];
                        });
                        var htmls = Regla(valorOptimo, valorAccion, valor100Porciento);
                        $('#nivelRango').html(htmls);
                    });
                }
            }
            //var aas = $('#accion_0').val();
        },
        error: function (data) {
            alert("error");
        }
    });
}
function Regla(optimo, accion, cien) {
    if (cien >= 300) {
        //CONFIGURACION TITAN
        if (accion >= 0 && accion < 459)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
        if (accion > 460 && accion < 560)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
        if (accion > 561 && accion < 661)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
        if (accion > 662 && accion < 772)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
        if (accion > 773 && accion < 1025)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    }

    if (cien >= 100 && cien <= 299) {
        //CONFIGURACION EUROPA
        if (accion > 0 && accion < 329)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
        if (accion > 329 && accion < 529)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
        if (accion > 530 && accion < 730)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
        if (accion > 731 && accion < 931)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
        if (accion > 932 && accion < 1025)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    }
    if (cien < 100) {
        //CONFIGURACION EUROPA
        if (accion > 0 && accion <= 129)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 129 && accion <= 300)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 300 && accion <= 400)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 400 && accion <= 500)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 500 && accion <= 1025)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';

    }
}
function GuardarTarea() {
    if (ValidarTarea()) {
        var nombreTarea = $('#nombreTarea').val();
        var fechaInicio = $('#fechaInicio').val();
        var fechaFin = $('#fechaFin').val();
        var idConf = $('#idConfHidden').val();
        var tokenPlacaId = $('#tokenPlacaIdHidden').val();
        var modeloPlacaId = $('#modeloIdHidden').val();

        var configuracion = "";
        var flag = 0;
        var valorAccion = 0;
        var valorOptimo = 0;
        $(".confvalue").each(function () {
            var value = $(this).val();
            var pinAnalogo = $(this).attr('pinanalogo');
            var pinDigital = $(this).attr('pindigital');
            var valor100Porciento = $(this).attr('valorCienPor');
            var values = value.split(',');
            valorAccion = values[1];
            valorOptimo = values[0];

            if (flag == 0) {
                //DEJAR EL MENOR SIEMRE EN VALOR OPTIMO
                configuracion += "" + pinAnalogo + "&" + valorAccion + "&>&" + valorOptimo + "&" + pinDigital + "";
                flag++;
            } else {
                configuracion += ";" + pinAnalogo + "&" + valorAccion + "&>&" + valorOptimo + "&" + pinDigital + "";
            }
        });
        //0&500&>&150&6;1&1000&>&500&7
        //alert(configuracion);
        //int tokenplacaId, int idConfig, string nombre, string fechaInicio, string fechaFin, string configuracion
        $.ajax({
            type: "GET",
            url: "GuardarTarea",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                tokenplacaId: tokenPlacaId,
                idConfig: idConf,
                nombre: nombreTarea,
                fechaInicio: fechaInicio,
                fechaFin: fechaFin,
                configuracion: configuracion,
                porcentajeOptimo: valorOptimo,
                porcentajeAccion: valorAccion
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.CodigoMensaje > 0) {
                    alert(data.Mensaje);
                } else {
                    alert("Tarea Ingresada.");
                    $('#myModalPlani').modal('hide');
                    ObtenerConfiguracionPlaca(tokenPlacaId, modeloPlacaId);
                }
            },
            error: function (data) {
                ObtenerConfiguracionPlaca(tokenPlacaId, modeloPlacaId);
            }
        });
    }
}
function ValidarTarea() {
    var countError = 0;
    var mensajeError = "";


    var nombreTarea = $('#nombreTarea').val();
    var fechaInicio = $('#fechaInicio').val();
    var fechaFin = $('#fechaFin').val();

    if (nombreTarea.length < 4) {
        countError++;
        mensajeError += "* Debe Ingresar un nombre de la tarea, al menos 3 caracteres. </br>";
    }
    if (fechaInicio.length < 4) {
        countError++;
        mensajeError += "* Debe Ingresar una fecha de inicio. </br>";
    }
    if (fechaFin.length < 4) {
        countError++;
        mensajeError += "* Debe Ingresar una fecha de fin. </br>";
    }

    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}