
function IsValid() {
    var validated = true;
    if (!$('#PurchaseReturn_PurchaseReturnDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
        return false;
    }
    if (!$('#PurchaseReturn_SupplierId').val()) {
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
    return validated;
}

function SubmitForm(command) {
    $.ajax({
        url: '/AM/Purchase/PostPurchaseReturn',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
               
                changeHref(data.PurchaseReturnId);
                assignPrintValues(data.Invoice, data.Detail);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.PurchaseReturnId) {
                    $('#PurchaseReturn_PurchaseReturnId').val(data.PurchaseReturnId);
                    $('#InvoiceNo').val(data.PurchaseReturnId);
                    $('#PurchaseInvoiceLabel').text(data.PurchaseReturnId);
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
                else {
                    if (!data.IsPosted) {
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                    }
                    if (!data.IsCancelled) {
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                    }
                    DisplayMessage('success', 'Successfully updated');
                }
            }
            else {
                DisplayMessage('error', 'Updation failed');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            RemoveEmptyRows();
            if (jqXHR.responseText) {
                $("html").html(jqXHR.responseText);
            }
            else
                DisplayMessage('error', errorThrown);
        }
    });
}

function FillForm() {
    var ex = new Object();

    var PurchaseReturn = new Object();
    PurchaseReturn.PurchaseReturnId = $('#PurchaseReturn_PurchaseReturnId').val();
    PurchaseReturn.PurchaseReturnDate = $('#PurchaseReturn_PurchaseReturnDate').val();
    PurchaseReturn.SupplierId = $('#PurchaseReturn_SupplierId').val();
    PurchaseReturn.Description = $('#PurchaseReturn_Description').val();
    PurchaseReturn.OtherCharges = 0;
    PurchaseReturn.LabourCharges = 0;
    PurchaseReturn.FareCharges = 0;
    PurchaseReturn.TotalAmount = $('#ftNetTotal').val();
    PurchaseReturn.NetTotal = $('#ftNetTotal').val();
    ex.PurchaseReturn = PurchaseReturn;

    var SupplierRefundInvoice = new Object();
    SupplierRefundInvoice.PurchaseReturnId = $('#SupplierRefundInvoice_PurchaseReturnId').val();
    SupplierRefundInvoice.Amount = $('#SupplierRefundInvoice_Amount').val();
    SupplierRefundInvoice.Description = $('#SupplierRefundInvoice_Description').val();
    ex.SupplierRefundInvoice = SupplierRefundInvoice;

    var PurchaseReturnProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var PurchaseReturnProducts = new Object();
            PurchaseReturnProducts.PurchaseReturnId = $('#PurchaseReturn_PurchaseReturnId').val();
            PurchaseReturnProducts.PurchaseReturnProductId = $(this).find('[id $= PurchaseReturnProductId]').val();
            PurchaseReturnProducts.ItemId = $(this).find('.ProductId').val();
            PurchaseReturnProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            PurchaseReturnProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            PurchaseReturnProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            PurchaseReturnProducts.CalLength = $(this).find('[id $= CalLength]').val();
            PurchaseReturnProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            PurchaseReturnProducts.Sheets = $(this).find('[id $= Sheets]').val();
            PurchaseReturnProducts.Quantity = $(this).find('[id $= Quantity]').val();
            PurchaseReturnProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            PurchaseReturnProducts.UnitPrice = $(this).find('[id $= UnitPrice]').val();
            PurchaseReturnProducts.LineTotal = $(this).find('[id $= LineTotal]').val();
            PurchaseReturnProducts.Discount = $(this).find('[id $= Discount]').val();
            PurchaseReturnProducts.Tax = $(this).find('[id $= Tax]').val();
            PurchaseReturnProducts.NetTotal = $(this).find('[id $= NetTotal]').val();
            PurchaseReturnProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            PurchaseReturnProducts.ConditionTypeId = $(this).find('[id $= ConditionTypeId]').val();
            PurchaseReturnProduct.push(PurchaseReturnProducts);
        }
    });

    ex.PurchaseReturnProduct = PurchaseReturnProduct;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= PurchaseReturnProductId]').val() === 0) {
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
            $(this).find('[id $= PurchaseReturnProductId]').val(detail[index].PurchaseReturnProductId);
            $(this).find('[id $= PurchaseReturnId]').val(detail[index].PurchaseReturnId);
        }
    });
}

function CalculateTotal() {
    $rows = $('#trans_details tbody tr:visible');
    var discount = 0, tax = 0, nettotal = 0;
    $rows.each(function (index, value) {
        discount += StringToFloat($(this).find('[id $= Discount]').val());
        tax += StringToFloat($(this).find('[id $= Tax]').val());
        nettotal += StringToFloat($(this).find('[id $= NetTotal]').val());
    });
    $('#ftDiscount').val(discount.toFixed(2));
    $('#ftTax').val(tax.toFixed(2));
    $('#PurchaseReturn_TotalAmount').val(nettotal.toFixed(2));
    var billTotal = (nettotal + StringToFloat($('#PurchaseReturn_OtherCharges').val()) + StringToFloat($('#PurchaseReturn_LabourCharges').val()) + StringToFloat($('#PurchaseReturn_FareCharges').val())) - StringToFloat($('#PurchaseReturn_Discount').val());
    $('#ftNetTotal').val(nettotal.toFixed(2));
    $('#topTotal').text(billTotal.toFixed(2));
    $('#PurchaseReturn_NetTotal').val(billTotal.toFixed(2));
    $('#PurchaseReturn_Balance').val(StringToFloat($('#PurchaseReturn_NetTotal').val()) - StringToFloat($('#PurchaseReturn_Received').val()));    

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
        if ($('#PurchaseReturn_PurchaseReturnId').val() > 0) {
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
    if (id === "lbUnPrintVoucher") {
        $('#mainsection').show();
        $('#printsection').hide();
        $('#lbUnPrintVoucher').hide();
        HideTopControls(false);
    }
    if (id === "lbReceivePayment") {
        if ($('#PurchaseReturn_PurchaseReturnId').val() > 0) {
            $("#AddPaymentModal").modal();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }
    if (id === "btnSavePayment") {
        if (IsValid()) {
            if (StringToFloat($('#SupplierRefundInvoice_Amount').val()) > StringToFloat($('#tNetBalance').text())) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Amount must be less than balance amount').show().delay(3000).fadeOut();
                $('#SupplierRefundInvoice_Amount').val($('#tNetBalance').text());
                $('#SupplierRefundInvoice_Amount').focus();
            }
            else {
                SubmitPayment('savePayment');
            }
        }
    }
    return false;
});

function SubmitPayment(command) {
    $.ajax({
        url: '/AM/Purchase/PostPurchaseReturn',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            var modal = $('#AddPaymentModal');
            if (data.IsSuccess) {
                assignPrintValues(data.Invoice, data.Detail);
                BindReceiptList(data.Receipts);
                $(modal).find('[id $= item_no_lbl]').css('color', 'green');
                $(modal).find('[id $= item_no_lbl]').text('Payment Saved').show().delay(3000).fadeOut();
            }
            else {
                $(modal).find('[id $= item_no_lbl]').css('color', 'red');
                $(modal).find('[id $= item_no_lbl]').text('Updation failed').show().delay(3000).fadeOut();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            RemoveEmptyRows();
            if (jqXHR.responseText) {
                $("html").html(jqXHR.responseText);
            }
            else
                DisplayMessage('error', errorThrown);
        }
    });
}

function LoadPayments(Invoice) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { InvoiceNo: Invoice },
        contentType: 'application/json',
        url: "/AM/Purchase/LoadPurchaseReturnInvoicePayments",
        success: function (result) {
            BindReceiptList(result);
        }
    })
};

function BindReceiptList(d) {
    if (d) {
        var data = '';
        $('#AddPaymentModal table tbody').html("");
        $.each(d, function (e) {
            if (!this.Description)
                this.Description = "";
            if (StringIsNullOrEmpty(this.VoucherId))
                td = '<td><span title="Voucher should be shown after posting" style="color:#ffbc00;"><i class="fa fa-warning"></i></span></td>';
            else
                td = "<td><a title='View Voucher' href='/Finance/VoucherAndReceipt/ViewVoucher/" + this.VoucherId + "'>" + this.VoucherId + "</a></td>";

            data += "<tr>" + td
                + "<td>" + this.Amount + "</td>"
                + "<td>" + this.Date + "</td>"
                + "<td>" + this.Description + "</td>"
                + "</tr>";
        });

        if (d.length === 0) {
            $('.EmptyData').show();
        } else {
            $("#AddPaymentModal table tbody").append(data);
        }
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

function assignPrintValues(Invoice, InvoiceDetail) {
    if (InvoiceDetail) {
        var data = '';
        var count = 0, LineTotal = 0;
        $.each(InvoiceDetail, function (e) {
            count++;
            LineTotal += this.LineTotal;
            data += "<tr class='reAssignable'>"
                + "<td>" + count + "</td>"
                + "<td>" + this.ProductName + "</td>"
                + "<td>" + StringToFloat(this.CalLength).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.CalWidth).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.Sheets).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.SqFeet).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.UnitPrice).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.LineTotal).toFixed(2) + "</td>"
                + "</tr>";
        });

        if (InvoiceDetail.length > 0) {
            $("#maintable tbody tr.reAssignable").html("");
            $("#invoiceFooter").before(data);
        }
    }
    if (Invoice) {
        $('#tGrandTotal').text(StringToFloat(LineTotal).toFixed(2));
        $('#tOthercharges').text(StringToFloat(Invoice.OtherCharges).toFixed(2));
        $('#tDiscount').text(StringToFloat(Invoice.Discount).toFixed(2));
        $('#tNetTotal').text(StringToFloat(Invoice.NetTotal).toFixed(2));
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#tNetBalance').text(StringToFloat(Invoice.NetBalance).toFixed(2));
        $('#tClientName').text('');
        $('#tClientName').append('<b>To ' + Invoice.ClientName + '</b>');
        $('#tInvoiceNo').text('');
        $('#tInvoiceNo').append('<b>Purchase Return Invoice # ' + Invoice.PurchaseReturnId + '</b>');
        $('#PurchaseReturn_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        $('#PurchaseReturn_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));    

        //$('#Reciept_Amount').val(StringToFloat(Invoice.NetBalance).toFixed(2));
    }
}

$(".Barcode").enterKey(function (e) {
    var that = this;
    e.preventDefault();
    if ($(this).val()) {
        $.ajax({
            url: '/AM/Purchase/ProductIdByBarcode',
            type: 'post',
            data: { barcode: $(this).val() },
            success: function (data) {
                if (data) {
                    var d = $(that).closest('tr');
                    $(that).closest('tr').find('.ProductId').val(data).trigger('change');
                }
                else {
                    DisplayMessage('error', 'Product not found');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                DisplayMessage('error', 'Product not found');
            }
        });
    }
})

$('.ProductId').change(function () {
    ProductDetailsById($(this).closest('tr'), $(this).val());
})

function ProductDetailsById(row, idd) {
    if (idd) {
        $.ajax({
            url: '/AM/Purchase/ProductDetailById',
            type: 'post',
            data: { id: idd },
            success: function (data) {
                 
                if (data.IsSuccess == true) {
                    ClearRow(row);
                    $(row).find('[id $= UnitPrice]').val(data.Data);
                    $(row).find('[id $= Quantity]').focus().select();
                    CalculateRowTotal(row);
                } else {
                    DisplayMessage('error', 'Product not found');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                DisplayMessage('error', errorThrown);
            }
        });
    }
}

$('.width,.length,.Sheets,.Quantity,.UnitPrice').bind("change keyup", function (event) {
    var row = $(this).closest('tr');
    CalculateRowTotal(row);
})

$(".tax,.discount").bind("change keyup", function (event) {
    CalculateNetTotal($(this).closest('tr'));
})

function CalculateRowTotal(row) {
    $(row).find('[id $= LineTotal]').val((StringToFloat($(row).find('[id $= Quantity]').val()) * StringToFloat($(row).find('[id $= UnitPrice]').val())).toFixed(2));
    CalculateNetTotal(row);
    CalculateTotal();
}

function CalculateNetTotal(row) {
    $(row).find('[id $= NetTotal]').val(((StringToFloat($(row).find('[id $= LineTotal]').val()) + StringToFloat($(row).find('[id $= Tax]').val())) - StringToFloat($(row).find('[id $= Discount]').val())).toFixed(2));
}

function ClearRow(row) {
    $(row).find('[id $= UnitPrice]').val('0');
    $(row).find('[id $= CalDigit]').val('1');
    $(row).find('[id $= OrgWidth]').val('1');
    $(row).find('[id $= OrgLength]').val('1');
    $(row).find('[id $= CalWidth]').val('1');
    $(row).find('[id $= CalLength]').val('1');
    $(row).find('[id $= Quantity]').val('1');
    $(row).find('[id $= SqFeet]').val('1');
    $(row).find('[id $= Sheets]').val('1');
    $(row).find('[id $= LineTotal]').val('0');
    $(row).find('[id $= Discount]').val('0');
    $(row).find('[id $= Tax]').val('0');
    $(row).find('[id $= NetTotal]').val('0');
}

$('#AddPaymentModal').on('shown.bs.modal', function () {
    var modal = $(this);
    LoadPayments($('#PurchaseReturn_PurchaseReturnId').val());
    $('#SupplierRefundInvoice_InvoiceId').val($('#PurchaseReturn_PurchaseReturnId').val());
    $('#myModalLabel').text('Make Payment for (' + $('#PurchaseReturn_PurchaseReturnId').val() + ')');
    $('#SupplierRefundInvoice_BillTotal').val($('#PurchaseReturn_NetTotal').val());
    $('#SupplierRefundInvoice_Amount').val($('#tNetBalance').text());
    $('#SupplierRefundInvoice_Amount').focus();

});

$('#AddPaymentModal').on('hidden.bs.modal', function () {
});