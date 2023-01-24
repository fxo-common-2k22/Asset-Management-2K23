window.onload = function () {
    if ($('#VoucherNo').length > 0) {
        $('#VoucherNo').focus().select();
    } else if ($('#InvoiceNo').length > 0) {
        $('#InvoiceNo').focus().select();
    } else if ($('#Barcode').length > 0) {
        $('#Barcode').focus().select();
    }
};
window.addEventListener('focus', function () {
    if ($('#VoucherNo').length > 0) {
        $('#VoucherNo').focus().select();
    } else if ($('#InvoiceNo').length > 0) {
        $('#InvoiceNo').focus().select();
    } else if ($('#Barcode').length > 0) {
        $('#Barcode').focus().select();
    }
});
var prevRow;

document.onkeydown = function (e) {
    e = e || window.event;//Get event
    if (e.ctrlKey) {
        var c = e.which || e.keyCode;//Get key code
        switch (c) {
            case 83://Ctrl+S
                if ($('#submitButton').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#submitButton').is(':visible')) {
                        $('#submitButton').trigger('click');
                    }
                }
                break;
            case 80://Ctrl+P
                if ($('#lbPrintVoucher').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#lbPrintVoucher').is(':visible')) {
                        $('#lbPrintVoucher').trigger('click');
                    }
                }
                break;
            case 82://Ctrl+R
                if ($('#lbReceivePayment').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#lbReceivePayment').is(':visible')) {
                        $('#lbReceivePayment').trigger('click');
                    }
                } else if ($('#SMSaleInvoice_ReceivedAmount').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#SMSaleInvoice_ReceivedAmount').is(':visible')) {
                        $('#SMSaleInvoice_ReceivedAmount').focus().select();
                    }
                }
                break;
            case 68://Ctrl+D
                if ($('#SMSaleInvoice_Discount').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#SMSaleInvoice_Discount').is(':visible')) {
                        $('#SMSaleInvoice_Discount').focus().select();
                    }
                }
                break;
            case 73://Ctrl+i
                if ($('#saveprint').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#saveprint').is(':visible')) {
                        $('#saveprint').trigger('click');
                    }
                }
                break;
            case 65://Ctrl+A
                if ($('#newInvoice').length > 0) {
                    e.preventDefault();
                    disabledEventPropagation(e);
                    if ($('#newInvoice').is(':visible')) {
                        //$('#newInvoice').trigger('click');
                        window.location = $('#newInvoice').attr('href');
                    }
                }
                break;
            case 67://Ctrl+C
                if ($('#PaymentType').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#PaymentType').is(':visible')) {
                        if ($('#PaymentType option').length > 0) {
                            $('#PaymentType').val('Cash').trigger('change');
                            if ($('#SMSaleInvoice_Received').length > 0 && $('#SMSaleInvoice_Received').is(':visible')) {
                                $('#SMSaleInvoice_Received').focus().select();
                            }
                        }
                    }
                }
                break;
            case 66://Ctrl+B
                if ($('#PaymentType').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#PaymentType').is(':visible')) {
                        if ($('#PaymentType option').length > 0) {
                            $('#PaymentType').val('Bank').trigger('change');
                            if ($('#SMSaleInvoice_Received').length > 0 && $('#SMSaleInvoice_Received').is(':visible')) {
                                $('#SMSaleInvoice_Received').focus().select();
                            }
                        }
                    }
                }
                break;
        }
    }
    else {
        var tag = e.target.tagName.toLowerCase();
        var inputName = null;
        if (e.target.name)
            inputName = e.target.name.substr(e.target.name.lastIndexOf('.') + 1);
        var closestDiv, closestDivid, closestTable, tbl, trows = null;
        var isDivFound = false;
        if (tag != "body") {
            closestDiv = $('#' + e.target.id).closest('div');
            closestTable = $('#' + e.target.id).closest('table[id=trans_details]');
            if (closestTable) {
                tbl = document.getElementById("trans_details");
                trows = $('#trans_details tbody .tr_row').not('tr[id=tr_clone]');
            }
            if (closestDiv.length > 0) {
                closestDivid = closestDiv.prop('id');
                if (closestDivid) {
                    isDivFound = true;
                }
            }
        } else {
            $('.headfirstchildforfocus').focus().select();
        }
        var c = e.which || e.keyCode;
        switch (c) {
            case 18://TAB Key

                break;
            case 35://End Key
                if (isDivFound === true) {
                    e.preventDefault();
                    e.stopPropagation();
                    switch (closestDivid) {
                        case "head":
                            if ($('.detailfirstchildforfocus').length > 0) {
                                if ($('.detailfirstchildforfocus').closest('select2').length > 0) {
                                    $('#' + $('.detailfirstchildforfocus')[1].id).select2('focus');
                                } else {
                                    $('.detailfirstchildforfocus')[1].focus();
                                }
                            }
                            break;
                        case "detail":
                            if ($('.footerfirstchildforfocus').length > 0) {
                                $('.footerfirstchildforfocus').focus().select();
                            } else {
                                $('.headfirstchildforfocus').focus().select();
                            }
                            break;
                        case "footerdiv":
                            $('.headfirstchildforfocus').focus().select();
                            break;
                    }
                }
                break;
            case 46: // Del Key
                e.preventDefault();
                e.stopPropagation();
                if (!prevRow) {
                    prevRow = trows[0];
                }
                if (trows) {
                    if (trows.length === 1) {
                    }
                    else {
                        $(prevRow).find('[data-action="Delete"]').trigger('click');
                        //highlightRow(trows[(prevRow.rowIndex - 2) + 1], inputName);
                    }
                }
                break;
            case 38: // UP arrow
                e.preventDefault();
                e.stopPropagation();
                if (!prevRow) {
                    prevRow = trows[0];
                }
                if ((prevRow.rowIndex - 2) === 0) {
                }
                else {
                    highlightRow(trows[(prevRow.rowIndex - 2) - 1], inputName);
                }
                break;
            case 40://Down Arrow Key
                e.preventDefault();
                e.stopPropagation();
                if (!prevRow) {
                    prevRow = trows[0];
                }
                if (isDivFound === true) {
                    if (closestTable.length > 0) {
                        if ((prevRow.rowIndex - 2) === trows.length - 1) { //higlight first row
                            //highlightRow(trows[2], inputName);
                        }
                        else { //highlight next row
                            if (prevRow.rowIndex == -1)
                                highlightRow(trows[1]);
                            else if ((prevRow.rowIndex - 2) == -1)
                                highlightRow(trows[2], inputName);
                            else
                                highlightRow(trows[(prevRow.rowIndex - 2) + 1], inputName);
                        }
                    }
                }
                break;
        }
    }
};

$(".bb").on("click", function () {
    var $example = $(".chosen-select").select2();
    $example.select2("open");
});

function highlightRow(rowObj, inputName) {
    if (rowObj) {
        if (prevRow) {
            $rows = $('#trans_details tbody tr:visible');
            $rows.each(function (index, value) {
                if ($(this).hasClass('.highlighted')) {
                    $(this).removeClass('.highlighted')
                }
            });
            //prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
        }
        if (inputName) {
            $(rowObj).find('[id $= ' + inputName + ']').focus().select();
        } else {
            $(rowObj).find('[id $= Barcode]').focus().select();
        }
        rowObj.className += ' highlighted';
        prevRow = rowObj;
    }
}

function disabledEventPropagation(e) {
    if (e) {
        if (e.stopPropagation) {
            e.stopPropagation();
        } else if (window.event) {
            window.event.cancelBubble = true;
        }
    }
}