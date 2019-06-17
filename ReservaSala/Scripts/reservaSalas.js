var sala = $("#sala");
var horaInicial = $("#horaInicial");
var horaFinal = $("#horaFinal");
var dia = $("#dataselecao");
var lista = [];

//tratemento da edição de uma reserva para o reserva Sala
var idReserva = parseInt($("#idReserva").val());
var custId = $("#custId").val(idReserva);

$(document).ready(function () {

    //Initialize Select2
    //$('#sel_users').select2({
    //    theme: "classic",
    //    placeholder: 'Selecione o usuário a reservar'
    //});

    //// Set option selected onchange
    //$('#user_selected').change(function () {
    //    var value = $(this).val();

    //    // Set selected
    //    $('#sel_users').val(value);
    //    $('#sel_users').select2().trigger('change');

    //});


    $("#dataselecao").datepicker({
        dateFormat: "dd/mm/yy",
        dayNames: ['Domingo', 'Segunda', 'Terça', 'Quarta', 'Quinta', 'Sexta', 'Sábado'],
        dayNamesMin: ['D', 'S', 'T', 'Q', 'Q', 'S', 'S', 'D'],
        dayNamesShort: ['Dom', 'Seg', 'Ter', 'Qua', 'Qui', 'Sex', 'Sáb', 'Dom'],
        monthNames: ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'],
        monthNamesShort: ['Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun', 'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez'],
        nextText: 'Próximo',
        prevText: 'Anterior',
        //showOn: "button",
        //buttonText: '<i class="fa fa-calendar"></i>',
        beforeShowDay: function (date) {
            var data = new Date();
            data.setDate(data.getDate() - 1);
            if (date < data) {
                return [false];
            } else {
                return [true];
            }
        },
        onSelect: function () {
            horaInicial.attr("readonly", false);
            var horaInicio = "";
            var horaFim = "";

            ajaxHoraIni();
        }
    });

    if (sala.val() == "") {
        $("#dataselecao").prop('disabled', true);
    } 


});

function ajaxHoraIni() {
    $.ajax({
        url: RangerHora,
        type: "POST",
        dataType: "JSON",
        cache: false,
        data: {
            IdResSala: custId.val(),
            IdSala: sala.val(),
            ReservaDia: dia.val()
        },
        success: function (result) {
            lista = [];
            Array.from(Object.keys(result), k => result[k]).forEach(function (element, index) {
                lista.push([element.HoraInicial, element.HoraFinal]);
            });

        },
        complete: function () {
            var hj = dateTimeNow;
            var hjDia;

            hjDia = dateTimeShortString;
            hjDia = dateTimeShortString;

            var horaMin;
            var minHora = dateTimeNowMinute;
            var url = window.location.href;

            if (dia.val().toString() == hjDia) {
                $("#horaInicial").timepicker('remove');
                if (url.includes("Edit") && horaInicial.val() <= hj) {
                    $("#horaInicial").val(horaInicial.val());
                } else {
                    $("#horaInicial").val("");
                }
                if (minHora <= 30) {
                    horaMin = dateTimeHourToString + ':45';
                } else {
                    horaMin = dateTimeAddHours + ':00';
                }
            } else {
                $("#horaInicial").val("");
                $("#horaInicial").timepicker('remove');
                horaMin = '00:00';
            }

            horaInicial.timepicker({
                timeFormat: 'H:i',
                minTime: horaMin.toString(),
                maxTime: '23:15',
                step: 15,
                disableTextInput: true,
                disableTimeRanges: lista,
            });

            horaInicial.change(function () {
                if (horaInicial.val() != null || horaInicial.val() != "") {
                    horaFinal.attr("readonly", false);
                    if (horaFinal.val() != "") {
                        $('#horaFinal').timepicker('remove');
                    }
                } else if (horaInicial.val() == hjDia) {
                    if (minHora <= 30) {
                        horaMin = dateTimeHourToString + ':45';
                    } else {
                        horaMin = dateTimeAddHours + ':00';
                    }
                }

                //horaFinal.val().;
                setTime();
            });

            function setTime() {

                horaFinal.val('');
                var horaIni = horaInicial.val().split(':');
                if (parseInt(horaIni[1]) > 40) {
                    horaIni[1] = '00';
                    if (horaIni[0] == '23') {
                        horaIni[0] = '00'
                    }
                    else {
                        horaIni[0] = (parseInt(horaIni[0]) + 1).toString();
                    }
                }
                else {
                    horaIni[1] = (parseInt(horaIni[1]) + 15).toString();
                }

                var nvHora = horaIni[0].toString() + ":" + horaIni[1].toString();

                var ticks = null;
                lista.forEach(function (x) {
                    var hora = TicksToHour(x[0]);
                    if (hora.split(':')[0] >= horaIni[0]) {
                        if (ticks == null || x[0] < ticks) {
                            ticks = x[0];
                        }
                    }
                })
                var horaFim = ticks == null ? '23:30' : TicksToHour(ticks);               

                $('#horaFinal').timepicker({
                    timeFormat: 'H:i',
                    minTime: nvHora,
                    maxTime: horaFim,
                    step: 15,
                    disableTimeRanges: lista,
                });
            };
        }
    });
}

jQuery(function ($) {
    $.validator.addMethod('date',
        function (value, element) {
            if (this.optional(element)) {
                return true;
            }

            var ok = true;
            try {
                $.datepicker.parseDate('dd/mm/yy', value);
            }
            catch (err) {
                ok = false;
            }
            return ok;
        });
});

function btnAbrirModal() {
    $("#modal_usuario").load("_modalUsuario", function () {
        $("#modal_usuario").modal('toggle');
    });
};

function TicksToHour(ticks) {
    var hora = Math.floor(ticks / 3600);
    var min = Math.floor(ticks % 3600) / 60;
    return hora + ':' + min;
}

function ajaxSala() {
    var infor = $("#informacao");
    var valorBusca = sala.val();

    $.ajax({
        url: DetalhesSalas,
        type: "POST",
        dataType: "JSON",
        cache: false,
        data: { IdSala: valorBusca },
        success: function (retorno) {

            // Interpretando retorno JSON
            var salaInfo = retorno;
            
            //listando as informacoes                            
            var item = '<button type="button" class="btn btn-primary form-group" onclick="btnDetalheReserva(' + salaInfo.idSala + ')"><i class="fas fa-info-circle"></i></button>' 
            infor.children().remove();
            infor.prepend(item);
        }
    });
}

//Quando sala selecionada manda ajax
sala.change(function () {
    var infor = $("#informacao");
    var salaVal = $("#sala").val();
   
    if (salaVal == "") {
        infor.children().remove();

        //bloqueia o datepicker
        $("#dataselecao").prop('disabled', true);
    }
    else {

        //retira o readonly da tela 
        $("#dataselecao").removeAttr('readonly');

        //permite o datepicker 
        $("#dataselecao").prop('disabled', false);
        
        ajaxSala();

    }
});

//Quando Usuario selecionado manda ajax


function Post() {
    var idUsuario = Number($("#idTcsUsuario").val());
    var nomeUsuario = $("#nomeUsuario").val();

    $.ajax({
        url: CreateUsuarioSala,
        type: "POST",
        cache: false,
        data:
        {
            idTcsUsuario: idUsuario,
            nomeUsuario: nomeUsuario
        },
        success: function (data) {
            $("#divPartialUsuario").html(data);
            $("#modal_usuario").modal('toggle');
        },
        error: function () {
            $("#modal_usuario").modal('toggle');
        }
    });
}

function btnDetalheReserva(id) {
    $("#modal_Sala").load(DetailsSalas + '/' + id, function () {
        $("#modal_Sala").modal('toggle');
    });
};