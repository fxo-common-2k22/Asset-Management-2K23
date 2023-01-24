//get data from actionresult and append it to modal

$(document).ready(function () {
});

function formatCurrency(total) {
    var neg = false;
    if (total < 0) {
        neg = true;
        total = Math.abs(total);
    }
    return (neg ? "-" : '') + parseFloat(total, 10).toFixed(2).replace(/(\d)(?=(\d{3})+\.)/g, "$1,").toString();
}

function customBinding() {
    $(".customBinding").each(function () {
        let val = $(this).data('val');
        $(this).val(val);
    })
}

function GetMobileNosForSms(durl) {
    $.ajax({
        url: durl,
        method: "GET",
        data: {}
    }).success(function (data) {
        if (Number(data.IssSuccess) !== 1) {
            $("#valMsg").html(data.Error);
            $("#addBtn").hide();
            $("#btnScheduleSMS").hide();
            $("#btnSendNotification").hide();
            $("#SendMessage_MobNos").val('');

            //toastr.options = {
            //    "closeButton": true,
            //    "debug": false,
            //    "newestOnTop": true,
            //    "progressBar": true,
            //    "positionClass": "toast-bottom-right",
            //    "showDuration": "300",
            //    "hideDuration": "1000",
            //    "timeOut": "5000",
            //    "extendedTimeOut": "1000",
            //    "showEasing": "swing",
            //    "hideEasing": "linear",
            //    "showMethod": "fadeIn",
            //    "hideMethod": "fadeOut"
            //}
            ErrorSuccess(data);
        } else {
            $("#addBtn").show();
            $("#btnScheduleSMS").show();
            $("#btnSendNotification").show();
            $("#SendMessage_MobNos").val(data.MobNo);
            $("#valMsg").html("");
        }

    }).fail(function (jqXHR, textStatus) {
        toastr.clear();
        toastr.error(textStatus, 'Failed');
    });
}

function GetCascadingDropDown(url, optiontext, targetId, targetClass) {
    $.ajax({
        url: url,
        method: "Get",
        success: function (data) {
            if (targetId != undefined && targetId != '') {
                $("#" + targetId).empty();
                if (optiontext != '') {
                    $("#" + targetId).append("<option value = ''>" + optiontext + "</option>");
                }
                for (var i = 0; i < data.length; i++) {
                    $("#" + targetId).append("<option value=" + data[i].Value + ">" + data[i].Text + "</option>")
                }
            }
            if (targetClass != undefined && targetClass != '') {
                $("." + targetClass).empty();
                if (optiontext != '') {
                    $("." + targetClass).append("<option value = ''>" + optiontext + "</option>");
                }
                for (var i = 0; i < data.length; i++) {
                    $("." + targetClass).append("<option value=" + data[i].Value + ">" + data[i].Text + "</option>")
                }
            }
        }
    });
}

function ConfirmBox(func, boxTitle, boxMessage, par1) {
    boxTitle = (typeof boxTitle !== 'undefined') ? boxTitle : 'Are You Sure ?';
    boxMessage = (typeof boxMessage !== 'undefined') ? boxMessage : 'You want to proceed ?';
    bootbox.confirm({
        title: boxTitle,
        message: boxMessage,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm'
            }
        },
        callback: function (response) {
            if (response) {
                func(par1);
            }
        }
    });
}

function GetDataToEdit(id, url, modalId, targetId) {
    $.ajax({
        url: url + "/" + id,
        method: "GET",
    }).done(function (data) {
        $("#" + modalId).modal("show");
        $("#" + targetId).html(data);
        $.getScript("/scripts/eakroko.min.js");
        //$.getScript("/Theme/js/plugins/icheck/jquery.icheck.min.js");


    });
}

function GetModalData(id, url, modalId, targetId) {
    $.ajax({
        url: url + "/" + id,
        method: "GET",
    }).done(function (data) {
        $("#" + modalId).modal("show");
        $(this).find('[autofocus]').focus();
        $("#" + targetId).html(data.PartialView);
        $.getScript("/scripts/eakroko.min.js");
        //$.getScript("/Theme/js/plugins/icheck/jquery.icheck.min.js");
    });
}

var pageNo = new function () {
    this.page = 1;
    this.getPage = function () {
        return this.page;
    };
}

function DeleteRow(id, url, targetId, boxTitle, boxMessage) {
    boxTitle = (typeof boxTitle !== 'undefined') ? boxTitle : 'Delete Record?';
    boxMessage = (typeof boxMessage !== 'undefined') ? boxMessage : 'Are you sure you want to delete this record?';
    bootbox.confirm({
        title: boxTitle,
        message: boxMessage,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm'
            }
        },
        callback: function (response) {
            if (response) {
                $.ajax({
                    url: url,
                    method: "POST",
                    data: { id: id },
                }).success(function (data) {
                    //$.getScript("/scripts/eakroko.min.js");
                    $("#" + targetId).html(data.PartialView);
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
                    ErrorSuccess(data);
                }).fail(function (jqXHR, textStatus) {
                    toastr.clear();
                    toastr.error(textStatus, 'Failed')
                });
            }
        }
    });
}


function DeleteRowPaged(id, url, control, boxTitle, boxMessage) {
    ; 
    boxTitle = (typeof boxTitle !== 'undefined') ? boxTitle : 'Delete Record?';
    boxMessage = (typeof boxMessage !== 'undefined') ? boxMessage : 'Are you sure you want to delete this record?';
    bootbox.confirm({
        title: boxTitle,
        message: boxMessage,
        buttons: {
            cancel: {
                label: '<i class="fa fa-times"></i> Cancel'
            },
            confirm: {
                label: '<i class="fa fa-check"></i> Confirm'
            }
        },
        callback: function (response) {
            if (response) {
                $.ajax({
                    url: url,
                    method: "POST",
                    data: { id: id },
                }).success(function (data) {
                    //$.getScript("/scripts/eakroko.min.js");
                    //$("#" + targetId).html(data.PartialView);
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
                    if (data.status === false) {
                        toastr.clear();
                        toastr.error(data.msg, 'Failed')
                    }
                    else {
                        $(control).closest('tr').remove();
                        // Used when we want to render multiple partialViews From a controller action
                        if (jQuery.type(data.PartialList) != "undefined") {
                            $.each(data.PartialList, function (index, val) {
                                if (val.IdToHideModalPopup) {
                                    $("#" + val.IdToHideModalPopup).modal('hide');
                                    if ($('.modal-backdrop.fade.in').length > 0) {
                                        $('.modal-backdrop.fade.in').remove();
                                    }
                                }
                                $('#' + val.divToReplace).html(val.partialData);
                                if (val.IdToShowModalPopup) {
                                    $("#" + val.IdToShowModalPopup).modal();
                                    $("#" + val.IdToShowModalPopup).removeAttr('tabindex');
                                }
                                if (val.ElementIdToSetFocus) {
                                    ElementIdToSetFocus = val.ElementIdToSetFocus;
                                    setTimeout(function () {
                                        $("#" + val.ElementIdToSetFocus).focus();
                                        //$("#" + val.ElementIdToSetFocus).select2('open');
                                    }, 1000);
                                }


                            });
                        }
                        toastr.clear();
                        toastr.success(data.Msg, 'Success');

                    }
                    //ErrorSuccess(data);
                }).fail(function (jqXHR, textStatus) {
                    toastr.clear();
                    toastr.error(textStatus, 'Failed')
                });
            }
        }
    });
}


function CustomResponseFromServer(responce) {
    ; 
    if (responce.status == true) {
        if (responce.msg) {
            if (jQuery.type(responce.PartialList) != "undefined") {
                $.each(responce.PartialList, function (index, val) {
                    if (val.IdToHideModalPopup) {
                        $("#" + val.IdToHideModalPopup).modal('hide');
                        if ($('.modal-backdrop.fade.in').length > 0) {
                            $('.modal-backdrop.fade.in').remove();
                        }
                    }
                    $('#' + val.divToReplace).html(val.partialData);
                    if (val.IdToShowModalPopup) {
                        $("#" + val.IdToShowModalPopup).modal();
                        $("#" + val.IdToShowModalPopup).removeAttr('tabindex');
                    }
                    if (val.ElementIdToSetFocus) {
                        ElementIdToSetFocus = val.ElementIdToSetFocus;
                        setTimeout(function () {
                            $("#" + val.ElementIdToSetFocus).focus();
                            //$("#" + val.ElementIdToSetFocus).select2('open');
                        }, 1000);
                    }


                });
            }
            DisplayMessage('success', responce.msg);
            if (responce.redirecToPayment != "") {
                var currentURL = window.location.href;
                window.location.href = currentURL + "/" + responce.redirecToPayment;
            }

        } else {
            if (responce.HideMsg) { } else {
                DisplayMessage('success', 'Successfully updated');
            }
        }
    }
    else {
        if (responce.msg) {
            DisplayMessage("error", responce.msg);
        }
    }


}


function ErrorSuccess(data) {
    toastr.clear();
    if (data.Messages != undefined && data.Messages != '') {
        toastr.success(data.Messages == '' ? 'Record has been deleted successfully...' : data.Messages, 'Success!')
    }
    if (data.Error != undefined && data.Error != '') {
        toastr.error(data.Error == '' ? 'Something went wrong while deleting record. Please try again later.' : data.Error, 'Failed!')
    }
    if (typeof (data.Warnings) != 'undefined' && data.Warnings != undefined && data.Warnings != '') {
        toastr.warning(data.Warnings == '' ? 'Something went wrong while processing request. Please try again later.' : data.Warnings, 'Warning!')
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
    } else if (type === "Warnings") {
        toastr.clear();
        toastr.warning(message, 'Warnings');
    } else {
        toastr.clear();
        toastr.success(message, 'Success');
    }
}

function DisplayAlerts(data) {
    if (data.Messages != undefined && data.Messages != '') {
        $('#alertDivId').html('<div class="alert alert-success h3 alert-dismissable" style = "margin-bottom:5px !important"><button type="button" class="close " data-dismiss="alert">×</button> <strong class="">Success!</strong> ' + data.Messages + '</div>');
    }
    if (data.Error != undefined && data.Error != '') {
        $('#alertDivId').html('<div class="alert alert-danger h3 alert-dismissable" style = "margin-bottom:5px !important"><button type="button" class="close" data-dismiss="alert">×</button> <strong>Error!</strong> ' + data.Error + '</div>');
    }
    if (typeof (data.Warnings) != 'undefined' && data.Warnings != undefined && data.Warnings != '') {
        $('#alertDivId').html('<div class="alert alert-warning h3 alert-dismissable" style = "margin-bottom:5px !important"><button type="button" class="close" data-dismiss="alert">×</button><strong>Warning!</strong> ' + data.Warnings + '</div>');
    }
}

function DisplayNotDismissableAlerts(data) {
    if (data.Messages != undefined && data.Messages != '') {
        $('#alertDivId').html('<div class="alert alert-success h3 alert-dismissable cannotAutoClose" style = "margin-bottom:5px !important"><button type="button" class="close " data-dismiss="alert">×</button> <strong class="">Success!</strong> ' + data.Messages + '</div>');
    }
    if (data.Error != undefined && data.Error != '') {
        $('#alertDivId').html('<div class="alert alert-danger h3 alert-dismissable cannotAutoClose" style = "margin-bottom:5px !important"><button type="button" class="close" data-dismiss="alert">×</button> <strong>Error!</strong> ' + data.Error + '</div>');
    }
    if (typeof (data.Warnings) != 'undefined' && data.Warnings != undefined && data.Warnings != '') {
        $('#alertDivId').html('<div class="alert alert-warning h3 alert-dismissable cannotAutoClose" style = "margin-bottom:5px !important"><button type="button" class="close" data-dismiss="alert">×</button><strong>Warning!</strong> ' + data.Warnings + '</div>');
    }
}
function OnBegin(request) {
    toastr.info('While your request is in processprocess...', 'Please Wait!')
}
function onEditSuccess(response) {
    
    if (response.Messages != undefined && response.Messages != '') {
        OnFailure(response);
    } else {
        //Added By Muhammad Uzair Haider
        $.fn.modal.Constructor.prototype.enforceFocus = function () { };
        //
        $("#" + response.ModalId).modal('show');
        $("#" + response.TargetId).html(response.PartialView);
        //$.getScript("/scripts/eakroko.min.js");
    }
    ErrorSuccess(response);
}


function OnCreateUpdateSuccess(response) {
     
    if (response !== '') {
        if (response.PartialView !== '') {
            if (response.ModalId != undefined && response.ModalId != '') {
                OnComplete(response.ModalId);
            }
            $("#" + (response.GridId !== '' ? response.GridId : "gridId")).html(response.PartialView);
        }
        if (response.PartialViewFrm != '') {
            $("#" + (response.GridFrmId !== '' ? response.GridFrmId : "gridFrmId")).html(response.PartialViewFrm);
        }

        if (response.PartialView1 != undefined && response.GridId1 !== undefined) {
            $("#" + (response.GridId1 !== '' ? response.GridId1 : "gridId1")).html(response.PartialView1);
        }

        if (response.TotalRecords != undefined && typeof (response.TotalRecords) != 'undefined') {
            $("#hashTotalId").html(response.TotalRecords);
        }

        if (typeof (response.ShowAlerts) != 'undefined' && response.ShowAlerts != undefined && response.ShowAlerts == true) {
            DisplayAlerts(response);
        }

        if (typeof (response.ShowNotDismissableAlerts) != 'undefined' && response.ShowNotDismissableAlerts != undefined && response.ShowNotDismissableAlerts == true) {
            DisplayNotDismissableAlerts(response);
        }
        //if (typeof response.Error != 'undefined' && response.Error != '') {
        //    toastr.clear();
        //    toastr.error(response.Error, 'Failed');
        //}
        //change the current tab url
        if (typeof response.Url != 'undefined') {
            HistoryPush(response.Url);
        }

        if (response.RedirectTo != undefined && typeof response.RedirectTo != 'undefined') {
            redirectToController(response.RedirectTo);
        }
        if (response.RedirectToWithParam != undefined && typeof response.RedirectToWithParam != 'undefined' && response.Error == '') {

            redirectToControllerWithId(response.RedirectToWithParam,response.id);
        }

        ErrorSuccess(response);
    } else {
        OnFailure(response);
    }
    if (response.Reset !== "false") {
        $('form').find('input[type=datetime],input[type=text], input[type=password], input[type=number], input[type=email], textarea, select').val('');
    }
    //$.getScript("/scripts/eakroko.min.js");
}

function OnComplete(modalId) {

    $("#" + modalId).modal('hide');
    $(".modal-backdrop").removeClass("modal-backdrop");
    $("body").removeClass("modal-open");
}

function OnLinkSuccess(response) {
    if (typeof response != 'undefined' && response != null) {
        $("#" + (typeof response.GridId != 'undefined' ? response.GridId : 'pageContent')).html(response.PartialView)

        if (typeof response.Url != 'undefined') {
            HistoryPush(response.Url);
            $("#quickLinks").load('/Forms/GetQuickLinks?Url=' + response.Url, function (response, status, xhr) { })
        }

        if (typeof response.LoadJs != 'undefined' && response.LoadJs == true) {
            $.getScript("/scripts/eakroko.min.js");
        }
        ErrorSuccess(response);
    }
}
function HistoryPush(url) {
    history.pushState(null, null, url);
}
function OnFailure(response) {
    toastr.clear();
    toastr.error(response.Messages, 'Failed')
}

function onSearchSuccess(response) {
    
    if (response != '') {
        if (response.PartialView != '') {
            if (response.GridId != '') {
                $("#" + response.GridId).html(response.PartialView);
                //$("#searchbar").removeClass('.vbox');
            } else {
                $("#gridId").html(response.PartialView);
            }
        }
        if (response.TotalRecords != undefined && typeof (response.TotalRecords) != 'undefined') {
            $("#hashTotalId").html(response.TotalRecords);
        }
        if (response.PartialView1 != undefined && response.PartialView1 != '') {
            if (response.GridId1 != '') {
                $("#" + response.GridId1).html(response.PartialView1);
            } else {
                $("#gridId1").html(response.PartialView1);
            }
        }
        if (typeof response.LoadFile !== 'undefined') {
            $.getScript(response.LoadFile);
        }
        ErrorSuccess(response);

        if (typeof response.MoveUp !== 'undefined') {
            $("html, body").animate({
                scrollTop: 0
            }, 500);
        }

        if (typeof response.Url != 'undefined') {
            HistoryPush(response.Url);
        }
    } else {
        OnSearchFailure(response);
    }
}

function OnSearchFailure(response) {
    toastr.clear();
    toastr.error(response.Messages, 'Failed')
}
//function OnGoToSuccess(response) {
//    if (response != '') {
//        if (response.Messages != 'Failed') {
//            $(location).attr('href', response.Url);
//        }
//    } else {
//        OnFailure(response);
//    }
//}

$(document).on('click', '[data-action $= Delete]', function (e) {
    var result = false;
    var that = this;
    var id = $(that).data("id");
    var url = $(that).data("url");
    var branchId = $("#BranchId").val();
    if (url) {
        bootbox.confirm({
            title: 'Are you sure you want to delete this?',
            message: 'You will not be able to recover this!!!',
            buttons: {
                cancel: {
                    label: '<i class="fa fa-times"></i> Cancel'
                },
                confirm: {
                    label: '<i class="fa fa-check"></i> Confirm'
                }
            },
            callback: function (response) {
                if (response) {
                    $.get(url, { "id": id, branch: branchId }, function (d) {
                        result = d;
                        if (result !== false) {
                            $.notify({
                                icon: 'fa fa-check',
                                message: 'Record removed successfully...',
                            }, { delay: 1000, });
                            $(that).closest('tr').remove();
                        } else {
                            $.notify({
                                icon: 'fa fa-check',
                                message: "Request failed: Something went wrong while processing your request...",
                            }, { delay: 1000, });
                        }
                        return false;
                    });
                }
            }
        });
    }
    else {
        if (id === -1) {
            if ($(that).closest('tr').next('tr').length) {
                swal({
                    title: "Are you sure?",
                    text: "You won't be able to revert this!",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55",
                    confirmButtonText: "Yes, delete it!",
                    closeOnConfirm: false
                },
                    function () {
                        $(that).closest('tr').remove();
                        swal({
                            title: "Deleted",
                            text: "",
                            type: "success",
                            timer: 1000
                        })
                        //swal("Deleted!", "", "success");
                    });
            }
            return false;
        }
    }
    return false;
});


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



//get section by class Id
$(".classId").change(function () {
    $.ajax({
        url: "/Academics/Students/GetSectionsByClass/" + this.value,
        method: "Get",
        success: function (data) {
            $(".sectionId").empty();
            $(".sectionId").append("<option >--Select Section--</option>");
            for (var i = 0; i < data.length; i++) {
                $("#SectionId").append("<option value=" + data[i].Value + ">" + data[i].Text + "</option>")
            }
        }
    });
})

function CustomLoad(targetId, url, libraryPath) {
    $("#" + targetId).load(url, function (response, status, xhr) {
        if (status != 'success') {
            DisplayMessage('error', 'Something went wrong while fetching data.');
        } else {
            if (libraryPath) {
                $.getScript(libraryPath);
            }
        }
    })
}



function fadeInOutText(element, message, status) {
    if (status != undefined && status != 'error') {
        element.removeClass('text-danger ');
        element.addClass('text-success ');
        element.fadeIn().text(message);
        element.fadeOut(1500);
    } else {
        element.removeClass('text-success ');
        element.addClass('text-danger ');
        element.fadeIn().text(message);
        element.fadeOut(1500);
    }
}


//form navigation by arrow keys

(function ($) {
    $.fn.formNavigation = function () {
        $(this).each(function (ev) {
            $(this).find('.navigation').on('keyup', function (e) {
                switch (e.which) {
                    case 39: {
                        $(this).closest('td').next().find('input').focus(); break;
                    }
                    case 37: {
                        $(this).closest('td').prev().find('input').focus(); break;
                    }
                    case 40: {
                        $(this).closest('tr').next().children().eq($(this).closest('td').index()).find('input').focus(); break;
                    }
                    case 38: {
                        $(this).closest('tr').prev().children().eq($(this).closest('td').index()).find('input').focus(); break;
                    }
                }
            });
        });
    };
})(jQuery);


function redirectToController(url) {
    window.location.href = url;
}
function redirectToControllerWithId(url,id) {
    window.location.href = url + "/"+id;
}

loadCSS = function (href) {
    var cssLink = $("<link>");
    $("head").append(cssLink); //IE hack: append before setting href
    cssLink.attr({
        rel: "stylesheet",
        type: "text/css",
        href: href
    });
};

function urlContains(input) {
    return window.location.pathname.indexOf(input);
}

function printDiv(divId, divToHide) {
    $('.' + divToHide).addClass('displayNone');
    var $div = $("<div>", { id: "temp" });
    $div.click(function () { /* ... */ });
    $div.append($('#' + divId).html());
    $div.printThis({ importCSS: true, importStyle: true, loadCSS: '/Uploads/pdf.css' });
    $('.' + divToHide).removeClass('displayNone');
}

function setDDValue(id, value) {
    $('#' + id).val(value).change();
}


function GetModalDataForPrint(id, TemplateTypeId, Certificateid, url, modalId, targetId) {
    $.ajax({
        url: url + "/",
        method: "GET",
        data: {
            id: id, TemplateTypeId: TemplateTypeId, Certificateid: Certificateid
        },
    }).done(function (data) {
        $("#" + modalId).modal("show");
        $(this).find('[autofocus]').focus();
        $("#" + targetId).html(data.PartialView);
        $.getScript("/scripts/eakroko.min.js");
        $('#printa').printThis();
        $('#frmModal').modal('hide')
        //$.getScript("/Theme/js/plugins/icheck/jquery.icheck.min.js");
    });
}

function printDiv2(divId,divToAdd) {
    $('#' + divToAdd).removeClass('displayNone');
    var $div = $("<div>", { id: "temp" });
    $div.click(function () { /* ... */ });
    $div.append($('#' + divId).html());
    $div.printThis({ importCsSS: true, importStyle: true, loadCSS: '/Uploads/pdf.css' });
    $('#' + divToAdd).addClass('displayNone');
}


function InsertInMembershipForms(FormName, ParentForm, FormURL, MenuText, MenuItemPriority, isActive, PageType, IsMenuItem, IsDashboardPart, ShowOnDesktop, IsMasterMenu, IsQuickLink, IsAction, ModuleId, PageDescription) {
    if (IsAction == undefined) {
        IsAction = false;
    }
    if (ModuleId == undefined) {
        ModuleId = 0;
    }
    if (ParentForm != "" && FormName != "" && FormURL != "" && FormURL != "") {
        $.ajax({
            url: '/Home/InsertMembershipForms',
            type: 'post',
            data: {
                FormName: FormName, ParentForm: ParentForm, FormURL: FormURL, MenuText: MenuText, MenuItemPriority: MenuItemPriority, isActive: isActive, PageType: PageType,
                IsMenuItem: IsMenuItem, IsDashboardPart: IsDashboardPart, ShowOnDesktop: ShowOnDesktop,
                IsMasterMenu: IsMasterMenu, IsQuickLink: IsQuickLink, IsAction: IsAction, ModuleId: ModuleId, PageDescription: PageDescription
            },
            success: function (data) {
                if (data.Messages != null && data.Messages != "" && data.Messages != undefined)
                {
                    toastr.success(data.Messages)
                }
            }
        });
    }
}
function InsertInMembershipFormsWithStaticFormId(FormName, ParentForm, FormURL, MenuText, MenuItemPriority, isActive, PageType, IsMenuItem, IsDashboardPart, ShowOnDesktop, IsMasterMenu, IsQuickLink, FormId, IsAction, ModuleId, PageDescription) {
    if (IsAction == undefined) {
        IsAction = false;
    }
    if (ModuleId == undefined) {
        ModuleId = 0;
    }
    if (ParentForm != "" && FormName != "" && FormURL != "" && FormURL != "") {
        $.ajax({
            url: '/Home/InsertMembershipFormsWithStaticFormId',
            type: 'post',
            data: {
                FormName: FormName, ParentForm: ParentForm, FormURL: FormURL, MenuText: MenuText, MenuItemPriority: MenuItemPriority, isActive: isActive, PageType: PageType,
                IsMenuItem: IsMenuItem, IsDashboardPart: IsDashboardPart, ShowOnDesktop: ShowOnDesktop,
                IsMasterMenu: IsMasterMenu, IsQuickLink: IsQuickLink, FormID: FormId, IsAction: IsAction, ModuleId: ModuleId, PageDescription: PageDescription
            },
            success: function (data) {
                if (data.Messages != null && data.Messages != "" && data.Messages != undefined)
                {
                    toastr.success(data.Messages)
                }
            }
        });
    }
}