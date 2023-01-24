document.onkeydown = function (e) {
    e = e || window.event;//Get event
    if (e.ctrlKey) {
        var c = e.which || e.keyCode;//Get key code
        switch (c) {
            case 70://Ctrl+f
                e.preventDefault();
                $(".remoteSearch").focus();
                $(".remoteSearch").select2('open');
                break;
        }
    }
};


$(window).load(function () {
    var pagesize = 20;
    //Method which is to be called for populating options in dropdown //dynamically
    $('.remoteSearch').select2({
        ajax: {
            delay: 150,
            global: false,
            url: $ajaxRemoteSearchUrl,
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    q: params.term
                };
            },
            processResults: function (data, params) {
                if (typeof ($count) != 'undefined' && $count != undefined) {
                    $count = 0;
                }
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: $ajaxRemotePlaceHolder,
        minimumInputLength: 3,
        allowClear: false,
        templateSelection: $ajaxRemoteOnTemplateSelection
    });
});

$(window).load(function () {
    var pagesize = 20;
    //Method which is to be called for populating options in dropdown //dynamically
    $('.remoteSearch1').select2({
        ajax: {
            delay: 150,
            global: false,
            url: $ajaxRemoteSearchUrl_1,
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    q: params.term
                };
            },
            processResults: function (data, params) {
                if (typeof ($count) != 'undefined' && $count != undefined) {
                    $count = 0;
                }
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: $ajaxRemotePlaceHolder_1,
        minimumInputLength: 3,
        allowClear: false,
        templateSelection: $ajaxRemoteOnTemplateSelection_1
    });
});


$(window).load(function () {
    var pagesize = 20;
    //Method which is to be called for populating options in dropdown //dynamically
    $('.remoteSearch2').select2({
        ajax: {
            delay: 150,
            global: false,
            url: $ajaxRemoteSearchUrl_2,
            dataType: 'json',
            data: function (params) {
                params.page = params.page || 1;
                return {
                    q: params.term
                };
            },
            processResults: function (data, params) {
                if (typeof ($count) != 'undefined' && $count != undefined) {
                    $count = 0;
                }
                params.page = params.page || 1;
                return {
                    results: data,
                };
            }
        },
        placeholder: $ajaxRemotePlaceHolder_2,
        minimumInputLength: 3,
        allowClear: false,
        templateSelection: $ajaxRemoteOnTemplateSelection_2
    });
});

function formatRepoSelection(repo) {
    if (repo.id) {
        window.location.replace($ajaxRemoteRedirectUrl + repo.id);
        return 'please wait...';
    }
    return repo.title || repo.text;
}


$(document).ajaxComplete(function (event, xhr, settings) {
    setTimeout(function () {
        $('.remoteSearch').select2({
            ajax: {
                delay: 150,
                global: false,
                url: (typeof $ajaxRemoteSearchUrl !== 'undefined' ? $ajaxRemoteSearchUrl : ''),
                dataType: 'json',
                data: function (params) {
                    return {
                        q: params.term,
                    };
                },
                processResults: function (data, params) {
                    if (typeof ($count) != 'undefined' && $count != undefined) {
                        $count = 0;
                    }
                    params.page = params.page || 1;
                    return {
                        results: data,
                    };
                }
            },
            placeholder: (typeof $ajaxRemotePlaceHolder !== 'undefined' ? $ajaxRemotePlaceHolder : '--Select--'),
            minimumInputLength: 3,
            allowClear: false,
            templateSelection: $ajaxRemoteOnTemplateSelection
        });
    }, 100);


    function redirectTo(repo) {
        if (repo.id && typeof $ajaxRemoteRedirectUrl !== 'undefined') {
            window.location.replace($ajaxRemoteRedirectUrl + repo.id);
            return 'please wait...';
        }
        return repo.title || repo.text;
    }
})

$(document).ajaxComplete(function (event, xhr, settings) {
    setTimeout(function () {
        $('.remoteSearch1').select2({
            ajax: {
                delay: 150,
                global: false,
                url: (typeof $ajaxRemoteSearchUrl_1 !== 'undefined' ? $ajaxRemoteSearchUrl_1 : ''),
                dataType: 'json',
                data: function (params) {
                    return {
                        q: params.term,
                    };
                },
                processResults: function (data, params) {
                    if (typeof ($count) != 'undefined' && $count != undefined) {
                        $count = 0;
                    }
                    params.page = params.page || 1;
                    return {
                        results: data,
                    };
                }
            },
            placeholder: (typeof $ajaxRemotePlaceHolder_1 !== 'undefined' ? $ajaxRemotePlaceHolder_1 : '--Select--'),
            minimumInputLength: 3,
            allowClear: false,
            templateSelection: $ajaxRemoteOnTemplateSelection_1
        });
    }, 100);


    function redirectTo(repo) {
        if (repo.id && typeof $ajaxRemoteRedirectUrl !== 'undefined') {
            window.location.replace($ajaxRemoteRedirectUrl_1 + repo.id);
            return 'please wait...';
        }
        return repo.title || repo.text;
    }
})



$(document).ajaxComplete(function (event, xhr, settings) {
    setTimeout(function () {
        $('.remoteSearch2').select2({
            ajax: {
                delay: 150,
                global: false,
                url: (typeof $ajaxRemoteSearchUrl_2 !== 'undefined' ? $ajaxRemoteSearchUrl_2 : ''),
                dataType: 'json',
                data: function (params) {
                    return {
                        q: params.term,
                    };
                },
                processResults: function (data, params) {
                    if (typeof ($count) != 'undefined' && $count != undefined) {
                        $count = 0;
                    }
                    params.page = params.page || 1;
                    return {
                        results: data,
                    };
                }
            },
            placeholder: (typeof $ajaxRemotePlaceHolder_2 !== 'undefined' ? $ajaxRemotePlaceHolder_2 : '--Select--'),
            minimumInputLength: 3,
            allowClear: false,
            templateSelection: $ajaxRemoteOnTemplateSelection_2
        });
    }, 100);


    function redirectTo(repo) {
        if (repo.id && typeof $ajaxRemoteRedirectUrl !== 'undefined') {
            window.location.replace($ajaxRemoteRedirectUrl_2 + repo.id);
            return 'please wait...';
        }
        return repo.title || repo.text;
    }
})








