//clone row
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
    $clone.find('select').select2({ width: 'resolve' }).end()
        .find('.datepicker').datepicker({ dateFormat: 'dd/mm/yy' });
    return $clone;
}

$('#trans_details > tbody > tr').on('blur', '.cloneable', function () {
    var count = $('table#trans_details > tbody > tr').length;
    var $parent_tr = $(this).closest('tr');
    var index = $parent_tr.index();
    if (index == count - 1) {
        var $row = $.fn.cloneRow($('#tr_clone'), 'table#trans_details > tbody');
    }
});


var $row = $.fn.cloneRow($('#tr_clone'), 'table#trans_details > tbody');