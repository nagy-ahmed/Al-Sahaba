var updatedRow;
var table;
var datatable;
var exportedCols = []

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
            confirmButton: 'btn btn-primary'
        }
    })
};

function onModalBegin() {
    $('body :submit').attr('disable', 'disable').attr('data-kt-indicator', 'on');
}

function onModalSuccess(item) {
    showMessage(1, "Saved Successfully");
    $('#Modal').modal('hide');


    if (updatedRow) {
        datatable.row(updatedRow).remove().draw();
        updatedRow = undefined;
    }

    var newRow = $(item)
    datatable.row.add(newRow).draw();

    KTMenu.init();
    KTMenu.initHandlers();

}
function onModalComplete() {
    $('body :submit').removeAttr('disable');
}


// DataTables

// Get the columns that are not marked as no-export
var headers = $('th');
$.each(headers, function (i) {
    var col = $(this);
    if (!col.hasClass('js-no-export')) {
        exportedCols.push(i)
    }
})

// Class definition
var KTDatatables = function () {
    // Shared variables


    // Private functions
    var initDatatable = function () {
        // Init datatable --- more info on datatables: https://datatables.net/manual/
        datatable = $(table).DataTable({
            "info": false,
            'pageLength': 10,
        });
    }

    // Hook export buttons
    var exportButtons = () => {
        const documentTitle = $('js-datatables').data('document-title');
        var buttons = new $.fn.dataTable.Buttons(table, {
            buttons: [
                {
                    extend: 'copyHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                },
                {
                    extend: 'excelHtml5',
                    title: documentTitle,
                    columns: exportedCols

                },
                {
                    extend: 'csvHtml5',
                    title: documentTitle,
                    columns: exportedCols

                },
                {
                    extend: 'pdfHtml5',
                    title: documentTitle,
                    exportOptions: {
                        columns: exportedCols
                    }
                }
            ]
        }).container().appendTo($('#kt_datatable_example_buttons'));

        // Hook dropdown menu click event to datatable export buttons
        const exportButtons = document.querySelectorAll('#kt_datatable_example_export_menu [data-kt-export]');
        exportButtons.forEach(exportButton => {
            exportButton.addEventListener('click', e => {
                e.preventDefault();

                // Get clicked export value
                const exportValue = e.target.getAttribute('data-kt-export');
                const target = document.querySelector('.dt-buttons .buttons-' + exportValue);

                // Trigger click event on hidden datatable export buttons
                target.click();
            });
        });
    }

    // Search Datatable --- official docs reference: https://datatables.net/reference/api/search()
    var handleSearchDatatable = () => {
        const filterSearch = document.querySelector('[data-kt-filter="search"]');
        filterSearch.addEventListener('keyup', function (e) {
            datatable.search(e.target.value).draw();
        });
    }

    // Public methods
    return {
        init: function () {
            table = document.querySelector('.js-datatables');

            if (!table) {
                return;
            }

            initDatatable();
            exportButtons();
            handleSearchDatatable();
        }
    };
}();

$(document).ready(function () {
    //SweetAlert
    var message = $("#Message").text();
    if (message !== '') {
        showMessage(1, message);
    }

    //Datatables
    KTUtil.onDOMContentLoaded(function () {
        KTDatatables.init();
    });

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

    // Handle Toggle Status
    $("body").delegate('.js-toggle-status', 'click', function () {
        var btn = $(this);
        bootbox.confirm({
            message: `Are you sure you want to toggle <span class="text-danger fw-bold">${btn.parents('tr').find('.js-name').text()}</span> Statue?`,
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function () {
                            var row = btn.parents('tr');

                            var status = row.find('.js-status');
                            var newStatus = status.text().trim() === 'Available' ? 'Deleted' : 'Available';
                            status.text(newStatus).toggleClass('badge-light-success badge-light-danger');

                            var lastUpdatedOn = row.find('.js-last-updated-on');
                            lastUpdatedOn.text(new Date().toLocaleString());

                            row.addClass('animate__animated animate__flash');

                            showMessage(1, 'Category status has been toggled successfully!');

                        },
                        error: function () {
                            showMessage(0, 'An error occurred while toggling category status!');
                        }
                    })
                }
            }
        });
    });
});