var pagesize = 20;
$ajaxRemoteControlId = 'ClientId';
$ajaxRemoteSearchUrl = '/Contact/Clients/GetCompanies';
$ajaxRemotePlaceHolder = "--(Ctrl+F) Seacrh Clients --";
$ajaxRemoteRedirectUrl = '/Contact/Clients/AddEditClient/';
$ajaxRemoteNavigate = false;
$ajaxBoldPlaceholder = false;

$ajaxRemoteControlId_2 = 'Bill_SupplierId';
$ajaxRemoteSearchUrl_2 = '/Finance/Billing/GetSuppliersList';
$ajaxRemotePlaceHolder_2 = "--Seacrh Suppliers--";
$ajaxRemoteRedirectUrl_2 = '';
$ajaxRemoteNavigate_2 = false;
$ajaxBoldPlaceholder_2 = false;

$ajaxRemoteControlId_3 = 'Bill_EmployeeId';
$ajaxRemoteSearchUrl_3 = '/Finance/Billing/GetEmployeesList';
$ajaxRemotePlaceHolder_3 = "--Seacrh Employees --";
$ajaxRemoteRedirectUrl_3 = '';
$ajaxRemoteNavigate_3 = false;
$ajaxBoldPlaceholder_3 = false;

document.onkeydown = function (e) {
    e = e || window.event;//Get event
    if (e.ctrlKey) {
        var c = e.which || e.keyCode;//Get key code
        switch (c) {
            case 70://Ctrl+f
                e.preventDefault();
                $("#" + $ajaxRemoteControlId).focus();
                $("#" + $ajaxRemoteControlId).select2('open');
                break;
        }
    }
};

$(window).load(function () {
    //Method which is to be called for populating options in dropdown //dynamically
    $('#' + $ajaxRemoteControlId).select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl !== 'undefined' ? $ajaxRemoteSearchUrl : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder !== 'undefined' ? $ajaxRemotePlaceHolder : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection
    });

    $('#' + $ajaxRemoteControlId_2).select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl_2 !== 'undefined' ? $ajaxRemoteSearchUrl_2 : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder_2 !== 'undefined' ? $ajaxRemotePlaceHolder_2 : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection_2
    });

    $('#' + $ajaxRemoteControlId_3).select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl_3 !== 'undefined' ? $ajaxRemoteSearchUrl_3 : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder_3 !== 'undefined' ? $ajaxRemotePlaceHolder_3 : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection_3
    });

});

function formatRepoSelection(repo) {
    if ($ajaxRemoteNavigate === true) {
        if (repo.id) {
            window.location.replace($ajaxRemoteRedirectUrl + repo.id);
            return 'please wait...';
        }
    }
    return repo.title || repo.text;
}

function formatRepoSelection_2(repo) {
    if ($ajaxRemoteNavigate_2 === true) {
        if (repo.id) {
            window.location.replace($ajaxRemoteRedirectUrl_2 + repo.id);
            return 'please wait...';
        }
    }
    return repo.title || repo.text;
}

function formatRepoSelection_3(repo) {
    if ($ajaxRemoteNavigate_3 === true) {
        if (repo.id) {
            window.location.replace($ajaxRemoteRedirectUrl_3 + repo.id);
            return 'please wait...';
        }
    }
    return repo.title || repo.text;
}

$.fn.livedropdown = function () {
    //if ($ajaxBoldPlaceholder === true) {
    //    $('.select2-selection__placeholder').css('font-size', '24px;');
    //}
    this.select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl !== 'undefined' ? $ajaxRemoteSearchUrl : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder !== 'undefined' ? $ajaxRemotePlaceHolder : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection
    });
}

$.fn.livedropdown_2 = function () {
    //if ($ajaxBoldPlaceholder === true) {
    //    $('.select2-selection__placeholder').css('font-size', '24px;');
    //}
    this.select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl_2 !== 'undefined' ? $ajaxRemoteSearchUrl_2 : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder_2 !== 'undefined' ? $ajaxRemotePlaceHolder_2 : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection_2
    });
}

$.fn.livedropdown_3 = function () {
    //if ($ajaxBoldPlaceholder === true) {
    //    $('.select2-selection__placeholder').css('font-size', '24px;');
    //}
    this.select2({
        ajax: {
            delay: 150,
            global: false,
            url: (typeof $ajaxRemoteSearchUrl_3 !== 'undefined' ? $ajaxRemoteSearchUrl_3 : ''),
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    search: params.term
                };
            },
            processResults: function (data, params) {
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: (typeof $ajaxRemotePlaceHolder_3 !== 'undefined' ? $ajaxRemotePlaceHolder_3 : ''),
        //placeholder: 'select order',
        minimumInputLength: 2,
        allowClear: true,
        templateSelection: formatRepoSelection_3
    });
}

function formatOnSelection(that, repo) {
    if ($(that).data('ajaxRemoteNavigate') === true) {
        if (repo.id) {
            window.location.replace($(that).data('ajaxRemoteRedirectUrl') + repo.id);
            return 'please wait...';
        }
    }
    return repo.title || repo.text;
}

setTimeout(function () {
    $('.livesearch').each(function () {
        var that = $(this);
        $(that).select2({
            ajax: {
                delay: 150,
                global: false,
                url: $(that).data('url'),
                dataType: 'json',
                data: function (params) {
                    params.page = params.page || 1;
                    return {
                        search: params.term
                    };
                },
                processResults: function (data, params) {
                    params.page = params.page || 1;
                    return {
                        results: data,
                    };
                }
            },
            placeholder: $(that).data('placeholder'),
            //placeholder: 'select order',
            minimumInputLength: 2,
            allowClear: true,
            templateSelection: formatOnSelection(that,this)
        });
    });

}, 1000);