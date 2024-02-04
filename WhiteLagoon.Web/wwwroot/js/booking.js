var dataTable;

$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $("#bookingTable").DataTable({
        "ajax": {
            url: '/booking/getall'
        },
        "columns": [
            { data: 'id', "wdith": "5%" },
            { data: 'name', "wdith": "15%" },
            { data: 'phone', "wdith": "10%" },
            { data: 'email', "wdith": "15%" },
            { data: 'status', "wdith": "10%" },
            { data: 'checkInDate', "wdith": "10%" },
            { data: 'nights', "wdith": "10%" },
            { data: 'totalCost', "wdith": "10%" },
            {
                data: 'id',
                render: function (data) {
                    return
                    `<div class="w-75 btn-group">
                        <a href="/booking/bookingDetails?bookingId=${data}" class="btn btn-outline-warning mx-2">
                            <i class="bi bi-pencil-square"></i> Details
                        </a>
                    </div>`
                }
            },
        ]
    });
}