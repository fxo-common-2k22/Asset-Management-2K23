$('#PurchaseOrderId').keydown(function (e) {
    if (e.which == 13) {//Enter key pressed
        e.preventDefault();
        window.location = '/AM/PurchaseOrder/AddEditPurchaseOrder/' + this.value;
    }
});
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

$("#PurchaseOrderId").keydown(function (e) {
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Description").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $("#ui-datepicker-div").show();
        $("#PurchaseOrderDate").focus();
    }
})
$("#PurchaseOrderDate").keydown(function (e) {
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Description").focus();
    }
    if (e.keyCode == 37)//arrow left
    {
        e.preventDefault();
        $("#ui-datepicker-div").hide();
        $("#PurchaseOrderId").focus();
    }
    if (e.keyCode == 39)//right arrow key
    {
        e.preventDefault();
        $("#ui-datepicker-div").hide();
        $("#SupplierId").focus();
    }
})
$("#SupplierId").keydown(function (e) {
    if (e.keyCode == 40)//arrow down
    {
        e.preventDefault();
        $("#Description").focus();
    }
    if (e.keyCode == 37)//arrow left
    {
        e.preventDefault();
        $("#ui-datepicker-div").show();
        $("#PurchaseOrderId").focus();
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
        $("#PurchaseOrderId").focus();
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

function IsValid() {
    var validated = true;
    if (!$('#PurchaseOrderDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
        return false;
    }
    if (!$('#SupplierId').val()) {
        DisplayMessage('error', 'Supplier is required')
        return false;
    }
    $rows = $('#trans_details tbody tr:visible');
    if ($rows.length === 1) {
        $rows.each(function (index, value) {
            if (!$(this).find('.ProductId').val()) {
                $(this).find('[id $= item_no_lbl]').css('color', 'red');
                $(this).find('[id $= item_no_lbl]').show().text('Select Product').delay(2000).fadeOut();
                validated = false;
                return false;
            }
        });
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

