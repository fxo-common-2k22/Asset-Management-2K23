
function IsValid() {
    var validated = true;
    if (!$('#SaleReturn_SaleReturnDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
        return false;
    }
    if (!$('#SaleReturn_ClientId').val()) {
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
        url: '/SM/Sale/AddEditSaleReturn',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                changeHref(data.SaleReturnId);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                assignPrintValues(data.Invoice, data.Detail);
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.SaleReturnId) {
                    $('#SaleReturn_SaleReturnId').val(data.SaleReturnId);
                    $('#InvoiceNo').val(data.SaleReturnId);
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
                //DisplayMessage('error', jqXHR.responseText);
            }
            else
                DisplayMessage('error', errorThrown);
        }
    });
}

function SubmitPayment(command) {
    $.ajax({
        url: '/SM/Sale/AddEditSaleReturn',
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

    var SaleReturn = new Object();
    SaleReturn.SaleReturnId = $('#SaleReturn_SaleReturnId').val();
    SaleReturn.SaleReturnDate = $('#SaleReturn_SaleReturnDate').val();
    SaleReturn.ClientId = $('#SaleReturn_ClientId').val();
    SaleReturn.Description = $('#SaleReturn_Description').val();
    SaleReturn.DealingPerson = $('#SaleReturn_DealingPerson').val();
    SaleReturn.PhoneNo = $('#SaleReturn_PhoneNo').val();
    SaleReturn.Address = $('#SaleReturn_Address').val();
    SaleReturn.OtherCharges = $('#SaleReturn_OtherCharges').val();
    SaleReturn.LabourCharges = $('#SaleReturn_LabourCharges').val();
    SaleReturn.FareCharges = $('#SaleReturn_FareCharges').val();
    SaleReturn.Discount = $('#SaleReturn_Discount').val();
    SaleReturn.NetTotal = $('#SaleReturn_NetTotal').val();
    ex.SaleReturn = SaleReturn;

    var ClientRefundInvoice = new Object();
    ClientRefundInvoice.SaleInvoiceId = $('#ClientRefundInvoice_SaleInvoiceId').val();
    ClientRefundInvoice.Amount = $('#ClientRefundInvoice_Amount').val();
    ClientRefundInvoice.Description = $('#ClientRefundInvoice_Description').val();
    ex.ClientRefundInvoice = ClientRefundInvoice;

    var SaleReturnProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var SaleReturnProducts = new Object();
            SaleReturnProducts.SaleReturnId = $('#SaleReturn_SaleReturnId').val();
            SaleReturnProducts.SaleReturnProductId = $(this).find('[id $= SaleReturnProductId]').val();
            SaleReturnProducts.ProductId = $(this).find('.ProductId').val();
            SaleReturnProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            SaleReturnProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            SaleReturnProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            SaleReturnProducts.CalLength = $(this).find('[id $= CalLength]').val();
            SaleReturnProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            SaleReturnProducts.Sheets = $(this).find('[id $= Sheets]').val();
            SaleReturnProducts.Quantity = $(this).find('[id $= Quantity]').val();
            SaleReturnProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            SaleReturnProducts.UnitPrice = $(this).find('[id $= UnitPrice]').val();
            SaleReturnProducts.LineTotal = $(this).find('[id $= LineTotal]').val();
            SaleReturnProducts.Discount = $(this).find('[id $= Discount]').val();
            SaleReturnProducts.Tax = $(this).find('[id $= Tax]').val();
            SaleReturnProducts.NetTotal = $(this).find('[id $= NetTotal]').val();
            SaleReturnProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            SaleReturnProduct.push(SaleReturnProducts);
        }
    });

    ex.SaleReturnProduct = SaleReturnProduct;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= SaleReturnProductId]').val() === 0) {
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
            $(this).find('[id $= SaleReturnProductId]').val(detail[index].SaleReturnProductId);
            $(this).find('[id $= SaleReturnId]').val(detail[index].SaleReturnId);
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
    $('#SaleReturn_TotalAmount').val(nettotal.toFixed(2));
    var billTotal = (nettotal + StringToFloat($('#SaleReturn_OtherCharges').val()) + StringToFloat($('#SaleReturn_LabourCharges').val()) + StringToFloat($('#SaleReturn_FareCharges').val())) - StringToFloat($('#SaleReturn_Discount').val());
    $('#topTotal').text(billTotal.toFixed(2));
    $('#SaleReturn_NetTotal').val(billTotal.toFixed(2));
    $('#SaleReturn_Balance').val(StringToFloat($('#SaleReturn_NetTotal').val()) - StringToFloat($('#SaleReturn_Received').val()));

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
            if (StringToFloat($('#ClientRefundInvoice_Amount').val()) > StringToFloat($('#tNetBalance').text())) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Amount must be less than balance amount').show().delay(3000).fadeOut();
                $('#ClientRefundInvoice_Amount').val($('#tNetBalance').text());
                $('#ClientRefundInvoice_Amount').focus();
            }
            else {
                SubmitPayment('savePayment');
            }
        }
    }
    if (id === "lbPrintVoucher") {
        if ($('#SaleReturn_SaleReturnId').val() > 0) {
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
        if ($('#SaleReturn_SaleReturnId').val() > 0) {
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
        url: "/SM/Sale/LoadSaleReturnInvoicePayments",
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

$(".width,.length,.Sheets,.Quantity").bind("change keyup", function (event) {
    var row = $(this).closest('tr');
    CalculateRowTotal(row);
})

$(".charges,.flatdiscount").bind("change keyup", function (event) {
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
    LoadPayments($('#SaleReturn_SaleReturnId').val(), "Sale");
    $('#ClientRefundInvoice_InvoiceId').val($('#SaleReturn_SaleReturnId').val());
    $('#ClientRefundInvoice_Type').val('Sale');
    $('#myModalLabel').text('Receive Payment for (' + $('#SaleReturn_SaleReturnId').val() + ')');
    $('#ClientRefundInvoice_BillTotal').val($('#SaleReturn_NetTotal').val());
    $('#ClientRefundInvoice_Amount').val($('#tNetBalance').text());
    $('#ClientRefundInvoice_Amount').focus();

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
        $('#tDiscount').text(StringToFloat(Invoice.Discount).toFixed(2));
        $('#tNetTotal').text(StringToFloat(Invoice.NetTotal).toFixed(2));
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#tNetBalance').text(StringToFloat(Invoice.NetBalance).toFixed(2));
        $('#tAddress').text('').append('<b>Address: ' + StringIsNull(Invoice.Address) + '</b>');
        $('#tDealingPerson').text('').append('<b>Client Name</b> ' + StringIsNull(Invoice.DealingPerson));;
        $('#tPhoneNo').text('').append('<b>Phone No: </b>' + StringIsNull(Invoice.PhoneNo));
        $('#tClientName').text('').append('<b>To</b> ' + Invoice.ClientName);
        $('#tInvoiceNo').text('').append('<b>Sale Return Invoice # ' + Invoice.SaleReturnId + '</b>');
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#SaleReturn_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        $('#SaleReturn_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));

        //$('#ClientRefundInvoice_Amount').val(StringToFloat(Invoice.NetBalance).toFixed(2));
    }
}