
function IsValid() {
    var validated = true;
    if (!$('#PurchaseInvoice_PurchaseInvoiceDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
        return false;
    }
    if (!$('#PurchaseInvoice_SupplierId').val()) {
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

function SubmitForm(command) {
    $.ajax({
        url: '/Shop/Purchase/PostPurchaseInvoice',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                changeHref(data.PurchaseInvoiceId);
                assignPrintValues(data.Invoice, data.Detail);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.PurchaseInvoiceId) {
                    $('#PurchaseInvoice_PurchaseInvoiceId').val(data.PurchaseInvoiceId);
                    $('#InvoiceNo').val(data.PurchaseInvoiceId);
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
                    if (StringToFloat(data.SaleInvoiceId) > 0) {
                        window.location = "/Shop/Sale/AddEditSaleInvoice/" + data.SaleInvoiceId;
                    }
                    if (data.SuccessMsg)
                        DisplayMessage('success', data.SuccessMsg);
                    else
                        DisplayMessage('success', 'Successfully updated');
                }
            }
            else {
                if (data.ErrorMsg)
                    DisplayMessage('error', data.ErrorMsg);
                else
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

    var PurchaseInvoice = new Object();
    PurchaseInvoice.PurchaseInvoiceId = $('#PurchaseInvoice_PurchaseInvoiceId').val();
    PurchaseInvoice.PurchaseInvoiceDate = $('#PurchaseInvoice_PurchaseInvoiceDate').val();
    PurchaseInvoice.SupplierId = $('#PurchaseInvoice_SupplierId').val();
    PurchaseInvoice.Description = $('#PurchaseInvoice_Description').val();
    PurchaseInvoice.OtherCharges = 0;
    PurchaseInvoice.LabourCharges = 0;
    PurchaseInvoice.FareCharges = 0;
    PurchaseInvoice.TotalAmount = $('#ftNetTotal').val();
    PurchaseInvoice.NetTotal = $('#ftNetTotal').val();
    ex.PurchaseInvoice = PurchaseInvoice;

    var SupplierInvoicePayment = new Object();
    SupplierInvoicePayment.PurchaseInvoiceId = $('#SupplierInvoicePayment_PurchaseInvoiceId').val();
    SupplierInvoicePayment.Amount = $('#SupplierInvoicePayment_Amount').val();
    SupplierInvoicePayment.Description = $('#SupplierInvoicePayment_Description').val();
    SupplierInvoicePayment.CreatedOn = $('#SupplierInvoicePayment_CreatedOn').val();
    ex.SupplierInvoicePayment = SupplierInvoicePayment;

    var PurchaseInvoiceProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var PurchaseInvoiceProducts = new Object();
            PurchaseInvoiceProducts.PurchaseInvoiceId = $('#PurchaseInvoice_PurchaseInvoiceId').val();
            PurchaseInvoiceProducts.PurchaseInvoiceProductId = $(this).find('[id $= PurchaseInvoiceProductId]').val();
            PurchaseInvoiceProducts.ProductId = $(this).find('.ProductId').val();
            PurchaseInvoiceProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            PurchaseInvoiceProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            PurchaseInvoiceProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            PurchaseInvoiceProducts.CalLength = $(this).find('[id $= CalLength]').val();
            PurchaseInvoiceProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            PurchaseInvoiceProducts.Sheets = $(this).find('[id $= Sheets]').val();
            PurchaseInvoiceProducts.Quantity = $(this).find('[id $= Quantity]').val();
            PurchaseInvoiceProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            PurchaseInvoiceProducts.UnitPrice = $(this).find('[id $= UnitPrice]').val();
            PurchaseInvoiceProducts.LineTotal = $(this).find('[id $= LineTotal]').val();
            PurchaseInvoiceProducts.Discount = $(this).find('[id $= Discount]').val();
            PurchaseInvoiceProducts.Tax = $(this).find('[id $= Tax]').val();
            PurchaseInvoiceProducts.NetTotal = $(this).find('[id $= NetTotal]').val();
            PurchaseInvoiceProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            PurchaseInvoiceProduct.push(PurchaseInvoiceProducts);
        }
    });
    ex.CashAccountId = $('#CashAccountId').val();
    ex.BankAccountId = $('#BankAccountId').val();
    ex.PaymentType = $('#PaymentType').val();
    ex.ClientId = $('#ClientId :selected').val();
    ex.SaleInvoiceDate = $('#SaleInvoiceDate').val();
    ex.PurchaseInvoiceProduct = PurchaseInvoiceProduct;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= PurchaseInvoiceProductId]').val() === 0) {
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
            $(this).find('[id $= PurchaseInvoiceProductId]').val(detail[index].PurchaseInvoiceProductId);
            $(this).find('[id $= PurchaseInvoiceId]').val(detail[index].PurchaseInvoiceId);
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
    $('#PurchaseInvoice_TotalAmount').val(nettotal.toFixed(2));
    var billTotal = (nettotal + StringToFloat($('#PurchaseInvoice_OtherCharges').val()) + StringToFloat($('#PurchaseInvoice_LabourCharges').val()) + StringToFloat($('#PurchaseInvoice_FareCharges').val())) - StringToFloat($('#PurchaseInvoice_Discount').val());
    $('#ftNetTotal').val(nettotal.toFixed(2));
    $('#topTotal').text(billTotal.toFixed(2));
    $('#PurchaseInvoice_NetTotal').val(billTotal.toFixed(2));
    $('#PurchaseInvoice_Balance').val(StringToFloat($('#PurchaseInvoice_NetTotal').val()) - StringToFloat($('#PurchaseInvoice_Received').val()));

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
        if ($('#PurchaseInvoice_PurchaseInvoiceId').val() > 0) {
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
        if ($('#PurchaseInvoice_PurchaseInvoiceId').val() > 0) {
            $("#AddPaymentModal").modal();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }
    if (id === "btnSavePayment") {
        if (IsValid()) {
            if (StringToFloat($('#SupplierInvoicePayment_Amount').val()) > StringToFloat($('#tNetBalance').text())) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Amount must be less than balance amount').show().delay(3000).fadeOut();
                $('#SupplierInvoicePayment_Amount').val($('#tNetBalance').text());
                $('#SupplierInvoicePayment_Amount').focus();
            } else if ($('#PaymentType').val() == "Cash" && !$('#CashAccountId').val()) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Select Cash Account').show().delay(3000).fadeOut();
                $('#CashAccountId').focus();
            }
            else if ($('#PaymentType').val() == "Bank" && !$('#BankAccountId').val()) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Select Bank Account').show().delay(3000).fadeOut();
                $('#BankAccountId').focus();
            }
            else if (!$('#SupplierInvoicePayment_CreatedOn').val()) {
                $('#AddPaymentModal').find('[id $= item_no_lbl]').css('color', 'red').text('Payment date is required').show().delay(3000).fadeOut();
                $('#SupplierInvoicePayment_CreatedOn').focus();
            }
            else {
                SubmitPayment('savePayment');
            }
        }
    }
    if (id === "lbConvertToSale") {
        if ($('#PurchaseInvoice_PurchaseInvoiceId').val() > 0) {
            $("#ConvertToSaleModal").modal();
            //$("#SaleInvoiceDate").focus();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }

    if (id === "btnCreateSale") {
        if (IsValid()) {
            if ($('#ClientId :selected').val()) {
                if ($('#SaleInvoiceDate').val()) {
                    SubmitForm('ConvertToSale');
                } else
                    DisplayMessage('error', 'Select Invoice Date first');
            } else
                DisplayMessage('error', 'Select Client first');
        }
    }
    if (id === "lbHistoryNote") {
        if ($('#PurchaseInvoice_PurchaseInvoiceId').val() > 0) {
            $("#AdminNotesModal").modal();
        } else {
            DisplayMessage('error', 'Invoice id not found');
        }
    }

    return false;
});

function SubmitPayment(command) {
    $.ajax({
        url: '/Shop/Purchase/PostPurchaseInvoice',
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
        url: "/Shop/Purchase/LoadInvoicePayments",
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
            data += "<tr>"
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
        $('#tDescription').text('').append('<b>Description : </b>' + Invoice.Description);
        $('#tGrandTotal').text(StringToFloat(LineTotal).toFixed(2));
        $('#tOthercharges').text(StringToFloat(Invoice.OtherCharges).toFixed(2));
        $('#tDiscount').text(StringToFloat(Invoice.Discount).toFixed(2));
        $('#tNetTotal').text(StringToFloat(Invoice.NetTotal).toFixed(2));
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#tNetBalance').text(StringToFloat(Invoice.NetBalance).toFixed(2));
        $('#tClientName').text('').append('<b>To: ' + Invoice.ClientName + '</b>');
        $('#tInvoiceNo').text('').append('<b>Purchase Invoice # ' + Invoice.PurchaseInvoiceId + '</b>');
        $('#PurchaseInvoice_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        $('#PurchaseInvoice_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));

        //$('#Reciept_Amount').val(StringToFloat(Invoice.NetBalance).toFixed(2));
    }
}

$(".Barcode").enterKey(function (e) {
    var that = this;
    e.preventDefault();
    if ($(this).val()) {
        $.ajax({
            url: '/Shop/Purchase/ProductIdByBarcode',
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
            url: '/Shop/Purchase/ProductDetailById',
            type: 'post',
            data: { id: idd },
            success: function (data) {
                if (data.IsSuccess == true) {
                    ClearRow(row);
                    $(row).find('[id $= UnitPrice]').val(data.Data.CostPrice);
                    $(row).find('[id $= PType]').val(data.Data.Type);
                    $(row).find('[id $= PWidth]').val(data.Data.Width);
                    $(row).find('[id $= PLength]').val(data.Data.Length);
                    $(row).find('[id $= CalWidth]').val(data.Data.Width);
                    $(row).find('[id $= CalLength]').val(data.Data.Length);
                    $(row).find('[id $= OrgWidth]').val(data.Data.Width);
                    $(row).find('[id $= OrgLength]').val(data.Data.Length);
                    if ($(row).find('[id $= Sheets]').length) {
                        $(row).find('[id $= Sheets]').focus().select();
                    }
                    CalculateSqFeet(row);
                    if ($(row).find('[id $= PType]').val() === "Sheet") {
                        GetProductLengthWidth(row, data.Data.Type, data.Data.Width, data.Data.Length)
                    }
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

$(".width,.length,.Sheets").bind("change keyup", function (e) {
    e = e || window.event;//Get event
    if (!e.ctrlKey) {
        var code = e.which || e.keyCode;
        if (code != 17) {
            ResetRadio();
            var row = $(this).closest('tr');
            SetValuesToOrignalWidthLength(row)
            CalculateSqFeet(row);
            GetProductLengthWidth(row, $(row).find('[id $= PType]').val(), $(row).find('[id $= PWidth]').val(), $(row).find('[id $= PLength]').val());
        }
    }
})

$(".tax,.discount").bind("change keyup", function (event) {
    CalculateNetTotal($(this).closest('tr'));
})

function CalculateSqFeet(row) {
    var sqFeet = ((StringToFloat($(row).find('[id $= CalWidth]').val()) * StringToFloat($(row).find('[id $= CalLength]').val())) / 144) * StringToFloat($(row).find('[id $= Sheets]').val());
    $(row).find('[id $= SqFeet]').val(sqFeet.toFixed(2));
    $(row).find('[id $= LineTotal]').val((StringToFloat($(row).find('[id $= SqFeet]').val()) * StringToFloat($(row).find('[id $= UnitPrice]').val())).toFixed(2));
    CalculateNetTotal(row);
    CalculateTotal();
}

function GetProductLengthWidth(row, PType, pWidth, pLength) {
    if (!PType && !pWidth && !pLength) {
        $.ajax({
            url: '/Shop/Purchase/ProductDetailById',
            type: 'post',
            data: { id: $(row).find('.ProductId').val() },
            success: function (data) {
                if (data.IsSuccess == true) {
                    $(row).find('[id $= PType]').val(data.Data.Type);
                    $(row).find('[id $= PWidth]').val(data.Data.Width);
                    $(row).find('[id $= PLength]').val(data.Data.Length);
                    if ($(row).find('[id $= PType]').val() === "Sheet") {
                        CalculateQty(row, data.Data.Width, data.Data.Length);
                    }
                } else {
                    DisplayMessage('error', 'Product not found');
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                DisplayMessage('error', errorThrown);
            }
        });
    } else {
        if (PType === "Sheet") {
            CalculateQty(row, pWidth, pLength);
        }
    }
}

function CalculateQty(row, width, Length) {
    var actualFeet = (StringToFloat(width) * StringToFloat(Length)) / 144;
    var derivedFeet = $(row).find('[id $= SqFeet]').val();
    var qty = (derivedFeet / actualFeet).toFixed(2);
    $(row).find('[id $= Quantity]').val(qty);
    Roundingformula($('[data-name="radio"]:checked').val());
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

$('[data-name="radio"]').change(function () {
    if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
        var row = $('#trans_details .highlighted');
        var val = $('[data-name="radio"]:checked').val();
        Roundingformula(val);
        CalculateSqFeet(row);
        SetValuesToOrignalWidthLength(row)
    }
})

function Roundingformula(val) {
    if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
        $row = $('#trans_details .highlighted');
        var OrgWidth = StringToFloat($($row).find('[id $= OrgWidth]').val());
        var OrgLength = StringToFloat($($row).find('[id $= OrgLength]').val());
        var Calwidth = 0, CalLength = 0;
        Calwidth = Math.ceil(OrgWidth / StringToFloat(val)) * StringToFloat(val);
        CalLength = Math.ceil(OrgLength / StringToFloat(val)) * StringToFloat(val);
        $($row).find('[id $= CalWidth]').val(Calwidth);
        $($row).find('[id $= CalLength]').val(CalLength)
        $($row).find('[id $= CalDigit]').val(val);
        $('#tdPtype').text('W= ' + Calwidth + ' L= ' + CalLength);

    }
}

function ResetRadio() {
    $('#CalDigit6').prop("checked", true);
}

function SetValuesToOrignalWidthLength(row) {
    if ($('[data-name="radio"]:checked').val() == "1") {
        $(row).find('[id $= OrgWidth]').val(StringToFloat($(row).find('[id $= CalWidth]').val()));
        $(row).find('[id $= OrgLength]').val(StringToFloat($(row).find('[id $= CalLength]').val()));
    }
}

$('input', '#trans_details > tbody > tr').focusin(function () {
    if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
        $row = $('#trans_details .highlighted');
        var roundingpoint = StringToInt($($row).closest('tr').find('[id $= CalDigit]').val());
        if (roundingpoint > 0) {
            $('#CalDigit' + roundingpoint).prop('checked', true);
            Roundingformula($('[data-name="radio"]:checked').val());
        } else {
            $('#CalDigit' + 6).prop('checked', true);
        }
    }
});

$('#AddPaymentModal').on('shown.bs.modal', function () {
    var modal = $(this);
    LoadPayments($('#PurchaseInvoice_PurchaseInvoiceId').val());
    $('#SupplierInvoicePayment_InvoiceId').val($('#PurchaseInvoice_PurchaseInvoiceId').val());
    $('#myModalLabel').text('Make Payment for (' + $('#PurchaseInvoice_PurchaseInvoiceId').val() + ')');
    $('#SupplierInvoicePayment_BillTotal').val($('#PurchaseInvoice_NetTotal').val());
    $('#SupplierInvoicePayment_Amount').val($('#tNetBalance').text());
    $('#SupplierInvoicePayment_Amount').focus();
    $('#SupplierInvoicePayment_Description').val('Payment for #' + $('#PurchaseInvoice_PurchaseInvoiceId').val());
    $('#SupplierInvoicePayment_CreatedOn').val(GetCurrentDate());
});

$('#AddPaymentModal').on('hidden.bs.modal', function () {
});

$('#AdminNotesModal').on('shown.bs.modal', function () {
    $("#Notes").focus();
    LoadAdminNotes($('#PurchaseInvoice_PurchaseInvoiceId').val());
});

$('#btnSaveNote').click(function () {
    var modal = $('#AdminNotesModal');
    var InvoiceNoteId = $('#InvoiceNoteId').val();
    var InvoiceId = $('#PurchaseInvoice_PurchaseInvoiceId').val();
    var Notes = $('#Notes').val();

    if (InvoiceId && Notes) {
        InsertUpdateInvoiceNote(modal, InvoiceNoteId, InvoiceId, Notes);
    } else {
        $(modal).find('[id $= item_no_lbl]').css('color', 'red');
        $(modal).find('[id $= item_no_lbl]').text('Something is missing').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
});

function InsertUpdateInvoiceNote(modal, InvoiceNoteId, InvoiceId, Notes) {
    $.get('/Shop/Purchase/InsertUpdateInvoiceNote', { id: InvoiceNoteId, InvoiceId: InvoiceId, Note: Notes, InvoiceType: "PurchaseInvoice", NoteType: "AdminNote" }, function (d) {
        $('#Notes').val('').focus();
        $('#InvoiceNoteId').val('');
        DisplayMessage('', d);        
        $(modal).find('[id $= item_no_lbl]').css('color', 'green');
        LoadAdminNotes($('#PurchaseInvoice_PurchaseInvoiceId').val());
        //$(modal).find('[id $= item_no_lbl]').text(d).show().delay(2000).fadeOut();
    });
}

function LoadAdminNotes(Invoice) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { InvoiceNo: Invoice, InvoiceType: "PurchaseInvoice" },
        contentType: 'application/json',
        url: "/Shop/Purchase/LoadAdminNotes",
        success: function (result) {
            BindAdminNotes(result);
        }
    })
};

function BindAdminNotes(d) {
    if (d) {
        var data = '';
        $('#AdminNotesModal table tbody').html("");
        var count = 0;
        $.each(d, function (e) {
            count++;
            data += "<tr>"
                + "<td><input id='AdminNotes_" + count + "_InvoiceNoteId' name='AdminNotes_" + count + "_InvoiceNoteId' type='hidden' value='" + this.InvoiceNoteId + "'>" + count + "</td>"
                + "<td>" + (this.ModifiedDate == "" ? this.CreatedDate : this.ModifiedDate) + "</td>"
                + "<td>" + (this.ModifiedUser == "" ? this.CreatedUser : this.ModifiedUser) + "</td>"
                + "<td class='hidden actualnotes'>" + this.StickyNote + "</td>"
                + "<td>" + this.StickyNote + " <a href='#' title='Edit note' class='editadminnote'> Edit</a></td>"
                + "</tr>";
        });

        if (d.length === 0) {
            $('.EmptyData').show();
        } else {
            $("#AdminNotesModal table tbody").append(data);
        }
    }
}

$(document).on('click','.editadminnote', function () {
    $('#InvoiceNoteId').val($(this).closest('tr').find('[id $= InvoiceNoteId]').val());
    $('#Notes').val($(this).closest('tr').find('.actualnotes').text());
    return false;
});

$('.PaymentMode').change(function () {
    togglecashbankaccounts($(this).val());
})

function togglecashbankaccounts(val) {
    if (val == "Cash") {
        $('.cash').show();
        $('.bank').hide();
    }
    if (val == "Bank") {
        $('.cash').hide();
        $('.bank').show();
    }
}