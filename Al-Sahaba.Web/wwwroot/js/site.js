var updatedRow;


//used to show message in sweet alert with icon and title based on flag
// 0 -> error
// 1 -> success
function showMessage(flag, message) {
    if (flag == 0) {
        icon = 'error'
        title = 'Oops...'
    }
    else if (flag == 1) {
        icon = 'success'
        title = 'Success'
    }
    Swal.fire({
        icon: icon,
        title: title,
        text: message,
        customClass: {
            confirmButton: 'btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary'
        }
    })
};
function onModalSuccess(item) {
    showMessage(1, "Saved Successfully");
    $('#Modal').modal('hide');
    if (updatedRow) {
        $(updatedRow).replaceWith(item);
        updatedRow = undefined;
    }
    else {
        $('tbody').append(item);
    }
    KTMenu.init();
    KTMenu.initHandlers();

}
$(document).ready(function () {
    var message = $("#Message").text();
    if (message !== '') {
        showMessage(1, message);
    }

    // Handle bootstrap Modal
    $('body').delegate('.js-render-modal', 'click', function () {
        var modal = $('#Modal');

        modal.find('.modal-title').text($(this).data('title'));

        if ($(this).data('update') !== undefined) {
            updatedRow = $(this).parents('tr');
        }

        $.get({
            url: $(this).data('url'),
            success: function (form) {
                modal.find('.modal-body').html(form);
                $.validator.unobtrusive.parse(modal);
            },
            error: function () {
                showMessage(0, 'An error occurred while processing your request');
            }

        })

        modal.modal('show');
    });
});