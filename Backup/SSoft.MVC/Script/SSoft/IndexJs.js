$(document).ready(function () {
    //$('#menuLateral').hide();
    var editEmpresaValidator = $('#editUserForm').validate({
        onkeyup: true,
        success: $.noop, // Odd workaround for errorPlacement not firing!
        rules: {
            Login: {
                required: true,
                minlength: 3,
            },
            Password: {
                required: true,
                minlength: 3
            }
        },
        messages: {
            Login: {
                required: "Este dato es Requerido",
                minlength: "Debe ser mayor a 2 caracteres"
            },
            Password: {
                required: "Este dato es Requerido",
                minlength: "Debe ser mayor a 2 caracteres"
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
                        $(document.body).css({ 'cursor': 'wait' })
                        return valid;
                    }
                },
                success: function (result) {
                    $('#menu').html("");
                    if (result.success == 0) {
                        EjecutarUrl('../../Perfil');
                    } else {
                        //alertModal(result.message, 'Alerta');
                        alertModal(result.message, 'Alerta');
                    }
                }
            };
            $(form).ajaxSubmit(opts);
        }
    });
    $('#olvidoClaveBtn').click(function () {
        $('#dialog-form2').show();
        $('#dialog-form').hide();
    });
    $('#atras').click(function () {
        $('#dialog-form2').hide();
        $('#dialog-form').show();
    });
    var editEmpresaValidator2 = $('#editUserForm2').validate({
        onkeyup: true,
        success: $.noop, // Odd workaround for errorPlacement not firing!
        rules: {
            Nick: {
                required: true
            },
            Correo: {
                required: true,
                email: true
            }
        },
        messages: {
            Nick: {
                required: "Este dato es Requerido"
            },
            Correo: {
                required: "Este dato es Requerido",
                email: "El correo es incorrecto"
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
                        $(document.body).css({ 'cursor': 'wait' })
                        return valid;
                    }
                },
                success: function (result) {
                    if (result == "OK") {
                        alertModal("llegara un correo con su contraseña actual", 'Correo');
                        //EjecutarUrl('../Index');
                    }
                    else {
                        alertModal(result, 'Alerta');
                    }
                }
            };
            $(form).ajaxSubmit(opts);
        }
    });
});

