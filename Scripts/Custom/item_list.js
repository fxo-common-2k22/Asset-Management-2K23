var prevRow;
$(".hasclear").keyup(function (e) {
    var t = $(this);
    t.prev('span').toggle(Boolean(t.val()));
    if (t.val()) {
        t.removeClass('filter_text_out');
        t.addClass('filter_text_in');
    } else {
        t.removeClass('filter_text_in');
        t.addClass('filter_text_out');
    }

    if (e.altKey && e.keyCode == 67) {
        t.removeClass('filter_text_in');
        t.addClass('filter_text_out');
        t.val('').focus();
        $("#btnListSearch").trigger("click");
    }
});

$(".hasclear").keypress(function (e) {
    var t = $(this);
    if (e.keyCode == 13) {
        $("#btnListSearch").trigger("click");
    }
});

$(".clearer").hide($(this).next('input').val());

$(".clearer").click(function () {
    $(this).next('input').val('').focus();
    $(this).hide();
    $(this).next('input').removeClass('filter_text_in');
    $(this).next('input').addClass('filter_text_out');
    $("#btnListSearch").trigger("click");
});

//Item list Events
(function () {
    var tbl = document.getElementById("ItemTable"),
        trows = tbl.rows;

    tbl.onclick = function (e) {
        var clicked = e.target;
        if (clicked.tagName == 'TD') {
            highlightRow(clicked.parentNode);
        }
        if (clicked.tagName == 'TH') { //highlight first row if header is clicked
            highlightRow(trows[1]);
        }
    }

    $(document).on('keydown', '#PatientModel', function (e) {
        if (!prevRow) {
            prevRow = tbl.rows[0];
        }
        row = $('#ItemTable').find('tr').eq(prevRow.rowIndex);
        switch (e.keyCode) {
            case 38: // UP arrow
                if (row.length) {
                    container.scrollTop(row.offset().top - container.offset().top + container.scrollTop() - 150);
                    if (prevRow.rowIndex === 1)
                        container.scrollTop(-1);
                }
                if (prevRow.rowIndex === 1) { 
                    if (prevRow) { 
                        prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
                        prevRow = null;
                    }
                    $('#PatientSerach').focus();
                }
                else { 
                    highlightRow(trows[prevRow.rowIndex - 1]);
                }
                break;
            case 40: // DOWN arrow
                if (row.length) {
                    var height = container.scrollTop();
                    var conoffset = container.offset().top;
                    var rowoffset = row.offset();
                    if (row.offset().top >= container.height() - 50 || row.offset().top < -1 || container.offset().top < -1)
                        container.scrollTop(row.offset().top - container.offset().top + container.scrollTop());
                    if (prevRow.rowIndex === trows.length - 1)
                        container.scrollTop(0);
                }
                if (prevRow.rowIndex === trows.length - 1) { //higlight first row
                    highlightRow(trows[1]);
                }
                else { //highlight next row
                    if (prevRow.rowIndex == -1)
                        highlightRow(trows[1]);
                    else
                        highlightRow(trows[prevRow.rowIndex + 1]);
                }
                break;
            case 13: // Enter key
                if (e.currentTarget.activeElement.attributes.id.nodeValue === 'PatientModel') {
                    var j = $('#ItemTable tr.highlighted').find("td:first").text();
                    if (j) {
                        $('#sItemBarcode').val(j);
                        findByBarcode(j);
                        $('#PatientModel').modal('hide');
                    }
                }
                break;
            case 113: // F2 Key
                if (e.currentTarget.activeElement.attributes.id.nodeValue === 'PatientModel') {
                    document.onkeydown = null;
                    if (prevRow) {
                        prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
                        prevRow = null;
                    }
                    $('#PatientSerach').focus();
                }
                break;
        }
    });

    $('#PatientModel').on('focus', function (e) {
        var container = $('#PatientModelScroll');
        var row;
        document.onkeydown = function (e) {
            e.preventDefault();
            e.stopPropagation();
            row = $('#ItemTable').find('tr').eq(prevRow.rowIndex);
            switch (e.keyCode) {
                case 38: // UP arrow
                    if (row.length) {
                        container.scrollTop(row.offset().top - container.offset().top + container.scrollTop() - 150);
                        if (prevRow.rowIndex === 1)
                            container.scrollTop(-1);
                    }
                    if (prevRow.rowIndex === 1) { //higlight last row
                        //highlightRow(trows[trows.length - 1]);
                        if (prevRow) { //you may want to remove highlight when table gets out of focus (remove this block if not needed)
                            prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
                            prevRow = null;
                        }
                        $('#PatientSerach').focus();
                    }
                    else { //highlight previous row
                        highlightRow(trows[prevRow.rowIndex - 1]);
                    }
                    break;
                case 40: // DOWN arrow
                    if (row.length) {
                        var height = container.scrollTop();
                        var conoffset = container.offset().top;
                        var rowoffset = row.offset();
                        if (row.offset().top >= container.height() - 50 || row.offset().top < -1 || container.offset().top < -1)
                            container.scrollTop(row.offset().top - container.offset().top + container.scrollTop());
                        if (prevRow.rowIndex === trows.length - 1)
                            container.scrollTop(0);
                    }
                    if (prevRow.rowIndex === trows.length - 1) { //higlight first row
                        highlightRow(trows[1]);
                    }
                    else { //highlight next row
                        if (prevRow.rowIndex == -1)
                            highlightRow(trows[1]);
                        else
                            highlightRow(trows[prevRow.rowIndex + 1]);
                    }
                    break;
                case 13: // Enter key
                    if (e.currentTarget.activeElement.attributes.id.nodeValue === 'PatientModel') {
                        var j = $('#ItemTable tr.highlighted').find("td:first").text();
                        if (j) {
                            $('#sItemBarcode').val(j);
                            findByBarcode(j);
                            $('#PatientModel').modal('hide');
                        }
                    }
                    break;
                case 113: // F2 Key
                    if (e.currentTarget.activeElement.attributes.id.nodeValue === 'PatientModel') {
                        document.onkeydown = null;
                        if (prevRow) { //you may want to remove highlight when table gets out of focus (remove this block if not needed)
                            prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
                            prevRow = null;
                        }
                        $('#PatientSerach').focus();
                    }
                    break;
            }
        }
    });
    tbl.onblur = function (e) {
        //remove onkeydown when the table gets out of focus
        document.onkeydown = null;
        if (prevRow) { //you may want to remove highlight when table gets out of focus (remove this block if not needed)
            prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
            prevRow = null;
        }
    }
    tbl.ondblclick = function (e) {
        $('#btnListOk').trigger('click');
    }
    var container = $('#PatientModelScroll');
    $('#PatientModel').on('blur', function (e) {
        //remove onkeydown when the table gets out of focus
        document.onkeydown = null;
        if (prevRow) { //you may want to remove highlight when table gets out of focus (remove this block if not needed)
            //prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
            //prevRow = null;
        }

    });
})();
//End Item List Events

function LoadItemList_OnClick(search, url) {
    $.ajax({
        type: 'GET',
        dataType: 'json',
        data: { search: search },
        contentType: 'application/json',
        url: url,
        success: function (result) {
            BindItemList(result);
        },
        beforeSend: function () {
            $('#ItemTable > tbody').html("");
            $('.EmptyData').hide();
            $(".loading").show();
        },
        complete: function () {
            $(".loading").hide();
        }
        //,
        //error: function (XMLHttpRequest, textStatus, errorThrown) {
        //    ShowMessageModal('Server Error', 'Server is not responding right now');
        //    //alert("Status: " + textStatus); alert("Error: " + errorThrown);
        //}
    })
};

function BindItemList(d) {
    var data = '';
    $('#ItemTable').html("");
    $("#ItemTable").append("<tr><th>MR No</th><th>Name</th><th>Phone</th><th>Blood Group</th></tr >");
    $.each(d, function (e) {
        data += "<tr>"
            + "<td class='PatientId hidden'>" + this.PatientId + "</td>" 
            + "<td class='MRNo'>" + this.MRNo + "</td>" 
            + "<td class='PatientName'>" + this.PatientName + "</td>" 
            + "<td class='Phone'>" + this.Phone + "</td>" 
            + "<td class='BloodGroup'>" + this.BloodGroup + "</td>"
            + "</tr>";
    });
    
    if (d.length == 0) {
        $('.EmptyData').show();
    } else {
        $("#ItemTable").append(data);
    }
}


function highlightRow(rowObj) {
    if (prevRow) { //remove previously highlighted row
        prevRow.className = prevRow.className.replace(/(?:^|\s)highlighted(?!\S)/, '');
        //$('#ItemTable tr').removeClass('highlighted');
    }
    rowObj.className += ' highlighted';
    prevRow = rowObj;
    //$('#PatientSearch').blur();
}


