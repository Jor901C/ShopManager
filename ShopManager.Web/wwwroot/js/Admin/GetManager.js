$(document).ready(function () {
    $("#showAllManagers").click(function () {
        $.ajax({
            url: "/Admin/GetOnlyManager", // URL to get all managers
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            success: function (data) {
                // Clear container content before updating
                $(".containerforResult").empty();

                // Create table and headers
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Имя").appendTo(headerRow);
                $("<th>").text("Отчество").appendTo(headerRow);
                $("<th>").text("Email").appendTo(headerRow);
                $("<th>").text("Номер телефона").appendTo(headerRow);

                // Add user data to the table
                data.forEach(function (manager) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(manager.name).appendTo(row);
                    $("<td>").text(manager.middleName).appendTo(row);
                    $("<td>").text(manager.email).appendTo(row);
                    $("<td>").text(manager.phoneNumber).appendTo(row);

                    // Delete button
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteUser(manager.stringId, row);
                    });
                });

                // Add the table to the container
                $(".containerforResult").append(table);
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    });
});
