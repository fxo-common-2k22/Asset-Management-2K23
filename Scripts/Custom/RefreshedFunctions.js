function RemoveTimeFromDate(date) {
    if (date) {
        var d = stringTimeToDate(date, "dd/MM/yyyy", "/");
        if (isNaN(d.getTime()))
            d = stringToDate(date, "dd/MM/yyyy", "/");
        var dd = d.getDate();
        var mm = d.getMonth() + 1;
        if (dd < 10) {
            dd = '0' + dd;
        }
        if (mm < 10) {
            mm = '0' + mm;
        }
        return [dd, '/', mm, '/', d.getFullYear()].join(''); // padding
    }
}
function stringTimeToDate(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    _date = _date.substring(0, _date.indexOf(' '));
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    month -= 1;
    var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
    return formatedDate;
}

function GetDate() {
    var d = new Date();
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];

    var mmm = months[d.getMonth()]; // getMonth() is zero-based
    var dd = d.getDate();
    if (dd < 10) {
        dd = '0' + dd;
    }

    return [dd, '-', mmm, '-', d.getFullYear()].join(''); // padding
};

function GetDay() {
    var d = new Date();
    var weekday = new Array(7);
    weekday[0] = "Sunday";
    weekday[1] = "Monday";
    weekday[2] = "Tuesday";
    weekday[3] = "Wednesday";
    weekday[4] = "Thursday";
    weekday[5] = "Friday";
    weekday[6] = "Saturday";
    var day = weekday[d.getDay()];

    return day; // padding
};
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }
    return true;
}

function base64ArrayBuffer(arrayBuffer) {
    var base64 = ''
    var encodings = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/'

    var bytes = new Uint8Array(arrayBuffer)
    var byteLength = bytes.byteLength
    var byteRemainder = byteLength % 3
    var mainLength = byteLength - byteRemainder

    var a, b, c, d
    var chunk

    // Main loop deals with bytes in chunks of 3
    for (var i = 0; i < mainLength; i = i + 3) {
        // Combine the three bytes into a single integer
        chunk = (bytes[i] << 16) | (bytes[i + 1] << 8) | bytes[i + 2]

        // Use bitmasks to extract 6-bit segments from the triplet
        a = (chunk & 16515072) >> 18 // 16515072 = (2^6 - 1) << 18
        b = (chunk & 258048) >> 12 // 258048   = (2^6 - 1) << 12
        c = (chunk & 4032) >> 6 // 4032     = (2^6 - 1) << 6
        d = chunk & 63               // 63       = 2^6 - 1

        // Convert the raw binary segments to the appropriate ASCII encoding
        base64 += encodings[a] + encodings[b] + encodings[c] + encodings[d]
    }

    // Deal with the remaining bytes and padding
    if (byteRemainder == 1) {
        chunk = bytes[mainLength]

        a = (chunk & 252) >> 2 // 252 = (2^6 - 1) << 2

        // Set the 4 least significant bits to zero
        b = (chunk & 3) << 4 // 3   = 2^2 - 1

        base64 += encodings[a] + encodings[b] + '=='
    } else if (byteRemainder == 2) {
        chunk = (bytes[mainLength] << 8) | bytes[mainLength + 1]

        a = (chunk & 64512) >> 10 // 64512 = (2^6 - 1) << 10
        b = (chunk & 1008) >> 4 // 1008  = (2^6 - 1) << 4

        // Set the 2 least significant bits to zero
        c = (chunk & 15) << 2 // 15    = 2^4 - 1

        base64 += encodings[a] + encodings[b] + encodings[c] + '='
    }

    return base64
}

function OnLoad() {
    $.ajax({
        type: 'GET',
        url: '/Home/loadgif',
        success: function (responce) {
        }
    })
}

function ClosedByDefault() {
    var $el = $(".content-closed"),
        content = $el.parents('.box').find(".box-content");
    content.slideToggle('fast', function () {
        $el.find("i").toggleClass('icon-angle-up').toggleClass("icon-angle-down");
        if (!$el.find("i").hasClass("icon-angle-up")) {
            if (content.hasClass('scrollable')) slimScrollUpdate(content);
        } else {
            if (content.hasClass('scrollable')) destroySlimscroll(content);
        }
    });
};

function stringToDate(_date, _format, _delimiter) {
    try {
        var formatLowerCase = _format.toLowerCase();
        var formatItems = formatLowerCase.split(_delimiter);
        var dateItems = _date.split(_delimiter);
        var monthIndex = formatItems.indexOf("mm");
        var dayIndex = formatItems.indexOf("dd");
        var yearIndex = formatItems.indexOf("yyyy");
        var month = parseInt(dateItems[monthIndex]);
        month -= 1;
        var formatedDate = new Date(dateItems[yearIndex], month, dateItems[dayIndex]);
        return formatedDate;
    } catch (e) {
        console.log(e);
    }    
}

function setDateValidation(_date, _format, _delimiter) {
    var formatLowerCase = _format.toLowerCase();
    var formatItems = formatLowerCase.split(_delimiter);
    var dateItems = _date.split(_delimiter);
    var monthIndex = formatItems.indexOf("mm");
    var dayIndex = formatItems.indexOf("dd");
    var yearIndex = formatItems.indexOf("yyyy");
    var month = parseInt(dateItems[monthIndex]);
    //month -= 1;
    var formatedDate = [month, '/', dateItems[dayIndex], '/', dateItems[yearIndex]].join('');
    return formatedDate;
}



function GetCurrentDate() {
    var d = new Date();
    var dd = d.getDate();
    var mm = d.getMonth() + 1;
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    return [dd, '/', mm, '/', d.getFullYear()].join(''); // padding
}

function GetCurrentTime() {
    var d = new Date();
    var tt = d.getTime();
    return tt; // padding
}




function DateToString(d) {
    //var d = new Date();
    var dd = d.getDate();
    var mm = d.getMonth() + 1;
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    return [dd, '/', mm, '/', d.getFullYear()].join(''); // padding
}

function DateDifference(fromDate, toDate) {
    try {
        var timeDiff = Math.abs(toDate.getTime() - fromDate.getTime());
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        if (diffDays == 0)
            return diffDays + 1;
        else
            return diffDays;
    } catch (e) {
        console.log(e);
    }    
}

function DateDifferenceInDays(fromDate, toDate) {
    try {
        //var timeDiff = Math.abs(toDate.getTime() - fromDate.getTime());
        var timeDiff = toDate.getTime() - fromDate.getTime();
        var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        if (diffDays == 0)
            return diffDays + 1;
        else
            return diffDays;
    } catch (e) {
        console.log(e);
    }
}

function DateDayDifference(fromDate, toDate) {
    var diff = new Date(toDate - fromDate);
    days = diff / 1000 / 60 / 60 / 24;
    return days;
}

function RefreshAllScripts() {
    $('.datePicker,.datepicker').datetimepicker({
        //datepicker: false,
        timepicker: false,
        format: 'd/m/Y'
    });

    $('.timepicker').datetimepicker({
        timepicker: true,
        datepicker: false,
        format: 'H:i',
    });

    $(".datepicker").each(function () {
        $(this).val(RemoveTimeFromDate($(this).val()));
    });

    $('.chosen-select').not('.nochosen').select2();
    //try { $('select').not('.mtz-monthpicker').not('.nochosen').not('.ajax').removeClass('form-control').select2({ width: 'resolve' }); } catch (ex) { }

    $('input[type=text], textarea').each(function (i, ele) {
        $(this).addClass('form-control');
    });
    $('#trans_details > tbody > tr').on('blur', '.cloneable', function () {
        var count = $('table#trans_details > tbody > tr').length;
        var $parent_tr = $(this).closest('tr');
        var index = $parent_tr.index();
        if (index == count - 1) {
            var $row = $.fn.cloneRow($('#tr_clone'), 'table#trans_details > tbody');
        }
    });
    $('#trans_details2 > tbody > tr').on('blur', '.cloneable', function () {
        var count = $('table#trans_details2 > tbody > tr').length;
        var $parent_tr = $(this).closest('tr');
        var index = $parent_tr.index();
        if (index == count - 1) {
            var $row = $.fn.cloneRow($('#tr_clone2'), 'table#trans_details2 > tbody');
        }
    });
}

function OnFailure(responce) {
    DisplayMessage("error", "Failed! Something went wrong while Processing your request ...!");
}

function getCurrentTime() {
    var dt = new Date();
    return dt.getHours() + ":" + dt.getMinutes();
}

function getCurrentDateTime() {
    var dt = new Date();
    return GetCurrentDate() + " " + dt.getHours() + ":" + dt.getMinutes();
}

function GetAreaNameFromUrl(str,char, occurrence) {
    tokens = str.split(char).slice(occurrence);
    return tokens[0];
}
function SetTopMenuActive1() {
    $('.main-nav li a').each(function () {
        var currentURL = window.location.href;
        var id = window.location.href.substr(window.location.href.lastIndexOf('/') + 1)
        if (StringToFloat(id) > 0) {
            currentURL = window.location.href.substring(0, window.location.href.lastIndexOf('/'))
        }
        if (currentURL == $(this).prop('href')) {
            console.log($(this).prop('href'));
            $(this).closest('.menu-li').addClass('active');
            return;
        }
    });
}

function SetTopMenuActive() {
    var currentArea = GetAreaNameFromUrl(window.location.pathname, '/', '1');
    var liArea = '';
    $('.main-nav li a').each(function () {
        liArea = GetAreaNameFromUrl($(this).prop('href'), '/', '3')
        if (currentArea == liArea) {
            console.log(currentArea);
            //$(this).closest('.menu-li').addClass('active');
            $(this).closest('li').addClass('active');
            return false;
        }
    });
}

function calculateTime(valuestart, valuestop, IsSingleDay) {
    if (typeof IsSingleDay !== "undefined") {
        if (IsSingleDay === "true") {
            var hrStart = new Date("01/01/2018 " + valuestart).getHours();
            var minStart = new Date("01/01/2018 " + valuestart).getMinutes();
            var hrEnd = new Date("01/01/2018 " + valuestop).getHours();
            var minEnd = new Date("01/01/2018 " + valuestop).getMinutes();
            var hourDiff = hrEnd - hrStart;
            var minDiff = minEnd - minStart;
            return hourDiff + '.' + minDiff;
        } else {
            var hrStart = new Date("01/01/2018 " + valuestart).getHours();
            var minStart = new Date("01/01/2018 " + valuestart).getMinutes();
            var hrEnd = new Date("02/01/2018 " + valuestop).getHours();
            var minEnd = new Date("02/01/2018 " + valuestop).getMinutes();
            var hourDiff = hrEnd - hrStart;
            var minDiff = minEnd - minStart;
            return hourDiff + '.' + minDiff;
        }
    }
}
function removeDisabledAttr() {
    $('input,select').removeAttr('disabled');
}
function setpaging() {
    $('#targetContainer').addClass('current-theme');
    var text = $("#Paging")
.clone()    //clone the element
.children() //select all the children
.remove()   //remove all the children
.end()  //again go back to selected element
.text();
    try {
        var ul1 = $("#Paging").clone().children()[0].outerHTML;
        if ($("#Paging").clone().children().length > 1) {
            var ul2 = $("#Paging").clone().children()[1].outerHTML;
            if ($('.pagingalign').length > 0) {
                $('#Paging').html(ul1 + ul2);
            } else {
                $('#Paging').html('<div class="pagingalign">' + text + '</div>' + ul1);
            }
        }
        else {
            $('#Paging').html('<div class="pagingalign">' + text + '</div>' + ul1);
        }
        if ($('.pagination-container').is(':visible') == false) {
            $('.pagination-container').show();
        }
    } catch (err)
    { }
}