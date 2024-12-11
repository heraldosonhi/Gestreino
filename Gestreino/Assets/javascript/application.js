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
        case 'groups': url = '../../Ajax/UsersGroups';
            break;
        case 'utilisub': url = '../../Ajax/UsersUtiliSub';
            break;
        case 'profiles': url = '../../Ajax/UsersProfiles';
            break;
        case 'atoms': url = '../../Ajax/UsersAtoms';
            break;
        case 'users': url = '../../Ajax/Users';
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
            ajax();
        },
        error: function () {
            $(".modal-crud-content").show().html('<span class="text-accent-red">Erro ao processar este pedido, por favor tente mais tarde!</span>');
        }
    });
});
