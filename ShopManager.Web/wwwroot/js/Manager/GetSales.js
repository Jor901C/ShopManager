$(document).ready(function () {
    $("#showAllRevenue").click(function () {
        $.ajax({
            url: "/Manager/GetFullName",
            type: 'GET',
            success: function (data) {

                $(".containerforResult").empty();
                var managerId = data.userId;

                // Nested AJAX request to get manager's sales data
                $.ajax({
                    url: "/Manager/GetSalesForManager",
                    type: 'GET',
                    data: {
                        managerId: managerId
                    },
                    success: function (salesData) {
                        // Create table and headers
                        var table = $("<table>").addClass("table");
                        var thead = $("<thead>").appendTo(table);
                        var tbody = $("<tbody>").appendTo(table);
                        var headerRow = $("<tr>").appendTo(thead);
                        $("<th>").text("Дата").appendTo(headerRow);
                        $("<th>").text("Выручка").appendTo(headerRow);
                        $("<th>").text("Магазин").appendTo(headerRow);
                        $("<th>").text("День Обновления").appendTo(headerRow);
                        $("<th>").text("Количество Обновлений").appendTo(headerRow);

                        var totalRevenue = 0; // Variable to store total revenue

                        salesData.forEach(function (sales) {
                            var row = $("<tr>").appendTo(tbody);
                            var createDate = new Date(sales.date);
                            $("<td>").text(createDate.toLocaleDateString('ru-RU')).appendTo(row);
                            $("<td>").text(sales.amount).appendTo(row);
                            $("<td>").text(sales.marketName).appendTo(row);
                            var updateDateText = ""; 

                            if (sales.changeDate.trim() === "") {
                                updateDateText = ""; 
                            } else {
                                var updateDate = new Date(sales.changeDate);
                                updateDateText = updateDate.toLocaleDateString('ru-RU'); 
                            }

                            $("<td>").text(updateDateText).appendTo(row);

                            $("<td>").text(sales.isChanged).appendTo(row);
                       


                            totalRevenue += sales.amount; // Add current revenue to total

                            var editBtn = $("<button>").text("Изменить").addClass("edit-btn").data("salesId", sales.id);
                            var actionCell = $("<td>").append(editBtn).appendTo(row);
                        });

                        
                        var totalRow = $("<tr>").appendTo(tbody);
                        $("<td>").text("Итого:").appendTo(totalRow);
                        $("<td>").text(totalRevenue).appendTo(totalRow); 
                        $("<td>").appendTo(totalRow);

                        $(".containerforResult").append(table);
                    },
                    error: function (xhr, status, error) {
                        console.error("Error:", error);
                    }
                });
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    });

    $(".containerforResult").on("click", ".edit-btn", function () {
        var salesId = $(this).data("salesId");

        $(".containerforResult").empty();

        // Create form for editing sales data
        var form = $("<form>").addClass("market-form").appendTo(".containerforResult");

        // Create containers for each form element and set their width
        var nameGroup = $("<div>").addClass("form-group").appendTo(form);
        var revenueGroup = $("<div>").addClass("form-group").appendTo(form);
        var dateGroup = $("<div>").addClass("form-group").appendTo(form);

        // Add label and input field for revenue
        $("<label>").text("Сумма выручки: ").addClass("control-label").appendTo(revenueGroup);
        var revenueInput = $("<input>").attr("type", "number").addClass("form-control").appendTo(revenueGroup);

        // Add label and input field for date
        var currentDate = new Date().toLocaleDateString('ru-RU');
        $("<label>").text("Дата: ").addClass("control-label").appendTo(dateGroup);
        var dateInput = $("<input>").attr("value", currentDate).addClass("form-control").appendTo(dateGroup);

        // Add label and select list for shops
        $("<label>").text("Магазин: ").addClass("control-label").appendTo(nameGroup);
        var selectInput = $("<select>").addClass("form-control").appendTo(nameGroup);

        // Send AJAX request to get the list of shops
        $.ajax({
            url: '/Manager/GetAllMarket',
            type: 'GET',
            success: function (markets) {
                markets.forEach(function (market) {
                    $("<option>").val(market.id).text(market.name).appendTo(selectInput);
                });
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });

        var buttonContainer = $("<div>").addClass("button-container").appendTo(form);
        var saveButton = $("<button>").text("Сохранить").addClass("btn btn-primary").appendTo(buttonContainer);

        saveButton.click(function (event) {
            event.preventDefault(); // Prevent default form submission

            var revenue = revenueInput.val();
            var date = dateInput.val();
            var marketId = selectInput.val();

            saveSales(salesId, revenue, date, marketId);
        });
    });


    // Function to save edited sales data
    function saveSales(salesId, revenue, date, marketId) {
        if (confirm("Вы уверены что хотите изменить отчетность?")) {
            $.ajax({
                url: "/Manager/ChangeSalesLastMounth", 
                type: 'POST',
                data: {
                    salesId: salesId,
                    newAmount: revenue,
                    newDate: date,
                    newMarketId: marketId
                },
                success: function (response) {
                    if (response) {
                        alert("Вы успешно изменили отчетность!")
                        $(".containerforResult").empty();
                    }
                    else {
                        alert("Некорректно указана дата  !")
                    }
                },
                error: function (xhr, status, error) {
                    console.error("Error:", error);
                }
            });
        }
        else {
            alert("Изминение отменено!");
            $(".containerforResult").empty();
        }
    }
});
