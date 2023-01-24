(function ($) {
    $.fn.sum = function () {
        var total = 0;
        this.each(function () {
            total += parseFloat($(this).text().replace(/,/g, ""), 10) || 0;
            //console.log($(this).text());
        });
        return total;
    };
})(jQuery);

(function ($) {
    $.fn.textSummarize = function () {
        $('.textsummarize').closest('table').each(function () {
            tablename = $(this).prop('id');
            //var tablename = $('.textsummarize').closest('table').prop('id');
            if (!tablename) {
                $('.textsummarize').closest('table').prop('id', 'gv');
                tablename = $('.textsummarize').closest('table').prop('id');
            }
            if (tablename) {
                if ($('#' + tablename + ' tfoot tr').length === 0) {
                    var tfoot = '<tr>';
                    $("#" + tablename + " tbody tr:first").find('td').each(function () {
                        tfoot += '<td></td>';
                    });
                    tfoot += '</tr>';
                    $("#" + tablename).append($('<tfoot/>').append(tfoot));
                }
                $("#" + tablename + " tbody tr:first").find('.textsummarize').each(function () {
                    var tot = $('#' + tablename + ' tbody tr :nth-child(' + (this.cellIndex + 1) + ')').sum().toFixed(2);
                    $('#' + tablename + ' tfoot tr :nth-child(' + (this.cellIndex + 1) + ')').text(tot);
                })
                $('#' + tablename + ' tfoot tr').css({ "background-color": 'rgb(243, 242, 242)', 'font-weight': 'bold' });
            }
        })
    };
})(jQuery);

$(function () {
    setTimeout(function () {
        for (var i = 4; i < 14; i++) {
            var tot = $('#gv3 tbody tr :nth-child(' + i + ')').sum().toFixed(2);
            $('#gv3 tfoot tr :nth-child(' + i + ')').text(tot);
        }
    }, 1000);

    setTimeout(function () {
        $('.justtogiveselector').textSummarize();
    }, 1000);
});

$('.cdelete').on('click', function () {
    return confirm('Are you sure you want to delete this?');
});

$.fn.cloneRow = function (element, appendTo) {
    $clone = element.clone(true, true);//
    $clone.removeAttr('id');
    $clone.show();
    $clone.appendTo(appendTo);

    var index = $clone.index() - 1;

    $clone.find('select, input, textarea').each(function (i, ele) {
        if (ele.id.indexOf('Sr') != -1)
            $(ele).closest('td').text(index + 1);
        ele.id = ele.id.replace(/-\d+/, index);
        ele.name = ele.name.replace(/-\d+/, index);
    });
    //$clone.find('select').select2({ width: 'resolve' }).end().find('.dp').datepicker({ autoclose: true, dateFormat: 'dd/mm/yy' });
    $clone.find('select').select2({ width: 'resolve' }).end().find('.datepicker').datetimepicker({ datepicker: false }).datepicker({ dateFormat: 'dd/mm/yy' });
    return $clone;
}

Date.prototype.ddmmmyyyy = function () {
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var mmm = months[this.getMonth()]; // getMonth() is zero-based
    var dd = this.getDate();

    return [dd.padLeft(2), '-', mmm, '-', this.getFullYear()].join(''); // padding
};

Number.prototype.padLeft = function (totalWidth, char) {
    that = this.toString();
    return Array(totalWidth - that.length + 1).join(char || '0') + that;
}

String.prototype.ukTextToDate = function () {
    var ddmmyyyy = this.split('/');
    return new Date(ddmmyyyy[2], ddmmyyyy[1] - 1, ddmmyyyy[0]);
}

$('[data-id="selectAll"]').click(function () {
    $('input[id ^= ' + $(this).data('checkbox-id') + ']').prop("checked", true);
    return false;
});

$('[data-id="deselectAll"]').click(function () {
    $('input[id ^= ' + $(this).data('checkbox-id') + ']').prop("checked", false);
    return false;
});

function GetDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = dd + '/' + mm + '/' + yyyy;
    return today;
}
function NowDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1; //January is 0!

    var yyyy = today.getFullYear();
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var today = dd + '/' + mm + '/' + yyyy;
    return today;
}
function printData(id, h1, beforehtml, afterhtml, iframe, printHeader) {
    if (!printHeader) {
        printHeader = true;
    }

    if (iframe) {
        var ifrm = document.getElementById(iframe);
        ifrm = (ifrm.contentWindow) ? ifrm.contentWindow : (ifrm.contentDocument.document) ? ifrm.contentDocument.document : ifrm.contentDocument;
        setTimeout(function () {
            ifrm.print();
        }, 500);
    }
    else {
        h1 = h1 || "";
        beforehtml = beforehtml || "";
        afterhtml = afterhtml || "";
        var divToPrint = document.getElementById(id);
        var ifrm = document.getElementById('myIframe');
        ifrm = (ifrm.contentWindow) ? ifrm.contentWindow : (ifrm.contentDocument.document) ? ifrm.contentDocument.document : ifrm.contentDocument;
        ifrm.document.open();
        ifrm.document.write('<!doctype html><html>');
        ifrm.document.write('<head><link rel="stylesheet" href="/uploads/pdf - Copy.css"></head>');
        ifrm.document.write('<body>');
        if (printHeader)
            ifrm.document.write('<div><img src="/uploads/Logos/BranchLogo@(FAPP.DAL.SessionHelper.BranchId).png" /></div>');
        ifrm.document.write('<h1>' + h1 + '</h1>');
        ifrm.document.write(beforehtml + divToPrint.outerHTML + afterhtml);
        ifrm.document.write('</body>');
        ifrm.document.write('</html>');

        setTimeout(function () {
            ifrm.print();
        }, 500);
    }
}

toastr.options = {
    "closeButton": true,
    "debug": false,
    "newestOnTop": true,
    "progressBar": true,
    "positionClass": "toast-bottom-right",
    "showDuration": "300",
    "hideDuration": "1000",
    "timeOut": "5000",
    "extendedTimeOut": "1000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
}

function DisplayMessage(type, message) {
    if (type === "error") {
        toastr.clear();
        toastr.error(message, 'Failed');
    } else {
        toastr.clear();
        toastr.success(message, 'Success');
    }
}

//function DisplayMessage(type, text) {
//    if (type === "error") {
//        DisplayGlitterMessage('Error!', text, false, 3000);
//    } else {
//        DisplayGlitterMessage('Success!', text, false, 3000);
//    }
//}



$(".ProductId").on("select2:open", function (e) {
    $('#trans_details tr').removeClass('highlighted');
    $(this).closest('tr').addClass('highlighted');
});

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

function StringIsNullOrEmpty(value) {
    if (typeof (value) != 'number' && (value == null || value.trim() === "")) {
        return true;
    }
    else {
        return false;
    }
}

function StringIsNull(value) {
    if (value) {
        return value;
    }
    else {
        return '';
    }
}

function StringToFloat(value) {
    var res = parseFloat(value);
    if (isNaN(res)) {
        return 0;
    }
    else {
        return res;
    }
}

function StringToInt(value) {
    var res = parseInt(value);
    if (isNaN(res)) {
        return 0;
    }
    else {
        return res;
    }
}

$.fn.enterKey = function (fnc, mod) {
    return this.each(function () {
        $(this).keypress(function (ev) {
            var keycode = (ev.keyCode ? ev.keyCode : ev.which);
            if ((keycode == '13' || keycode == '10') && (!mod || ev[mod + 'Key'])) {
                fnc.call(this, ev);
            }
        })
    })
}

$("#InvoiceNo").enterKey(function (e) {
    e.preventDefault();
    submitPostForm('LoadInvoice');
})

function submitPostForm(val) {
    $('#' + val).remove();
    $('form[id=invoiceform]').append("<input id='" + val + "' type='hidden' name='Command' value='" + val + "' />");
    $('form[id=invoiceform]').submit();
    return true;
}

function ShowMessageModal(title, message) {
    $("#MessageModal .modal-title").html(title);
    $("#MessageModal #error_msg").html(message);
    $('#MessageModal').on('shown.bs.modal', function () {
        $('#modal_btnclose').focus();
    })
    $('#MessageModal').on('hidden.bs.modal', function () {
        ConfirmYesNo = false;
        clickedbtn = null;
    })
    $("#MessageModal").modal({ backdrop: 'static' });
}

function ValidateFields(val) {
    if (val == null || val == "") {
        return false;
    }
    return true;
}

function ValidateDropDown(val) {
    if (val == null || val == "" || val == "-1") {
        return false;
    }
    return true;
}

function DisplayGlitterMessage(title, message, sticky, time) {
    var overlay;

    $.gritter.add({
        title: (typeof title !== 'undefined') ? title : 'Message - Head',
        text: (typeof message !== 'undefined') ? message : 'Body',
        image: (typeof image !== 'undefined') ? image : null,
        sticky: (typeof sticky !== 'undefined') ? sticky : false,
        time: (typeof time !== 'undefined') ? time : 3000,
        class_name: title == 'Error!' ? 'glittererror' : 'glittersuccess'
    });
}

//window.onload = function () {
//    if ($('#VoucherNo').length > 0) {
//        $('#VoucherNo').focus().select();
//    } else if ($('#InvoiceNo').length > 0) {
//        $('#InvoiceNo').focus().select();
//    } else if ($('#Barcode').length > 0) {
//        $('#Barcode').focus().select();
//    } else if ($('#Search').length > 0) {
//        $('#Search').focus().select();
//    } else if ($('#RoomCode').length > 0) {
//        $('#RoomCode').focus().select();
//    }
//};

//window.addEventListener('focus, onload', function () {
//    if ($('#VoucherNo').length > 0) {
//        $('#VoucherNo').focus().select();
//    } else if ($('#InvoiceNo').length > 0) {
//        $('#InvoiceNo').focus().select();
//    } else if ($('#Barcode').length > 0) {
//        $('#Barcode').focus().select();
//    } else if ($('#Search').length > 0) {
//        $('#Search').focus().select();
//    } else if ($('#RoomCode').length > 0) {
//        $('#RoomCode').focus().select();
//    }
//});

$(window).on('focus, load', function (e) {
    if ($('#VoucherNo').length > 0) {
        $('#VoucherNo').focus().select();
    } else if ($('#InvoiceNo').length > 0) {
        $('#InvoiceNo').focus().select();
    } else if ($('#Barcode').length > 0) {
        $('#Barcode').focus().select();
    } else if ($('#Search').length > 0) {
        $('#Search').focus().select();
    } else if ($('#RoomCode').length > 0) {
        $('#RoomCode').focus().select();
    }
});

var showChar = 10;
var ellipsestext = ".... ";
var moretext = "+";
var lesstext = "Collapse";
//comment more

$.fn.showLess = function () {
    $('.minimizable').each(function () {
        var t = $(this).text();
        if (t.length < showChar) return;

        $(this).html(
            t.slice(0, showChar) + '<span>' + ellipsestext + '</span><a href="#" title="Click to see more" class="more">' + moretext + '</a>' +
            '<span style="display:none;">' + t.slice(showChar, t.length) + ' <a href="#" title="Click to less" class="less">' + lesstext + '</a></span>'
        );
    });
}

$(document).on('click', 'a.more', function (e) {
    e.preventDefault();
    $(this).hide().prev().hide();
    $(this).next().show();
    return false;
});

$(document).on('click', 'a.less', function (e) {
    e.preventDefault();
    $(this).parent().hide().prev().show().prev().show();
    return false;
});


function handleDocumentKeyDown(event) {
    if (event.ctrlKey || event.shiftKey) {
    } else {
        if (event.which === 37) {
            $.tabPrev();
            if ($('.xdsoft_datetimepicker').css('display') == 'block') {
                $('.xdsoft_datetimepicker').css('display', 'none');
            }
        }
        if (event.which === 39) {
            $.tabNext();
            if ($('.xdsoft_datetimepicker').css('display') == 'block') {
                $('.xdsoft_datetimepicker').css('display', 'none');
            }
        }
    }
}

if ($($('form')[1]).find('input[type=text],input[type=number]').filter(':visible:first').length > 0) {
    $($('form')[1]).find('input[type=text],input[type=number]').filter(':visible:first').focus();
}

function getFileExtension(filename) {
    return filename.slice((filename.lastIndexOf(".") - 1 >>> 0) + 2);
}

function IsDisableControl(bool) {
    if (bool === false)
        $('.inputenabledisable').removeAttr('disabled');
    else if (bool === true)
        $('.inputenabledisable').attr('disabled', 'disabled');
}

$(document).ready(function () {
    if (location.hash) {
        $('a[href=\'' + location.hash + '\']').tab('show');
    } else {
        if ($('a[data-toggle="tab"]')[0] !== undefined) {
            $('a[data-toggle="tab"]')[0].click();
        }
    }
    var activeTab = localStorage.getItem('activeTab');
    if (activeTab) {
        $('a[href="' + activeTab + '"]').tab('show').click();
    }

    $('body').on('click', 'a[data-toggle=\'tab\']', function (e) {
        e.preventDefault()
        var tab_name = this.getAttribute('href')
        if (history.pushState) {
            history.pushState(null, null, tab_name)
        }
        else {
            location.hash = tab_name
        }
        localStorage.setItem('activeTab', tab_name)

        $(this).tab('show');
        return false;
    });
    $(window).on('popstate', function () {
        var anchor = location.hash ||
            $('a[data-toggle=\'tab\']').first().attr('href');
        $('a[href=\'' + anchor + '\']').tab('show');
    });
})

function FillData(data) {
    var totalColumns = Object.keys(data[0]).length;
    var columnNames = [];
    columnNames = Object.keys(data[0]);

    //Create a HTML Table element.
    var table = document.createElement("TABLE");
    //table.border = "1";
    table.setAttribute('class', 'table table-striped table-hover no-head-border');

    //Add the header row.
    var row = table.insertRow(-1);
    for (var i = 0; i < totalColumns; i++) {
        var headerCell = document.createElement("TH");
        headerCell.innerHTML = columnNames[i];
        row.appendChild(headerCell);
    }

    // Add the data rows.
    for (var i = 0; i < data.length; i++) {
        row = table.insertRow(-1);
        columnNames.forEach(function (columnName) {
            var cell = row.insertCell(-1);
            cell.innerHTML = data[i][columnName];
        });
    }

    var dvTable = document.getElementById("dvTable");
    dvTable.innerHTML = "";
    dvTable.appendChild(table);
}

$(document).on('click', '[data-action $= ConfirmYesNo]', function (e) {
    var that = this;
    ConfirmYesNo = true;
    clickedbtn = this;
    var message = $(that).attr('data-message');
    var title = $(that).attr('data-title');
    ShowMessageModal(title, message);
    return false;
});

var ConfirmYesNo = false;
var clickedbtn = null;
$(document).on('click', '[data-action $= ConfirmPopup]', function (e) {
    ConfirmYesNo = true;
    clickedbtn = this;
    var message = $(clickedbtn).attr('data-message');
    var title = $(clickedbtn).attr('data-title');
    ShowMessageModal(title, message);
    return false;
});

$(document).on('click', '#modal_btnYes', function () {
    if (ConfirmYesNo === true) {
        if (clickedbtn) {
            $.ajax({
                url: $(clickedbtn).attr('data-href'),
                type: 'post',
                data: { id: $(clickedbtn).attr('data-id') },
                success: function (data) {
                    OnResponceBackFromServer(data);
                }
            });
        }
    }
    $('#MessageModal').modal('hide');
});