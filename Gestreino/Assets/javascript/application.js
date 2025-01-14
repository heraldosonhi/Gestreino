

/* ############################################
  DATEPICKER FUNCTIONS JS
 ##############################################
 */
//Definir plugin datepicker nos inputs do tipo date
function SetUpDatepicker(id) {
    $('.datepicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        locale: {
            format: 'DD-MM-YYYY', daysOfWeek: [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            monthNames: [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
        },
        minYear: 1999,
        maxYear: new Date().getFullYear() + 15
    });
    $('.datepickerDisablePastDate').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        locale: {
            format: 'DD-MM-YYYY', daysOfWeek: [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            monthNames: [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
        },
        minYear: 1999,
        minDate: new Date(),
        maxYear: new Date().getFullYear() + 15
    });
    $('.datepickerDisableFutureDate').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        locale: {
            format: 'DD-MM-YYYY', daysOfWeek: [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            monthNames: [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
        },
        minYear: 1905,
        /*minDate: new Date(),*/
        maxDate: new Date()
    });
    $('.Disablecalendardatepicker').daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        locale: {
            format: 'DD-MM-YYYY', daysOfWeek: [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            monthNames: [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
        },
        minDate: new Date($("#DisableDateIni").val()),
        maxDate: new Date($("#DisableDateEnd").val())
    });
    // Disablecalendardatepicker with Loop over date options
    $('.Disablecalendardatepicker_' + id).daterangepicker({
        singleDatePicker: true,
        showDropdowns: true,
        autoUpdateInput: false,
        autoApply: true,
        locale: {
            format: 'DD-MM-YYYY', daysOfWeek: [
                "Dom",
                "Seg",
                "Ter",
                "Qua",
                "Qui",
                "Sex",
                "Sab"
            ],
            monthNames: [
                "Janeiro",
                "Fevereiro",
                "Março",
                "Abril",
                "Maio",
                "Junho",
                "Julho",
                "Agosto",
                "Setembro",
                "Outubro",
                "Novembro",
                "Dezembro"
            ],
        },
        minDate: new Date($(".DisableDateIni_" + id).val()),
        maxDate: new Date($(".DisableDateEnd_" + id).val())
    });

    $('.datepicker, .datepickerDisablePastDate, .datepickerDisableFutureDate, .Disablecalendardatepicker, .Disablecalendardatepicker_' + id).on('apply.daterangepicker', function (ev, picker) {
        $(this).val(picker.startDate.format('DD-MM-YYYY'));
    });
    // Prevent data entry :: Readonly Input
    $('.datepicker, .datepickerDisablePastDate, .datepickerDisableFutureDate, .Disablecalendardatepicker, .Disablecalendardatepicker_' + id).on('keydown paste focus mousedown', function (e) {
        if (e.keyCode != 9)
            e.preventDefault();
    });
}


/*
 ############################################
  DINAMICALLY CREATED CHECKBOX - GET IDS[]
 ##############################################
 */
var values = [];



/*
       ******************************************
       *******************************************
       JAVASCRIPT FORM MODALS CALL HERE
       ******************************************
       *******************************************
*/


/* ############################################
  AJAXMODAL CRUDCONTROL FORMS GETCLICK EVENT 
 ##############################################
 */
$(document).on('click', '.open-modal-crud', function (e) {
    var id = $(e.target).attr('data-id') || $(e.target).closest('.open-modal-crud').attr('data-id');
    var Action = $(this).data('action');
    var Entity = $(this).data('entity');
    var Upload = $(this).data('upload');
    var url = null;

    //Clear content
    //$(".modal-crud-content").show().html("");

    //Trigger check event in each table to Add/Remove Ids from selected items
    var tableCheck = $(this).parent().parent().parent().parent().find('table tbody input[type="checkbox"]:first');
    //Trigger twice to update Ids array
    tableCheck.trigger('click');
    tableCheck.trigger('click');

    //loadIn();
    switch (Entity) {
        case 'groups': url = '../../Ajax/Groups';
            break;
        case 'utilisub': url = '../../Ajax/UsersUtiliSub';
            break;
        case 'profiles': url = '../../Ajax/Profiles';
            break;
        case 'atoms': url = '../../Ajax/Atoms';
            break;
        case 'users': url = '../../Ajax/Users';
            break;
        case 'usergroups': url = '../../Ajax/UserGroups';
            break;
        case 'grldoctype': url = '../../Ajax/GRLDocType';
            break;
        case 'grltokentype': url = '../../Ajax/GRLTokenType';
            break;
        case 'grlendpais': url = '../../Ajax/GRLEndPais';
            break;
        case 'grlendcidade': url = '../../Ajax/GRLEndCidade';
            break;
        case 'grlenddistr': url = '../../Ajax/GRLEndDistr';
            break;
        case 'pesfamily': url = '../../Ajax/PESFamily';
            break;
        case 'pesprofessional': url = '../../Ajax/PESProfessional';
            break;
        case 'pesdisability': url = '../../Ajax/PESDisability';
            break;
        case 'grlidenttype': url = '../../Ajax/GRLIdentType';
            break;
        case 'grlestadocivil': url = '../../Ajax/GRLEstadoCivil';
            break;
        case 'grlendtype': url = '../../Ajax/GRLEndType';
            break;
        case 'grlprofissao': url = '../../Ajax/GRLProfissao';
            break;
        case 'grlprofcontract': url = '../../Ajax/GRLProfContract';
            break;
        case 'grlprofregime': url = '../../Ajax/GRLProfRegime';
            break;
        case 'grlpesagregado': url = '../../Ajax/GRLPESAgregado';
            break;
        case 'grlpesgruposang': url = '../../Ajax/GRLPESGrupoSang';
            break;
        case 'grltipodef': url = '../../Ajax/GRLPESDef';
            break;
        case 'grlgtduracao': url = '../../Ajax/GRLGTDuracaoPlano';
            break;
        case 'grlgtfasetreino': url = '../../Ajax/GRLGTFaseTreino';
            break;
        case 'fileupload': url = '../../Ajax/FileManagement';
            break;
        case 'pesident': url = '../../Ajax/PESDadosPessoaisIdent'
            break;
        case 'gtexercicio': url = '../../Ajax/GTExercicio'
            break;
        case 'grlgttipotreino': url = '../../Ajax/GRLGTTipoTreino'
            break;
        case 'gtavaliado': url = '../../Ajax/GTAvaliado'
            break;
        case 'gttreinos': url = '../../Ajax/GTTreinos'
            break;
        case 'gtquest': url = '../../Ajax/GTQuest'
            break;
        default: null
    }
    $.ajax({
        type: "POST", url: url,
        data: { "action": Action, "id": id, "BulkIds": values, "upload": Upload },
        cache: false,
        beforeSend: function () {
        },
        complete: function () {
        },
        success: function (result) {
            //loadOut();
            // Deliver modal content
            $(".modal-crud-content").show().html(result);
            // Call javascript functions to enable modal support
            SetUpDatepicker();
            //addressHelper();
            //setupSelect2();
            //quillEditor();
            checkDisabled("ScheduledStatus");
            //ajax();
            anxientForm();
            handleDataUsers();
            handleDataAtomos();
            handleDataGrupos();
            handleDataPerfis();
        },
        error: function () {
            $(".modal-crud-content").show().html('<span class="text-accent-red">Erro ao processar este pedido, por favor tente mais tarde!</span>');
        }
    });
});

function loadIn() {
    $('.page-loader').show();
    $('.page-loader').addClass('preloader');
    //$(".preloader").fadeIn(), domFactory.handler.upgradeAll();
}
function loadOut() {
    //$(".preloader").fadeOut(), domFactory.handler.upgradeAll();
}

function handleFailure(response) {
    $('.handle-success').hide();
    $('.handle-success-message').text('');
    $('.handle-error').fadeOut(100).fadeIn(100).show();
    $('.handle-error-message').text(response.error + ' Erro ao processar pedido, por favor tentar mais tarde..');
    $('html,body').scrollTop(0);
    $("#crudControlModal").scrollTop(0);
}
function handleSuccess(response) {
    if (response.result) {
        $('.handle-error').hide();
        $('.handle-error-message').text('');
        $('.handle-success').fadeOut(100).fadeIn(100).show();
        $('.handle-success-message').text(response.success);
        $('#crudControlModal').modal('hide');
        if (response.table) {
            var table = $('#' + response.table).DataTable();
            table.draw();
            //reload document table just in case as needed by some entities
            var table = $('#tblInstituicoesArquivos').DataTable();
            table.draw();
            //reload fancytree //disable buttons
            if (response.table == "tblInstituicoesArquivos") {
                FancyTreeInit("listbydate");
                var tree = $('#tree').fancytree('getTree');
                tree.reload();
                disableFancytreeButtons();
            }
            if (response.flexAct) { 
                //Flexitest
                var progressperc = Number(response.flexAct.split("-", 1)[0]);
                var progressbar = response.tentativas == '' ? (progressperc * 1.25) : progressperc;
                var progresspercAnt = Number(response.flexAnt.split("-", 1)[0]);
                var progressbarAnt = response.tentativas == '' ? (progresspercAnt * 1.25) : progresspercAnt;
                $('#pgrflexActual').css("width", progressbar + '%')
                $('#pgrflexActual').text(progressperc)
                $('#lblflexActual').text(response.flexAct.split("-", 2)[1])
                $('#pgrflexAnterior').css("width", progressbarAnt + '%')
                $('#pgrflexAnterior').text(progresspercAnt)
                $('#lblflexAnterior').text(response.flexAnt.split("-", 2)[1])
                $('#ESPERADO').val(response.tentativas.split("-", 1)[0]);
                $('#RESULTADO').val(response.tentativas.split("-", 2)[1]);
            }
        }
        if (response.url) {
            window.location = response.url;
        }
        if (response.resetForm) {
            $('form').trigger("reset");
        }
        if (response.calendar) {
            //refreshCalendar('#' + response.calendar, response.calendarColumn)
        }
        if (response.risk) {
            processquest(response.success);
        }
        if (response.reload) {
            location.reload();
        }
        if (response.showToastr == true) {

            var position = "toast-bottom-left";
            var timeout = "1000";
            if (response.from != null && response.from == "studentPortal") {
                position = "toast-top-center"; timeout = "8000";
            }
            toastr.options = {
                "closeButton": true,
                "progressBar": true,
                "positionClass": position,
                "preventDuplicates": true,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": timeout,
                "extendedTimeOut": "1000",
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut"
            }
            if (response.from != null && response.from == "studentPortal") {
                if (response.isError == "true")
                    toastr.error(response.toastrMessage, { "iconClass": 'customer-info' });
                else if (response.isError == "false") {
                    toastr.success(response.toastrMessage, { "iconClass": 'customer-info' });
                    $(".portalrequestupdate").prop("hidden", false);
                }
            } else {
                toastr.success(response.toastrMessage, { "iconClass": 'customer-info' });
            }
        }
    } else {
        // Not submited
        $('.handle-error').fadeOut(100).fadeIn(100).show();
        $('.handle-error-message').text(response.error);
        $('.handle-success').hide();
        $('.handle-success-message').text('');
        $('html,body').scrollTop(0);
        $("#crudControlModal").scrollTop(0);
    }
}








/*
* 
#####################################################
   DATATABLES
#####################################################
*/
function handleDataFileUploaderView() {
    var table = $("#tblInstituicoesArquivos").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../Ajax/GetFiles", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "EntityId": $('#EntityId').val(), "ArquivoId": $('#ArquivoId').val(), "upload": $('#upload').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/' + full.Link + '" target="_blank" class=""><i class="fa fa-search"/></i></a>' +
                        ' <a title="Descarregar" href="/' + full.Link + '" target="_blank" download="" class=""><i class="fa fa-cloud-download"/></i></a>' +
                        ' <a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="fileupload" data-upload="' + $('#upload').val() + '" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"/></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="fileupload" data-upload="' + $('#upload').val() + '" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"/></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "DESCRICAO", "name": "DESCRICAO", "autoWidth": true },
            { "data": "DOCUMENTO", "name": "DOCUMENTO", "autoWidth": true },
            { "data": "TIPO", "name": "TIPO", "autoWidth": true },
            { "data": "TAMANHO", "name": "TAMANHO", "autoWidth": true },
            { "data": "ESTADO", "name": "ESTADO", "autoWidth": true },
            { "data": "DATAACTIVACAO", "name": "DATAACTIVACAO", "autoWidth": true },
            { "data": "DATADESACTIVACAO", "name": "DATADESACTIVACAO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
            { "data": "Link", "name": null, "autoWidth": true, "visible": true }
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoInstArquivo');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#tblInstituicoesArquivos_paginate').appendTo('#paginateInstArquivo');
        },
    });
}; 

function handleDataUsers() {
    var table = $("#UserTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetUser", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "GroupId": $('#GroupId').val(), "ProfileId": $('#ProfileId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/administration/viewusers/' + full.Id + '"><i class="fa fa-search"/></i></a>';

                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "LOGIN", "name": "LOGIN", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "GRUPOS", "name": "GRUPOS", "autoWidth": true },
            { "data": "PERFIS", "name": "PERFIS", "autoWidth": true },
            { "data": "ESTADO", "name": "ESTADO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ESTADO == "Inactivo") $(row).closest("tr").addClass("bg-inactive");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUser');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserTable_paginate').appendTo('#paginateUser');
        },
    });
};
function handleDataGrupos() {
    var table = $("#GroupTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGroups", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/administration/viewgroups/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="groups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="groups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGroup');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GroupTable_paginate').appendTo('#paginateGroup');
        },
    });
};
function handleDataAtomos() {
    var table = $("#AtomTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetAtoms", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/administration/viewatoms/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="atoms" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="atoms" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoAtom');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#AtomTable_paginate').appendTo('#paginateAtom');
        },
    });
};
function handleDataPerfis() {
    var table = $("#ProfileTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetProfiles", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/administration/viewprofiles/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="profiles" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="profiles" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoProfile');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#ProfileTable_paginate').appendTo('#paginateProfile');
        },
    });
};
function handleDataUserGroups() {
    var table = $("#UserGroupTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetUserGroup", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { GroupId: $("#GroupId").val(), UserId: $("#UserId").val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="RemoverGroupUtil" data-entity="usergroups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "GRUPO", "name": "GRUPO", "autoWidth": true },
            { "data": "UTILIZADOR", "name": "UTILIZADOR", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserGroupTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserGroupTable_paginate').appendTo('#paginateUserGroupTable');
        },
    });
};
function handleDataAtomGroups() {
    var table = $("#AtomGroupTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetAtomGroup", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { GroupId: $("#GroupId").val(), AtomId: $("#AtomId").val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="RemoverGroupAtom" data-entity="usergroups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "GRUPO", "name": "GRUPO", "autoWidth": true },
            { "data": "ATOMO", "name": "ATOMO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoAtomGroupTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#AtomGroupTable_paginate').appendTo('#paginateAtomGroupTable');
        },
    });
};
function handleDataProfileAtoms() {
    var table = $("#ProfileAtomTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetProfileAtom", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { ProfileId: $("#ProfileId").val(), AtomId: $("#AtomId").val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="RemoverAtomProfile" data-entity="usergroups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "PERFIL", "name": "PERFIL", "autoWidth": true },
            { "data": "ATOMO", "name": "ATOMO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoProfileAtomTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#ProfileAtomTable_paginate').appendTo('#paginateProfileAtomTable');
        },
    });
};
function handleDataProfileUtil() {
    var table = $("#ProfileUtilTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetProfileUtil", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { ProfileId: $("#ProfileId").val(), UserId: $("#UserId").val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="RemoverAtomProfile" data-entity="usergroups" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "UTILIZADOR", "name": "UTILIZADOR", "autoWidth": true },
            { "data": "PERFIL", "name": "PERFIL", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoProfileUtilTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#ProfileUtilTable_paginate').appendTo('#paginateProfileUtilTable');
        },
    });
};
function handleDataUserLogTable() {
    var table = $("#UserLogTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetUserLogs", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { UserId: $("#UserId").val()}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return ' ';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "LOGIN", "name": "LOGIN", "autoWidth": true },
            { "data": "IP", "name": "IP", "autoWidth": true },
            { "data": "MAC", "name": "MAC", "autoWidth": true },
            { "data": "HOST", "name": "HOST", "autoWidth": true },
            { "data": "DEVICE", "name": "DEVICE", "autoWidth": true },
            { "data": "URL", "name": "URL", "autoWidth": true },
            { "data": "DATA", "name": "DATA", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserLogTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserLogTable_paginate').appendTo('#paginateUserLogTable');
        },
    });
};
function handleDataAtomsAccessTable() {
    var table = $("#AtomsAccessTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetUserAtoms", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { UserId: $("#UserId").val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return ' ';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            if (data.ACTIVO == "Inactivo") $(row).closest("tr").addClass("piaget-danger");
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoAtomsAccessTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#AtomsAccessTable_paginate').appendTo('#paginateAtomsAccessTable');
        },
    });
};
function handleDataGRLTipoDocTable() {
    var table = $("#GRLTipoDocTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLDocTypes", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grldoctype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grldoctype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLTipoDocTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLTipoDocTable_paginate').appendTo('#paginateGRLTipoDocTable');
        },
    });
};
function handleDataGRLTipoTokenTable() {
    var table = $("#GRLTipoTokenTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLTokenTypes", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grltokentype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grltokentype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLTipoTokenTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLTipoTokenTable_paginate').appendTo('#paginateGRLTipoTokenTable');
        },
    });
};
function handleDataGRLEndPaisTable() {
    var table = $("#GRLEndPaisTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLEndPais", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlendpais" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlendpais" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "CODIGO", "name": "CODIGO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLEndPaisTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLEndPaisTable_paginate').appendTo('#paginateGRLEndPaisTable');
        },
    });
};
function handleDataGRLEndCidadeTable() {
    var table = $("#GRLEndCidadeTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLEndCidade", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlendcidade" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlendcidade" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "PAIS", "name": "PAIS", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLEndCidadeTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLEndCidadeTable_paginate').appendTo('#paginateGRLEndCidadeTable');
        },
    });
};
function handleDataGRLEndDistrTable() {
    var table = $("#GRLEndDistrTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLEndDistr", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlenddistr" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlenddistr" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "CIDADE", "name": "CIDADE", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLEndDistrTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLEndDistrTable_paginate').appendTo('#paginateGRLEndDistrTable');
        },
    });
};
function handleDataGPUsers() {
    var table = $("#DadosPessoaisTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisi��o
        "serverSide": true, // Para processar as requisi��es no back-end
        //"filter": false, // : est� comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Op��o de ordena��o para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "/gtmanagement/GetUsers", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/gtmanagement/viewathletes/' + full.Id + '" class=""><i class="fa fa-search" /></i></a>';
                }
            },
            {
                sortable: true,
                "render": function (data, type, full, meta) {
                    return `
                                <div class="media flex-nowrap align-items-center"
                                         style="white-space: nowrap;">
                                        <div class="avatar avatar-sm mr-8pt">
                                            <img src="${full.FOTOGRAFIA}"
                                                 alt="${full.NOME}"
                                                 class="avatar-img rounded-circle">
                                        </div>
                                        <div class="media-body">
                                            <div class="d-flex align-items-center">
                                                <div class="flex d-flex flex-column">
                                                    <p class="mb-0"><strong class="js-lists-values-name">${full.NOME}</strong></p>
                                                    <small class="js-lists-values-email text-50">${full.USER}</small>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                            `
                }
            },
            { "data": "SOCIO", "name": "SOCIO", "autoWidth": true },
            { "data": "TELEFONE", "name": "TELEFONE", "autoWidth": true },
            { "data": "EMAIL", "name": "EMAIL", "autoWidth": true },
            //{ "data": "UTILIZADOR", "name": "UTILIZADOR", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true }
        ],
        //Configura��o da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Draw table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoDadosPessoais');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#DadosPessoaisTable_paginate').appendTo('#paginateDadosPessoais');
        },
    });
};
function handleDataGPUsersIdent() {
    var table = $("#UserIdentTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisi��o
        "serverSide": true, // Para processar as requisi��es no back-end
        //"filter": false, // : est� comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Op��o de ordena��o para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "/gtmanagement/GetUsersIdentification", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "Id": $('#PesId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlEdit + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="pesident" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil" /></i></a> <a style="display:' + full.AccessControlDelete + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="pesident" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash" /></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "IDENTIFICACAO", "name": "IDENTIFICACAO", "autoWidth": true },
            { "data": "NUMERO", "name": "NUMERO", "autoWidth": true },
            { "data": "DATAEMISSAO", "name": "DATAEMISSAO", "autoWidth": true },
            { "data": "DATAVALIDADE", "name": "DATAVALIDADE", "autoWidth": true },
            { "data": "LOCALEMISSAO", "name": "LOCALEMISSAO", "autoWidth": true },
            { "data": "ORGAOEMISSOR", "name": "ORGAOEMISSOR", "autoWidth": true },
            { "data": "OBSERVACAO", "name": "OBSERVACAO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true }
        ],
        //Configura��o da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Draw table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserIdent');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserIdentTable_paginate').appendTo('#paginateUserIdent');
        },
    });
};
function handleDataGPUsersProfissao() {
    var table = $("#UserProfissaoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisi��o
        "serverSide": true, // Para processar as requisi��es no back-end
        //"filter": false, // : est� comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Op��o de ordena��o para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "/gtmanagement/GetUsersProfessional", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "Id": $('#PesId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a style="display:' + full.AccessControlEdit + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="pesprofessional" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil" /></i></a> <a style="display:' + full.AccessControlDelete + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="pesprofessional" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash" /></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "EMPRESA", "name": "EMPRESA", "autoWidth": true },
            { "data": "FUNCAO", "name": "FUNCAO", "autoWidth": true },
            { "data": "CONTRACTO", "name": "CONTRACTO", "autoWidth": true },
            { "data": "REGIME", "name": "REGIME", "autoWidth": true },
            { "data": "DATAINICIAL", "name": "DATAINICIAL", "autoWidth": true },
            { "data": "DATAFIM", "name": "DATAFIM", "autoWidth": true },
            { "data": "DESCRICAO", "name": "DESCRICAO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true }
        ],
        //Configura��o da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Draw table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserProfissaoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserProfissaoTable_paginate').appendTo('#paginateUserProfissaoTable');
        },
    });
};
function handleDataGPUsersFamily() {
    var table = $("#UserFamilyTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisi��o
        "serverSide": true, // Para processar as requisi��es no back-end
        //"filter": false, // : est� comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Op��o de ordena��o para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "/gtmanagement/GetUsersFamily", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "Id": $('#PesId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a  style="display:' + full.AccessControlEdit + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="pesfamily" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil" /></i></a> <a  style="display:' + full.AccessControlDelete + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="pesfamily" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash" /></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "AGREGADO", "name": "AGREGADO", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "PROFISSAO", "name": "PROFISSAO", "autoWidth": true },
            { "data": "TELEFONE", "name": "TELEFONE", "autoWidth": true },
            { "data": "TELEFONEALTERNATIVO", "name": "TELEFONEALTERNATIVO", "autoWidth": true },
            { "data": "FAX", "name": "FAX", "autoWidth": true },
            { "data": "EMAIL", "name": "EMAIL", "autoWidth": true },
            { "data": "URL", "name": "URL", "autoWidth": true },
            { "data": "ENDERECO", "name": "ENDERECO", "autoWidth": true },
            { "data": "MORADA", "name": "MORADA", "autoWidth": true },
            { "data": "RUA", "name": "RUA", "autoWidth": true },
            { "data": "NUMERO", "name": "NUMERO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true }
        ],
        //Configura��o da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Draw table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserFamilyTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserFamilyTable_paginate').appendTo('#paginateUserFamilyTable');
        },
    });
};
function handleDataGPUsersDisability() {
    var table = $("#UserDisabilityTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisi��o
        "serverSide": true, // Para processar as requisi��es no back-end
        //"filter": false, // : est� comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Op��o de ordena��o para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "/gtmanagement/GetUsersDisability", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "Id": $('#PesId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a  style="display:' + full.AccessControlEdit + '" title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="pesdisability" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil" /></i></a> <a  style="display:' + full.AccessControlDelete + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="pesdisability" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash" /></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "DEFICIENCIA", "name": "DEFICIENCIA", "autoWidth": true },
            { "data": "GRAU", "name": "GRAU", "autoWidth": true },
            { "data": "DESCRICAO", "name": "DESCRICAO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true }
        ],
        //Configura��o da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Draw table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoUserDisabilityTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#UserDisabilityTable_paginate').appendTo('#paginateUserDisabilityTable');
        },
    });
};
function handleDataGRLIdentTable() {
    var table = $("#GRLIdentTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLIdentType", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlidenttype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlidenttype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLIdentTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLIdentTable_paginate').appendTo('#paginateGRLIdentTable');
        },
    });
};
function handleDataGRLEstadoCivilTable() {
    var table = $("#GRLEstadoCivilTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLEstadoCivil", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlestadocivil" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlestadocivil" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLEstadoCivilTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLEstadoCivilTable_paginate').appendTo('#paginateGRLEstadoCivilTable');
        },
    });
};
function handleDataGRLEndTipoEndTable() {
    var table = $("#GRLEndTipoEndTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLEndType", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlendtype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlendtype" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLEndTipoEndTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLEndTipoEndTable_paginate').appendTo('#paginateGRLEndTipoEndTable');
        },
    });
};
function handleDataGRLProfissaoTable() {
    var table = $("#GRLProfissaoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLProfissao", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlprofissao" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlprofissao" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLProfissaoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLProfissaoTable_paginate').appendTo('#paginateGRLProfissaoTable');
        },
    });
};
function handleDataGRLProfissaoTipoContractoTable() {
    var table = $("#GRLProfissaoTipoContractoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLProfContract", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlprofcontract" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlprofcontract" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLProfissaoTipoContractoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLProfissaoTipoContractoTable_paginate').appendTo('#paginateGRLProfissaoTipoContractoTable');
        },
    });
};
function handleDataGRLProfissaoTipoRegimeTable() {
    var table = $("#GRLProfissaoTipoRegimeTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLProfRegime", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlprofregime" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlprofregime" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLProfissaoTipoRegimeTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLProfissaoTipoRegimeTable_paginate').appendTo('#paginateGRLProfissaoTipoRegimeTable');
        },
    });
};
function handleDataGRLAgregadoTable() {
    var table = $("#GRLAgregadoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLPESAgregado", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlpesagregado" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlpesagregado" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLAgregadoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLAgregadoTable_paginate').appendTo('#paginateGRLAgregadoTable');
        },
    });
};
function handleDataGRLGroupSangTable() {
    var table = $("#GRLGroupSangTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLPESGrupoSang", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlpesgruposang" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlpesgruposang" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLGroupSangTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLGroupSangTable_paginate').appendTo('#paginateGRLGroupSangTable');
        },
    });
};
function handleDataGRLTipoDefTable() {
    var table = $("#GRLTipoDefTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLPESDef", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grltipodef" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grltipodef" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLTipoDefTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLTipoDefTable_paginate').appendTo('#paginateGRLTipoDefTable');
        },
    });
};
function handleDataGRLGTDuracaoTable() {
    var table = $("#GRLGTDuracaoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLGTDuracaoPlano", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlgtduracao" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlgtduracao" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "DURACAO", "name": "DURACAO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLGTDuracaoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLGTDuracaoTable_paginate').appendTo('#paginateGRLGTDuracaoTable');
        },
    });
};
function handleDataGRLGTFaseTreinoTable() {
    var table = $("#GRLGTFaseTreinoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLGTFaseTreinoTable", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlgtfasetreino" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlgtfasetreino" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "SERIE", "name": "SERIE", "autoWidth": true },
            { "data": "REPS", "name": "REPS", "autoWidth": true },
            { "data": "RM", "name": "RM", "autoWidth": true },
            { "data": "DESCANSO", "name": "DESCANSO", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLGTFaseTreinoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLGTFaseTreinoTable_paginate').appendTo('#paginateGRLGTFaseTreinoTable');
        },
    });
};
function handleDataGRLTipoTreinoTable() {
    var table = $("#GRLTipoTreinoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../administration/GetGRLGTTipoTreino", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="grlgttipotreino" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="grlgttipotreino" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "SIGLA", "name": "SIGLA", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLTipoTreinoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLTipoTreinoTable_paginate').appendTo('#paginateGRLTipoTreinoTable');
        },
    });
};
function handleDataGRLExercicioTable() {
    var table = $("#GRLExercicioTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../gtmanagement/GetGRLExercicioTable", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: {}
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="/gtmanagement/viewexercises/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
                    ' <a title="Editar" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Editar" data-entity="gtexercicio" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-pencil"></i></a>' +
                        ' <a title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="gtexercicio" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "TREINO", "name": "TREINO", "autoWidth": true },
            { "data": "NOME", "name": "NOME", "autoWidth": true },
            { "data": "ALONGAMENTO", "name": "ALONGAMENTO", "autoWidth": true },
            { "data": "SEQUENCIA", "name": "SEQUENCIA", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGRLExercicioTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GRLExercicioTable_paginate').appendTo('#paginateGRLExercicioTable');
        },
    });
};
function handleDataGTTreinoTable() {
    var table = $("#GTTreinoTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../gtmanagement/GetGTTreinoTable", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "PesId": $('#PEsId').val(), "GTTipoTreinoId": $('#GTTipoTreinoId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="' + full.LINK + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="gttreinos" data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "DATEINI", "name": "DATEINI", "autoWidth": true },
            { "data": "DATEFIM", "name": "DATEFIM", "autoWidth": true },
            { "data": "OBS", "name": "OBS", "autoWidth": true },
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGTTreinoTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GTTreinoTable_paginate').appendTo('#paginateGTTreinoTable');
        },
    });
};
function handleDataGTQuestTable() {
    var table = $("#GTQuestTable").DataTable({
        "processing": true, // Para exibir mensagem de processamento a cada requisição
        "serverSide": true, // Para processar as requisições no back-end
        //"filter": false, // : está comentado porque estamos a usar filtros que enviamos no back-end
        "orderMulti": false, // Opção de ordenação para uma coluna de cada vez.
        //Linguagem PT
        "language": {
            "url": "/Assets/lib/datatable/pt-PT.json"
        },
        fixedHeader: {
            header: true,
            footer: true
        },
        "dom": '<"toolbox">rtp',//remove componentes i - for pagination information, l -length, p -pagination
        "ajax": {
            "url": "../../gtmanagement/GetGTQuestTable", // POST TO CONTROLLER
            "type": "POST",
            "datatype": "json",
            data: { "PesId": $('#PEsId').val(), "GT_Res": $('#GT_Res').val(), "TipoId": $('#TipoId').val() }
        },
        "columns": [
            { "data": "Id", "name": null, "autoWidth": true },
            //Column customizada
            {
                sortable: false,
                "render": function (data, type, full, meta) {
                    return '<a title="Visualizar" href="' + full.LINK + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title="Remover" href="javascript:void(0)" class="open-modal-crud" data-id="' + full.Id + '" data-action="Remover" data-entity="gtquest" data-upload="'+full.UPLOAD+'"  data-toggle="modal" data-target="#crudControlModal"><i class="fa fa-trash"></i></a>';
                }
            },
            //Cada dado representa uma coluna da tabela
            { "data": "INSERCAO", "name": "INSERCAO", "autoWidth": true },
            { "data": "DATAINSERCAO", "name": "DATAINSERCAO", "autoWidth": true },
            { "data": "ACTUALIZACAO", "name": "ACTUALIZACAO", "autoWidth": true },
            { "data": "DATAACTUALIZACAO", "name": "DATAACTUALIZACAO", "autoWidth": true },
        ],
        //Configuração da tabela para os checkboxes
        'columnDefs': [
            {
                'targets': 0,
                'checkboxes': {
                    'selectRow': true
                },
            }
        ], 'select': {
            'style': 'multi'
        },
        'order': [[1, 'false']],
        'rowCallback': function (row, data, dataIndex) {
            // Get row ID
            var rowId = data["Id"];
            //console.log(rowId)
            //Dra table and add selected option to previously selected checkboxes
            $.each(values, function (i, r) {
                if (rowId == r) {
                    $(row).find('input[type="checkbox"]').prop('checked', true);
                    $(row).closest("tr").addClass("selected");
                }
            })
        },
        drawCallback: function () {
            processInfo(this.api().page.info(), 'paginateInfoGTQuestTable');
        },
        //Remove pagination from table and add to custom Div
        initComplete: (settings, json) => {
            $('#GTQuestTable_paginate').appendTo('#paginateGTQuestTable');
        },
    });
};
/*
* 
#####################################################
   DATATABLES
#####################################################
*/



/*
* 
#####################################################
FUNCTIONS SUPPORTING DATATABLE APIS -- NAO DUPLICAR
#####################################################
*/
//Remove length from table and add to custom Div
$('.length_change').change(function () {
    var table = $(this).data('id');
    //tableGrupos.page.len($(this).val()).draw(); 
    $("#" + table).DataTable().page.len($(this).val()).draw();
});
//Remove pagination information from datatable and add to custom dom element
function processInfo(info, tablename) {
    //console.log(info);
    $('.' + tablename).html('A ver ' + (info.start + 1) + ' - ' + (info.end) + ' de ' + (info.recordsTotal))
}
//Show/Hide MultiSelect checkboxes
$(document).on('click', '.btnSelect', function () {
    var tablename = $(this).data('id');
    $(this).css("background", "#e9ecef");
    $('#' + tablename).removeClass('hideInputCheck');
});
//Search Event trigger and redraw table
$('.btnSearch').click(function () {
    var tablename = $(this).data('id');
    SearchDataTable(tablename);
});
function SearchDataTable(tablename) {
    var table = $("#" + tablename).DataTable();
    var th = 0;
    $('#' + tablename + ' thead tr th').each(function () {
        if ($(this).text() != '') {
            table.columns(th).search($('#' + $(this).data('name')).val()); //val().trim());
            th++;
        }
    });
    //Refresh
    if (tablename == 'RHTimeTrackTbl') {
        $("#" + tablename + ' tbody tr').remove();
        table.draw();
    }
    else
        table.draw();
}
$('table').on('click', 'thead input[type="checkbox"]', function (e) {
    var tablename = $(this).closest('table').attr('id');
    if (this.checked) {
        $('#' + tablename + ' tbody input[type="checkbox"]:not(:checked)').trigger('click');
    } else {
        $('#' + tablename + ' tbody input[type="checkbox"]:checked').trigger('click');
    }
    //Enable/Disabled button for multiple options
    var button = $('#' + tablename).parent().parent().parent().find('.RemoverMultiplos');
    var buttonAddMultiple = $('#' + tablename).parent().parent().parent().find('.AdicionarMultiplos');
    var buttonDownloadMultiple = $('#' + tablename).parent().parent().parent().find('.DescarregarMultiplos');
    if (values.length == 0) { $(button).prop('disabled', true); $(buttonAddMultiple).prop('disabled', true); $(buttonDownloadMultiple).prop('disabled', true); } else { $(button).prop('disabled', false); $(buttonAddMultiple).prop('disabled', false); $(buttonDownloadMultiple).prop('disabled', false); }
    // Prevent click event from propagating to parent
    e.stopPropagation();
});
//Process input checkbox selection
var linksToDownload = [];
$(document).on("change", "table.dataTable input[type='checkbox']", function () {
    var tablename = $(this).closest('table.dataTable').attr('id');
    var table = $('#' + tablename).DataTable();
    var rows = table.column(0).checkboxes.selected();
    var link = table.row($(this).closest('td')).data()["Link"];
    values = [];
    $.each(rows, function (index, rowId) {
        values[index] = rowId;
    })
    if ($(this).is(":checked")) {
        $(this).closest("tr").removeClass("piaget-success")
        $(this).closest("tr").removeClass("piaget-danger")
        $(this).closest("tr").addClass("selected")
    } else {
        $(this).closest("tr").removeClass("selected")
    }
    if ($(this).is(":checked")) {
        $(this).closest("tr").addClass("selected");
        linksToDownload.push(link);
    } else {
        $(this).closest("tr").removeClass("selected");
        const index = linksToDownload.indexOf(link);
        if (index > -1) { // only splice array when item is found
            linksToDownload.splice(index, 1); // 2nd parameter means remove one item only
        }
    }
    //Enable/Disabled button for multiple options
    var button = $('#' + tablename).parent().parent().parent().find('.RemoverMultiplos');
    var buttonAddMultiple = $('#' + tablename).parent().parent().parent().find('.AdicionarMultiplos');
    var buttonDownloadMultiple = $('#' + tablename).parent().parent().parent().find('.DescarregarMultiplos');
    if (values.length == 0) { $(button).prop('disabled', true); $(buttonAddMultiple).prop('disabled', true); $(buttonDownloadMultiple).prop('disabled', true); } else { $(button).prop('disabled', false); $(buttonAddMultiple).prop('disabled', false); $(buttonDownloadMultiple).prop('disabled', false); }
});
//Clean search Fields
$(".btnLimpar").click(function () {
    var tablename = $(this).data('id');
    var table = $("#" + tablename).DataTable();
    var th = 0;

    $('#' + tablename + ' thead tr th').each(function () {
        if ($(this).text() != '') {
            $('#' + $(this).data('name')).val('')
            $(".select-control").val('').trigger('change'); //Select2
            table.columns(th).search($('#' + $(this).data('name')).val()); //val().trim());
            th++;
        }
    });
    //Refresh
    table.draw();
    //Clear Checkboxes throughout
    $("#GARequestIsentPag").prop("checked", false);
    $("#GAInsExaPortal").prop("checked", true);
    $("#GAInsExaSecAcad").prop("checked", true);
    $("#GAComplaintsUnreadThr").prop("checked", false);
    $("#GAComplaintsUnreadAns").prop("checked", false);
    $("#GARequestCertidao").prop("checked", true);
    $("#GARequestPedido").prop("checked", true);
    $("#CandPautaSearchCheck").prop("checked", false);
    $("#CandSalaSearchCheck").prop("checked", false);
})
//Trigger Search Event on Select Change
$(document).on("change", "table.dataTable  thead tr select", function () {
    SearchDataTable($(this).closest("table").attr('id'));
})
/*
 * 
 #####################################################
  FUNCTIONS SUPPORTING DATATABLE APIS -- NAO DUPLICAR
 #####################################################
 */





/* ############################################
  AUTHENTICATION HELPERS
 ##############################################
 */
$(document).on("change", "#isAutomaticPasswordEmail", function () {
    if ($(this).is(":checked")) {
        $('#Password').attr('readonly', true);
        $('#ConfirmPassword').attr('readonly', true);
        $('#Password').val('');
        $('#ConfirmPassword').val('');
        $('.GenPasswordRow').show();
        //$('#Email').prop('required', true);
        generatePassword();
    } else {
        $('#Password').attr('readonly', false);
        $('#ConfirmPassword').attr('readonly', false);
        $('#Password').val('');
        $('#ConfirmPassword').val('');
        $('.GenPasswordRow').hide();
        //$('#Email').prop('required', false);
    }
});
/*$(document).on("change", "#isAutomaticEmail", function () {
    if ($(this).is(":checked")) {
        $('.GenPasswordRowEmail').show();
        $('#Email').prop('required', true);
        generatePassword();
    } else {
        $('.GenPasswordRowEmail').hide();
        $('#Email').prop('required', false);
    }
});*/


/* ############################################
  PASSWORD GENERATOR
 ##############################################
 */
function generatePassword() {
    var passwordLength = 16;
    var numberChars = "0123456789";
    var upperChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    var lowerChars = "abcdefghijklmnopqrstuvwxyz";
    var specialChars = "!@#$%^&*(){}/-\+_=.,?";
    var allChars = numberChars + upperChars + lowerChars + specialChars;
    var randPasswordArray = Array(passwordLength);
    randPasswordArray[0] = numberChars;
    randPasswordArray[1] = upperChars;
    randPasswordArray[2] = lowerChars;
    randPasswordArray[3] = specialChars;
    randPasswordArray = randPasswordArray.fill(allChars, 4);
    var randomPassword = shuffleArray(randPasswordArray.map(function (x) { return x[Math.floor(Math.random() * x.length)] })).join('');
    // Copy password to DOM
    document.getElementById("GenPassword").value = randomPassword;
    document.getElementById("Password").value = randomPassword;
    document.getElementById("ConfirmPassword").value = randomPassword;
}
function copyPassword() {
    var copyText = document.getElementById("GenPassword");
    copyText.select();
    document.execCommand("copy");
}
function ShufflePassword() {
    generatePassword();
}
function shuffleArray(array) {
    for (var i = array.length - 1; i > 0; i--) {
        var j = Math.floor(Math.random() * (i + 1));
        var temp = array[i];
        array[i] = array[j];
        array[j] = temp;
    }
    return array;
}





function appenditem() {
    var html = '';
    for (var i = 0; i < values.length; i++) {
        html += '<input type"hidden" value=' + values[i] + ' name="items[]"/>'
    }
    $('#appenditem').html(html);
    $('#appenditemForm').click();
}








/* ############################################
  AJAXForm FILE UPLOAD FORM GET FORM SUBMIT EVENT
 ##############################################
 */
function ajax() {
    // Initialize progress bar DOM elements
    var container = $('.progress-container');
    var bar = $('.progress-bar');

    $('#FileUploader').ajaxForm({
        beforeSend: function (xhr) {
            // Validate file size
            var input = document.getElementById('file');
            if (!input.files) {
                //console.error("This browser doesn't seem to support the `files` property of file inputs.");
            } else if (!input.files[0]) {
                //console.log("Please select a file before clicking 'Load'");
            } else {
                var file = input.files[0];
                //var filename = file.name;
                var filesize = file.size; // Bytes
                var allowedfilesize = $('#file').data('filesize');
                if (filesize > allowedfilesize) {
                    const obj = { result: false, error: "Por favor adicionar um documento com a capacidade permitida!" };
                    handleSuccess(obj);
                    return false;
                }
            }
            var percentVal = '0%';
            bar.width(percentVal)
            container.show();
            loadIn();
        },
        uploadProgress: function (event, position, total, percentComplete) {
            var percentVal = percentComplete + '%';
            bar.width(percentVal)
            bar.html(percentVal);
        },
        success: function () {
            var percentVal = '100%';
            bar.width(percentVal)
        },
        complete: function (xhr) {
            container.hide();
            loadOut();
            handleSuccess(xhr.responseJSON);
        },
        failure: function (xhr) {
            handleFailure(xhr.responseJSON);
        }
    });
};


/* ############################################
  CROPPER JS PROFILE PICTURE FUNCTION
 ##############################################
 */
function jscropperprofile(userid, pesid) {
    var avatar = $('.profile-image-uploader');
    var image = document.getElementById('image');
    var input = document.getElementById('input');
    var $progress = $('.progress');
    var $progressBar = $('.progress-bar');
    var $modal = $('#modal-profile-image-changer');
    var cropper;
    let container = new DataTransfer();

    $('[data-toggle="tooltip"]').tooltip();

    input.addEventListener('change', function (e) {
        var files = e.target.files;
        var done = function (url) {
            input.value = '';
            image.src = url;
            $modal.modal('show');
        };
        var reader;
        var file;
        var url;
        if (files && files.length > 0) {
            file = files[0];
            if (URL) {
                done(URL.createObjectURL(file));
            } else if (FileReader) {
                reader = new FileReader();
                reader.onload = function (e) {
                    done(reader.result);
                };
                reader.readAsDataURL(file);
            }
        }
    });
    $modal.on('shown.bs.modal', function () {
        cropper = new Cropper(image, {
            aspectRatio: 1,
            viewMode: 3,
        });
    }).on('hidden.bs.modal', function () {
        cropper.destroy();
        cropper = null;
    });
    document.getElementById('crop').addEventListener('click', function () {
        var initialAvatarURL;
        var canvas;
        $modal.modal('hide');
        if (cropper) {
            canvas = cropper.getCroppedCanvas({
                minWidth: document.getElementById("minwidth").value,
                minHeight: document.getElementById("minheigth").value,
                maxWidth: document.getElementById("maxwidth").value,
                maxHeight: document.getElementById("maxheigth").value,
                fillColor: document.getElementById("fillcolor").value,
                imageSmoothingEnabled: true,
                imageSmoothingQuality: 'high',
            });
            initialAvatarURL = avatar.src;
            avatar.src = canvas.toDataURL();
            $progress.show();
            canvas.toBlob(function (blob) {
                var formData = new FormData();
                //Add canvas file to input type
                filex = new File([blob], "img.jpg", { type: "image/jpeg", lastModified: new Date().getTime() });
                container.items.add(filex);
                input.files = container.files;
                // Form Submit Click Event MVC Upload
                //$('#BtnSubmitAvatar').click();
            });
            // OR 
            // Send as ImgBase64
            $.ajax({
                type: "POST",
                url: "/gtmanagement/UpdateProfilePhoto/",
                data: {
                    UserId: userid,
                    PES_PESSOA_ID: pesid,
                    WebcamImgBase64: canvas.toDataURL("image/jpeg"),
                    file: null
                },
                beforeSend: function () { },
                success: function (data) {
                    setPictureWebCam(data)
                    $progress.hide();
                }
            })
        }
    });
}
function setChangePicturePES(pesid) {
    window.open("/gtmanagement/profilephoto?id=" + pesid + "&from=newenrollment", "profilephoto", "location=yes,resizable=no,height=620,width=550,scrollbars=yes,status=yes")
}
function setPictureWebcamBtn(userid, pesid) {
    window.open("/gtmanagement/webcam?id=" + userid + "&pesId=" + pesid + "", "webcam", "location=yes,resizable=no,height=620,width=550,scrollbars=yes,status=yes")
}
function setPictureWebCam(data) {
    var imageUrl = "/" + data.imageUrl

    if (data.result) {
        $(".profile-image-uploader").attr('src', imageUrl)//.width("100%").height("100%")
        $(".profile-image-uploader").parent().attr('href', imageUrl)
    }
    handleSuccess(data)
    loadOut();
}
function closephotoenrollment() {
    var src = $('.profile-image-uploader').attr('src');
    window.opener.setPicture(src)
    window.close()
}
function setPicture(imageUrl) {
    $(".profile-image-uploader").attr('src', imageUrl)//.width("100%").height("100%")
    $(".profile-image-uploader").parent().attr('href', imageUrl)
}





//Isencao de Agregado Familiar
$(document).on("change", ".PesFamilyIsento", function () {
    if ($(this).is(":checked"))
        $('.AddFamilyAgregadoIsento').attr("disabled", true);
    else
        $('.AddFamilyAgregadoIsento').attr("disabled", false);

})

$(document).on("change", "#ScheduledStatus", function () {
    if ($(this).is(":checked")) {
        $('#DateAct').attr('disabled', false);
        $('#DateDisact').attr('disabled', false);
        $('#DateAct').val('');
        $('#DateDisact').val('');
        $('#DateAct').prop('required', true);
        $('#DateDisact').prop('required', true);
        $('#Status').attr('disabled', true);
        $('#Status').hide();
        $('#StatusPending').show();
        $('.dateActRow').show();
    } else {
        $('#DateAct').attr('disabled', true);
        $('#DateDisact').attr('disabled', true);
        $('#DateAct').val('');
        $('#DateDisact').val('');
        $('#DateAct').prop('required', false);
        $('#DateDisact').prop('required', false);
        $('#Status').attr('disabled', false);
        $('#Status').show();
        $('#StatusPending').hide();
        $('.dateActRow').hide();
    }
});
function checkDisabled(entity) {
    if (entity == "isOAuth2Valid") {
        if ($('#isOAuth2Valid').is(":checked")) {
            $('#OAuth2Provider').attr('disabled', false);
            $('#OAuth2ID').attr('disabled', false);
            $('#OAuth2Provider').prop('required', true);
            $('#OAuth2ID').prop('required', true);
            $('.OAuth2Row').show();
        } else {
            $('#OAuth2Provider').attr('disabled', true);
            $('#OAuth2ID').attr('disabled', true);
            $('#OAuth2Provider').prop('required', false);
            $('#OAuth2ID').prop('required', false);
            $('.OAuth2Row').hide();
        }
    }
    if (entity == "isLDAPValid") {
        if ($('#isLDAPValid').is(":checked")) {
            $('#LADPHost').attr('disabled', false);
            $('#LADPBase').attr('disabled', false);
            $('#LADPHost').prop('required', true);
            $('#LADPBase').prop('required', true);
            $('.LDAPRow').show();
        } else {
            $('#LADPHost').attr('disabled', true);
            $('#LADPBase').attr('disabled', true);
            $('#LADPHost').prop('required', false);
            $('#LADPBase').prop('required', false);
            $('.LDAPRow').hide();
        }
    }
    if (entity == "ScheduledStatus") {
        if ($('#ScheduledStatus').is(":checked")) {
            $('#DateAct').attr('disabled', false);
            $('#DateDisact').attr('disabled', false);
            //$('#DateAct').val('');
            //$('#DateDisact').val('');
            $('#DateAct').prop('required', true);
            $('#DateDisact').prop('required', true);
            $('#Status').attr('disabled', true);
            $('#Status').hide();
            $('#StatusPending').show();
            $('.dateActRow').show();
        } else {
            $('#DateAct').attr('disabled', true);
            $('#DateDisact').attr('disabled', true);
            //$('#DateAct').val('');
            //$('#DateDisact').val('');
            $('#DateAct').prop('required', false);
            $('#DateDisact').prop('required', false);
            $('#Status').attr('disabled', false);
            $('#Status').show();
            $('#StatusPending').hide();
            $('.dateActRow').hide();
        }
    }
}






/* ############################################
  COUNTRY AND CITY ADDRESS HELPERS START
 ##############################################
 */
function addressHelper() {
    // Get Input helpers
    var CountryId = $('.SelectCountry').val();
    var AO = null; // ANGOLAN ID NUMBER FROM [GRL_ENDERECO_PAIS]
    var url = '../../Ajax/AddressHelper';
    //Retrieve selected options from Ajax
    $.ajax({
        type: "GET",
        url: "/ajax/GetConfigValuesStandardCountryID",
        data: {},
        cache: false,
        beforeSend: function () { },
        complete: function () { },
        success: function (data) {
            AO = data;
            // On Edit View process Html Helpers
            if (CountryId == '') {
                $('.AddressHelper').hide();
                $('.AddressHelperAO').hide();
            }
            else if (CountryId == AO) {
                $('.AddressHelper').show();
                $('.AddressHelperAO').show();
                $('.newCity').attr('disabled', true);
                $('.SelectDistrict').prop("required", true);
            }
            else {
                $('.AddressHelper').show();
                $('.AddressHelperAO').hide();
                $('.newCity').attr('disabled', false);
            }
        }
    });
    // Set Onchange events for Country Select
    $(document).on("change", ".SelectCountry", function () {
        var thisvar = $(this);
        // Loadin
        loadIn();
        // Set Id and Get Select from ajax
        var Id = $(this).val();
        // Process Html Helpers
        if (Id == '') {
            $(thisvar).parent().parent().parent().find('.AddressHelper').hide();
            $(thisvar).parent().parent().parent().find('.AddressHelperAO').hide();
            Id = 0;
        }
        else if (Id == AO) {
            $(thisvar).parent().parent().parent().find('.AddressHelper').show();
            $(thisvar).parent().parent().parent().find('.AddressHelperAO').show();
            $(thisvar).parent().parent().parent().find('.newCity').attr('disabled', true);
        } else {
            $(thisvar).parent().parent().parent().find('.AddressHelper').show();
            $(thisvar).parent().parent().parent().find('.AddressHelperAO').hide();
            $(thisvar).parent().parent().parent().find('.newCity').attr('disabled', false);
        }
        // Retrieve selected options from Ajax
        $.ajax({
            type: "GET",
            url: "/ajax/GetCityByCountry",
            data: { "id": Id },
            cache: false,
            beforeSend: function () {
                // Loadin
                loadIn();
            },
            complete: function () {
            },
            success: function (data) {
                $(thisvar).parent().parent().parent().find('.SelectCity option').remove();
                var s = '<option value="">Selecionar uma opção</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].ID + '">' + data[i].Nome + '</option>';
                }
                $(thisvar).parent().parent().parent().find('.SelectCity').html(s);
                // Loadout
                loadOut();
            }
        });
    });
    // Set Onchange events for City Select
    $(document).on("change", ".SelectCity", function () {
        var thisvar = $(this);
        // Loadin
        loadIn();
        // Set Id and Get Select from ajax
        var Id = $(this).val();
        var Action = 'mun';
        if (Id == '') Id = 0; // Select options cannot be null for Model
        // Retrieve selected options from Ajax
        $.ajax({
            type: "GET",
            url: "/ajax/GetDistrictByCity",
            data: { "id": Id },
            cache: false,
            beforeSend: function () {
                // Loadin
                loadIn();
            },
            complete: function () {
            },
            success: function (data) {
                $(thisvar).parent().parent().parent().find('.SelectDistrict option').remove();
                var s = '<option value="">Selecionar uma opção</option>';
                for (var i = 0; i < data.length; i++) {
                    s += '<option value="' + data[i].ID + '">' + data[i].Nome + '</option>';
                }
                $(thisvar).parent().parent().parent().find('.SelectDistrict').html(s);
                // Add requirement for District/Municipio selection if Country=AO
                if ($(thisvar).parent().parent().parent().find('.SelectCountry').val() == AO)
                    $(thisvar).parent().parent().parent().find('.SelectDistrict').prop("required", true);
                else
                    $(thisvar).parent().parent().parent().find('.SelectDistrict').prop("required", false);
                // Loadout
                loadOut();
            }
        });
    });
}
// Display Popup window to add new city
function addressHelperPopUpNewCity(thisvar) {
    var CountryId = $(thisvar).parent().parent().parent().parent().find('.SelectCountry').val();
    if (CountryId != '')
        window.open('/administration/address/' + CountryId + '?entityfrom=addnewcity&var=' + thisvar, 'newwin', 'width=400,height=400');
    return false;
}
// Update Citylist on Select options with newly updated Id, trigger select change
function updateCityList(CidadeId, thisvar) {
    var CountryId = $(thisvar).parent().parent().parent().parent().find('.SelectCountry').val();
    //var CountryId = $('.SelectCountry').val();
    $(thisvar).parent().parent().parent().parent().find('.SelectCountry').val(CountryId).trigger('change');
    //$('.SelectCountry').val(CountryId).trigger('change');
    updateCityIdList(CidadeId, thisvar);
}
// Automatically select updated option from Popup window with new Id
function updateCityIdList(CidadeId, thisvar) {
    setTimeout(function () {
        $(thisvar).parent().parent().parent().parent().find('.SelectCity').val(CidadeId).trigger('change');
        // $('.SelectCity').val(CidadeId).trigger('change');
    }, 500); // Wait for select change trigger, timeout 500 milli secons = 0.5 Seconds
}
/* ############################################
  COUNTRY AND CITY ADDRESS HELPERS END
 ##############################################
 */












//PLANOS 
$(document).on("click", "#gttreinoclr", function () {
    $("#list2 li").remove();
    $("#list1 li button").attr("disabled",false);
})
$(document).on("change", "#FaseTreinoId", function () {
    if ($(this).val() != '') {
        if ($("#list2 li").length == 0) {
            jsonObj = [];
            var response = { result: false, error: "Não existem exercícios no plano!" };
            handleSuccess(response);
            $("#FaseTreinoId > option").prop("selected", "");
        } else {
            $.ajax({
                type: "GET",
                url: "/gtmanagement/GetFasesTreino",
                data: { "id": $(this).val() },
                cache: false,
                beforeSend: function () {
                    //loadIn();
                },
                complete: function () {
                },
                success: function (data) {
                    $("#list2").find(("input[name='exSeries[]']")).val(data[0].GT_Series_ID);
                    $("#list2").find(("input[name='exRepeticoes[]']")).val(data[0].GT_Repeticoes_ID);
                    $("#list2").find(("input[name='exCarga[]']")).val(data[0].GT_Carga_ID);
                    $("#list2").find(("input[name='exTempo[]']")).val(data[0].GT_TempoDescanso_ID);
                    //loadOut();
                }
            });
        }
    }
})
$(document).on("change", "#GTTreinoId", function () {
    if ($(this).val() != '') {
        var gttipotreino = $('#GT_EXERCISE_TYPE_BODYMASS').val();
        if (gttipotreino == $('#GTTipoTreinoId').val()) 
            window.location = "/gtmanagement/bodymassplans/" + $(this).val() + "?predefined=true";
        else
            window.location = "/gtmanagement/cardioplans/" + $(this).val() + "?predefined=true";
    }
})
$(document).on("change", "#plantype1", function () {
        $(".newplangtreino").hide();
    $('#GTTreinoId').attr("required", false);
    $('#Nome').attr("disabled", false);
    $('#Nome').val("");
})
$(document).on("change", "#plantype2", function () {
        $(".newplangtreino").show();
    $('#GTTreinoId').attr("required", true);
    $('#Nome').attr("disabled", true);
    $('#Nome').val("");
})
$(document).on("change", "#planningtype1", function () {
    $(".newplanfasetreino").hide();
    $('#FaseTreinoId').attr("required", false);
})
$(document).on("change", "#planningtype2", function () {
    $(".newplanfasetreino").show();
    $('#FaseTreinoId').attr("required", true);
})
$(document).on('click', '.addlistplan1', function () {
    var gttipotreino = $('#GT_EXERCISE_TYPE_BODYMASS').val();
    $(this).removeClass('addlistplan1').addClass('removegaclass1');
    $(this).removeClass('btn-success').addClass('btn-danger');
    $(this).find('i').removeClass('fa-plus-circle').addClass('fa-times');
    //$(this).parent().find('input').attr('name', 'exIds[]');

    var dataid = $(this).data('id');
    var html = '<div class="inputs_' + dataid + '"><table><tr>';
    if (gttipotreino == $('#GTTipoTreinoId').val()) {
         html +='<td></td> <td>N° de séries:</td><td>N° de repetições:</td><td>% 1RM:</td><td>Tempo descanso:</td><td>Reps:</td><td>Carga</td><td>RM</td>';
    } else {
         html += '<td></td> <td>Duração (Min.):</td><td>FC (Min/Máx) bpm:</td><td>Nível / Resist. / Velocidade::</td><td>Inclinação:</td>';
    }
    html+='</tr>';
    html += '<tr><td><input readonly type="hidden" name="exIds[]" value="' + dataid + '"></td>';

    if (gttipotreino == $('#GTTipoTreinoId').val()) {
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exSeries[]" value="' + $('#GT_Series_ID').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exRepeticoes[]" value="' + $('#GT_Repeticoes_ID').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exCarga[]" value="' + $('#GT_Carga_ID').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exTempo[]" value="' + $('#GT_TempoDescanso_ID').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exReps[]" value="' + $('#Reps').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exCargaUsada[]" value="' + $('#CargaUsada').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exRM[]" value="' + $('#RM').val() + '"></td>';
    } else {
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exDuracao[]" value="' + $('#GT_DuracaoTreinoCardio_ID').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exFC[]" value="' + $('#FC').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exNivel[]" value="' + $('#Nivel').val() + '"></td>';
        html += '<td><input class="form-control form-control-sm w-70" readonly type="" name="exDistancia[]" value="' + $('#Distancia').val() + '"></td>';
    }

    html += '</tr></table></div>';

    $(this).parent().append(html)
    $(this).parent().clone(true).appendTo('#list2');
    //Reverse
    $(this).attr("disabled", true);
    $(this).removeClass('removegaclass1').addClass('addlistplan1');
    $(this).removeClass('btn-danger').addClass('btn-success');
    $(this).find('i').removeClass('fa-times').addClass('fa-plus-circle');
    $(this).parent().find('.inputs_' + dataid + '').remove()
});
$(document).on('click', '.removegaclass1', function () {
    var dataid = $(this).data('id');
    $('#ex_' + dataid+' button').attr("disabled", false);
    $(this).parent().remove();
});


//Avalicao psicologica e diversos
function anxientForm() {
    $('#anxientForm').clone(true).appendTo('#anxientFormClone');
    $("#anxientFormClone form:first-child").submit();
    $('#anxientFormClone input[name="frmaction"] ').val('1');
}
function processquest(result){
    if (result == "0") {
        $('#riskRB1').css("background", "green").css("color","#ddd")
        $('#riskRB2').css("background", "green").css("color", "#ddd")
        $('#riskRB3').css("background", "green").css("color", "#ddd")
        $('#riskRB4').css("background", "green").css("color", "#ddd")
    }
    if (result == "1") {
        $('#riskRM1').css("background", "green").css("color", "#ddd")
        $('#riskRM2').css("background", "green").css("color", "#ddd")
        $('#riskRM3').css("background", "green").css("color", "#ddd")
        $('#riskRM4').css("background", "green").css("color", "#ddd")
    }
    if (result == "2") {
        $('#riskRE1').css("background", "green").css("color", "#ddd")
        $('#riskRE2').css("background", "green").css("color", "#ddd")
        $('#riskRE3').css("background", "green").css("color", "#ddd")
        $('#riskRE4').css("background", "green").css("color", "#ddd")
    }
}
function submitanxientForm() { $("#anxientFormClone form:first-child").submit(); }

$(document).on('change', '.chkcoronaryRisk', function () {
   
    if ($(this).is(":checked")) {
        $(this).parent().parent().find('input[type="text"]').attr("disabled", false);
        //$(this).parent().parent().find('input[type="text"]').val('');
    } else {
        $(this).parent().parent().find('input[type="text"]').attr("disabled", true);
        //$(this).parent().parent().find('input[type="text"]').val('');
    }
});
$(document).on('change', '.radhealth', function () {

    if ($(this).val()=='1') {
        $(this).parent().parent().find('fieldset').attr("disabled", false);
        //$(this).parent().parent().find('input[type="text"]').val('');
    } else {
        $(this).parent().parent().find('fieldset').attr("disabled", true);
        //$(this).parent().parent().find('input[type="text"]').val('');
    }
});
//Flexibility
$("input:checkbox").change(function () {
    var group = ":checkbox[name='" + $(this).attr("name") + "']";
    if ($(this).is(':checked')) {
        $(group).not($(this)).attr("checked", false);
        flexSetClassificacao($(this).val());
        let s = Number($('#flexNumber').val());
        $('#flexflexNumberArr_' + s).val($(this).val())
    }
});
function flexLoadNumberArr() {
    let s = Number($('#flexNumber').val());
    let ss = $('#flexflexNumberArr_' + s).val()
    if (ss != '') {
        $("input:checkbox[value=" + ss + "]").prop('checked', true);
        flexSetClassificacao(ss);
    }
}
function flexProximo(btn) {
    let s = Number($('#flexNumber').val());
    let ss = s + 1;
    flexUpDown(ss)
}
function flexUltimo(btn) {
    let max = Number($('#flexNumber').attr('max'));
    flexUpDown(max)
}
function flexAnterior(btn) {
    let s = Number($('#flexNumber').val());
    let ss = s - 1;
    flexUpDown(ss)
}
function flexPrimeiro(btn) {
    let min = Number($('#flexNumber').attr('min'));
    flexUpDown(min)
}
function flexUpDown(ss) {
    let s = Number($('#flexNumber').val());
    let min = Number($('#flexNumber').attr('min'));
    let max = Number($('#flexNumber').attr('max'));
    //let ss = s + 1;
    if (ss <= max && ss >= min) {
        $('#flexNumber').val(ss)
        $('#flexDesc').val("Teste " + ss + " de " + max)
        flexClearCheck();
        flexSetClassificacao();
        flexLoadImg(ss);
        flexLoadNumberArr();
    }
}
function flexClearCheck() {
    var group = ":checkbox[name='flex']";
        $(group).attr("checked", false);
}
function flexSetClassificacao(val) {
    var cl = '';

        switch (val) {
            case "0":
                cl ='Muito Fraco'
                break;
            case "1":
                cl = 'Fraco'
                break;
            case "2":
                cl = 'Médio'
                break;
            case "3":
                cl = 'Bom'
                break;
            case "4":
                cl = 'Excelente'
                break;
            default:
        }
        $('#flexClassificacao').val(cl)
}
function flexLoadImg(id) {
    $("#flexImg").attr('src', '/Assets/images/imagesflexi/f' + id + '.jpg').width("100%").height("100%")
}
function loadflexitype(id) {
    //if (confirm('Tem a certeza que pretende mudar de avaliação?')) {
        window.location = '/gtmanagement/flexibility?flexitype=' + id;
    //} else {}
}