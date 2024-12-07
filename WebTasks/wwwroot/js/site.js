function loadModal(url, modalId) {
    $.get(url, function (data) {
        $(modalId + " .modal-content").html(data);
        $(modalId).modal("show");
    });
}

$(document).on("submit", "#createForm, #editForm, #deleteForm", function (e) {
    e.preventDefault();
    const form = $(this);

    $.ajax({
        url: form.attr("action"),
        type: form.attr("method"),
        data: form.serialize(),
        success: function (response) {
            if (response.success) {
                $("#actionModal").modal("hide");
                alert(response.message);
                location.reload();
            } else {
                alert(response.message);
            }
        },
        error: function () {
            alert("An error occurred.");
        }
    });
});

function loadModal(url, modalId) {
    $.get(url, function (data) {
        $(modalId + " .modal-content").html(data);
        $(modalId).modal("show");
    }).fail(function () {
        alert("Failed to load modal.");
    });
}
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

