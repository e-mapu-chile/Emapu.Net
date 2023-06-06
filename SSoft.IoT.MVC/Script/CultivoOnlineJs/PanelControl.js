$(document).ready(function () {
    ObtenerModelos();
 
    $(':checkbox').checkboxpicker();
    $('.cheek').change(function () {
        //CorreoChange();
        var at = $('#correoChk').prop('checked');
        if (at == true)
            $('#panelEnvio').show();
        else
            $('#panelEnvio').hide();
    });

});
function LimpiarForm() {
    $('#correoChk').prop('checked', false);
    $('#tareasChk').prop('checked', false);
    $('#minChk').prop('checked', false);
    $('#informeChk').prop('checked', false);
    $('#panelEnvio').hide();
}
function Notificaciones(id, e) {
    $('#tokenPlacaIdHidden').val(id);
    SetPanelControl();

}
function Acciones(cellvalue, options, rowObject) {
    var htmls = ' <button data-toggle="modal" data-target="#myModal" style="background-color: orange;color:black;border:none;" title="Planificar" onclick="Notificaciones(' + rowObject[0] + ',' + rowObject[3] + ');">Notificaciones</button>';
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
function SetPanelControl() {
    var tokenId = $('#tokenPlacaIdHidden').val();
    LimpiarForm();
    $.ajax({
        type: "GET",
        url: "ObtenerPanelControl",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            tokenId: tokenId
        },
        success: function (data) {
            if (data.Id > 0) {
               
                $('#panelControlIdHidden').val(data.Id);
                if(data.AguaDadaMas3Min)
                    $('#minChk').prop('checked', true);
                else
                    $('#minChk').prop('checked', false);
                if (data.EnvioCorreo) {
                    $('#correoChk').prop('checked', true);
                    $('#panelEnvio').show();
                }
                else {
                    $('#correoChk').prop('checked', false);
                    $('#panelEnvio').hide();
                }
                if(data.SinTareaEjecutando)
                    $('#tareasChk').prop('checked', true);
                else
                    $('#tareasChk').prop('checked', false);
                if(data.EnvioCorreoInforme1Hora)
                    $('#informeChk').prop('checked', true);
                else
                    $('#informeChk').prop('checked', false);
                    
            }
        },
        error: function (data) {
            alert("error");
        }
    });
}
function CorreoChange() {
    //panelId, bool correo, bool tarea, bool minutos, bool informe
    var tokenId = $('#tokenPlacaIdHidden').val();
    var panelId = $('#panelControlIdHidden').val();
    var correo = $('#correoChk').prop('checked');
    var tarea = $('#tareasChk').prop('checked');
    var minutos = $('#minChk').prop('checked');
    var informe = $('#informeChk').prop('checked');
    $.ajax({
        type: "GET",
        url: "GuardarPanel",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            tokenId: tokenId,
            panelId: panelId,
            correo: correo,
            tarea: tarea,
            minutos: minutos,
            informe: informe
        },
        success: function (data) {
            if (data.IdNuevo > 0) {
                $('#panelControlIdHidden').val(data.IdNuevo);
                alert("Accion realizada");
                $('#myModal').modal('hide');
            }
        },
        error: function (data) {
            alert("error");
        }
    });
}