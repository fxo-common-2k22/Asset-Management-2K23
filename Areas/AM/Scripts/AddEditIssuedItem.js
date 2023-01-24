$(document).on('change', '.getItemCodes', function (event) { 
//$(".getItemCodes").change(function (event) {
    var that = this;
    $(that).closest('tr').find("[id $= ItemCode]").empty().change();
    if ($(that).val()) {
        $.ajax({
            url: "/AM/Issue/GetItemCodesById/" + $(that).val(),
            type: "Get",
            contentType: "application/json; charset=utf-8",
            datatype: 'JSON',
            success: function (data) {
                binditemcodes(data, that);
            },
            error: function (jqXHR, textStatus, errorThrown) {
                rowReset(that);
                DisplayMessage('error', 'Item not found');
            }
        });
    }
});

function binditemcodes(data, that) {
    if (data.codes.length > 0) {
        $(that).closest('tr').find("[id $= ItemCode]").empty();
        $.each(data.codes, function (i, value) {
            $(that).closest('tr').find('[id $= ItemCode]').append("<option data-qty=" + value.Qty + " value= " + value.Value + ">" + value.Text + "</option>");
        });
        if (data.IsConsumable && data.IsConsumable == true) {
            $(that).closest('tr').find("[id $= Quantity]").prop('readonly', false);
            $(that).closest('tr').find("[id $= Quantity]").prop('disabled', false);
            $(that).closest('tr').find("[id $= Quantity]").val(1);
            $(that).closest('tr').find('[id $= ItemCode]').prop('readonly', false);
            $(that).closest('tr').find('[id $= ItemCode]').prop({ disabled: false });
        } else {
            $(that).closest('tr').find('[id $= ItemCode]').prop('readonly', false);
            $(that).closest('tr').find('[id $= ItemCode]').prop({ disabled: false });
            $(that).closest('tr').find("[id $= Quantity]").val(1);
            $(that).closest('tr').find("[id $= Quantity]").prop('readonly', true);
            $(that).closest('tr').find("[id $= Quantity]").prop('disabled', true);
        }
    } else {
        rowReset(that);
    }
}

function rowReset(that) {
    $(that).closest('tr').find("[id $= ItemId]").val('').change();
    $(that).closest('tr').find("[id $= ItemCode]").empty();
    $(that).closest('tr').find("[id $= Quantity]").prop('readonly', false);
    $(that).closest('tr').find("[id $= Quantity]").prop('disabled', false);
    $(that).closest('tr').find("[id $= ItemCode]").prop('readonly', true);
    $(that).closest('tr').find("[id $= ItemCode]").prop({ disabled: true });
}

$(document).on('keydown', '.barcodeField', function (e) {
    if (e.which == 13) {//Enter key pressed
        var that = this;
        e.preventDefault();
        if ($(this).val()) {
            $.ajax({
                url: '/AM/Issue/GetItemByBarcode',
                type: 'post',
                data: { id: $(this).val() },
                success: function (data) {
                    if (data) {
                        if (data.ItemId) {
                            $(that).closest('tr').find("[id $= ItemId]").val(data.ItemId).change();
                        }
                    }
                    else {
                        rowReset(that);
                        DisplayMessage('error', 'Item not found');
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    rowReset(that);
                    DisplayMessage('error', 'Item not found');
                }
            });
        }
    }
})
function changeHref(idd) {
    if (idd) {
        var currentURL = window.location.href;
        var id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1)
        if (StringToFloat(id) > 0) {
            var uu = window.location.href.substring(0, window.location.href.lastIndexOf('/'))
            var newURL = uu + "/" + idd;
            window.history.replaceState(null, null, newURL);
        } else {
            var uu = window.location.href;
            var newURL = uu + "/" + idd;
            window.history.replaceState(null, null, newURL);
        }
    }
}
$('.move>input').keydown(function (e) {
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $(this).parent().next().children().focus();
    }
    if (e.keyCode == 37)//left arrow key
    {
        e.preventDefault();
        $(this).parent().prev().children().focus();
    }
    if (e.keyCode == 38)//arrow up
    {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex)
        rows.find('td').eq(tdIndex).find('input').focus();
        let id = $(rows.find('td').eq(tdIndex).find('input')).attr('id');
        if (id === 'Details_-1__Barcode' || id === 'Details_-1__Quantity' || id === 'Details_-1__Description') {
            //find upar grid
            $("#Description").focus();
        }
    }
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex + 2)
        rows.find('td').eq(tdIndex).find('input').focusin();
    }
});
$('.move>select').keydown(function (e) {
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $(this).parent().next().children().focus();
    }
    if (e.keyCode == 37)//left arrow key
    {
        e.preventDefault();
        $(this).parent().prev().children().focus();
    }
    if (e.keyCode == 38)//arrow up
    {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex)
        rows.find('td').eq(tdIndex).find('select').focus();
        rows.find('td').eq(tdIndex).find('select').click();
        let id = $(rows.find('td').eq(tdIndex).find('select')).attr('id');
        if (id === 'Details_-1__ItemId' || id === 'Details_-1__ItemCode' || id === 'Details_-1__EmployeeId' || id === 'Details_-1__RoomId') {
            //find upar grid
            $("#Description").focus();
        }
    }
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex + 2)
        rows.find('td').eq(tdIndex).find('select').focus();
        rows.find('td').eq(tdIndex).find('select').next().click()
        rows.find('td').eq(tdIndex).find('select').next().addClass('select2-container--below select2-container--focus ');
    }
});
$("#IssuedItemId").keydown(function (e) {
    e.preventDefault();
    if (e.keyCode == 40)//arrow down
    {
        $("#Description").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        $("#ui-datepicker-div").show();
        $("#IssueDate").focus();
    }
})
$("#IssueDate").keydown(function (e) {
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Description").focus();
    }
    if (e.keyCode == 37)//arrow left
    {
        e.preventDefault();
        $("#ui-datepicker-div").hide();
        $("#IssuedItemId").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $("#ui-datepicker-div").hide();
        $("#DepartmentId").focus();
    }
})
$("#DepartmentId").keydown(function (e) {
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Description").focus();
    }
    if (e.keyCode == 37)//arrow left
    {
        e.preventDefault();
        $("#ui-datepicker-div").show();
        $("#IssueDate").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $("#Description").focus();
    }
})
$("#Description").keydown(function (e) {
    if (e.keyCode == 38)//arrow up
    {
        e.preventDefault();
        $("#IssuedItemId").focus();
    }
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Details_0__Barcode").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        ("#Details_0__Barcode").focus();
    }
})
function printGV() {
    $("#PrintSection").removeClass("displayNone");
    $("#PrintSection").printThis();
    $("#PrintSection").addClass("displayNone");
}
$(document).keydown(function (event) {
    if ((event.which == 83) && (event.ctrlKey)) {
        event.preventDefault();
        $("#ReqSaveLbl").click();
    }
    if ((event.which == 80) && (event.ctrlKey)) {
        event.preventDefault();
        $("#ReqPrintLbl").click();
    }
});

$(document).on('keydown', '#IssuedItemId', function () {
    //$('#IssuedItemId').keydown(function (e) {
    if (e.which == 13) {//Enter key pressed
        e.preventDefault();
        window.location = '/AM/Issue/AddEditIssuedItem/' + this.value;
    }
});

function IsValid() {
    var validated = true;
    if (!$('#IssueDate').val()) {
        $('#IssueDate').focus();
        DisplayMessage('error', 'Date is required')
        return false;
    }
    if (!$('#DepartmentId').val()) {
        $('#DepartmentId').select2('open');
        DisplayMessage('error', 'Department is required')
        return false;
    }
    $rows = $('#trans_details tbody tr:visible');
    if ($rows.length === 1) {
        $rows.each(function (index, value) {
            if (!$(this).find('.getItemCodes').val()) {
                DisplayMessage('error', 'Select atleast one item')
                $(this).find('.getItemCodes').select2('open');
                validated = false;
                return false;
            }
        });
    }
    if (validated) {
        $('input').removeAttr('disabled');
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

$(document).on('click', '.voucherLink', function () {
    var id = $(this).attr("id");
    if (id === "lbPrintVoucher") {
        try {
            setTimeout(function () {
                $('#toast-container').remove();
                $.unblockUI();
                PrintInvoice();
            }, 1000);
        } catch (err) { console.log('toaster not found') }
    }
    if (id === "lbUnPrintVoucher") {
        $('#mainsection').show();
        $('#printsection').hide();
        $('#lbUnPrintVoucher').hide();
        HideTopControls(false);
    }
    return false;
});

function HideTopControls(val) {
    if (val) {
        $('.topLi').each(function (index, value) {
            $(this).addClass('nodisplay');
        });
    } else {
        $('.topLi').each(function (index, value) {
            $(this).removeClass('nodisplay');
        });
    }
}

function PrintInvoice() {
    if ($('#IssuedItemId').val() > 0) {
        $('#mainsection').hide();
        $('#printsection').show();
        $('#lbUnPrintVoucher').show();
        HideTopControls(true);
        window.print();
        setTimeout(function () {
            $('#mainsection').show();
            $('#printsection').hide();
            $('#lbUnPrintVoucher').hide();
            HideTopControls(false);
        }, 0);

    } else {
        DisplayMessage('error', 'Invoice does not exist');
    }
}
