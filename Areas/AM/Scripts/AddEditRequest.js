$('.move>input').keydown(function (e) {
    if (e.keyCode == 39) {
        e.preventDefault();
        $(this).parent().next().children().focus();
    }
    if (e.keyCode == 37) {
        e.preventDefault();
        $(this).parent().prev().children().focus();
    }
    if (e.keyCode == 38) {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex)
        rows.find('td').eq(tdIndex).find('input').focus();
        //$(this).parent().next().children().focus();
    }
    if (e.keyCode == 40) {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex + 2)
        rows.find('td').eq(tdIndex).find('input').focusin();
        //$(this).parent().prev().children().focus();
    }
});
$('.move>select').keydown(function (e) {
    if (e.keyCode == 39) {
        e.preventDefault();
        $(this).parent().next().children().focus();
    }
    if (e.keyCode == 37) {
        e.preventDefault();
        $(this).parent().prev().children().focus();
    }
    if (e.keyCode == 38) {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex)
        rows.find('td').eq(tdIndex).find('select').focus();
        //$(this).parent().next().children().focus();
    }
    if (e.keyCode == 40) {
        e.preventDefault();
        var trIndex = $(this).closest('tr').index();
        var tdIndex = $(this).closest('td').index();
        var rows = $('#trans_details tr').eq(trIndex + 2)
        rows.find('td').eq(tdIndex).find('select').focus();
        rows.find('td').eq(tdIndex).find('select').next().addClass('select2-container--below select2-container--focus ');
        //$(this).parent().prev().children().focus();
    }
});

function printGV() {
    $("#PrintSection").removeClass("displayNone");
    $("#PrintSection").printThis();
    $("#PrintSection").addClass("displayNone");
}
$(document).keydown(function (event) {
    if ((event.which == 83) && (event.ctrlKey)) {
        event.preventDefault();
        $("#ReqSaveLbl").click();
    }
    if ((event.which == 80) && (event.ctrlKey)) {
        event.preventDefault();
        $("#ReqPrintLbl").click();
    }
});
