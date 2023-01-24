$('.datepicker').datetimepicker({
    timepicker: true,
    format: 'd/m/Y H:i'
});


function getFormattedDate() {
    var today = new Date();
    var dd = today.getDate();
    var mm = today.getMonth() + 1;
    if (dd < 10) {
        dd = '0' + dd;
    }
    if (mm < 10) {
        mm = '0' + mm;
    }
    var date = dd + '/' + mm + '/' + today.getFullYear();
    var time = today.getHours() + ":00";
    var dateTime = date + ' ' + time;
    $('.datepicker').val(dateTime);
}

function Isvalid() {
    var validated = true;
    if (!$('[id $= PatientId]').val().length) {
        $('[id $= PatientId]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Select Patient').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    if (!$('[id $= ServiceTypeId]').val().length) {
        $('[id $= ServiceTypeId]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Select any service Type').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    if (!$('[id $= SpecializationId]').val().length) {
        $('[id $= SpecializationId]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Specialization / Department is Required').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    if (!$('[id $= ServiceId]').val().length) {
        $('[id $= ServiceId]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Select any service').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    if (!$('[id $= AppointmentDate]').val().length) {
        $('[id $= AppointmentDate]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Date is Required').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    
    if (!$('[id $= DoctorId]').val().length) {
        $('[id $= DoctorId]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Doctor is Required').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    if (!$('[id $= Amount]').val().length) {
        $('[id $= Amount]').focus();
        $('#Message_lbl').css('color', 'red');
        $('#Message_lbl').text('Amount is Required').show().delay(2000).fadeOut();
        validated = false;
        return false;
    }
    else {
        if ($('[id $= Amount]').val() <= 0) {
            $('[id $= Amount]').focus();
            $('#Message_lbl').css('color', 'red');
            $('#Message_lbl').text('Amount must be greater than Zero').show().delay(2000).fadeOut();
            validated = false;
            return false;
        }
    }
    return validated;
}

$("#Patient").enterKey(function (e) {
    e.preventDefault();
    Patientlist();
    $("#PatientModel").modal();
})
var url = '/Clinic/OPD/SearchPatients';
var Patientlist = function (e) {
    $('#PatientModel').on('shown.bs.modal', function () {
        var modal = $(this);
        var focusableChildren = modal.find('input');
        var numElements = focusableChildren.length;
        var currentIndex = 0;

        $(document.activeElement).blur();

        var focus = function () {
            var focusableElement = focusableChildren[currentIndex];
            if (focusableElement)
                focusableElement.focus();
        };
        var focusPrevious = function () {
            currentIndex--;
            if (currentIndex < 0)
                currentIndex = numElements - 1;
            focus();
            return false;
        };

        var focusNext = function () {
            currentIndex++;
            if (currentIndex >= numElements)
                currentIndex = 0;
            focus();
            return false;
        };

        focus();

        $(document).on('keydown', function (e) {
            if (e.keyCode === 9 && e.shiftKey) {
                e.preventDefault();
                focusPrevious();
            }
            else if (e.keyCode === 9) {
                e.preventDefault();
                focusNext();
            }
            else if (e.keyCode === 27) {
                e.preventDefault();
                $("#btnListClose").trigger("click");
            }
            else if (e.altKey && e.keyCode === 79) {
                e.preventDefault();
                $("#btnListOk").trigger("click");
            }
            else if (e.altKey && e.keyCode === 83) {
                e.preventDefault();
                $("#btnListSearch").trigger("click");
            }
        });
    });
    $('#PatientModel').on('hidden.bs.modal', function () {
    });
    $("#PatientModel").modal({ backdrop: 'static', keyboard: false });
    LoadItemList_OnClick($('#PatientSerach').val(), url);
};

$('#btnListClose').on('click', function (e) {
    $('#PatientModel').modal('hide');
    $('#loading').hide();
    $('[id $= Patient]').focus();
});

$('#btnListSearch').on('click', function (e) {
    LoadItemList_OnClick($('#PatientSerach').val(), url);
});


var PatientId = 0, MRNo = '', PatientName = '';

function selectTextFromList() {
    PatientId = $('#ItemTable tr.highlighted').find("td.PatientId").html();
    MRNo = $('#ItemTable tr.highlighted').find("td.MRNo").html();
    PatientName = $('#ItemTable tr.highlighted').find("td.PatientName").html();
    $('[id $= PatientId]').val(PatientId);
    $('[id $= Patient]').val(MRNo + "-" + PatientName);

    if (PatientId)
        return true;
    else
        return false;
}

$('#btnListOk').on('click', function (e) {
    if (selectTextFromList()) {
        $('#PatientModel').modal('hide');
        PatientAppointments($('[id $= PatientId]').val());
    }
});

function PatientAppointments(PatientId) {
    $.get('/Clinic/OPD/LoadPatientAppointments', { id: PatientId }, function (d) {
        $('#AppointmentDiv').empty();
        $.each(d, function () {
            var date = setFormattedDate(parseJsonDate(this.AppointmentDate));
            $("#AppointmentDiv").append("<a id=\"HyperLink1\"title=" + this.OPDNo + " " + date + " href=/Clinic/OPD/Edit/" + this.OPDId + ">" + this.OPDNo + " " + date + "</a><br/><br/>");
        });
    });
}

function parseJsonDate(jsonDateString) {
    return new Date(parseInt(jsonDateString.replace('/Date(', '')));
}

function setFormattedDate(today) {
    var dd = today.getDate();
    if (dd < 10) {
        dd = '0' + dd;
    }
    var months = ["Jan", "Feb", "Mar", "Apr", "May", "June", "July", "Aug", "Sep", "Oct", "Nov", "Dec"];
    var mm = months[today.getMonth()];
    var date = dd + ' ' + mm + ' ' + today.getFullYear();
    var time = today.getHours() + ":" + today.getMinutes();
    var dateTime = date + ' ' + time;
    return dateTime;
}

$("#PatientSerach").enterKey(function (e) {
    e.preventDefault();
    $('#btnListSearch').trigger('click');
})