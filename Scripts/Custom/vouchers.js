
function LoadCurrencyVal(val) {
    $('#Voucher_ExchangeRate').empty();
    $.get('/Finance/VoucherAndReceipt/LoadCurrencyValue', { id: val }, function (d) {
        $('#Voucher_ExchangeRate').val(d);
    });
}


function IsValid() {
    var validated = true;
    //if (!$('#Voucher_VoucherId').val()) {
    //    $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').show().text('Voucher Id not found').delay(10000).fadeOut();
    //    return false;
    //}
    if (!$('#Voucher_TransactionDate').val()) {
        DisplayMessage('error', 'Transaction Date is required')
        //$('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').show().text('Transaction Date is required').delay(10000).fadeOut();
        return false;
    }
    var vop = stringToDate($('#Voucher_TransactionDate').val(), 'dd/mm/yyyy', '/');
    if (vop < stringToDate(FiscalStartDate, 'dd/mm/yyyy', '/') || vop > stringToDate(FiscalEndDate, 'dd/mm/yyyy', '/')) {
        DisplayMessage('error', 'Date must be between selected fiscal year');
        return false;
    }
    if (!$('#Voucher_CurrencyId').val()) {
        DisplayMessage('error', 'Currency is required')
        //$('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').show().text('Currency is required').delay(10000).fadeOut();
        return false;
    }
    if (!$('#Voucher_ExchangeRate').val()) {
        DisplayMessage('error', 'Exchange Rate is required')
        //$('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').show().text('Exchange Rate is required').delay(10000).fadeOut();
        return false;
    }
    if (!$('#Voucher_CBAccountId').val()) {
        DisplayMessage('error', 'Account is required')
        //$('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').show().text('Account is required').delay(10000).fadeOut();
        return false;
    }


    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var amount = 0;
        if ($(this).find('[id $= TransactionType]').val() == "Cr") {
            amount = StringToFloat($(this).find('[id $= Credit]').val());
        } else {
            amount = StringToFloat($(this).find('[id $= Debit]').val());
        }
        if ($(this).find('[id $= TransactionId]').val() && amount > 0) {

            if (!$(this).find('[id $= AccountId]').val()) {
                $(this).find('[id $= item_no_lbl]').css('color', 'red');
                $(this).find('[id $= item_no_lbl]').show().text('Account is Required').delay(2000).fadeOut();
                validated = false;
                return false;
            }
        }
    });
    return validated;
}

function SubmitForm(command) {
    $.ajax({
        //url: '/Finance/VoucherAndReceipt/CashBankVoucher',
        url: '/Finance/VoucherAndReceipt/' + command + action,
        type: 'post',
        data: { ex: FillForm(), Command: command },
        success: function (data) {
            if (data.IsSuccess) {
                //var currentURL = window.location.href;
                //var id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1)
                //if (StringToFloat(id) === 0) {
                //    var newURL = currentURL + "/" + data.VoucherId;
                //    window.history.replaceState(null, null, newURL);
                //}
                changeHref(data.VoucherId);
                HideTopControls(false);
                RemoveEmptyRows();
                CalculateTotal();
                if (data.VoucherId) {
                    $('#Voucher_VoucherId').val(data.VoucherId);
                    $('#VoucherNo').val(data.VoucherId);
                }
                if (data.Detail) {
                    SetDetailRows(data.Detail);
                    assignPrintValues(data.Detail, data.Particulars);
                }
                $('#tdCreation').text(data.Modification);

                if (data.Case === "Post") {
                    if (data.IsPosted) {
                        HideButtons(true);
                        $('#lbUnPostVoucher').closest('li').removeClass('hidden');
                        $('#lbPostVoucher').closest('li').addClass('hidden');
                        $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text('Voucher Posted Successfully').delay(10000).fadeOut();
                    }
                }
                else if (data.Case === "UnPost") {
                    if (!data.IsPosted) {
                        HideButtons(false);
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                        $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text('Voucher Unposted Successfully').delay(10000).fadeOut();

                    }
                }
                else if (data.Case === "Cancel") {
                    if (data.IsCancelled) {
                        HideButtons(true);
                        $('#lbUnPostVoucher').closest('li').addClass('hidden');
                        $('#lbPostVoucher').closest('li').removeClass('hidden');
                        $('#lbUnCancelVoucher').closest('li').removeClass('hidden');
                        $('#lbCancelVoucher').closest('li').addClass('hidden');
                        $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text('Voucher Cancelled Successfully').delay(10000).fadeOut();

                    }
                }
                else if (data.Case === "UnCancel") {
                    if (!data.IsCancelled) {
                        HideButtons(false);
                        $('#lbUnCancelVoucher').closest('li').addClass('hidden');
                        $('#lbCancelVoucher').closest('li').removeClass('hidden');
                        $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text('Voucher Uncancelled Successfully').delay(10000).fadeOut();

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
                    $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text('Successfully updated').delay(10000).fadeOut();
                }
            }
            else {
                $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'red').show().text('Updation failed').delay(10000).fadeOut();
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            RemoveEmptyRows();
            if (jqXHR.responseText) {
                $("html").html(jqXHR.responseText);
            }
            else
                $('#messageDiv').show().delay(10000).fadeOut(), $('#messagelabel').css('color', 'green').show().text(errorThrown).delay(10000).fadeOut();
        }
    });
}

function FillForm() {
    var ex = new Object();

    var Voucher = new Object();
    Voucher.VoucherId = $('#Voucher_VoucherId').val();
    Voucher.TransactionDate = $('#Voucher_TransactionDate').val();
    Voucher.VoucherType = $('#Voucher_VoucherType').val();
    Voucher.CurrencyId = $('#Voucher_CurrencyId').val();
    Voucher.ExchangeRate = $('#Voucher_ExchangeRate').val();
    Voucher.CBAccountId = $('#Voucher_CBAccountId').val();
    Voucher.Particulars = $('#Voucher_Particulars').val();
    ex.Voucher = Voucher;

    var VoucherDetail = new Array();

    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var amount = 0;
        if ($(this).find('[id $= TransactionType]').val() == "Cr") {
            amount = StringToFloat($(this).find('[id $= Credit]').val());
        } else {
            amount = StringToFloat($(this).find('[id $= Debit]').val());
        }
        if ($(this).find('[id $= AccountId]').val() && amount > 0) {
            var VoucherDetails = new Object();
            VoucherDetails.VoucherId = $('#Voucher_VoucherId').val();
            VoucherDetails.VoucherDetailId = $(this).find('[id $= VoucherDetailId]').val();
            VoucherDetails.TransactionId = $(this).find('[id $= TransactionId]').val();
            VoucherDetails.AccountId = $(this).find('[id $= AccountId]').val();
            VoucherDetails.Debit = $(this).find('[id $= Debit]').val();
            VoucherDetails.Credit = $(this).find('[id $= Credit]').val();
            VoucherDetails.ChequeNo = $(this).find('[id $= ChequeNo]').val();
            VoucherDetails.ChequeDate = $(this).find('[id $= ChequeDate]').val();
            VoucherDetails.Narration = $(this).find('[id $= Narration]').val();
            VoucherDetails.CostGroupId = $(this).find('[id $= CostGroupId]').val();
            VoucherDetail.push(VoucherDetails);
        }
    });

    ex.VoucherDetail = VoucherDetail;
    return ex;
}

function RemoveEmptyRows() {
    $rows = $('#trans_details tbody tr:visible');
    $rows.each(function (index, value) {
        var amount = 0;
        if ($(this).find('[id $= TransactionType]').val() == "Cr") {
            amount = StringToFloat($(this).find('[id $= Credit]').val());
        } else {
            amount = StringToFloat($(this).find('[id $= Debit]').val());
        }
        if (!$(this).find('[id $= AccountId]').val() && amount === 0) {
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
            if (detail[index].TransactionId !== 1) {
                $(this).find('[id $= VoucherDetailId]').val(detail[index].VoucherDetailId);
                $(this).find('[id $= TransactionId]').val(detail[index].TransactionId);
                $(this).find('[id $= AccountId]').val(detail[index].AccountId);
                $(this).find('[id $= ChequeNo]').val(detail[index].ChequeNo);
                //$(this).find('[id $= ChequeDate]').val(detail[index].ChequeDate);
                $(this).find('[id $= Narration]').val(detail[index].Narration);
                $(this).find('[id $= CostGroupId]').val(detail[index].CostGroupId);
            }
        }
    });
}

function CalculateTotal() {
    $rows = $('#trans_details tbody tr:visible');
    var amount = 0;
    $rows.each(function (index, value) {
        if ($(this).find('[id $= TransactionType]').val() == "Cr") {
            amount += StringToFloat($(this).find('[id $= Credit]').val());
        } else {
            amount += StringToFloat($(this).find('[id $= Debit]').val());
        }
    });
    $('#ftTotal').val(amount.toFixed(2));
    $('#topTotal').text(amount.toFixed(2));
}

jQuery('.voucherLink').click(function (event) {
    var id = $(this).attr("id");
    if (id === "lbUnPostVoucher") {
        if (IsValid()) {
            SubmitForm('UnPost');
        }
    }
    if (id === "lbPostVoucher") {
        if (IsValid()) {
            SubmitForm('Post');
        }
    }
    if (id === "lbUnCancelVoucher") {
        if (IsValid()) {
            SubmitForm('UnCancel');
        }
    }
    if (id === "lbCancelVoucher") {
        if (IsValid()) {
            SubmitForm('Cancel');
        }
    }
    if (id === "submitButton") {
        if (IsValid()) {
            if (StringToFloat($('#Voucher_VoucherId').val()) == 0)
                SubmitForm('Insert');
            else
                SubmitForm('Update');
        }
    }
    //if (id === "lbPrintVoucher") {
    //    if ($('#Voucher_VoucherId').val() > 0) {
    //        var CurrentURL = "/Finance/VoucherAndReceipt/VoucherPrint/" + $('#Voucher_VoucherId').val();
    //        window.open(CurrentURL, '_blank');
    //    }
    //}
    if (id === "lbPrintVoucher") {
        if ($('#Voucher_VoucherId').val() > 0) {
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
            DisplayMessage('error', 'Voucher does not exist');
        }
    }
    if (id === "lbPrintVoucher11") {
        if ($('#Voucher_VoucherId').val() > 0) {
            $('#mainsection').hide();
            $('#printsection11').show();
            $('#lbUnPrintVoucher').show();
            HideTopControls(true);
            window.print();
            setTimeout(function () {
                $('#mainsection').show();
                $('#printsection11').hide();
                $('#lbUnPrintVoucher').hide();
                HideTopControls(false);
            }, 0);

        } else {
            DisplayMessage('error', 'Voucher does not exist');
        }
    }
    if (id === "lbUnPrintVoucher") {
        $('#mainsection').show();
        $('#printsection').hide();
        $('#lbUnPrintVoucher').hide();
        HideTopControls(false);
    }

    if (id === "lbHistoryNote") {
        if ($('#Voucher_VoucherId').val() > 0) {
            $("#AdminNotesModal").modal();
        } else {
            DisplayMessage('error', 'Voucher id not found');
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
function assignPrintValues(InvoiceDetail, Particulars) {
    if (InvoiceDetail) {
        var data = '';
        var credit = 0, debit = 0;
        $.each(InvoiceDetail, function (e) {
            credit += this.Credit;
            debit += this.Debit;
            data += "<tr class='reAssignable'>"
                + "<td>" + this.AccountName + "</td>"
                + "<td>" + this.Narration + "</td>"
                + "<td>" + this.ChequeNo + "</td>"
                + "<td>" + this.ChequeDate + "</td>"
                + "<td>" + StringToFloat(this.Debit).toFixed(2) + "</td>"
                + "<td>" + StringToFloat(this.Credit).toFixed(2) + "</td>"
                + "</tr>";
        });

        if (InvoiceDetail.length > 0) {
            $("#maintable tbody tr.reAssignable").html("");
            $("#invoiceFooter").before(data);
            $('#tCredit').text(StringToFloat(credit).toFixed(2));
            $('#tDebit').text(StringToFloat(debit).toFixed(2));
            $('#tVoucherNo').text('');
            $('#tVoucherNo').append('<b>Voucher No. ' + $('#Voucher_VoucherId').val() + '</b>');
            $('#tParticulars').text(Particulars);
        }
    }
}

$('#AdminNotesModal').on('shown.bs.modal', function () {
    $("#Notes").focus();
    LoadAdminNotes($('#Voucher_VoucherId').val());
});

$('#btnSaveNote').click(function () {
    var modal = $('#AdminNotesModal');
    var InvoiceNoteId = $('#InvoiceNoteId').val();
    var InvoiceId = $('#Voucher_VoucherId').val();
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
    $.get('/Shop/Purchase/InsertUpdateInvoiceNote', { id: InvoiceNoteId, InvoiceId: InvoiceId, Note: Notes, InvoiceType: "Finance_Voucher", NoteType: "AdminNote" }, function (d) {
        $('#Notes').val('').focus();
        $('#InvoiceNoteId').val('');
        DisplayMessage('', d);
        $(modal).find('[id $= item_no_lbl]').css('color', 'green');
        LoadAdminNotes($('#Voucher_VoucherId').val());
    });
}

function LoadAdminNotes(Invoice) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { InvoiceNo: Invoice, InvoiceType: "Finance_Voucher" },
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

$(document).on('click', '.editadminnote', function () {
    $('#InvoiceNoteId').val($(this).closest('tr').find('[id $= InvoiceNoteId]').val());
    $('#Notes').val($(this).closest('tr').find('.actualnotes').text());
    return false;
});