$(document).ready(function () {
    $("#showAllShops").click(function () {
        $.ajax({
            url: "/Admin/GetAllMarket",
            type: 'GET',
            success: function (data) {
                // Очистка содержимого контейнера перед обновлением
                $(".containerforResult").empty();

                // Создание таблицы и заголовков
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Название").appendTo(headerRow);
                $("<th>").text("Адрес").appendTo(headerRow);
                // Добавление данных пользователей в таблицу
                data.forEach(function (market) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(market.name).appendTo(row);
                    $("<td>").text(market.address).appendTo(row);
                    

                    // Кнопка "Удалить"
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteMarket(market.id, row);
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
function deleteMarket(marketId, row) {
    if (confirm("Уверены ли вы, что хотите удалить пользователя?")) {
        $.ajax({
            url: "/Admin/DeleteMarket",
            type: 'POST',
            data: { marketId: marketId },
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