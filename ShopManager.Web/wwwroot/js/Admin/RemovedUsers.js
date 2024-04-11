$(document).ready(function () {
    $("#showRemovedUsers").click(function () {
        $.ajax({
            url: "/Admin/GetRemovedUsers",
            type: 'GET',
            success: function (data) {
                $(".containerforResult").empty();

                // Creat table
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Имя").appendTo(headerRow);
                $("<th>").text("Отчество").appendTo(headerRow);
                $("<th>").text("Email").appendTo(headerRow);
                $("<th>").text("Телефон").appendTo(headerRow);

                // Add data to table
                data.forEach(function (user) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(user.name).appendTo(row);
                    $("<td>").text(user.middleName).appendTo(row);
                    $("<td>").text(user.email).appendTo(row);
                    $("<td>").text(user.phoneNumber).appendTo(row);

                    var deleteButton = $("<button>").text("Удалить безвозратно").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteUserFinally(user.stringId, row);
                    });

                    var assignButton = $("<button>").text("Вернуть").addClass("btn btn-primary").appendTo($("<td>").appendTo(row));
                    assignButton.click(function () {
                        ReturnUser(user.stringId, user.name, row);
                    });
                });

                $(".containerforResult").append(table);
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    });
});

// Function to permanently delete a user
function deleteUserFinally(stringId, row) {
    if (confirm("Уверены ли вы, что хотите удалить пользователя?")) {
        $.ajax({
            url: "/Admin/RemoveUserFinally",
            type: 'POST',
            data: { userId: stringId },
            success: function (data) {
                if (data) {
                    row.remove();
                    alert("Пользователь успешно удален");
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
        alert("Удаление отменено");
    }
}

// Function to return a user
function ReturnUser(stringId, name, row) {
    if (confirm("Уверены ли вы, что хотите вернуть" + " " + name + "?")) {
        $.ajax({
            url: "/Admin/ReturnUser",
            type: 'POST',
            data: { userId: stringId },
            success: function (data) {
                if (data) {
                    row.remove();
                    alert(name + " " + "пользователь возвращен! ")
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
        alert("Возврощение отменено");
    }
}
