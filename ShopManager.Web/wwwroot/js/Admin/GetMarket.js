$(document).ready(function () {
    // When "Show All Shops" button is clicked
    $("#showAllShops").click(function () {
        $.ajax({
            url: "/Admin/GetAllMarket",
            type: 'GET',
            success: function (data) {
                // Clear container content before updating
                $(".containerforResult").empty();

                // Create table and headers
                var table = $("<table>").addClass("table");
                var thead = $("<thead>").appendTo(table);
                var tbody = $("<tbody>").appendTo(table);
                var headerRow = $("<tr>").appendTo(thead);
                $("<th>").text("Название").appendTo(headerRow);
                $("<th>").text("Адрес").appendTo(headerRow);
                // Add market data to the table
                data.forEach(function (market) {
                    var row = $("<tr>").appendTo(tbody);
                    $("<td>").text(market.name).appendTo(row);
                    $("<td>").text(market.address).appendTo(row);
                    

                    // Delete button
                    var deleteButton = $("<button>").text("Удалить").addClass("btn btn-danger").appendTo($("<td>").appendTo(row));
                    deleteButton.click(function () {
                        deleteMarket(market.id, row);
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


// Function to delete a market
function deleteMarket(marketId, row) {
    if (confirm("Уверены ли вы, что хотите удалить магазин?")) {
        $.ajax({
            url: "/Admin/DeleteMarket", // URL to delete market
            type: 'POST',
            data: { marketId: marketId },
            success: function (data) {
                // Handle successful deletion
                if (data) {
                    row.remove();
                    aler("Магазин успешно удален");
                }
                else{
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