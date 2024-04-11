$(document).ready(function () {
    // When "Show All Users" button is clicked
    $("#showAllUsers").click(function () {
        $.ajax({
            url: "/Admin/GetAllUser",
            type: 'GET',
            success: function (data) {
                // Clear container content before updating
                $(".containerforResult").empty();

                // Создание таблицы и заголовков
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Имя").appendTo(headerRow);
                $("<th>").text("Отчество").appendTo(headerRow);
                $("<th>").text("Email").appendTo(headerRow);
                $("<th>").text("Телефон").appendTo(headerRow);

                // Create table and headers
                data.forEach(function (user) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(user.name).appendTo(row);
                    $("<td>").text(user.middleName).appendTo(row);
                    $("<td>").text(user.email).appendTo(row);
                    $("<td>").text(user.phoneNumber).appendTo(row);

                    // Add user data to the table
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteUser(user.stringId , row);
                    });

                    // Delete button
                    var assignButton = $("<button>").text("Назначить менеджером").addClass("btn btn-primary").appendTo($("<td>").appendTo(row));
                    // Add logic for the assign button if necessary
                    assignButton.click(function () {
                        AddRole(user.stringId, user.name , row);
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

// Function to delete a user
function deleteUser(stringId , row) {
    if (confirm("Уверены ли вы, что хотите удалить пользователя?")) {
        $.ajax({
            url: "/Admin/DeleteUser",
            type: 'POST',
            data: { managerId: stringId },
            success: function (data) {
                // Handle successful deletion
                if (data) {
                    row.remove();
                    alert("Пользователь успешно удален");
                }
                else {
                    alert("Ошибка")
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Удаление отменено");
    }
}


// Function to assign a role
function AddRole(stringId , name , row) {
    if (confirm("Уверены ли вы, что хотите назначить" + " " + name + " " + "менеджером ?")) {
        $.ajax({
            url: "/Admin/AddRole",
            type: 'POST',
            data: { userId: stringId },
            success: function (data) {
                // Handle successful role assignment
                if (data) {
                    row.remove();
                    alert(name + " " + "назначем менеджером")
                }
                else {
                    alert("Ошибка");
                }
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Назначение отменено");
    }
}

