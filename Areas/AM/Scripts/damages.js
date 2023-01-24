
function IsValid() {
    var validated = true;
    if (!$('#Damage_DamageDate').val()) {
        DisplayMessage('error', 'Invoice Date is required')
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
        url: '/SM/Damages/AddEditDamage',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                //var currentURL = window.location.href;
                //var id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1)
                //if (StringToFloat(id) === 0) {
                //    var newURL = currentURL + "/" + data.DamageId;
                //    window.history.replaceState(null, null, newURL);
                //}
                changeHref(data.DamageId);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                assignPrintValues(data.Invoice, data.Detail);
                if (data.Detail) {
                    SetDetailRows(data.Detail)
                }
                $('#tdCreation').text(data.Modification);
                if (data.DamageId) {
                    $('#Damage_DamageId').val(data.DamageId);
                    $('#InvoiceNo').val(data.DamageId);
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
            }
            else
                DisplayMessage('error', errorThrown);
        }
    });
}

function FillForm() {
    var ex = new Object();

    var Damage = new Object();
    Damage.DamageId = $('#Damage_DamageId').val();
    Damage.DamageDate = $('#Damage_DamageDate').val();
    Damage.Description = $('#Damage_Description').val();
    //Damage.NetTotal = $('#Damage_NetTotal').val();
    Damage.NetTotal = $('#ftNetTotal').val();

    ex.Damage = Damage;

    var DamageProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var DamageProducts = new Object();
            DamageProducts.DamageId = $('#Damage_DamageId').val();
            DamageProducts.DamageProductId = $(this).find('[id $= DamageProductId]').val();
            DamageProducts.ProductId = $(this).find('.ProductId').val();
            DamageProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            DamageProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            DamageProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            DamageProducts.CalLength = $(this).find('[id $= CalLength]').val();
            DamageProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            DamageProducts.Sheets = $(this).find('[id $= Sheets]').val();
            DamageProducts.Quantity = $(this).find('[id $= Quantity]').val();
            DamageProducts.SqFeet = $(this).find('[id $= SqFeet]').val();
            DamageProducts.UnitPrice = $(this).find('[id $= UnitPrice]').val();
            DamageProducts.LineTotal = $(this).find('[id $= LineTotal]').val();
            DamageProducts.Discount = $(this).find('[id $= Discount]').val();
            DamageProducts.Tax = $(this).find('[id $= Tax]').val();
            DamageProducts.NetTotal = $(this).find('[id $= NetTotal]').val();
            DamageProducts.WareHouseId = $(this).find('[id $= WareHouseId]').val();
            DamageProduct.push(DamageProducts);
        }
    });

    ex.DamageProduct = DamageProduct;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        if ($(this).find('[id $= DamageProductId]').val() === 0) {
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
            $(this).find('[id $= DamageProductId]').val(detail[index].DamageProductId);
            $(this).find('[id $= DamageId]').val(detail[index].DamageId);
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
    $('#Damage_TotalAmount').val(nettotal.toFixed(2));
    var billTotal = (nettotal + StringToFloat($('#Damage_OtherCharges').val()) + StringToFloat($('#Damage_LabourCharges').val()) + StringToFloat($('#Damage_FareCharges').val())) - StringToFloat($('#Damage_Discount').val());
    $('#topTotal').text(billTotal.toFixed(2));
    $('#Damage_NetTotal').val(billTotal.toFixed(2));
    $('#Damage_Balance').val(StringToFloat($('#Damage_NetTotal').val()) - StringToFloat($('#Damage_Received').val()));

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
        if ($('#Damage_DamageId').val() > 0) {
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
        if ($('#Damage_DamageId').val() > 0) {
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
                if (data.IsSuccess == true) {
                    ClearRow(row);
                    $(row).find('[id $= UnitPrice]').val(data.Data.SalePrice);
                    $(row).find('[id $= PType]').val(data.Data.Type);
                    $(row).find('[id $= PWidth]').val(data.Data.Width);
                    $(row).find('[id $= PLength]').val(data.Data.Length);
                    $(row).find('[id $= Quantity]').focus().select();
                    CalculateRowTotal(row);
                    //if ($(row).find('[id $= PType]').val() === "Sheet") {
                    //    GetProductLengthWidth(row, data.Data.Type, data.Data.Width, data.Data.Length)
                    //}
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
    //ResetRadio();
    var row = $(this).closest('tr');
    //SetValuesToOrignalWidthLength(row)
    CalculateRowTotal(row);
    //GetProductLengthWidth(row, $(row).find('[id $= PType]').val(), $(row).find('[id $= PWidth]').val(), $(row).find('[id $= PLength]').val());
})

$(".charges,.flatdiscount").bind("change keyup", function (event) {
    CalculateTotal();
})

$(".tax,.discount").bind("change keyup", function (event) {
    CalculateNetTotal($(this).closest('tr'));
})

function CalculateRowTotal(row) {
    //var sqFeet = ((StringToFloat($(row).find('[id $= CalWidth]').val()) * StringToFloat($(row).find('[id $= CalLength]').val())) / 144) * StringToFloat($(row).find('[id $= Sheets]').val());
    //$(row).find('[id $= SqFeet]').val(sqFeet.toFixed(2));
    $(row).find('[id $= LineTotal]').val((StringToFloat($(row).find('[id $= Quantity]').val()) * StringToFloat($(row).find('[id $= UnitPrice]').val())).toFixed(2));
    CalculateNetTotal(row);
    CalculateTotal();
}

function GetProductLengthWidth(row, PType, pWidth, pLength) {
    if (!PType && !pWidth && !pLength) {
        $.ajax({
            url: '/SM/Purchase/ProductDetailById',
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
        var val = $(this).val();
        Roundingformula(val);
        CalculateRowTotal(row);
        SetValuesToOrignalWidthLength($(this).closest('tr'))
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
    }
}

function ResetRadio() {
    $('#CalDigit1').prop("checked", true);
}

function SetValuesToOrignalWidthLength(row) {
    if ($('[data-name="radio"]:checked').val() == "1") {
        $(row).find('[id $= OrgWidth]').val(StringToFloat($(row).find('[id $= CalWidth]').val()));
        $(row).find('[id $= OrgLength]').val(StringToFloat($(row).find('[id $= CalLength]').val()));
    }
}

$('input', '#trans_details > tbody > tr').focusin(function () {
    var roundingpoint = $(this).closest('tr').find('[id $= CalDigit]').val();
    $('#CalDigit' + roundingpoint).prop('checked', true);
});

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
        $('#tInvoiceNo').append('<b>Sale Invoice # ' + Invoice.DamageId + '</b>');
        $('#tReceived').text(StringToFloat(Invoice.Received).toFixed(2));
        $('#Damage_Received').val(StringToFloat(Invoice.Received).toFixed(2));
        $('#Damage_Balance').val(StringToFloat(Invoice.NetTotal) - StringToFloat(Invoice.Received));

        //$('#ClientInvoicePayment_Amount').val(StringToFloat(Invoice.NetBalance).toFixed(2));
    }
}