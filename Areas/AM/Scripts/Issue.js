Array.prototype.remove = function (x) {
    var i;
    for (i in this) {
        if (this[i].toString() == x.toString()) {
            this.splice(i, 1)
        }
    }
}
//posting Student Credits-----------------------------
window.VoucherIds = [];
function OnCheckboxChange(that) {

    $('.voucherCheck').each(function (i, u) {

        if ($(that).prop('checked') == true) {
            $(u).prop('checked', true);
        } else {
            $(u).prop('checked', false);
        }
        checkChange(u);
    });

    ShowAndHideStickyButtons()
}


function checkChange(that) {

    if ($(that).prop('checked') == true) {
        window.VoucherIds.push(that.value)
    }
    else {
        window.VoucherIds.remove(that.value)
    }
    ShowAndHideStickyButtons()
}
function ShowAndHideStickyButtons() {
    if (window.VoucherIds.length > 0) {
        var status = $('#Status').val();
        if (status == 1) {
            $('.stickyButtonsIssue').removeClass('hidden');
            $('.stickyButtonsDamage').removeClass('hidden');
            $('.stickyButtonsReturn').addClass('hidden');
            $('.stickyButtonsTransfer').addClass('hidden');
        } else if (status == 2) {
            $('.stickyButtonsReturn').removeClass('hidden');
            $('.stickyButtonsDamage').removeClass('hidden');
            $('.stickyButtonsIssue').addClass('hidden');
            $('.stickyButtonsTransfer').removeClass('hidden');
        }

        //$('#noOfSelectedVouchersId').removeClass('displayNone');
        $('#noOfSelectedVouchersId,#IssuedModalTitle').text(window.VoucherIds.length + ' Item(s) Selected for Registration');
    }
    else {
        $('.stickyButtonsIssue').addClass('hidden');
        $('.stickyButtonsReturn').addClass('hidden');
        $('.stickyButtonsDamage').addClass('hidden');
        $('.stickyButtonsTransfer').addClass('hidden');
        //  $('#noOfSelectedVouchersId').addClass('displayNone');
    }
}
function popupModal(type) {
     //debugger;
    if (type == 'Return') {
        $('#ModalDescription,#ModalIssueDate,#ModalConditionTypeId').removeClass('hidden');
        $('#ModalDepartment,#ModalEmployee,#ModalLocation').addClass('hidden');
        $('.MstickyButtonsIssue,.MstickyButtonsDamage,.MstickyButtonsTransfer').addClass('hidden');
        $('.MstickyButtonsReturn').removeClass('hidden');
    } else if (type == 'Damage') {
        $('#ModalDescription,#ModalIssueDate').removeClass('hidden');
        $('#ModalDepartment,#ModalEmployee,#ModalLocation,#ModalConditionTypeId').addClass('hidden');
        $('.MstickyButtonsIssue,.MstickyButtonsReturn,.MstickyButtonsTransfer').addClass('hidden');
        $('.MstickyButtonsDamage').removeClass('hidden');
    } else if (type == 'Issuance') {
        $('#ModalDepartment,#ModalEmployee,#ModalLocation,#ModalConditionTypeId,#ModalDescription,#ModalIssueDate').removeClass('hidden');
        $('.MstickyButtonsReturn,.MstickyButtonsDamage,.MstickyButtonsTransfer').addClass('hidden');
        $('.MstickyButtonsIssue').removeClass('hidden');
    } else {
        $('#ModalDepartment,#ModalEmployee,#ModalLocation,#ModalConditionTypeId,#ModalDescription,#ModalIssueDate').removeClass('hidden');
        $('.MstickyButtonsReturn,.MstickyButtonsDamage,.MstickyButtonsIssue').addClass('hidden');
        $('.MstickyButtonsTransfer').removeClass('hidden');
    }
}

function IsValid(type) {

    var validated = true;
    //if (!$('#IssueDate').val()) {
    //    $('#IssueDate').focus();
    //    DisplayMessage('error', 'Date is required')
    //    return false;
    //}
    //if (!$('#DepartmentId').val()) {
    //    $('#DepartmentId').select2('open');
    //    DisplayMessage('error', 'Department is required')
    //    return false;
    //}

    if (type == 'Transfer' || type == 'Issue') {
        //  debugger;
        $rows = $('#tblIssueItems tbody tr td input.voucherCheck:checked');
        if ($rows.length > 0 && window.VoucherIds.length > 0) {
            var departmentId = $('#ModalDepartmentId').val();
            if (!departmentId) {
                $('#ModalDepartmentId').select2('open');
                DisplayMessage('error', 'Department is required')
                validated = false;
                return false;
            }
            //var employeeId = $('#EmployeeId').val();
            //if (!employeeId) {
            //    $(that).find('#EmployeeId').select2('open');
            //    DisplayMessage('error', 'Employee is required')
            //    validated = false;
            //    return false;
            //}
            //  $rows.each(function (index, value) {
            //var that = $(this).parent().parent();
            //console.log('Department', $(that).find('.DepartmentId').val());
            //console.log('EmployeeId', $(that).find('.EmployeeId').val());
            //console.log('LocationId', $(that).find('.LocationId').val());
            //if (!$(that).find('.DepartmentId').val()) {
            //    $(that).find('.DepartmentId').select2('open');
            //    DisplayMessage('error', 'Department is required')
            //    validated = false;
            //    return false;
            //}
            //if (!$(that).find('.EmployeeId').val()) {
            //    $(that).find('.EmployeeId').select2('open');
            //    DisplayMessage('error', 'Employee is required')
            //    validated = false;
            //    return false;
            //}
            //if (!$(that).find('.LocationId').val()) {
            //    $(that).find('.LocationId').select2('open');
            //    DisplayMessage('error', 'Location is required')
            //    validated = false;
            //    return false;
            //}

            //if (!$(this).find('.getItemCodes').val()) {
            //    DisplayMessage('error', 'Select atleast one item')
            //    $(this).find('.getItemCodes').select2('open');
            //    validated = false;
            //    return false;
            //}
            //  });
        } else {
            DisplayMessage('error', 'No Product Selected');
            validated = false;
            return false;
        }
    }


    //$rows.each(function (index, value) {
    //    var amount = 0;
    //    if ($(this).find('[id $= TransactionType]').val() == "Cr") {
    //        amount = StringToFloat($(this).find('[id $= Credit]').val());
    //    } else {
    //        amount = StringToFloat($(this).find('[id $= Debit]').val());
    //    }
    //    if ($(this).find('[id $= TransactionId]').val() && amount > 0) {

    //        if (!$(this).find('[id $= AccountId]').val()) {
    //            $(this).find('[id $= item_no_lbl]').css('color', 'red');
    //            $(this).find('[id $= item_no_lbl]').show().text('Account is Required').delay(2000).fadeOut();
    //            validated = false;
    //            return false;
    //        }
    //    }
    //});
    return validated;
}

$(document).ready(function () {

    $(".datepicker").each(function () {
        $(this).val(RemoveTimeFromDate($(this).val()));
    });
});