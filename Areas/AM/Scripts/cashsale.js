var control, IsPrint = false;

function IsValid() {
    var validated = true;
    //if (!$('#PosSaleInvoice_SaleInvoiceDate').val()) {
    //    DisplayMessage('error', 'Invoice Date is required')
    //    return false;
    //}
    //if (!$('#PosSaleInvoice_ClientId').val()) {
    //    DisplayMessage('error', 'Client is required')
    //    return false;
    //}
    $rows = $('#trans_details tbody tr:visible');
    if ($rows.length === 0) {
        DisplayMessage('error', 'First add some items then save')
        return false;
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
function ValidateDropDown(val) {
    if (val == null || val == "" || val == "-1") {
        return false;
    }
    return true;
}
function SubmitForm(command) {
    $.ajax({
        url: '/SM/CashSale/AddEditCashSale',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                changeHref(data.SaleInvoiceId);
                CalculateTotal();
                SetDetailRows(data.Detail);
                assignPrintValues(data.Invoice, data.Detail);
                $('#tdCreation').text(data.Modification);
                if (data.SaleInvoiceId) {
                    $('#SMSaleInvoice_SaleInvoiceId').val(data.SaleInvoiceId);
                    HideTopControls(false);
                    //HideButtons(false);
                }
                if (ValidateFields(data.ErrorMsg)) {
                    GritterNotifiy('Error', data.ErrorMsg, null, false, 500);
                    //DisplayMessage('error', data.ErrorMsg);
                } else {
                    if (IsPrint)
                        PrintInvoice();
                    GritterNotifiy('Success', 'Updated Succssfully...', null, false, 500);
                    //DisplayMessage('success', 'Successfully updated');
                }
            }
            else {
                DisplayMessage('error', 'Updation failed');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
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

    var SMSaleInvoice = new Object();
    SMSaleInvoice.SaleInvoiceId = $('#SMSaleInvoice_SaleInvoiceId').val();
    SMSaleInvoice.SaleInvoiceDate = $('#PosSaleInvoice_SaleInvoiceDate').val();
    SMSaleInvoice.ClientId = $('#PosSaleInvoice_ClientId').val();
    SMSaleInvoice.Description = 'Cash CashSale';
    SMSaleInvoice.DealingPerson = $('#PosSaleInvoice_DealingPerson').val();
    SMSaleInvoice.PhoneNo = $('#PosSaleInvoice_PhoneNo').val();
    SMSaleInvoice.Address = $('#PosSaleInvoice_Address').val();
    SMSaleInvoice.OtherCharges = $('#PosSaleInvoice_OtherCharges').val();
    SMSaleInvoice.LabourCharges = $('#PosSaleInvoice_LabourCharges').val();
    SMSaleInvoice.FareCharges = $('#PosSaleInvoice_FareCharges').val();
    SMSaleInvoice.Discount = $('#PosSaleInvoice_Discount').val();
    SMSaleInvoice.DiscountPer = $('#PosSaleInvoice_DiscountPer').val();
    SMSaleInvoice.OtherDiscount = $('#PosSaleInvoice_OtherDiscount').val();
    SMSaleInvoice.ReceivedAmount = $('#PosSaleInvoice_ReceivedAmount').val();
    SMSaleInvoice.NetTotal = $('.nettotal').text();
    ex.SMSaleInvoice = SMSaleInvoice;

    var PosClientInvoicePayment = new Object();
    PosClientInvoicePayment.SaleInvoiceId = $('#SMSaleInvoice_SaleInvoiceId').val();
    PosClientInvoicePayment.Amount = $('#SMSaleInvoice_Received').val();
    PosClientInvoicePayment.Description = $('#ClientInvoicePayment_Description').val();
    ex.SMClientInvoicePayment = PosClientInvoicePayment;

    var SMSaleInvoiceProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var SMSaleInvoiceProducts = new Object();
            SMSaleInvoiceProducts.SaleInvoiceId = $('#SMSaleInvoice_SaleInvoiceId').val();
            SMSaleInvoiceProducts.SaleInvoiceProductId = $(this).find('.SaleInvoiceProductId').val();
            SMSaleInvoiceProducts.ProductId = $(this).find('.ProductId').val();
            SMSaleInvoiceProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            SMSaleInvoiceProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            SMSaleInvoiceProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            SMSaleInvoiceProducts.CalLength = $(this).find('[id $= CalLength]').val();
            SMSaleInvoiceProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            SMSaleInvoiceProducts.Sheets = $(this).find('[id $= Sheets]').val();
            SMSaleInvoiceProducts.Quantity = $(this).find('[id $= Quantity]').val();
            SMSaleInvoiceProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            SMSaleInvoiceProducts.UnitPrice = $(this).find('.UnitPrice').text();
            SMSaleInvoiceProducts.LineTotal = $(this).find('.LineTotal').text();
            SMSaleInvoiceProducts.Discount = $(this).find('[id $= Discount]').val();
            SMSaleInvoiceProducts.Tax = $(this).find('[id $= Tax]').val();
            SMSaleInvoiceProducts.NetTotal = $(this).find('.LineTotal').text();
            SMSaleInvoiceProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            SMSaleInvoiceProduct.push(SMSaleInvoiceProducts);
        }
    });
    ex.SMSaleInvoiceProducts = SMSaleInvoiceProduct;
    return ex;
}

function ClearGrid() {
    $('#trans_details tbody tr:visible').remove();
    $('#SMSaleInvoice_Discount').val('0');
}

function CalculateTotal() {
    $rows = $('#trans_details tbody tr:visible');
    var linetotal = 0;
    $rows.each(function (index, value) {
        linetotal += StringToFloat($(this).find('.LineTotal').text());
    });
    $('.nettotal').text(linetotal.toFixed(2));
    $('#SMSaleInvoice_Received').val(linetotal.toFixed(2) - StringToFloat($('#SMSaleInvoice_Discount').val()))
    $('#SMSaleInvoice_ReceivedAmount').val($('#SMSaleInvoice_Received').val())
    $('#SMSaleInvoice_Change').val(StringToFloat($('#SMSaleInvoice_ReceivedAmount').val()) - StringToFloat($('#SMSaleInvoice_Received').val()));
    $('#PosSaleInvoice_TotalAmount').val(linetotal.toFixed(2));
    $('#PosSaleInvoice_NetTotal').val(linetotal.toFixed(2));
    $('#PosSaleInvoice_Balance').val(StringToFloat($('#PosSaleInvoice_NetTotal').val()) - StringToFloat($('#PosSaleInvoice_Received').val()));

}

jQuery('.voucherLink').click(function (event) {
    var id = $(this).attr("id");
    if (id === "submitButton") {
        if (IsValid()) {
            SubmitForm('InsertUpdate');
        }
    }
    if (id === "saveprint") {
        IsPrint = true;
        if (IsValid()) {
            SubmitForm('InsertUpdate');
        }
    }
    if (id === "lbPrintVoucher") {
        PrintInvoice();
    }
    if (id === "lbUnPrintVoucher") {
        $('#mainsection').show();
        $('#printsection').hide();
        $('#lbUnPrintVoucher').hide();
        IsPrint = false;
        HideTopControls(false);
    }
    return false;
});

function PrintInvoice() {
    if ($('#SMSaleInvoice_SaleInvoiceId').val() > 0) {
        $('#mainsection').hide();
        $('#printsection').show();
        $('#lbUnPrintVoucher').show();
        HideTopControls(true);
        window.print();
        setTimeout(function () {
            $('#mainsection').show();
            $('#printsection').hide();
            $('#lbUnPrintVoucher').hide();
            IsPrint = false;
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
        $("#btnAdd, #saveprint").addClass('hidden');
    } else {
        $('[data-action $= Delete]').each(function (index, value) {
            var id = $(this).data("id");
            if (id !== -1) {
                $(this).removeClass('hidden');
            }
        });
        $("#submitButton").closest('li').removeClass('hidden');
        $("#btnAdd, #saveprint").removeClass('hidden');
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

$('.productdropdown').change(function () {
    ProductDetailsById(this);
})

function ProductDetailsById(that) {
    var productId = 0, unitPrice = 0, barcode = 0;
    if (that) {
        if ($(that).val()) {
            $("#UnitPrice").val($(that).find(':selected').data('price'));
            $("#Barcode").val($(that).find(':selected').data('barcode'));
            $("#Quantity").val('1').select().focus();
            CalculateLineTotal();
        }
    }
}

$("#Quantity,#UnitPrice").on("change keyup", function (event) {
    CalculateLineTotal();
})

$("#SMSaleInvoice_Discount").on("change keyup", function (event) {
    $('#SMSaleInvoice_Received').val(StringToFloat($('.nettotal').text()) - StringToFloat($('#SMSaleInvoice_Discount').val()));
    $('#SMSaleInvoice_ReceivedAmount').val($('#SMSaleInvoice_Received').val());
    $('#SMSaleInvoice_Change').val(StringToFloat($('#SMSaleInvoice_ReceivedAmount').val()) - StringToFloat($('#SMSaleInvoice_Received').val()));
})

$("#SMSaleInvoice_ReceivedAmount").on("change keyup", function (event) {
    $('#SMSaleInvoice_Change').val(StringToFloat($('#SMSaleInvoice_ReceivedAmount').val()) - StringToFloat($('#SMSaleInvoice_Received').val()));
})

$(document).on('change keyup', '.gridQty', function () {
    CalculateRowTotal($(this).closest('tr'));
});

function CalculateRowTotal(row) {
    $(row).find('.LineTotal').text((StringToFloat($(row).find('[id $= Quantity]').val()) * StringToFloat($(row).find('.UnitPrice').text())).toFixed(2));
    CalculateTotal();
}

function CalculateLineTotal() {
    $('#LineTotal').val((StringToFloat($('#Quantity').val()) * StringToFloat($('#UnitPrice').val())).toFixed(2));
}

function ClearTopRow() {
    $('#ProductId').val('').trigger('change');
    $('#Quantity').val('');
    $('#UnitPrice').val('');
    $('#LineTotal').val('');
    $('#Barcode').val('').focus();
}

function AddEntrytoGrid() {
    var content = '';
    var rowIndex = $('#trans_details tbody tr').length;
    var rowCount = 0;
    if (rowIndex) {
        rowCount = rowIndex + 1;
    } else {
        rowCount = 1;
    }
    var isfound = false;
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val() == $('#ProductId :selected').val()) {
            isfound = true;
            var qty = $(this).find('.gridQty').val();
            $(this).find('.gridQty').val(StringToFloat(qty) + StringToFloat($('#Quantity').val()));
            CalculateRowTotal(this);
        }
    })
    if (isfound) {
        isfound = false;
    } else {
        content += '<tr class="tr_row">' +
            '<td><input id="SaleInvoiceProduct_' + rowIndex + '__SaleInvoiceProductId" name="PosSaleInvoiceProduct[' + rowIndex + '].SaleInvoiceProductId" type="hidden" value="0" class="SaleInvoiceProductId"> <input id="SaleInvoiceProduct_' + rowIndex + '__ProductId" name="PosSaleInvoiceProduct[' + rowIndex + '].ProductId" type="hidden" class="ProductId" value="' + $('#ProductId :selected').val() + '">' +
            rowCount + '</td>' +
            '<td class="ProductName">' + $('#ProductId :selected').text() + '</td>' +
            '<td><input class="numeric cloneable gridQty detailfirstchildforfocus form-control" id="SaleInvoiceProduct_' + rowIndex + '__Quantity" name="PosSaleInvoiceProduct[' + rowIndex + '].Quantity" placeholder="Qty" type="text" value="' + $('#Quantity').val() + '"></td>' +
            '<td>GM</td>' +
            '<td class="numeric UnitPrice">' + $('#UnitPrice').val() + '</td>' +
            '<td class="numeric LineTotal">' + $('#LineTotal').val() + '</td>' +
            '<td><a href="#" class="btn btn-danger" data-action="Delete" data-id="-1">Delete</a></td>' +
            '</tr>';

        $('#trans_details').append(content);
    }
    ClearTopRow();
    CalculateTotal();
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
            //$(this).find('[id $= SaleInvoiceId]').val(detail[index].SaleInvoiceId);
            //$(this).find('[id $= AccountId]').val(detail[index].AccountId);
            //$(this).find('[id $= ChequeNo]').val(detail[index].ChequeNo);
            //$(this).find('[id $= ChequeDate]').val(detail[index].ChequeDate);
            //$(this).find('[id $= Narration]').val(detail[index].Narration);
            //$(this).find('[id $= CostGroupId]').val(detail[index].CostGroupId);
        }
    });
}

function assignPrintValues(Invoice, InvoiceDetail) {
    if (InvoiceDetail) {
        var data = '';
        var count = 0, LineTotal = 0;
        //$.each(InvoiceDetail, function (e) {
        //    count++;
        //    LineTotal += this.LineTotal;
        //    data += "<tr class='reAssignable'>"
        //        + "<td>" + count + "</td>"
        //        + "<td>" + this.ProductName + "</td>"
        //        + "<td>" + StringToFloat(this.Quantity).toFixed(2) + "</td>"
        //        + "<td>" + StringToFloat(this.UnitPrice).toFixed(2) + "</td>"
        //        + "<td>" + StringToFloat(this.LineTotal).toFixed(2) + "</td>"
        //        + "</tr>";
        //});

        $rows = $('#trans_details tbody tr:visible');
        $rows.each(function (index, value) {
            var iddd = $(this).find('.SaleInvoiceProductId').val();
            if ($(this).find('.SaleInvoiceProductId').val()) {
                count++;
                LineTotal += StringToFloat($(this).find('.LineTotal').text());
                data += "<tr class='reAssignable'>"
                + "<td>" + count + "</td>"
                + "<td>" + $(this).find('.ProductName').text().trim() + "</td>"
                + "<td>" + StringToFloat($(this).find('.gridQty').val()).toFixed(2) + "</td>"
                + "<td>" + StringToFloat($(this).find('.UnitPrice').text()).toFixed(2) + "</td>"
                + "<td>" + StringToFloat($(this).find('.LineTotal').text()).toFixed(2) + "</td>"
                + "</tr>";
            }
        })

        if (InvoiceDetail.length > 0) {
            $("#maintable tbody tr.reAssignable").html("");
            $("#invoiceFooter").before(data);
        }
    }
    if (Invoice) {        
        $('#tDiscount').text(StringToFloat(Invoice.Discount).toFixed(2));
        $('#tNetTotal').text(StringToFloat(Invoice.NetTotal).toFixed(2));
        $('#tNetBalance').text(StringToFloat(Invoice.NetBalance).toFixed(2));
        $('#tClientName').text('').append('<b>To</b> ' + Invoice.ClientName);
        $('#tInvoiceNo').text('').append('CashSale Invoice # ' + Invoice.SaleInvoiceId);
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#tChange').text(StringToFloat(Invoice.ReceivedAmount) - StringToFloat(Invoice.Received).toFixed(2));
        //$('#PosSaleInvoice_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        //$('#PosSaleInvoice_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));
    }
}
