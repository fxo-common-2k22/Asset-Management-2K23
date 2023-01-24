function IsValid() {
    var validated = true;
    if (!$('#PurchaseInvoice_PurchaseInvoiceDate').val()) {
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
        url: '/AM/Asset/PostPurchaseInvoice',
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            OnResponceBackFromServer(data);
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

    var IssuedItem = new Object();
    IssuedItem.Description = $('#IssuedItem_Description').val();
    IssuedItem.DepartmentId = $('#IssuedItem_DepartmentId').val();
    ex.IssuedItem = IssuedItem;

    var PurchaseInvoiceProduct = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var iddd = $(this).find('.ProductId').val();
        if ($(this).find('.ProductId').val()) {
            var PurchaseInvoiceProducts = new Object();
            PurchaseInvoiceProducts.PurchaseInvoiceId = $('#PurchaseInvoice_PurchaseInvoiceId').val();
            PurchaseInvoiceProducts.PurchaseInvoiceProductId = $(this).find('[id $= PurchaseInvoiceProductId]').val();
            PurchaseInvoiceProducts.ItemId = $(this).find('.ProductId').val();
            PurchaseInvoiceProducts.Description = $(this).find('.Description').val();
            PurchaseInvoiceProducts.OrgWidth = $(this).find('[id $= OrgWidth]').val();
            PurchaseInvoiceProducts.OrgLength = $(this).find('[id $= OrgLength]').val();
            PurchaseInvoiceProducts.CalWidth = $(this).find('[id $= CalWidth]').val();
            PurchaseInvoiceProducts.CalLength = $(this).find('[id $= CalLength]').val();
            PurchaseInvoiceProducts.CalDigit = $(this).find('[id $= CalDigit]').val();
            PurchaseInvoiceProducts.Sheets = $(this).find('[id $= Sheets]').val();
            PurchaseInvoiceProducts.Quantity = $(this).find('[id $= Quantity]').val();
            PurchaseInvoiceProducts.ManufacturerProductNo = $(this).find('[id $= ManufacturerProductNo]').val();
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
            $(this).find('[id $= PurchaseInvoiceProductId]').val(detail[index].PurchaseInvoiceProductId);
            $(this).find('[id $= PurchaseInvoiceId]').val(detail[index].PurchaseInvoiceId);
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

$(document).on('click', '.voucherLink', function () {
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
            console.log('Print clicked');
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

$(document).on('enterKey', '.Barcode', function () {
    var that = this;
    e.preventDefault();
    if ($(this).val()) {
        $.ajax({
            url: '/AM/Asset/ProductIdByBarcode',
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

$(document).on('change', '.ProductId', function () {
    ProductDetailsById($(this).closest('tr'), $(this).val());
})

function ProductDetailsById(row, idd) {
    if (idd) {
        $.ajax({
            url: '/AM/Asset/ProductDetailById',
            type: 'post',
            data: { id: idd },
            success: function (data) {
                if (data.IsSuccess == true) {
                    ClearRow(row);
                    $(row).find('[id $= UnitPrice]').val(data.Data.Price);
                    CalculateRowTotal(row);
                    $(row).find('[id $= Quantity]').focus().select();
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

$(document).on('change keyup', '.width,.length,.Sheets,.Quantity,.unitprice', function () {
    var row = $(this).closest('tr');
    CalculateRowTotal(row);
})

$(document).on('change keyup', '.tax,.discount', function () {
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
