
function IsValid() {
    var validated = true;
    if (!$('#Request_RequestDate').val()) {
        DisplayMessage('error', 'Date is required')
        $('#Request_RequestDate').focus();
        validated = false;
        return false;
    }
    if (!$('#Request_StatusId').val()) {
        DisplayMessage('error', 'Status is required')
        $('#Request_StatusId').select2('open');
        validated = false;
        return false;
    }
    $rows = $('#trans_details tbody tr:visible');
    if ($rows.length === 1) {
        $rows.each(function (index, value) {
            if (!$(this).find('.ItemId').val()) {
                DisplayMessage('error', 'Select Item')
                $(this).find('.ItemId').select2('open');
                validated = false;
                return false;
            }
        });
    }
    //$rows.each(function (index, value) {
    //    if (!$(this).find('[id $= RoomId]').val()) {
    //        DisplayMessage('error', 'Select WareHouse')
    //        $(this).find('[id $= RoomId]').select2('open');
    //        validated = false;
    //        return false;
    //    }
    //});
    return validated;
}

function SubmitForm(command) {
    $.ajax({
        url: '/AM/Requests/AddEditRequest',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            // debugger;
            if (data.IsSuccess) {

                console.log('Status', data.Invoice.StatusId);
                changeHref(data.RequestId);
                $('#lblRequestid').html('Request No: ' + data.RequestId);
                HideTopControls(false);

                RemoveEmptyRows();
                assignPrintValues(data.Invoice, data.Detail);
                OnResponceBackFromServer(data);
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.RequestId) {
                    $('#Request_RequestId').val(data.RequestId);
                    $('#InvoiceNo').val(data.RequestId);
                }
                if (data.Case === "PostVoucher") {
                    if (data.IsPosted) {
                        HideButtons(true);
                        $('#lbUnPostVoucher').closest('li').removeClass('hidden');
                        $('#lbPostVoucher').closest('li').addClass('hidden');
                        DisplayMessage('success', 'Invoice Posted Successfully');
                    }
                }
                else if (data.Case === "UnPostVoucher") {
                    if (!data.IsPosted) {
                        HideButtons(false);
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                        DisplayMessage('success', 'Invoice Unposted Successfully');
                    }
                }
                else if (data.Case === "CancelVoucher") {
                    if (data.IsCancelled) {
                        HideButtons(true);
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                        $('#lbUnCancelVoucher').closest('li').removeClass('hidden');
                        $('#lbCancelVoucher').closest('li').addClass('hidden');
                        DisplayMessage('success', 'Invoice Cancelled Successfully');
                    }
                }
                else if (data.Case === "UnCancelVoucher") {
                    if (!data.IsCancelled) {
                        HideButtons(false);
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                        DisplayMessage('success', 'Invoice Uncancelled Successfully');
                    }
                }
                else if (data.Case === "savePayment") {
                    var modal = $('#AddPaymentModal');
                    $(modal).find('[id $= item_no_lbl]').css('color', 'green');
                    $(modal).find('[id $= item_no_lbl]').text('Saved Successfully').show().delay(2000).fadeOut();
                }
                else if (data.Case === "savePurchaseOrder") {
                    if (data.Invoice.PurchaseOrder != undefined) {
                        if (data.Invoice.PurchaseOrder.PurchaseOrderId != 0) {
                            $('#extendedPageHeader p').append("<label class=\"label label-blue \" style=\"cursor: pointer;\"  onclick = \"changeRef(" + data.Invoice.PurchaseOrder.PurchaseOrderId + ")\" title = \"Go to Order\" > Order No: " + data.Invoice.PurchaseOrder.PurchaseOrderId + "</label > ");
                        }
                        //   console.log("Order", JSON.parse(data.Invoice));

                    }
                    $('#convertPurchaseOrder').closest('li').addClass('hidden');

                }
                else if (data.Case === 'InsertUpdate') {
                    if (data.Invoice.StatusId === 3) {

                        $('#convertPurchaseOrder').closest('li').removeClass('hidden');
                    } else {
                        $('#convertPurchaseOrder').closest('li').addClass('hidden');
                    }
                }
                else if (data.Case === "submitButton") {
                    $('#convertPurchaseOrder').closest('li').removeClass('hidden');
                }
                else {
                    if (!data.IsPosted) {
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                    }
                    if (!data.IsCancelled) {
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                    }
                    if (ValidateFields(data.ErrorMsg)) {
                        DisplayMessage('error', data.ErrorMsg);
                    } else {
                        if (ValidateFields(data.SuccessMsg))
                            DisplayMessage('success', data.SuccessMsg);
                        else
                            DisplayMessage('success', 'Successfully updated');
                    }
                }

            }
            else {
                DisplayMessage('error', 'Updation failed');
            }
        }
    });
}

function FillForm() {
    var ex = new Object();

    var Request = new Object();
    Request.RequestId = $('#Request_RequestId').val();
    Request.RequestDate = $('#Request_RequestDate').val();
    Request.Description = $('#Request_Description').val();
    Request.StatusId = $('#Request_StatusId').val();
    Request.EmployeeId = $('#Request_EmployeeId').val();
    Request.DepartmentId = $('#Request_DepartmentId').val();

    ex.Request = Request;

    var PurchaseOrder = new Object();
    PurchaseOrder.Description = $('#PurchaseOrder_Description').val();
    PurchaseOrder.SupplierId = $('#PurchaseOrder_SupplierId').val();
    ex.PurchaseOrder = PurchaseOrder;

    var RequestDetail = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ItemId').val();
        if ($(this).find('.ItemId').val()) {
            var RequestDetails = new Object();
            RequestDetails.RequestId = $('#Request_RequestId').val();
            RequestDetails.RequestDetailId = $(this).find('[id $= RequestDetailId]').val();
            RequestDetails.ProductId = $(this).find('.ItemId').val();
            RequestDetails.Quantity = $(this).find('[id $= Quantity]').val();
            RequestDetails.Description = $(this).find('[id $= Description]').val();
            RequestDetails.RoomId = $(this).find('[id $= RoomId]').val();
            RequestDetails.ConditionTypeId = $(this).find('[id $= ConditionTypeId]').val();
            RequestDetail.push(RequestDetails);
        }
    });

    ex.RequestDetail = RequestDetail;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= RequestItemId]').val() === 0) {
            if ($(this).next('tr').length) {
                $(this).remove();
            }
        }
    });
}

function SetDetailRows(detail) {
    var count = detail.length;
    var amount = 0;
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if (index < count) {
            $(this).find('[id $= RequestDetailId]').val(detail[index].RequestDetailId);
            $(this).find('[id $= RequestId]').val(detail[index].RequestId);
        }
    });
}

jQuery('.voucherLink').click(function (event) {
    var id = $(this).attr("id");
    if (id === "lbUnPostVoucher") {
        if (IsValid()) {
            SubmitForm('UnPostVoucher');
        }
    }
    if (id === "lbPostVoucher") {
        if (IsValid()) {
            SubmitForm('PostVoucher');
        }
    }
    if (id === "lbUnCancelVoucher") {
        if (IsValid()) {
            SubmitForm('UnCancelVoucher');
        }
    }
    if (id === "lbCancelVoucher") {
        if (IsValid()) {
            SubmitForm('CancelVoucher');
        }
    }
    if (id === "submitButton") {
        if (IsValid()) {
            SubmitForm('InsertUpdate');
        }
    }
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

    if (id === "lbReceivePayment") {
        if ($('#Request_RequestId').val() > 0) {
            $("#AddPaymentModal").modal();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }
    if (id === "convertPurchaseOrder") {
        if ($('#Request_RequestId').val() > 0) {
            LoadPurchaseOrder($('#Request_RequestId').val());

        } else {
            DisplayMessage('error', 'Request id not found');
        }
    }
    if (id === "btnPurchaseOrder") {
        if (IsValid()) {
            if (!$('#PurchaseOrder_SupplierId').val()) {
                $('#ServiceModal').find('[id $= item_no_lbl]').css('color', 'red').text('Supplier is required').show().delay(3000).fadeOut();
                $('#PurchaseOrder_SupplierId').select2('open');
            }
            else {
                SubmitForm('savePurchaseOrder');
            }
        }
    }
    return false;
});

function PrintInvoice() {
    if ($('#Request_RequestId').val() > 0) {
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

function HideButtons(val) {
    if (val) {
        $('[data-action $= Delete]').each(function (index, value) {
            var id = $(this).data("id");
            if (id !== -1) {
                $(this).addClass('hidden');
            }
        });
        $("#submitButton").closest('li').addClass('hidden');
    } else {
        $('[data-action $= Delete]').each(function (index, value) {
            var id = $(this).data("id");
            if (id !== -1) {
                $(this).removeClass('hidden');
            }
        });
        $("#submitButton").closest('li').removeClass('hidden');
    }
}

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

$('.ItemId').change(function () {
    ProductDetailsById($(this).closest('tr'), $(this).val());
})
function ProductDetailsById(row, idd) {
    $(row).find('[id $= Quantity]').val('1').focus().select();
}

$("#InvoiceNo").enterKey(function (e) {
    e.preventDefault();
    submitForm('LoadInvoice');
})

function submitForm(val) {
    $('form').append("<input type='hidden' name='Command' value='" + val + "' />");
    $('form').submit();
    $('form').find("<input type='hidden' name='Command' value='" + val + "' />").remove();
    return true;
}

function assignPrintValues(Invoice, InvoiceDetail) {
    if (InvoiceDetail) {
        var data = '';
        var count = 0, Quantity = 0;
        $.each(InvoiceDetail, function (e) {
            count++;
            Quantity += this.Quantity;
            data += "<tr class='reAssignable'>"
                + "<td>" + count + "</td>"
                + "<td>" + this.ItemName + "</td>"
                + "<td>" + StringToFloat(this.Quantity).toFixed(2) + "</td>"
                + "<td>" + this.Description + "</td>"
                + "<td>" + this.RoomId + "</td>"
                + "</tr>";
        });

        if (InvoiceDetail.length > 0) {
            $("#maintable tbody tr.reAssignable").html("");
            $("#invoiceFooter").before(data);
        }
    }
    if (Invoice) {
        $('#convertPurchaseOrder').closest('li').removeClass('hidden');
        $('#tInvoiceNo').html('<b>Request # ' + Invoice.RequestId + '</b>');
        $('#tQuantity').val(Quantity);
        $('#tDescription').val(Invoice.Description);
    }
}
function LoadPurchaseOrder(Invoice) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { InvoiceNo: Invoice },
        contentType: 'application/json',
        url: "/AM/Requests/LoadPurchaseOrder",
        success: function (result) {
            if (result) {
                if (StringToFloat(result) > 0) {
                    window.location.href = "/AM/PurchaseOrder/AddEditPurchaseOrder/" + result;
                    return;
                } else {
                    $('#ClientService_Description').focus();
                    $("#ServiceModal").modal();
                }
            } else {
                $('#ClientService_Description').focus();
                $("#ServiceModal").modal();
            }
        }
    })
};