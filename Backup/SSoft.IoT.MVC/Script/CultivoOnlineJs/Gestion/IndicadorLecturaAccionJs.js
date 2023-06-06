$(document).ready(function () {
    GraficoDiario();
    ObtenerSalud();
    ObtenerAcciones();
    setInterval(function () { ObtenerSalud(); ObtenerAcciones(); }, 60000);
    $('#refrescar').click(function () {
        GraficoDiario();
        ObtenerSalud();
        ObtenerAcciones();
    });
});
function GraficoDiario() {
    $.ajaxSetup({ cache: false });
    $.ajax({
        type: "GET",
        url: '../Gestion/ObtenerIndicadorLectura',
        dataType: 'json',
        success: function (data, result) {
            if (data == null) {
                $('#chart1Header').html('');
                $('#chart1').html('<h1><p>No se encontrarón datos en el dia de hoy para graficar.</p></h1>');
                return false;
            }
            //$('#chart1Header').html('<button class="export" onclick="ExportarExcel(1,' + idCpe + ',0,0,1900);">Exportar a Excel</button>');
            //CreateIconExport();
            var jsonString = '[{name: "Equipo Lectura",type: "line",pointInterval: 300000,data: [' + data.NacionalSubida + ']}]';
            //var jsonString = "[{name: 'Nacional Subida',type: 'line',pointInterval: 604800000,data: [" + data.NacionalSubida + "]}}]";
            //PintarGraficoMinutos(jsonString, 'chart1');
            PintarGraficoMinutos(data.NacionalSubida, 'chart1');
            ObtenerConfigLectura();
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
                return '<b>' + this.series.name + '</b><br/>' + totalFor.toFixed(2) + ' (C)';
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
    var string = rowObject[5];
    var arr = string.split('&');

    var valorOptimo = arr[1];
    var valorAccion = arr[0];
    var valor100 = arr[2];


    //var op = valorOptimo;
    //var ac = valorAccion;

    //var pOp = (parseFloat(op) / valor100) * 100;
    //var totalOp = 100 - pOp;
    //var pAc = (parseFloat(ac) / valor100) * 100;
    //var totalAc = 100 - pAc;

    var opI = parseInt(valorOptimo);
    var acI = parseInt(valorAccion);
    var cien = parseInt(valor100);



    //var htmls = "entre " + valorAccion + " y " + valorOptimo + "";
    return Regla(opI, acI, cien);
}
function Regla(optimo, accion, cien) {
    if (cien >= 300) {
        //CONFIGURACION TITAN
        if (accion > 0 && accion <= 459)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 460 && accion <= 560)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 561 && accion <= 661)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 662 && accion <= 772)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 773 && accion <= 1025)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
    }

    if (cien >= 100 && cien <= 299) {
        //CONFIGURACION EUROPA
        if (accion > 0 && accion <= 329)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #006B41;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 329 && accion <= 529)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #00A052;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 530 && accion <= 730)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #E2E54E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 731 && accion <= 931)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #F48432;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
        if (accion > 932 && accion <= 1025)
            return '<div style="margin-left:23%;width: 30px;height: 30px;-moz-border-radius: 50%;-webkit-border-radius: 50%;border-radius: 50%;background: #C5231E;cursor: pointer;" title="entre ' + accion + 'y ' + optimo + '"></div>';
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
function ObtenerConfigLectura() {
    var mypostData = $("#tareasTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Nombre, s.Semaforo, s.Porcentajes,s.PorcentajeActual, s.Encendido
    $("#tareasTable").jqGrid({
        url: "../Gestion/ObtenerConfiguraciones", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Nombre del Equipo", "Nombre Tarea", "Semaforo", "Valor Actual", "Nivel Humedad Esperado", "Agua Accionada", "Nivel Humedad Actual", "Nivel Rango"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "NombreEquipo", index: "Nombre", align: "center", width: 100, sorttype: "int", hidden: false },
        { name: "Nombre", index: "Nombre", align: "center", width: 100, sorttype: "int", hidden: false },
        { name: "Semaforo", index: "Semaforo", sorttype: "int", hidden: true },
        { name: "PorcentajeActual", index: "PorcentajeActual", align: "center", width: 80, sorttype: "string", hidden: false },
        { name: "Porcentajes", index: "Porcentajes", align: "center", width: 80, sorttype: "string", hidden: false, formatter: RangoPorcentaje },
        { name: "Encendido", index: "Encendido", align: "center", width: 90, hidden: false, formatter: Encendido },
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
        url: "../Gestion/ObtenerConfiguraciones", page: 1
    }).trigger("reloadGrid");
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
function ObtenerSalud() {
    var mypostData = $("#saludTable").jqGrid("getGridParam", "postData");

    //s.Id, s.Nombre, s.Semaforo, s.Porcentajes,s.PorcentajeActual, s.Encendido
    $("#saludTable").jqGrid({
        url: "../Gestion/ObtenerSalud", //int establecimientoId, int especieId, string fechaIngreso
        datatype: "json",
        mtype: "POST",
        colNames: ["Id", "Nombre del Equipo", "¿Ejecutando una Tarea?", "Estado del Equipo", "", ""],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Nombre", index: "Nombre", align: "center", width: 160, sorttype: "int", hidden: false },
        { name: "EstadoTarea", index: "EstadoTarea", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "MensajeEstadoSalud", index: "MensajeEstadoSalud", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "EstadoSalud", index: "EstadoSalud", align: "center", width: 30, hidden: true },
        { name: "", index: "", align: "center", width: 55, hidden: false, formatter: Salud }],
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
        colNames: ["Id", "Nombre del Equipo", "Nombre de la Tarea", "Fecha Ejecución", "Acción Ejecutada"],
        colModel: [
        { name: "Id", index: "Id", sorttype: "int", hidden: true },
        { name: "Nombre", index: "Nombre", align: "center", width: 160, sorttype: "int", hidden: false },
        { name: "NombreTarea", index: "NombreTarea", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "FechaAccion", index: "FechaAccion", align: "center", width: 130, sorttype: "string", hidden: false },
        { name: "Accion", index: "Accion", align: "center", width: 130, sorttype: "string", hidden: false, formatter: BotonesAcciones }],
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