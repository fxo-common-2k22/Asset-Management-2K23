//window.onload = function () {
//    if ($('#VoucherNo').length > 0) {
//        $('#VoucherNo').focus().select();
//    } else if ($('#InvoiceNo').length > 0) {
//        $('#InvoiceNo').focus().select();
//    } else if ($('#Barcode').length > 0) {
//        $('#Barcode').focus().select();
//    } else if ($('#Search').length > 0) {
//        $('#Search').focus().select();
//    }
//};
//window.addEventListener('focus', function () {
//    if ($('#VoucherNo').length > 0) {
//        $('#VoucherNo').focus().select();
//    } else if ($('#InvoiceNo').length > 0) {
//        $('#InvoiceNo').focus().select();
//    } else if ($('#Barcode').length > 0) {
//        $('#Barcode').focus().select();
//    } else if ($('#Search').length > 0) {
//        $('#Search').focus().select();
//    }
//});
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
                } else if ($('#SaleInvoice_Received').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#SaleInvoice_Received').is(':visible')) {
                        $('#SaleInvoice_Received').focus().select();
                    }
                }
                break;
            case 68://Ctrl+D
                if ($('#SaleInvoice_Discount').length > 0) {
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#SaleInvoice_Discount').is(':visible')) {
                        $('#SaleInvoice_Discount').focus().select();
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
            case 78://Ctrl+N
                if ($('#newInvoice').length > 0) {
                    e.preventDefault();
                    disabledEventPropagation(e);
                    if ($('#newInvoice').is(':visible')) {
                        $('#newInvoice').trigger('click');
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
                            if ($('#SaleInvoice_Received').length > 0 && $('#SaleInvoice_Received').is(':visible')) {
                                $('#SaleInvoice_Received').focus().select();
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
                            if ($('#SaleInvoice_Received').length > 0 && $('#SaleInvoice_Received').is(':visible')) {
                                $('#SaleInvoice_Received').focus().select();
                            }
                        }
                    }
                }
                break;
            case 48://Ctrl+0
                if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                    assignValueToRadio(e, 1);
                }
                break;
            case 50://Ctrl+2
                if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                    assignValueToRadio(e, 12);
                }
                break;
            case 51://Ctrl+3
                if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                    assignValueToRadio(e, 3);
                }
                break;
            case 54://Ctrl+6
                if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                    assignValueToRadio(e, 6);
                }
                break;
            case 57://Ctrl+9
                if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                    assignValueToRadio(e, 9);
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
            case 37: // left arrow
                e.preventDefault();
                e.stopPropagation();
                var input = document.activeElement.id;
                if (input) {
                    var td;
                    td = $('#' + input).closest('td').prev('td');
                    if ($(td).hasClass('hidden')) {
                        td = $('#' + input).closest('td').prev('td.hidden').prev('td');
                    }
                    if (td && td.length > 0) {
                        if ($(td).find('input,select').not(".hidden")) {
                            if ($(td).find('input,select').not(".hidden").is('[readonly]')) {
                                if ($(td).find('select').not(".hidden").length > 0) {
                                    $(td).prev('td').find('input').not('.hidden').focus().select();
                                } else {
                                    $(td).find('input').not(".hidden").focus().select();
                                }
                            } else {
                                if ($(td).find('select').not(".hidden").length > 0) {
                                    $(td).prev('td').find('input').not('.hidden').focus().select();
                                } else {
                                    $(td).find('input').not(".hidden").focus().select();
                                }
                            }
                        }
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
                case 39: // right arrow
                    e.preventDefault();
                    e.stopPropagation();
                    if ($('#trans_details > tbody > tr').hasClass('highlighted')) {
                        var input = document.activeElement.id;
                        if (input) {
                            var td = $('#' + input).closest('td').next('td');
                            if ($(td).hasClass('hidden')) {
                                td = $('#' + input).closest('td').next('td.hidden').next('td');
                            }
                            if (td && td.length > 0) {
                                if ($(td).find('input,select').not(".hidden").length > 0) {
                                    if ($(td).find('input,select').not(".hidden").is('[readonly]')) {
                                        if ($(td).find('select').not(".hidden").length > 0) {
                                            $(td).next('td').find('input').not('.hidden').focus().select();
                                        } else {
                                            $(td).find('input').not(".hidden").focus().select();
                                        }
                                    } else {
                                        if ($(td).find('select').not(".hidden").length > 0) {
                                            $(td).next('td').find('input').not('.hidden').focus().select();
                                        } else {
                                            $(td).find('input').not(".hidden").focus().select();
                                        }
                                    }
                                }
                            }
                        }
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
                            highlightRow(trows[2], inputName);
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

function assignValueToRadio(e, value) {
    e.preventDefault();
    e.stopPropagation();
    var input = document.activeElement.id;
    $('#CalDigit' + value).prop("checked", true);
    $row = $('#trans_details .highlighted');
    if ($row.length > 0) {
        $($row).closest('tr').find('[id $= CalDigit]').val(value);
        $('[data-name="radio"]').trigger('change');
    }
}