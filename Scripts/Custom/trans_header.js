$(function () {
    $('#trans_details > tbody > tr').on('blur', '.cloneable', function () {
        var count = $('table#trans_details > tbody > tr').length;
        var $parent_tr = $(this).closest('tr');
        var index = $parent_tr.index();
        if (index == count - 1) {
            var $row = $.fn.cloneRow($('#tr_clone'), 'table#trans_details > tbody');
        }
    });


    var $row = $.fn.cloneRow($('#tr_clone'), 'table#trans_details > tbody');

    //2
    $('#trans_details2 > tbody > tr').on('blur', '.cloneable', function () {
        var count = $('table#trans_details2 > tbody > tr').length;
        var $parent_tr = $(this).closest('tr');
        var index = $parent_tr.index();
        if (index === count - 1) {
            var $row = $.fn.cloneRow($('#tr_clone2'), 'table#trans_details2 > tbody');
        }
    });

    var $row = $.fn.cloneRow($('#tr_clone2'), 'table#trans_details2 > tbody');


    $(document).on('click', '[data-action $= Delete]', function (e) {
        var result = false;
        var that = this;
        var id = $(that).data("id");
        var url = $(that).data("url");
        if (url) {
            swal({
                title: "Are you sure?",
                text: "You will not be able to recover this!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $.get(url, { "id": id }, function (d) {
                            result = d;
                            if (result !== false) {
                                swal("Deleted!", "Deleted successfully", "success");
                                $(that).closest('tr').remove();
                            } else {
                                swal("Error", "Can't delete", "error");
                            }
                            return false;
                        });
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

    $(document).on('click', '[data-action $= ConfirmDelete]', function (e) {
         
        var result = false;
        var that = this;
        swal({
            title: "Are you sure?",
            text: "You will not be able to recover this!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Yes, delete it!",
            closeOnConfirm: false
        },
            function (t) {
                if (t) {
                    result = true;
                    swal("Deleted!", "", "success");
                }
            });
        return result;
    });

    $(document).on('keyup', '[data-action $= livesearch]', function (e) {
        var input, filter, table, tr, td, i;
        var that = this;
        input = document.getElementById($(that).attr('data-id'));
        table = document.getElementById($(that).attr('data-tableId'));
        if (input && table) {
            filter = input.value.toUpperCase();
            tr = table.getElementsByTagName("tr");
            for (i = 0; i < tr.length; i++) {
                td = tr[i].getElementsByTagName("td")[0];
                if (td) {
                    if (td.innerHTML.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }

    })

    $(document).on('click', '[data-action $= DeleteDoc]', function (e) {
        var result = false;
        var that = this;
        var id = $(that).data("id");
        var url = $(that).data("url");
        if (url) {
            swal({
                title: "Are you sure?",
                text: "You will not be able to recover this!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $.get(url, { "id": id }, function (d) {
                            result = d;
                            if (result !== false) {
                                swal("Deleted!", "Deleted successfully", "success");
                                $(that).closest('.onedoc').remove();
                            } else {
                                swal("Error", "Can't delete", "error");
                            }
                            return false;
                        });
                    }
                });
        }
        return false;
    });

    $('#trans_details > tbody > tr').on('focusin', 'input', function () {
        $('#trans_details tr').removeClass('highlighted');
        $(this).closest('tr').addClass('highlighted');
    });

    $(document).on('click', '[data-action $= DeleteAccount]', function (e) {
        var result = false;
        var that = this;
        var id = $(that).data("id");
        var url = $(that).data("url");
        if (url) {
            swal({
                title: "Are you sure?",
                text: "You will not be able to recover this!",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, delete it!",
                closeOnConfirm: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        $.get(url, { "id": id }, function (d) {
                            result = d;
                            if (result.status !== false) {
                                swal("Deleted!", result.Msg, "success");
                                $(that).closest('tr').remove();
                            } else {
                                if (!result.Msg) {
                                    swal("Error", "Can't delete", "error");
                                }
                                else {
                                    swal("Error", result.Msg, "error");
                                }
                            }
                            return false;
                        });
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

    $(document).on('click', '[data-action=ConfirmThenRedirect]', function (e) {
        var that = this;
        Redirectform($(that).data('title'), $(that).data('msg'), $(that).data('url'))
        return true;
    })

    function Redirectform(title, message, url) {
        swal({
            title: title,
            text: message,
            type: 'warning',
            showCancelButton: true,
            closeOnConfirm: true,
            confirmButtonClass: "btn-success",
            confirmButtonText: "Yes",
            confirmButtonColor: "#1fae66"
        },
            function (isConfirm) {
                if (isConfirm) {
                    if (url) {
                        window.location.href = url;
                    }
                }
            });
    }

});