
function IsValid() {
    var validated = true;
    if (!$('#SaleInvoice_SaleInvoiceDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
        return false;
    }
    if (!$('#SaleInvoice_ClientId').val()) {
        DisplayMessage('error', 'Client is required')
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

function SubmitForm(command) {
    $.ajax({
        url: '/SM/Sale/AddEditSaleInvoice',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                changeHref(data.SaleInvoiceId);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                assignPrintValues(data.Invoice, data.Detail);
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.SaleInvoiceId) {
                    $('#SaleInvoice_SaleInvoiceId').val(data.SaleInvoiceId);
                    $('#InvoiceNo').val(data.SaleInvoiceId);
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
                        DisplayMessage('success', 'Successfully updated');
                    }
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
function SubmitPayment(command) {
    $.ajax({
        url: '/SM/Sale/AddEditSaleInvoice',
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
            DisplayMessage('error', errorThrown);
        }
    });
}

function FillForm() {
    var ex = new Object();

    var SaleInvoice = new Object();
    SaleInvoice.SaleInvoiceId = $('#SaleInvoice_SaleInvoiceId').val();
    SaleInvoice.SaleInvoiceDate = $('#SaleInvoice_SaleInvoiceDate').val();
    SaleInvoice.ClientId = $('#SaleInvoice_ClientId').val();
    SaleInvoice.Description = $('#SaleInvoice_Description').val();
    SaleInvoice.DealingPerson = $('#SaleInvoice_DealingPerson').val();
    SaleInvoice.PhoneNo = $('#SaleInvoice_PhoneNo').val();
    SaleInvoice.Address = $('#SaleInvoice_Address').val();
    SaleInvoice.OtherCharges = $('#SaleInvoice_OtherCharges').val();
    SaleInvoice.LabourCharges = $('#SaleInvoice_LabourCharges').val();
    SaleInvoice.FareCharges = $('#SaleInvoice_FareCharges').val();
    SaleInvoice.Discount = $('#SaleInvoice_Discount').val();
    SaleInvoice.DiscountPer = $('#SaleInvoice_DiscountPer').val();
    SaleInvoice.OtherDiscount = $('#SaleInvoice_OtherDiscount').val();
    SaleInvoice.NetTotal = $('#SaleInvoice_NetTotal').val();
    ex.SaleInvoice = SaleInvoice;

    var ClientInvoicePayment = new Object();
    ClientInvoicePayment.SaleInvoiceId = $('#ClientInvoicePayment_SaleInvoiceId').val();
    ClientInvoicePayment.Amount = $('#ClientInvoicePayment_Amount').val();
    ClientInvoicePayment.Description = $('#ClientInvoicePayment_Description').val();
    ex.ClientInvoicePayment = ClientInvoicePayment;

    var SaleInvoiceProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var SaleInvoiceProducts = new Object();
            SaleInvoiceProducts.SaleInvoiceId = $('#SaleInvoice_SaleInvoiceId').val();
            SaleInvoiceProducts.SaleInvoiceProductId = $(this).find('[id $= SaleInvoiceProductId]').val();
            SaleInvoiceProducts.ProductId = $(this).find('.ProductId').val();
            SaleInvoiceProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            SaleInvoiceProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            SaleInvoiceProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            SaleInvoiceProducts.CalLength = $(this).find('[id $= CalLength]').val();
            SaleInvoiceProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            SaleInvoiceProducts.Sheets = $(this).find('[id $= Sheets]').val();
            SaleInvoiceProducts.Quantity = $(this).find('[id $= Quantity]').val();
            SaleInvoiceProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            SaleInvoiceProducts.UnitPrice = $(this).find('[id $= UnitPrice]').val();
            SaleInvoiceProducts.LineTotal = $(this).find('[id $= LineTotal]').val();
            SaleInvoiceProducts.Discount = $(this).find('[id $= Discount]').val();
            SaleInvoiceProducts.Tax = $(this).find('[id $= Tax]').val();
            SaleInvoiceProducts.NetTotal = $(this).find('[id $= NetTotal]').val();
            SaleInvoiceProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            SaleInvoiceProduct.push(SaleInvoiceProducts);
        }
    });

    ex.SaleInvoiceProduct = SaleInvoiceProduct;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= SaleInvoiceProductId]').val() === 0) {
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
            //amount = StringToFloat(detail[index].Credit).toFixed(2);
            //if (detail[index].TransactionType == "Cr") {
            //    $(this).find('[id $= Credit]').val(StringToFloat(detail[index].Credit).toFixed(2));
            //    $(this).find('[id $= Debit]').val(0);
            //} else {
            //    $(this).find('[id $= Credit]').val(0);
            //    $(this).find('[id $= Debit]').val(StringToFloat(detail[index].Debit).toFixed(2));
            //}
            $(this).find('[id $= SaleInvoiceProductId]').val(detail[index].SaleInvoiceProductId);
            $(this).find('[id $= SaleInvoiceId]').val(detail[index].SaleInvoiceId);
            //$(this).find('[id $= AccountId]').val(detail[index].AccountId);
            //$(this).find('[id $= ChequeNo]').val(detail[index].ChequeNo);
            //$(this).find('[id $= ChequeDate]').val(detail[index].ChequeDate);
            //$(this).find('[id $= Narration]').val(detail[index].Narration);
            //$(this).find('[id $= CostGroupId]').val(detail[index].CostGroupId);
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
    $('#ftNetTotal').val(nettotal.toFixed(2));
    $('#SaleInvoice_TotalAmount').val(nettotal.toFixed(2));
    var billTotal = (nettotal
        + StringToFloat($('#SaleInvoice_OtherCharges').val())
        + StringToFloat($('#SaleInvoice_LabourCharges').val())
        + StringToFloat($('#SaleInvoice_FareCharges').val()))
        - (StringToFloat($('#SaleInvoice_Discount').val()) + StringToFloat($('#SaleInvoice_OtherDiscount').val()));
    $('#topTotal').text(billTotal.toFixed(2));
    $('#SaleInvoice_NetTotal').val(billTotal.toFixed(2));
    $('#SaleInvoice_Balance').val(StringToFloat($('#SaleInvoice_NetTotal').val()) - StringToFloat($('#SaleInvoice_Received').val()));

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
    if (id === "btnSavePayment") {
        if (IsValid()) {
            if (StringToFloat($('#ClientInvoicePayment_Amount').val()) > StringToFloat($('#tNetBalance').text())) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Amount must be less than balance amount').show().delay(3000).fadeOut();
                $('#ClientInvoicePayment_Amount').val($('#tNetBalance').text());
                $('#ClientInvoicePayment_Amount').focus();
            }
            else {
                SubmitPayment('savePayment');
            }
        }
    }
    if (id === "lbPrintVoucher") {
        if ($('#SaleInvoice_SaleInvoiceId').val() > 0) {
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
        if ($('#SaleInvoice_SaleInvoiceId').val() > 0) {
            $("#AddPaymentModal").modal();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }
    return false;
});

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

function LoadPayments(Invoice, type) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { InvoiceNo: Invoice, Type: type },
        contentType: 'application/json',
        url: "/SM/Sale/LoadInvoicePayments",
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

$(".Barcode").enterKey(function (e) {
    var that = this;
    e.preventDefault();
    if ($(this).val()) {
        $.ajax({
            url: '/SM/Sale/ProductIdByBarcode',
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

$('.ClientId').change(function () {
    $('#SaleInvoice_Description').focus().select();
})

function ProductDetailsById(row, idd) {
    if (idd) {
        $.ajax({
            url: '/SM/Sale/ProductDetailById',
            type: 'post',
            data: { id: idd },
            success: function (data) {
                if (data.IsSuccess === true) {
                    ClearRow(row);
                    $(row).find('[id $= UnitPrice]').val(data.Data.SalePrice);
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

$(".width,.length,.Sheets,.unitPrice,.tax,.Quantity").bind("change keyup", function (event) {
    var row = $(this).closest('tr');
    CalculateRowTotal(row);
})

$(".charges,.OtherDiscount").bind("change keyup", function (event) {
    CalculateTotal();
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
    LoadPayments($('#SaleInvoice_SaleInvoiceId').val(), "Sale");
    $('#ClientInvoicePayment_InvoiceId').val($('#SaleInvoice_SaleInvoiceId').val());
    $('#ClientInvoicePayment_Type').val('Sale');
    $('#myModalLabel').text('Receive Payment for (' + $('#SaleInvoice_SaleInvoiceId').val() + ')');
    $('#ClientInvoicePayment_BillTotal').val($('#SaleInvoice_NetTotal').val());
    $('#ClientInvoicePayment_Amount').val($('#tNetBalance').text());
    $('#ClientInvoicePayment_Amount').focus();

});
$('#AddPaymentModal').on('hidden.bs.modal', function () {
});

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
        $('#tOtherDiscount').text(StringToFloat(Invoice.OtherDiscount).toFixed(2));
        $('#tDiscount').text(StringToFloat(Invoice.Discount).toFixed(2));
        $('#tNetTotal').text(StringToFloat(Invoice.NetTotal).toFixed(2));
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#tNetBalance').text(StringToFloat(Invoice.NetBalance).toFixed(2));
        $('#tAddress').text('').append('<b>Address: ' + StringIsNull(Invoice.Address) + '</b>');
        $('#tDealingPerson').text('').append('<b>Client Name</b> ' + StringIsNull(Invoice.DealingPerson));;
        $('#tPhoneNo').text('').append('<b>Phone No: </b>' + StringIsNull(Invoice.PhoneNo));
        $('#tClientName').text('').append('<b>To</b> ' + Invoice.ClientName);
        $('#tInvoiceNo').text('').append('Sale Invoice # ' + Invoice.SaleInvoiceId);
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#SaleInvoice_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        $('#SaleInvoice_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));

        //$('#ClientInvoicePayment_Amount').val(StringToFloat(Invoice.NetBalance).toFixed(2));
    }
}

$(".discountper").bind("change keyup", function (event) {
    $('.flatdiscount').val(((StringToFloat($('#ftNetTotal').val()) * StringToFloat($('.discountper').val())) / 100));
    CalculateTotal();
})

$(".flatdiscount").bind("change keyup", function (event) {
    $('.discountper').val(((StringToFloat($('.flatdiscount').val()) * 100) / StringToFloat($('#ftNetTotal').val())));
    CalculateTotal();
})
function StringIsNull(value) {
    if (value) {
        return value;
    }
    else {
        return '';
    }
}
function ValidateFields(val) {
    if (val == null || val == "") {
        return false;
    }
    return true;
}