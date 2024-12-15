function showMessage(icon,title,message) {
    Swal.fire({
        icon: icon,
        title: title,
        text: message,
        customClass: {
            confirmButton: 'btn btn-outline btn-outline-dashed btn-outline-primary btn-active-light-primary'
        }
    })
};
$(document).ready(function () {
    var message = $("#Message").text();
    if (message !== '') {
        showMessage('success', 'Success', message);
    }
});