$(document).ready(function () {
    $("#showAllUsers").click(function () {
        $.ajax({
            url: "/Admin/GetAllUser",
            type: 'GET',
            success: function (data) {
                // Очистка содержимого контейнера перед обновлением
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

                // Добавление данных пользователей в таблицу
                data.forEach(function (user) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(user.name).appendTo(row);
                    $("<td>").text(user.middleName).appendTo(row);
                    $("<td>").text(user.email).appendTo(row);
                    $("<td>").text(user.phoneNumber).appendTo(row);

                    // Кнопка "Удалить"
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteUser(user.stringId , row);
                    });

                    // Кнопка "Назначить"
                    var assignButton = $("<button>").text("Назначить менеджером").addClass("btn btn-primary").appendTo($("<td>").appendTo(row));
                    // Добавьте логику для кнопки "Назначить", если необходимо
                    assignButton.click(function () {
                        AddRole(user.stringId, user.name , row);
                    });
                });

                // Добавление таблицы в контейнер
                $(".containerforResult").append(table);
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    });
});

function deleteUser(stringId , row) {
    if (confirm("Уверены ли вы, что хотите удалить пользователя?")) {
        $.ajax({
            url: "/Admin/DeleteUser",
            type: 'POST',
            data: { managerId: stringId },
            success: function (data) {
                // Обработка успешного удаления пользователя
                row.remove();
                console.log("Пользователь успешно удален");
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        console.log("Удаление отменено");
    }
}
function AddRole(stringId , name , row) {
    if (confirm("Уверены ли вы, что хотите назначить" + " " + name + " " + "менеджером ?")) {
        $.ajax({
            url: "/Admin/AddRole",
            type: 'POST',
            data: { userId: stringId },
            success: function (data) {
                // Обработка успешного удаления пользователя
                row.remove();
                alert(name + " " + "назначем менеджером")
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Назначение отменено");
    }
}

