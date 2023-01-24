if (typeof Sys !== "undefined") {

    var prm = Sys.WebForms.PageRequestManager.getInstance();
    // first function gets executed on the beginning of a request
    prm.add_beginRequest(BeginRequestHandler);
    // second function gets executed on the end of the request
    prm.add_endRequest(EndRequestHandler);
}
function BeginRequestHandler(sender, args) {
    $.blockUI({ message: '<i class="fa fa-spinner fa-spin"></i> <span>Processing your request, please be patient...</span>' });
}
function EndRequestHandler(sender, args) {
    $.unblockUI();
}

$('.cdelete').click(function () {
    return confirm('Are you sure you want to delete!');
});

$('.csure').click(function () {
    return confirm('Are you sure!');
});

$.fn.addRow = function (o) {
    var source = $('#rowTemplate').html();
    var template = Handlebars.compile(source);
    var html = template(o);
    $('#notifications').prepend(html);
}
function loadNotifications(userId) {
    //$('#notifications').empty();
    $.get('/Handlers/GetNotifications.ashx', { 'u': userId }, function (o) {
        if (o.length > 0) {
            $('#LabelNotificationCount').text(o.length);
            for (var i = 0; i < o.length; i++) {
                $.fn.addRow(o[i]);
            }
        }
    });
}

function init() {
    var now = new Date();
    var dobDate = new Date(now.getFullYear(), now.getMonth(), 1);

    try {

        $('.cshift').click(function () {
            return confirm('Are you sure you want to shift selected students!');
        });
        $.datepicker.setDefaults({ dateFormat: 'dd/mm/yy', changeMonth: true, changeYear: true, showWeek: true });
        $('.datebox').datepicker();
        $('.dobbox').datepicker({ defaultDate: '-5y', firstDay: 1, showWeek: true });
    }
    catch (ex) { alert(ex.message) }
    try { $('.mybox').add('.monthbox').monthpicker(); } catch (ex) { }
    try {
        //$('.dobbox').datetimepicker({
        //    format: 'd/m/Y',
        //    inline: false,
        //    timepicker: false,
        //    maxDate: now,
        //    defaultDate: dobDate,
        //    scrollInput: false,
        //    scrollMonth: false
        //});

        //$('.datebox').datetimepicker({
        //    format: 'd/m/Y',
        //    inline: false,
        //    timepicker: false,
        //    scrollInput: false,
        //    scrollMonth: false
        //});

        $('.dateboxNow').datetimepicker({
            format: 'd/m/Y',
            inline: false,
            timepicker: false,
            maxDate: now,
            scrollInput: false,
            scrollMonth: false
        });

        $('.datetimebox').datetimepicker({
            format: 'd/m/Y H:i',
            inline: false,
            //maxDate: now,
            scrollInput: false,
            scrollMonth: false
        });

        $('input.timebox').datetimepicker({
            datepicker: false,
            format: 'H:i',
            step: 5,
            //mask: true,
            maxDate: now,
            scrollInput: false,
            scrollMonth: false
        });

        //jQuery('.dateFrom').periodpicker({
        //    end: '.dateTo'
        //});

        jQuery('.dateFrom').datetimepicker({
            format: 'd/m/Y',
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('.dateTo').val() ? jQuery('.dateTo').val() : false
                })
            },
            timepicker: false
        });
        jQuery('.dateTo').datetimepicker({
            format: 'd/m/Y',
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('.dateFrom').val() ? jQuery('.dateFrom').val() : false
                })
            },
            timepicker: false
        });

        jQuery('.datetimeFrom').datetimepicker({
            format: 'd/m/Y h:i a',
            step: 15,
            onShow: function (ct) {
                this.setOptions({
                    maxDate: jQuery('.datetimeTo').val() ? jQuery('.datetimeTo').val() : false
                })
            },
            timepicker: true
        });
        jQuery('.datetimeTo').datetimepicker({
            format: 'd/m/Y h:i a',
            step: 15,
            onShow: function (ct) {
                this.setOptions({
                    minDate: jQuery('.datetimeFrom').val() ? jQuery('.datetimeFrom').val() : false
                })
            },
            timepicker: true
        });

    }

    catch (ex) {
        alert(ex.message);
    }
    try { $('select').not('.mtz-monthpicker').not('.nochosen').not('.ajax').removeClass('form-control').select2({ width: 'resolve' }); } catch (ex) { }
    try {
        $('#tabs').tabs({
            activate: function (event, ui) {
                $.cookie($(this).prop('id'), ui.newTab.index(), { expires: 10 });
                resizeContent();
            }
        });
        $("#tabs").tabs("option", "active", $.cookie('tabs'));
    } catch (ex) { }

    try {
        var icons = {
            header: "ui-icon-circle-arrow-e",
            activeHeader: "ui-icon-circle-arrow-s"
        };
        $('.accord').accordion({ heightStyle: "content", icons: icons });
    } catch (ex) { alert('error'); }

    $('table.xslx').add('table.gv').removeClass('xslx').addClass('table table-hover table-striped table-condensed');
    $('table.fv').addClass('table borderless');

    $('input[type="text"],input[type="email"],input[type="password"],textarea').removeClass('form-control').addClass('form-control');

    $('.main-nav > .dropdown-submenu').removeClass('dropdown-submenu');
    $('.dropdown-submenu > a > .caret').remove();

    if (!$('.pagination a').length) {
        $('.pagination').parent().empty();
    }

    $('.small.green').addClass('btn btn-small btn-success');
    $('.small.red').addClass('btn btn-small btn-danger');
    $('.small.blue').addClass('btn btn-small btn-primary');
    $('.small.orange').addClass('btn btn-small btn-danger');
    $('.small.white').addClass('btn btn-small btn-teal');
    $('.small.rosy').addClass('btn btn-small btn-grey');
    $('.small.gray').addClass('btn btn-grey');

    $('.medium.green').addClass('btn btn-success');
    $('.medium.red').addClass('btn btn-danger');
    $('.medium.blue').addClass('btn btn-primary');
    $('.medium.orange').addClass('btn btn-warning');
    $('.medium.white').addClass('btn btn-teal');
    $('.medium.rosy').addClass('btn btn-grey');
    $('.medium.gray').addClass('btn btn-grey');

    $('[id $= "DataPager1"]').each(function () { $(this).html($(this).html().replace(/&nbsp;/g, '')) });

    $('[id $= "DataPager1"] > a').each(function () { $(this).wrap('<li/>') });
    $('[id $= "DataPager1"] > span').each(function () { $(this).wrap('<li class="active"></li>') });
    $('[id $= "DataPager1"]').each(function () { $(this).wrapInner('<ul class="pagination"></ul>'); });

    $('.cboxList.inline > input').each(function () { $(this).next('label').andSelf().wrapAll('<div class="checkbox checkbox-primary checkbox-inline">'); });
    $('.cboxList > input').each(function () { $(this).next('label').andSelf().wrapAll('<div class="checkbox checkbox-primary">'); });

    $('.rbl.inline > input').each(function () { $(this).next('label').andSelf().wrapAll('<div class="radio radio-primary radio-inline">'); });
    $('.rbl > input').each(function () { $(this).next('label').andSelf().wrapAll('<div class="radio radio-primary">'); });
    resizeContent();
}

$(window).load(function () {
    init();
});
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
        //var ifrm = document.getElementById('myIframe2');

        var ifrm = document.createElement('iframe');
        ifrm.name = "frame1";
        ifrm.style.position = "absolute";
        ifrm.style.top = "-1000000px";
        document.body.appendChild(ifrm);

        ifrm = (ifrm.contentWindow) ? ifrm.contentWindow : (ifrm.contentDocument.document) ? ifrm.contentDocument.document : ifrm.contentDocument;
        ifrm.document.open();
        ifrm.document.write('<!doctype html><html>');
        ifrm.document.write('<head><link rel="stylesheet" href="/uploads/pdf.css"></head>');
        ifrm.document.write('<body>');
      

        if (printHeader) {
            ifrm.document.write('<div><img src="/uploads/Logos/BranchLogo' + branchId+'.png" width="63" height="40" /></div>');

        }
        ifrm.document.write('<h1>' + h1 + '</h1>');
        ifrm.document.write('<h2> Report Generated on ' + reportDate + '</h2><hr/>');


        ifrm.document.write(beforehtml + divToPrint.outerHTML + afterhtml);
        ifrm.document.write('</body>');
        ifrm.document.write('</html>');

        ifrm.document.close();
        setTimeout(function () {
            ifrm.print();
            document.body.removeChild(ifrm);
        }, 1000);
    }
}

