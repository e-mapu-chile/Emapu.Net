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
    var btnFinish = $('<button></button>').text('Finalizar')
                                     .addClass('btn btn-info')
                                     .on('click', function () {
                                         Finalizar();
                                     });
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
            toolbarExtraButtons: [btnFinish, btnCancel]
        }
    });

    $("#ajaxUploadExcel2").ajaxForm({
        iframe: true,
        dataType: "json",
        beforeSubmit: function () {
            //$('#tabs').hide();
            //$('#espereCreandoProtocolo').show();
            //$('#adjuntarExcelDiv2').dialog('close');
            var idEst = $('#establecimientoHidden').val();
            var idEsp = $('#especieHidden').val();
            var nombreLote = $('#lote').val();
            $('#EstablecimientoIdFile').val(idEst);
            $('#EspecieIdFile').val(idEsp);
            $('#NombreLoteFile').val(nombreLote);
            var es = $('#EstablecimientoIdFile').val();
        },
        success: function (result) {
            if (result.id > 0) {
                GrillaAnimales();
                alert(result.mensaje);
                $('#myModalFile').modal('hide');

                //$('#adjuntarExcelDiv2').dialog('close');
                //$('#espereCreandoProtocolo').hide();
                //$('#errorProtocolo').hide();
                //$('#resumenProtocolo').show();
                //$('#numeroProtocoloR').html("<b>" + result.id + "</b>");
                //var enfermedad = $('#enfermedadSelect option:selected').text();
                //$('#enfermedadR').html("<b>" + enfermedad + "</b>");
                //var especie = $('#especieSelect option:selected').text();
                //$('#especieR').html("<b>" + especie + "</b>");
                //var tipoPrueba = $('#tipoPruebaSelect option:selected').text();
                //$('#tipoPruebaR').html("<b>" + tipoPrueba + "</b>");
                //$('#totalAnalisisR').html('<b>' + result.cantidad + '</b>');

                //$('#tabs').hide();

            } else {
                alert(result.mensaje);
                //$('#espereCreandoProtocolo').hide();
                //$('#errorProtocolo').show();

                //$('#categoriaError').html("<b>" + result.cantidadCategoriaError + "</b>");
                //$('#tipoIdentificacionError').html("<b>" + result.cantidadTipoIdentificacionError + "</b>");
                //$('#unidadEdadError').html("<b>" + result.cantidadUnidadEdadError + "</b>");


                //$('#adjuntarExcelDiv2').dialog('close');
                //jAlert(result.mensaje, 'Alerta');
            }
        },
        error: function (result) {
            //jAlert(result.mensaje, 'Alerta');
        }
    });

    var animalFormValidator = $('#animalAddForm').validate({
        onkeyup: true,
        success: $.noop, // Odd workaround for errorPlacement not firing!
        rules: {
            establecimientoSelect: {
                required: true
            },
            especieSelect: {
                required: true
            },
            //fechaIngresoHtml: {
            //    required: true,
            //    date: true
            //},
            IdentificacionAnimal: {
                required: true
            },
            categoriaSelect: {
                required: true
            },
            sexoSelect: {
                required: true
            },
            fechaCompraHtml: {
                required: true,
                date: true
            },
            fechaNacimientoHtml: {
                required: true,
                date: true
            },
            KgIngreso: {
                required: true
            }
        },
        messages: {
            establecimientoSelect: {
                required: "Este dato es Requerido"
            },
            especieSelect: {
                required: "Este dato es Requerido"
            },
            //fechaIngresoHtml: {
            //    required: "Este dato es Requerido",
            //    date: "Debe ser una fecha"
            //},
            IdentificacionAnimal: {
                required: "Esta dato es Requerido"
            },
            categoriaSelect: {
                required: "Este dato es Requerido"
            },
            sexoSelect: {
                required: "Esta dato es Requerido"
            },
            fechaCompraHtml: {
                required: "Este dato es Requerido",
                date: "Debe ser una fecha"
            },
            fechaNacimientoHtml: {
                required: "Este dato es Requerido",
                date: "Debe ser una fecha"
            },
            KgIngreso: {
                required: "Este dato es Requerido"
            }
        },
        submitHandler: function (form) {
            var opts = {
                dataType: 'json',
                beforeSubmit: function () {
                    var valid = $(form).valid();
                    // recolectar los roles
                    if (valid) {
                        //EspereShow();
                        var idEst = $('#establecimientoHidden').val();
                        var idEsp = $('#especieHidden').val();
                        var nombreLote = $('#lote').val();
                        var fechaCompra = $('#fechaCompraHtml').val();
                        var fechaNaci = $('#fechaNacimientoHtml').val();
                        $('#EstablecimientoId').val(idEst);
                        $('#EspecieId').val(idEsp);
                        $('#FechaCompra').val(fechaCompra);
                        $('#FechaNacimiento').val(fechaNaci);
                        $('#NombreLote').val(nombreLote);
                        var es = $('#EstablecimientoId').val();

                        return valid;
                    }
                },
                success: function (result) {
                    if (result.id == 1) {
                        alert("Animal Ingresado Correctamente.");
                        $('#myModal').modal('hide');
                        GrillaAnimales();
                    } else {
                        alert(result.mensaje);
                        //alertModal(result.message, 'Alerta');
                        //alertModal(result.message, 'Alerta');
                    }
                }
            };
            $(form).ajaxSubmit(opts);
        }
    });


    GrillaAnimales();

    $('#lote').blur(function () {
        LoteDisponible();

    });

    $('#establecimientoSelect').change(function () {
        var id = $(this).val();
        $('#establecimientoHidden').val(id);
        //$('#EstablecimientoIdHiddenForm').val(id);
        ObtenerEstablecimiento(id);
    });
    $('#especieSelect').change(function () {
        var id = $(this).val();
        $('#especieHidden').val(id);
        ObtenerCategoria(id);
    });
    $('#categoriaSelect').change(function () {
        var id = $(this).val();
        $('#CategoriaId').val(id);
    });
    $('#sexoSelect').change(function () {
        var id = $(this).val();
        $('#Sexo').val(id);
    });
    $('#agregar').click(function () {
        alert("ASAS");
        $('#f8Dialog').modal('show');


    });
});
function Finalizar() {
    if (ValidarFormularioFinalizar()) {
        var establecimientoId = $('#establecimientoSelect').val();
        var especieId = $('#especieSelect').val();
        var nombreLote = $('#lote').val();
        $.ajax({
            type: "GET",
            url: "FinalizarLote",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                establecimientoId: establecimientoId,
                lote: nombreLote
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.id < 1) {
                    alert(data.mensaje);
                }
                else {
                    alert("Lote Registrado Correctamente, Ahora puede continuar con su producción.")
                    EjecutarUrl('../../Perfil');
                }
            },
            error: function (data) {
                alert(data.mensaje);
            }
        });
    }
}
function setValoresFor() {
    var idEst = $('#establecimientoHidden').val();
    var idEsp = $('#especieHidden').val();
    var nombreLote = $('#lote').val();
    var fechaCompra = $('#fechaCompraHtml').val();
    var fechaNaci = $('#fechaNacimientoHtml').val();
    $('#EstablecimientoId').val(idEst);
    $('#EspecieId').val(idEsp);
    $('#NombreLote').val(nombreLote);
    $('#FechaCompra').val(fechaCompra);
    $('#FechaNacimiento').val(fechaNaci);
}
function setValoresForFile() {
    var idEst = $('#establecimientoHidden').val();
    var idEsp = $('#especieHidden').val();
    var nombreLote = $('#lote').val();
    $('#EstablecimientoIdFile').val(idEst);
    $('#EspecieIdFile').val(idEsp);
    $('#NombreLoteFile').val(nombreLote);
}
function Siguiente(fichaActual, fichaSiguiente) {
    $('#' + fichaActual).hide();
    $('#' + fichaSiguiente).show();
}
function Atras(fichaActual, fichaAnterior) {
    $('#' + fichaActual).hide();
    $('#' + fichaAnterior).show();
}
function LoteDisponible() {
    var establecimientoId = $('#establecimientoSelect').val();
    var especieId = $('#especieSelect').val();
    //var fechaIngreso = $('#fechaIngresoHtml').val();
    var nombreLote = $('#lote').val();
    if (establecimientoId > 0 && especieId > 0 && nombreLote.length > 0) {
        $.ajax({
            type: "GET",
            url: "LoteDisponible",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                establecimientoId: establecimientoId,
                lote: nombreLote
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                if (data.id < 1) {
                    $('#animalesTable').jqGrid('clearGridData');
                    $('#lote').val('');
                    alert(data.mensaje);
                }
                else {
                    GrillaAnimales();
                }

            },
            error: function (data) {
                alert(data.mensaje);
            }
        });
    }
}
function QuitarAnimal(id) {
    var r = confirm("¿Esta seguro de quitar el registro?");
    if (r == true) {
        $.ajax({
            type: "GET",
            url: "EliminarAnimal",
            dataType: 'json',
            contentType: 'application/json',
            data: {
                id: id
            },
            success: function (data) {
                //$('#especieSelect').removeAttr('disabled', 'disabled');
                GrillaAnimales();
            },
            error: function (data) {
                GrillaAnimales();
            }
        });
    }
}
function Acciones(cellvalue, options, rowObject) {
    //var htmls = "<p><a id=\"nuevaMuestraBtn" + rowObject.Id + "\" style=\"cursor: pointer; \" class=\"botonera\" ";
    //htmls += " onclick=\"AgregarAnalisis(" + rowObject.Id + ")\" > <img src='../Images/terminada.png' title='Agregar Nueva Prueba' /></a>";

    ////htmls += " <a id=\"cerrarPruebaBtn" + rowObject.Id + "\" style=\"cursor: pointer;display:none; \" class=\"botonera\" ";
    ////htmls += " onclick=\"CerrarPrueba(" + rowObject.Id + ")\" > <img src='../Images/firmar.png' title='Cerrar Prueba' /></a>";

    ////htmls += " <a id=\"copiarMuestraBtn" + rowObject.Id + "\" style=\"cursor: pointer;display:none; \" class=\"botonera\" ";
    ////htmls += " onclick=\"IngresoResultado(" + rowObject.Id + ")\" > <img src='../Images/editarDatco.png' title='Ingresar Resultado' /></a>";

    //htmls += " <a id=\"eliminarBtn" + rowObject.Id + "\" style=\"cursor: pointer;display:none; \" class=\"botonera\" ";
    //htmls += " onclick=\"ElimarRegistro2(" + rowObject.Id + "," + rowObject.DesdeServer + ")\" > <img src='../Images/eliminarDatco.png' title='Eliminar' /></a>";
    //htmls += "</p>";
    var htmls = ' <button style="background-color: red;color:white;border:none;" title="Quitar" onclick="QuitarAnimal(' + rowObject[0] + ');">Quitar</button>';
    return htmls;
}
function GrillaAnimales() {
    var establecimientoId = $('#establecimientoSelect').val();
    var especieId = $('#especieSelect').val();
    //var fechaIngreso = $('#fechaIngresoHtml').val();
    var nombreLote = $('#lote').val();

    if (establecimientoId > 0 && especieId > 0) {
        var mypostData = $("#animalesTable").jqGrid("getGridParam", "postData");

        //s.Id, s.Identificacion, s.Categoria, s.FechaNacimiento, s.FechaCompra, s.KGIngreso, s.PrecioCompra
        $("#animalesTable").jqGrid({
            url: "ObtenerRegistroAnimalesTable?establecimientoId=" + establecimientoId + "&especieId=" + especieId + "&lote=" + nombreLote + "", //int establecimientoId, int especieId, string fechaIngreso
            datatype: "json",
            mtype: "POST",
            colNames: ["Id", "Identificación", "Categoria", "Fecha Nacimiento", "Fecha de Compra", "KG Ingreso", "Precio de Compra", "Acciones"],
            colModel: [
            { name: "Id", index: "Id", sorttype: "int", hidden: true },
            { name: "Identificacion", index: "Identificacion", width: 80, sorttype: "int", hidden: false },
            { name: "Categoria", index: "Categoria", width: 100, align: "left", sorttype: "string", sortable: true, search: true },
            { name: "FechaNacimiento", index: "FechaNacimiento", width: 80, align: "left", sorttype: "string", sortable: true, search: true },
            { name: "FechaCompra", index: "FechaCompra", width: 80, align: "center", sorttype: "string", sortable: true, search: true },
            { name: "KGIngreso", index: "KGIngreso", width: 80, hidden: false },
            { name: "PrecioCompra", index: "PrecioCompra", width: 80, sorttype: "string", hidden: false },
            { name: 'acciones', width: 80, index: status, align: "center", editable: true, formatter: Acciones }],
            width: '95%',
            height: 500,
            toppager: true,
            pager: $("#animalesPager"),
            rowNum: 200,
            rowList: [200],
            viewrecords: true, // Specify if "total number of records" is displayed
            sortname: "Id",
            sortorder: "asc",
            caption: "",
            loadComplete: function () {
                $("#animalesTable").jqGrid('setGridWidth', $(window).width() - 270, true);
            }
        }).navGrid("#animalesPager",
        { refresh: true, add: false, edit: false, del: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ['cn'] }, // Search options. Some options can be set on column level
            { closeAfterSearch: true }
         );
        jQuery("#animalesTable").jqGrid('setGridParam', {
            url: "ObtenerRegistroAnimalesTable?establecimientoId=" + establecimientoId + "&especieId=" + especieId + "&lote=" + nombreLote + "", page: 1
        }).trigger("reloadGrid");
    } else {
        var mypostData = $("#animalesTable").jqGrid("getGridParam", "postData");

        //s.Id, s.Identificacion, s.Categoria, s.FechaNacimiento, s.FechaCompra, s.KGIngreso, s.PrecioCompra
        $("#animalesTable").jqGrid({
            datatype: "json",
            mtype: "POST",
            colNames: ["Id", "Identificación", "Categoria", "Fecha Nacimiento", "Fecha de Compra", "KG Ingreso", "Precio de Compra", "Acciones"],
            colModel: [
            { name: "Id", index: "Id", sorttype: "int", hidden: true },
            { name: "Identificacion", index: "Identificacion", width: 80, sorttype: "int", hidden: false },
            { name: "Categoria", index: "Categoria", width: 100, align: "left", sorttype: "string", sortable: true, search: true },
            { name: "FechaNacimiento", index: "FechaNacimiento", width: 80, align: "left", sorttype: "string", sortable: true, search: true },
            { name: "FechaCompra", index: "FechaCompra", width: 80, align: "center", sorttype: "string", sortable: true, search: true },
            { name: "KGIngreso", index: "KGIngreso", width: 80, hidden: false },
            { name: "PrecioCompra", index: "PrecioCompra", width: 80, sorttype: "string", hidden: false },
            { name: 'acciones', width: 80, index: status, align: "center", editable: true, formatter: Acciones }],
            width: '95%',
            height: 500,
            toppager: true,
            pager: $("#animalesPager"),
            rowNum: 200,
            rowList: [200],
            viewrecords: true, // Specify if "total number of records" is displayed
            sortname: "Id",
            sortorder: "asc",
            caption: "",
            loadComplete: function () {
                $("#animalesTable").jqGrid('setGridWidth', $(window).width() - 270, true);
            }
        }).navGrid("#animalesPager",
        { refresh: true, add: false, edit: false, del: false },
            {}, // settings for edit
            {}, // settings for add
            {}, // settings for delete
            { sopt: ['cn'] }, // Search options. Some options can be set on column level
            { closeAfterSearch: true }
         );
    }
}
function ObtenerEstablecimiento(id) {
    $('#establecimientoSelect').attr('disabled', 'disabled');
    $.ajax({
        type: "GET",
        url: "ObtenerEstablecimiento",
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
function ObtenerCategoria(idEspecie) {
    $('#especieSelect').attr('disabled', 'disabled');
    $.ajax({
        type: "GET",
        url: "ObtenerCategoria",
        dataType: 'json',
        contentType: 'application/json',
        data: {
            idEspecie: idEspecie
        },
        success: function (data) {
            $('#especieSelect').removeAttr('disabled', 'disabled');
            if (data.Mensaje == "OK") {
                $('#categoriaSelect option').remove();
                $('#categoriaSelect').append('<option value="0">Seleccione </option>');
                if (data.Categorias.length > 0) {
                    for (var i = 0; i < data.Categorias.length; i++) {
                        $('#categoriaSelect').append('<option value="' + data.Categorias[i].Id + '">' + data.Categorias[i].Nombre + '</option>');
                    }
                    ////$('#categoriaSelect').val(idEnfermedad);
                    //if (idEnfermedad > 0) {
                    //    $('#especieSelect').attr('disabled', 'disabled');
                    //    $('#categoriaSelect').attr('disabled', 'disabled');
                    //}
                }
            } else {
                $('#categoriaSelect option').remove();
                $('#categoriaSelect').append('<option value="0">Seleccione </option>');
                $('#categoriaSelect').append('<option value="0">No existen</option>');
            }
        },
        error: function (data) {
            $('#especieSelect').removeAttr('disabled', 'disabled');
            jAlert(data.Mensaje, 'Alerta');
        }
    });
}
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
        var especieId = $('#especieSelect').val();
        var nombreLote = $('#lote').val();

        if (especieId == 0) {
            countError++;
            mensajeError += "* Debe Seleccionar una Especie. </br>";
        }
        if (nombreLote.length < 4) {
            countError++;
            mensajeError += "* Debe Ingresar un Lote, al menos 3 caracteres. </br>";
        }
    }
    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function ValidarFormularioFinalizar() {
    var countError = 0;
    var mensajeError = "";

    var establecimientoId = $('#establecimientoSelect').val();
    var especieId = $('#especieSelect').val();
    var nombreLote = $('#lote').val();
    var grid = $('#animalesTable');
    var rows = grid.jqGrid('getDataIDs');
    var cantidadAnimales = rows.length;

    if (establecimientoId == 0) {
        countError++;
        mensajeError += "* Debe Seleccionar un Establecimiento. </br>";
    }
    if (especieId == 0) {
        countError++;
        mensajeError += "* Debe Seleccionar una Especie. </br>";
    }
    if (nombreLote.length < 4) {
        countError++;
        mensajeError += "* Debe Ingresar un Lote, al menos 3 caracteres. </br>";
    }
    if (cantidadAnimales == 0) {
        countError++;
        mensajeError += "* Debe Ingresar al menos un animal. </br>";
    }
    if (countError > 0) {
        alert(mensajeError);
        return false;
    }
    return true;
}
function DescargarPlanilla() {
    var idEspecie = $('#especieSelect').val();
    var idEnfermedad = $('#enfermedadSelect').val();
    var idPrueba = $('#tipoPruebaSelect').val();
    //var esCuanti = $('#tipoPruebaSelect  option:selected').attr('escuantitativa');

    if (idEspecie > 0)
        window.location.href = '../Animal/DescargarPlantilla?idEspecie=' + idEspecie + '';
}