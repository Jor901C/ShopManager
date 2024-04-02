$(document).ready(function () {
    $("#showAllManagers").click(function () {
        $.ajax({
            url: "/Admin/GetOnlyManager",
            type: 'GET',
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
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
                data.forEach(function (manager) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(manager.name).appendTo(row);
                    $("<td>").text(manager.middleName).appendTo(row);
                    $("<td>").text(manager.email).appendTo(row);
                    $("<td>").text(manager.phoneNumber).appendTo(row);

                    // Кнопка "Удалить"
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteUser(manager.stringId, row);
                    });

                    // Кнопка "Назначить"
                    var dontKnowButton = $("<button>").text("Пока не знаю").addClass("btn btn-primary").appendTo($("<td>").appendTo(row));
                    // Добавьте логику для кнопки "Назначить", если необходимо
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
