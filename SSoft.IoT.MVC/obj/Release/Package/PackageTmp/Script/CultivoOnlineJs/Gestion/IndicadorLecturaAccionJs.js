$(document).ready(function () {
    ObtenerSalud();
    $('#refrescar').click(function () {
        ObtenerSalud();
    });
    $('#consultaGrafico').click(function () {
        GraficoDiario();
    });
});
function GraficoDiario() {
    var fechaConsulta = $('#fechaConsulta').val();
    $('#chart1Header').html('');
    $('#chart1').html('<div class="loading ui-state-default ui-state-active" style="margin-left:20%;margin-top:10%; width:200px;">Cargando Informacion...</div>');
    $.ajaxSetup({ cache: false });
    $.ajax({
        type: "GET",
        url: '../Gestion/ObtenerIndicadorLectura?fecha=' + fechaConsulta + '',
        dataType: 'json',
        success: function (data, result) {
            if (data == null) {
                $('#chart1Header').html('');
                $('#chart1').html('</br></br><h4><p>No se encontrarón datos en el dia  para graficar.</p></h4>');
                return false;
            }
            //$('#chart1Header').html('<button class="export" onclick="ExportarExcel(1,' + idCpe + ',0,0,1900);">Exportar a Excel</button>');
            //CreateIconExport();
            var jsonString = '[{name: "Equipo Lectura",type: "line",pointInterval: 300000,data: [' + data.NacionalSubida + ']}]';
            //var jsonString = "[{name: 'Nacional Subida',type: 'line',pointInterval: 604800000,data: [" + data.NacionalSubida + "]}}]";
            //PintarGraficoMinutos(jsonString, 'chart1');
            PintarGraficoMinutos(data.NacionalSubida, 'chart1');

        },
        error: function () {

        }
    });
}
//Grafico Diario
function PintarGraficoMinutos(data, obj) {
    new Highcharts.Chart('chart1', {
        chart: {
            type: 'spline',
            marginRight: 10,
            marginBottom: 25,
            zoomType: 'xy',
            time: {
                timezone: 'America/Santiago'
            },
            backgroundColor: null
        },
        title: {
            text: ''
        },
        subtitle: {
            text: 'Lecturas con los valores entregados por los sensores'
        },
        xAxis: {
            type: 'datetime'//,
            //labels: {
            //    format: '{value:%Y-%b-%e}'
            //}
            /*   dateTimeLabelFormats: {
                 day: "%e. %b",
                 month: "%b '%y",
                 year: "%Y"
               }*/
        },
        yAxis: {
            title: {
                text: ''
            },
            plotLines: [{
                value: 5,
                width: 1//,
                //color: '#808080'
            }],
            min: -5
        },
        tooltip: {
            // headerFormat: '<b>{series.name}</b><br>',
            // pointFormat: '{point.x:%e. %b}: {point.y:.2f} m'
            formatter: function () {
                var valor = this.y.toString();

                //var porcentaje = (parseFloat(valor) / 1024.0) * 100;
                var totalFor = parseFloat(valor); //0 - porcentaje;
                //  var lens = valor.length;
                // valor = valor.substring(0, (lens - 3));
                if (this.y == -1) {
                    return '<b>' + this.series.name + '</b><br/>' + this.x + ':<b style="color:red"> </b>';
                }
                return '<b>' + this.series.name + '</b><br/>' + totalFor.toFixed(2) + ' %';
            }
        },

        plotOptions: {
            spline: {
                marker: {
                    enabled: true
                }
            }
        },
        colors: ['green', 'blue', 'red', '#036', '#000'],
        series: eval('(' + data + ')')
    });
}
function RangoPorcentaje(cellvalue, options, rowObject) {
    var htmls = '';
    if (rowObject[7] == 1) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
    }
    if (rowObject[7] == 2) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
    }
    if (rowObject[7] == 3) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
    }
    if (rowObject[7] == 4) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
    }
    if (rowObject[7] == 5) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    }

    return htmls;

}

function PorcentajeActual(cellvalue, options, rowObject) {
    var value = rowObject[4];
    var porcentaje = (parseFloat(value) / 1024.0) * 100;
    var totalFor = 100 - porcentaje;
    var lens = totalFor.length;
    var totalForSt = String(totalFor).substring(0, (lens - 3));

    return totalFor;
}
function Encendido(cellvalue, options, rowObject) {
    if (rowObject[6] == 1) {
        var htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #37CB50;cursor: pointer;" title="Encendido"></div>';
        return htmls;
    } else {
        var htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: red;cursor: pointer;" title="Apagado"></div>';
        return htmls;
    }
}
function Semaforo(cellvalue, options, rowObject) {
    var htmls = '';

    htmls += '';
    if (rowObject[3] == 1) {
        //htmls += 'ROJO';
        htmls += '<div style="cursor: pointer;"><img src="../../../Image/nivelBajoAgua.png" width="35" height="30" title="por debajo de lo esperado" /></div>';

    }
    if (rowObject[3] == 2) {
        //htmls += 'VERDE';
        htmls += '<div style="cursor: pointer;"><img src="../../../Image/nivelMedioAgua.png" width="35" height="30" title="nivel esperado" /></div>';
    }
    if (rowObject[3] == 3) {
        //htmls += 'AMARILLO';
        htmls += '<div style="cursor: pointer;"><img src="../../../Image/nivelAltoAgua.png" width="35" height="30" title="por sobre lo esperado" /></div>';
    }
    htmls += '';
    return htmls;
}
function NivelAgua(cellvalue, options, rowObject) {
    var htmls = '';

    htmls += '';
    if (rowObject[7] == 1) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="Muy Humedo"></div>';
    }
    if (rowObject[7] == 2) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="Humedo"></div>';
    }
    if (rowObject[7] == 3) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="Medio Humedo"></div>';
    }
    if (rowObject[7] == 4) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="Medio Seco"></div>';
    }
    if (rowObject[7] == 5) {
        htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="Seco"></div>';
    }
    htmls += '';
    return htmls;
}
function ObtenerConfigLectura(id) {
    var mypostData = $("#tareasTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Nombre, s.Semaforo, s.Porcentajes,s.PorcentajeActual, s.Encendido
    $("#tareasTable").jqGrid({
        url: "../Gestion/ObtenerConfiguraciones?idMaquina=" + id + "", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Nombre del Equipo", "Nombre Tarea", "Semaforo", "Valor Actual", "Nivel Humedad Esperado", "Segundos/Agua", "Nivel Humedad Actual", "Nivel Rango"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "NombreEquipo", index: "Nombre", align: "center", width: 100, sorttype: "int", hidden: false },
        { name: "Nombre", index: "Nombre", align: "center", width: 100, sorttype: "int", hidden: false },
        { name: "Semaforo", index: "Semaforo", sorttype: "int", hidden: true },
        { name: "PorcentajeActual", index: "PorcentajeActual", align: "center", width: 80, sorttype: "string", hidden: false },
        { name: "Porcentajes", index: "Porcentajes", align: "center", width: 80, sorttype: "string", hidden: false },//, formatter: RangoPorcentaje },
        { name: "Encendido", index: "Encendido", align: "center", width: 90, hidden: false },
        { name: "Nivel", index: "Porcentajes", align: "center", width: 90, sorttype: "string", hidden: false, formatter: NivelAgua },
        { name: 'acciones', width: 130, index: status, align: "center", hidden: true, editable: true, formatter: Semaforo }],
        width: '80%',
        height: 150,
        toppager: true,
        pager: $("#tareasPager"),
        rowNum: 200,
        rowList: [200],
        viewrecords: true, // Specify if "total number of records" is displayed
        sortname: "Id",
        sortorder: "asc",
        caption: "",
        loadComplete: function () {
            var intViewportWidth = window.innerWidth - 50;
            if (intViewportWidth < 500)
                $('#tareasTable').setGridWidth(intViewportWidth);
            //$("#tareasTable").jqGrid('setGridWidth', $(window).width() - 150, true);

        }
    }).navGrid("#tareasPager",
    { refresh: true, add: false, edit: false, del: false },
        {}, // settings for edit
        {}, // settings for add
        {}, // settings for delete
        { sopt: ['cn'] }, // Search options. Some options can be set on column level
        { closeAfterSearch: true }
     );
    jQuery("#tareasTable").jqGrid('setGridParam', {
        url: "../Gestion/ObtenerConfiguraciones?idMaquina=" + id + "", page: 1
    }).trigger("reloadGrid");
}
function VerConf(id) {
    ObtenerConfigLectura(id);
}
function VerGr(id) {
    GraficoDiario();
}
function VerDet(id) {
    ObtenerAcciones();
}
function Salud(cellvalue, options, rowObject) {
    if (rowObject[4] == 1) {
        var htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #37CB50;cursor: pointer;" title="' + rowObject[3] + '"></div>';
        return htmls;
    } else {
        var htmls = '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: red;cursor: pointer;" title="' + rowObject[3] + '"></div>';
        return htmls;
    }
}
function OpcionesSalud(cellValue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModal" style="background-color: green;color:white;border:none;" title="Ver" onclick="VerConf(' + rowObject[0] + ');" >Ver Resumen</button>';
    return htmls;
}
function OpcionGrafico(cellValue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModalGr" style="background-color: yellow;color:black;border:none;" title="Ver" onclick="VerGr(' + rowObject[0] + ');" >Ver Historial</button>';
    return htmls;
}
function OpcionDetalle(cellValue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModalDetalle" style="background-color: blue;color:white;border:none;" title="Ver" onclick="VerDet(' + rowObject[0] + ');" >Ver Detalle</button>';
    return htmls;
}
function ObtenerSalud() {
    var mypostData = $("#saludTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Nombre, s.Semaforo, s.Porcentajes,s.PorcentajeActual, s.Encendido
    $("#saludTable").jqGrid({
        url: "../Gestion/ObtenerSalud", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Nombre del Equipo", "¿Ejecutando una Tarea?", "Estado del Equipo", "", "", "", "", ""],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Nombre", index: "Nombre", align: "center", width: 160, sorttype: "int", hidden: false },
        { name: "EstadoTarea", index: "EstadoTarea", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "MensajeEstadoSalud", index: "MensajeEstadoSalud", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "EstadoSalud", index: "EstadoSalud", align: "center", width: 30, hidden: true },
        { name: "", index: "", align: "center", width: 55, hidden: false, formatter: Salud },
        { name: "", index: "", align: "center", width: 55, hidden: false, formatter: OpcionesSalud },
        { name: "", index: "", align: "center", width: 55, hidden: false, formatter: OpcionGrafico },
        { name: "", index: "", align: "center", width: 55, hidden: false, formatter: OpcionDetalle }],
        width: '90%',
        height: 200,
        toppager: true,
        pager: $("#saludPager"),
        rowNum: 200,
        rowList: [200],
        viewrecords: true, // Specify if "total number of records" is displayed
        sortname: "Id",
        sortorder: "asc",
        caption: "",
        loadComplete: function () {
            var intViewportWidth = window.innerWidth - 50;
            if (intViewportWidth < 500)
                $('#saludTable').setGridWidth(intViewportWidth);
            //$("#saludTable").jqGrid('setGridWidth', $(window).width() - 6500, true);

        }
    }).navGrid("#saludPager",
    { refresh: true, add: false, edit: false, del: false },
        {}, // settings for edit
        {}, // settings for add
        {}, // settings for delete
        { sopt: ['cn'] }, // Search options. Some options can be set on column level
        { closeAfterSearch: true }
     );
    jQuery("#saludTable").jqGrid('setGridParam', {
        url: "../Gestion/ObtenerSalud", page: 1
    }).trigger("reloadGrid");
}
function BotonesAcciones(cellvalue, options, rowObject) {
    if (rowObject[4] == "Encendido") {
        var htmls = '<div style="color:green;">' + rowObject[4] + ' </div>';
        return htmls;
    } else {
        var htmls = '<div style="color:red;">' + rowObject[4] + ' </div>';
        return htmls;
    }
}
function ObtenerAcciones() {
    var mypostData = $("#accionesTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Nombre, s.Semaforo, s.Porcentajes,s.PorcentajeActual, s.Encendido
    $("#accionesTable").jqGrid({
        url: "../Gestion/ObtenerAccionesEjecutadasMaquinas", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Nombre del Equipo", "Nombre de la Tarea", "Fecha Registro", "Acción Ejecutada", "Litros/Minutos"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Nombre", index: "Nombre", align: "center", width: 160, sorttype: "int", hidden: false },
        { name: "NombreTarea", index: "NombreTarea", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "FechaAccion", index: "FechaAccion", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "Accion", index: "Accion", align: "center", width: 130, sorttype: "string", hidden: true, formatter: BotonesAcciones },
        { name: "LitrosMin", index: "LitrosMin", align: "center", width: 130, sorttype: "string", hidden: false }],
        width: '90%',
        height: 400,
        toppager: true,
        pager: $("#accionesPager"),
        rowNum: 200,
        rowList: [200],
        viewrecords: true, // Specify if "total number of records" is displayed
        sortname: "Id",
        sortorder: "asc",
        caption: "",
        loadComplete: function () {
            var intViewportWidth = window.innerWidth - 50;
            if (intViewportWidth < 500)
                $('#accionesTable').setGridWidth(intViewportWidth);
            //$("#saludTable").jqGrid('setGridWidth', $(window).width() - 6500, true);

        }
    }).navGrid("#accionesPager",
    { refresh: true, add: false, edit: false, del: false },
        {}, // settings for edit
        {}, // settings for add
        {}, // settings for delete
        { sopt: ['cn'] }, // Search options. Some options can be set on column level
        { closeAfterSearch: true }
     );
    jQuery("#accionesTable").jqGrid('setGridParam', {
        url: "../Gestion/ObtenerAccionesEjecutadasMaquinas", page: 1
    }).trigger("reloadGrid");
}