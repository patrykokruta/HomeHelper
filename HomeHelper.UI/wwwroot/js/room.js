var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#roomData').DataTable({
        "ajax": {
            "url": "/Admin/Room/GetAll",
            "dataSrc": '',
        },
        "columns": [
            { "data": "name", "width": "60%" },
            {
                "data": "id",
                "render": function (data) {
                    return `
                            <div class="text-center">
                                <a href="#" class="btn btn-success text-white hh-rounded mb-1">Edit</a>
                                <a href="#" class="btn btn-danger text-white hh-rounded mb-1">Delete</a>
                            </div>
                           `;
                }, "width": "40%"
            }
        ]
    });
}
