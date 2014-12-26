$(function () {


    $('.datePicker').datepicker();
    $(".datePickerMin").datepicker({ minDate: 0 });

    $(".datePickerMax").each(function () {
        var id = $(this).attr('id');
        $(this).attr('id', '');
    });
    $(".datePickerMax").datepicker({ maxDate: 0 });
    window.openDialog = function (modalId, title, height, width, callbackFunction) {
        var left = ($(window).width() - width) / 2;
        var top = ($(window).height() - height) / 2;
        $('#' + modalId).dialog({
            modal: true,
            title: title,
            height: height,
            top: top,
            left: left,
            width: width,
            autoOpen: false,
            close: function () {
                if (typeof callbackFunction == 'function') {
                    callbackFunction();
                }
            }
        });
        $('#' + modalId).dialog('open');
    }
});
$(document).ready(function () {
    /* setup toastr responsive alerts */
    //setup_toastr();
    if ($('.grid tr').not('.grid-title').length === 0) {
        $(".grid").after('<div class="gridfooter">No records found...</div>');
    }
});