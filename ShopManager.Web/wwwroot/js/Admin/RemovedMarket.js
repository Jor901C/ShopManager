$(document).ready(function () {
    $("#showRemovedMarket").click(function () {
        $.ajax({
            url: "/Admin/RemovedMarket",
            type: 'GET',
            success: function (data) {
                // Clean Container
                $(".containerforResult").empty();

                // Craet table 
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Название").appendTo(headerRow);
                $("<th>").text("Адресс").appendTo(headerRow);
                

                // Add data to table
                data.forEach(function (market) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(market.name).appendTo(row);
                    $("<td>").text(market.address).appendTo(row);

                    var deleteButton = $("<button>").text("Удалить безвозратно").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteMarketFinally(market.id, row);
                    });

                    var assignButton = $("<button>").text("Вернуть").addClass("btn btn-primary").appendTo($("<td>").appendTo(row));
                    assignButton.click(function () {
                        ReturnMarket(market.id, market.name, row);
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
//Delete market in database
function deleteMarketFinally(marketId, row) {
    if (confirm("Уверены ли вы, что хотите удалить магазин?")) {
        $.ajax({
            url: "/Admin/RemoveMarketFinnaly",
            type: 'POST',
            data: { marketId: marketId },
            success: function (data) {
                if (data) {
                    row.remove();
                    alert("Магазин успешно удален");
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


//IsDelete false
function ReturnMarket(marketId, name, row) {
    if (confirm("Уверены ли вы, что хотите вернуть" + " " + name + "магазин?")) {
        $.ajax({
            url: "/Admin/ReturnMarket",
            type: 'POST',
            data: { marketId: marketId },
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
