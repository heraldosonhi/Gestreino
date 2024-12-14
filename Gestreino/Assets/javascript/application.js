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
            //SetUpDatepicker();
            //addressHelper();
            //setupSelect2();
            //quillEditor();
            //checkDisabled("ScheduledStatus");
            //ajax();
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
            if (response.table == "manageProfilesAndGroups") {
                $("#fancyProfile").fancytree('getTree').reload();
                $("#fancyGroup").fancytree('getTree').reload();
                FancyTreeLoadProfilesAndGroups($("#Id").val());
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
        if (response.cand) {
            //courseModels(response.validationId, 3, response.processId, response.PesId, null);
            //refreshCalendar('#' + response.calendar, response.calendarColumn)
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
            "url": "../../gtmanagement/GetUser", // POST TO CONTROLLER
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
                    return '<a title="Visualizar" href="/gtmanagement/viewusers/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
                        ' <a style="display:' + full.AccessControlEdit + '" title = "Editar" href = "/administration/edituser/' + full.Id + '?entityfrom=maininfo" > <i class="fa fa-pencil" /></i ></a >' +
                        ' <a style="display:' + full.AccessControlAddGroup + '" title = "Adicionar Grupos" href = "/administration/appendusersitems/' + full.Id + '?entityfrom=usergroups" ><i class="fa fa-user-plus"></i></a>' +
                        ' <a style="display:' + full.AccessControlAddProfile + '" title = "Adicionar Perfis" href = "/administration/appendusersitems/' + full.Id + '?entityfrom=userprofiles" ><i class="fa fa-user-shield"></i></a>';

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
            "url": "../../gtmanagement/GetGroups", // POST TO CONTROLLER
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
                    return '<a title="Visualizar" href="/gtmanagement/viewgroups/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
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
            "url": "../../gtmanagement/GetAtoms", // POST TO CONTROLLER
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
                    return '<a title="Visualizar" href="/gtmanagement/viewatoms/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
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
            "url": "../../gtmanagement/GetProfiles", // POST TO CONTROLLER
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
                    return '<a title="Visualizar" href="/gtmanagement/viewprofiles/' + full.Id + '"><i class="fa fa-search"/></i></a>' +
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
            "url": "../../gtmanagement/GetUserGroup", // POST TO CONTROLLER
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
            "url": "../../gtmanagement/GetAtomGroup", // POST TO CONTROLLER
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
            "url": "../../gtmanagement/GetProfileAtom", // POST TO CONTROLLER
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
            "url": "../../gtmanagement/GetProfileUtil", // POST TO CONTROLLER
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




function appenditem() {
    var html = '';
    for (var i = 0; i < values.length; i++) {
        html += '<input type"hidden" value=' + values[i] + ' name="items[]"/>'
    }
    $('#appenditem').html(html);
    $('#appenditemForm').click();
}