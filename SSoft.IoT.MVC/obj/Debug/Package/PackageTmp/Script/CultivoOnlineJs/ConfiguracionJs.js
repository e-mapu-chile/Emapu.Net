$(document).ready(function () {
    ObtenerModelos();

    $('#guardarConfig').click(function () {
        GuardarTarea();
    });

    //var slider = new Slider("#ex6");
    //slider.on("slide", function (sliderValue) {
    //    document.getElementById("ex6SliderVal").textContent = sliderValue;
    //});
});
function Configurar(id, modeloId) {
    //$("#ex6").slider();
    //$("#ex6").on("slide", function (slideEvt) {
    //    $("#ex6SliderVal").text(slideEvt.value);
    //});

    $('#tokenPlacaIdHidden').val(id);
    $('#modeloIdHidden').val(modeloId);
    ObtenerConfiguracionPlaca(id, modeloId);

    $('#input1').val(100);
    $('#input2').val(100);
    $('#input3').val(100);
    $('#input4').val(100);
    $('#input1').change(function () {
        var value = $(this).val();
        $('#input1Valor').text(value);
        var accion = parseInt(value);
        //CONFIGURACION EUROPA
        ColoresConfiguracion(1, accion)

    });
    $('#input2').change(function () {
        var value = $(this).val();
        $('#input2Valor').text(value);
        var accion = parseInt(value);
        //CONFIGURACION EUROPA
        ColoresConfiguracion(2, accion)
    });
    $('#input3').change(function () {
        var value = $(this).val();
        $('#input3Valor').text(value);
        var accion = parseInt(value);
        //CONFIGURACION EUROPA
        ColoresConfiguracion(3, accion)
    });
    $('#input4').change(function () {
        var value = $(this).val();
        $('#input4Valor').text(value);
        var accion = parseInt(value);
        //CONFIGURACION EUROPA
        ColoresConfiguracion(4, accion)
    });
}

function ColoresConfiguracion(numInput, accion) {
    //CONFIGURACION EUROPA
    if (accion > 0 && accion < 280)
        $('#input' + numInput + 'Class').attr('class', 'muyHumedo');
    if (accion >= 280 && accion < 529)
        $('#input' + numInput + 'Class').attr('class', 'humedo');
    if (accion >= 529 && accion < 730)
        $('#input' + numInput + 'Class').attr('class', 'medioHumedo');
    if (accion >= 730 && accion < 931)
        $('#input' + numInput + 'Class').attr('class', 'medioSeco');
    if (accion >= 931 && accion < 1025)
        $('#input' + numInput + 'Class').attr('class', 'seco');
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
            if (data.Id > 0) {
                $('#nombreTarea').val(data.Nombre);
                $('#idConfHidden').val(data.Id);
                $('#fechaInicio').val(data.FechaInicio);
                $('#fechaFin').val(data.FechaFin);
                $('#input1').val(data.Valor1);
                $('#input2').val(data.Valor2);
                $('#input3').val(data.Valor3);
                $('#input4').val(data.Valor4);
                $('#numeroIteracciones').val(data.Iteracciones);
                $('#ejecuntandoTarea').val(data.EjecutandoTarea);
                $('#segundos').val(data.SegundosAgua);

                $('#input1Valor').text(data.Valor1);
                var accion = parseInt(data.Valor1);
                //CONFIGURACION EUROPA
                ColoresConfiguracion(1, accion)

                $('#input2Valor').text(data.Valor2);
                var accion2 = parseInt(data.Valor2);
                //CONFIGURACION EUROPA
                ColoresConfiguracion(2, accion2)

                $('#input3Valor').text(data.Valor3);
                var accion3 = parseInt(data.Valor3);
                //CONFIGURACION EUROPA
                ColoresConfiguracion(3, accion3)

                $('#input4Valor').text(data.Valor4);
                var accion4 = parseInt(data.Valor4);
                //CONFIGURACION EUROPA
                ColoresConfiguracion(4, accion4)
            }
            else {
                $('#nombreTarea').val('');
                $('#idConfHidden').val(0);
                $('#fechaInicio').val("");
                $('#fechaFin').val("");
                $('#input1').val(100);
                $('#input2').val(100);
                $('#input3').val(100);
                $('#input4').val(100);
                $('#numeroIteracciones').val(2);
                $('#ejecuntandoTarea').val(1);

                $('#input1Valor').text('100');
                var accion = parseInt('100');
                //CONFIGURACION EUROPA
                ColoresConfiguracion(1, accion)

                $('#input2Valor').text('100');
                var accion2 = parseInt('100');
                //CONFIGURACION EUROPA
                ColoresConfiguracion(2, accion2)

                $('#input3Valor').text('100');
                var accion3 = parseInt('100');
                //CONFIGURACION EUROPA
                ColoresConfiguracion(3, accion3)

                $('#input4Valor').text('100');
                var accion4 = parseInt('100');
                //CONFIGURACION EUROPA
                ColoresConfiguracion(4, accion4)
            }
        },
        error: function (data) {
            alert("error");
        }
    });
}
function Regla(optimo, accion, min, max) {

    ////////////CONFIGURACION CAPACITIVO
    ////////////SERIA ENTONCES SECO - HUMEDO / 5 => ESE VALOR ES LA SUMATORIA
    //////////int diferenciaSecoHumedo = ton.ValorBajo.Value - ton.ValorSensor.Value;
    //////////decimal flo = diferenciaSecoHumedo / 5;
    //////////decimal roundedB = Math.Round(flo, 0, MidpointRounding.AwayFromZero); // Output: 2
    //////////int x = Convert.ToInt32(roundedB);

    //////////int muyHumedo = ton.ValorSensor.Value + x;
    //////////if (valorLecturaPr >= 0 && valorLecturaPr <= muyHumedo)
    //////////    o.Nivel = 1;//muy humedo
    //////////int humedo = muyHumedo + x;
    //////////if (valorLecturaPr > muyHumedo && valorLecturaPr <= humedo)
    //////////    o.Nivel = 2;// humedo
    //////////int medioHumedo = humedo + x;
    //////////if (valorLecturaPr > humedo && valorLecturaPr <= medioHumedo)
    //////////    o.Nivel = 3;//medio humedo
    //////////int medioSeco = medioHumedo + x;
    //////////if (valorLecturaPr > medioHumedo && valorLecturaPr <= medioSeco)
    //////////    o.Nivel = 4;//medio seco
    //////////if (valorLecturaPr > medioSeco && valorLecturaPr < 1025)
    //////////    o.Nivel = 5;//seco
    var diferenciaSecoHumedo = max - min;
    var flo = diferenciaSecoHumedo / 5;//SENSOR CAPACITIVO EN EL AIRE
    var x = Math.round(flo); // x ES LA CANTIDAD QUE HAY QUE SUMAR

    //CONFIGURACION CAPACITIVO
    var muyHumedo = parseInt(min) + parseInt(x);
    if (accion >= 0 && accion <= muyHumedo)
        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
    var humedo = parseInt(muyHumedo) + x;
    if (accion > x && accion <= humedo)
        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
    var medioHumedo = parseInt(humedo) + x;
    if (accion > humedo && accion <= medioHumedo)
        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
    var medioSeco = parseInt(medioHumedo) + x;
    if (accion > medioHumedo && accion <= medioSeco)
        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
    var seco = parseInt(medioSeco) + x;
    if (accion > medioSeco && accion <= 1024)
        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';

    //////////if (cien >= 300) {
    //////////    //CONFIGURACION TITAN
    //////////    if (accion >= 0 && accion < 382)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
    //////////    if (accion > 381 && accion < 460)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
    //////////    if (accion > 461 && accion < 549)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
    //////////    if (accion > 550 && accion < 674)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
    //////////    if (accion > 675 && accion < 1024)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    //////////}

    //////////if (cien >= 100 && cien <= 299) {
    //////////    //CONFIGURACION EUROPA
    //////////    if (accion > 0 && accion < 329)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
    //////////    if (accion > 329 && accion < 529)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
    //////////    if (accion > 530 && accion < 730)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
    //////////    if (accion > 731 && accion < 931)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
    //////////    if (accion > 932 && accion < 1025)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    //////////}
    //////////if (cien < 100) {
    //////////    //CONFIGURACION EUROPA
    //////////    if (accion > 0 && accion <= 129)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
    //////////    if (accion > 129 && accion <= 300)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
    //////////    if (accion > 300 && accion <= 400)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
    //////////    if (accion > 400 && accion <= 500)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
    //////////    if (accion > 500 && accion <= 1025)
    //////////        return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';

    //////////}
}
function GuardarTarea() {
    if (ValidarTarea()) {
        var nombreTarea = $('#nombreTarea').val();
        var fechaInicio = $('#fechaInicio').val();
        var fechaFin = $('#fechaFin').val();
        var idConf = $('#idConfHidden').val();
        var tokenPlacaId = $('#tokenPlacaIdHidden').val();
        var modeloPlacaId = $('#modeloIdHidden').val();
        var numeroIteracciones = $('#numeroIteracciones').val();
        var apagarTodo = $('#ejecuntandoTarea').val();
        var segundos = $('#segundos').val();
        if (numeroIteracciones.length == 0)
            numeroIteracciones = 2;

        var configuracion = "";
        var flag = 0;
        var valorAccion = 0;
        var valorOptimo = 0;
        var valor1 = $('#input1').val();
        var valor2 = $('#input2').val();
        var valor3 = $('#input3').val();
        var valor4 = $('#input4').val();

        configuracion += "e|" + valor1 + "|" + valor2 + "|" + valor3 + "|" + valor4 + "|" + numeroIteracciones + "|0|" + apagarTodo + "|" + segundos + "";

        //alert(configuracion);

        //e|700|760|800|810|5|0|0|0
        //alert(configuracion);
        //int tokenplacaId, int idConfig, string nombre, string fechaInicio, string fechaFin, string configuracion,int segundos
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
                porcentajeOptimo: 0,
                porcentajeAccion: 0
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