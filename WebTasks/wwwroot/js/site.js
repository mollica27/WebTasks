function showToast(message, isSuccess) {
    var bgClass = isSuccess ? 'bg-primary text-white' : 'bg-danger text-white';
    var toastHtml = `
        <div class="toast align-items-center ${bgClass}" role="alert" aria-live="assertive" aria-atomic="true">
          <div class="d-flex">
            <div class="toast-body">
              ${message}
            </div>
            <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast" aria-label="Close"></button>
          </div>
        </div>
    `;

    var $toast = $(toastHtml).appendTo('#toastContainer');

    var toast = new bootstrap.Toast($toast[0]);
    toast.show();

    setTimeout(function () {
        $toast.remove();
    }, 5000);
}
function loadModal(url, modalId) {
    $.get(url, function (data) {
        $(modalId + " .modal-content").html(data);
        $(modalId).modal("show");
    }).fail(function () {
        showToast("Failed to load modal.", false);
    });
}

$(document).on("submit", "#createForm, #editForm, #deleteForm", function (e) {
    e.preventDefault(); 
    const form = $(this);

    $.ajax({
        url: form.attr("action"),
        type: form.attr("method"),
        data: form.serialize(),
        dataType: 'json', 
        success: function (response) {
            if (response.success) {
                $("#modaltask").modal("hide"); 
                showToast(response.message, true); 
                setTimeout(function () {
                    location.reload();
                }, 1000);
            } else {
                showToast(response.message, false); 
            }
        },
        error: function () {
            showToast("An error occurred while processing the request.", false);
        }
    });
});

$(document).ready(function () {
    var table = $('#taskTable').DataTable({
        paging: true,
        searching: true,
        ordering: true,
        dom: 'Bfrtip',
        buttons: ['copy', 'csv', 'excel', 'pdf'],
        language: {
            url: 'https://cdn.datatables.net/plug-ins/1.12.1/i18n/Portuguese.json'
        }
    });

    $('#exportBtn').on('click', function () {
        table.buttons([0, 1, 2, 3]).trigger();
    });
});
