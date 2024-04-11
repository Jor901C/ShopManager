$(document).ready(function () {
    $("#showRevenue").click(function () {
        $(".containerforResult").empty();
        // Creat Form Filter
        var filterForm = $("<form>").addClass("filter-form").appendTo(".containerforResult");

        // Container for managers
        var managerFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
        $("<label>").text("Менеджеры: ").css("font-weight", "bold").css("color", "darkblue").css("margin-right", "20px").appendTo(managerFilterContainer);

        // Container for managers name and chackbox
        var managerNamesContainer = $("<div>").addClass("names-container").css("display", "flex").css("flex-direction", "column-reverse").appendTo(managerFilterContainer);

        // AJAX-call for get managers
        $.ajax({
            url: "/Admin/GetOnlyManager",
            type: "GET",
            success: function (data) {
                data.forEach(function (manager) {
                    var managerContainer = $("<div>").addClass("manager-container").appendTo(managerNamesContainer);
                    $("<input>").attr("type", "checkbox").addClass("manager-checkbox").attr("data-manager-id", manager.stringId).appendTo(managerContainer);
                    $("<span>").text(manager.name + " " + manager.middleName).appendTo(managerContainer);
                });

                loadShops();
            },
            error: function (xhr, status, error) {
                console.error("Ошибка:", error);
            }
        });

        function loadShops() {
            // Container for markets
            var shopFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
            $("<label>").text("Магазины: ").css("font-weight", "bold").css("color", "darkblue").css("margin-right", "20px").appendTo(shopFilterContainer);

            // Контейнер для названий магазинов и чекбоксов
            var shopNamesContainer = $("<div>").addClass("names-container").css("display", "flex").css("flex-direction", "column-reverse").appendTo(shopFilterContainer);

            // AJAX-call for get all market
            $.ajax({
                url: "/Admin/GetAllMarket",
                type: "GET",
                success: function (data) {
                    data.forEach(function (shop) {
                        var shopContainer = $("<div>").addClass("shop-container").appendTo(shopNamesContainer);
                        $("<input>").attr("type", "checkbox").addClass("shop-checkbox").data("market-id", shop.id).appendTo(shopContainer);
                        $("<span>").text(shop.name).appendTo(shopContainer);
                    });

                    addShowButton();
                },
                error: function (xhr, status, error) {
                    console.error("Ошибка:", error);
                }
            });
        }

        function addShowButton() {

            var dateFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
         $("<label>").text("Фильтр по дате: ").css("font-weight", "bold").css("color", "darkblue").css("display", "block").appendTo(dateFilterContainer);

         var firstDayOfMonth = new Date(new Date().getFullYear(), new Date().getMonth(), 1);
         var lastDay = new Date();

         var fromDateInput = $("<input>").attr("type", "date").val(firstDayOfMonth.toISOString().substr(0, 10)).appendTo(dateFilterContainer);
         var toDateInput = $("<input>").attr("type", "date").val(lastDay.toISOString().substr(0, 10)).css("margin-left","10px").appendTo(dateFilterContainer);

         var showButtonContainer = $("<div>").addClass("show-button-container").appendTo(filterForm);
         var showButton = $("<button>").text("Показать").addClass("btn btn-primary").appendTo(showButtonContainer);


            showButton.click(function (event) {
                event.preventDefault();
                var selectedManagers = [];
                $(".manager-checkbox:checked").each(function () {
                    selectedManagers.push($(this).data("manager-id"));
                });

                var selectedShops = [];
                $(".shop-checkbox:checked").each(function () {
                    selectedShops.push($(this).data("market-id"));
                });

                var fromDate = fromDateInput.val();
                var toDate = toDateInput.val();
                $.ajax({
                    url: "/Admin/GetRevenueFilter",
                    type: "POST",
                    data: {
                        managersId: selectedManagers,
                        shopsId: selectedShops,
                        fromDate: fromDate,
                        toDate: toDate
                    },
                    success: function (response) {
                        // Clear container
                        $(".containerforResult").empty();

                        var table = $("<table>").addClass("table").appendTo(".containerforResult");
                        $("<thead>").html("<tr><th>Дата отчета</th><th>Имя Магазина</th><th>Имя Менеджера</th><th>Сумма</th></tr>").appendTo(table);
                        var tbody = $("<tbody>").appendTo(table);
                        //Creat variable for total revenue
                        var totalRevenue = 0; 
                        response.forEach(function (item) {
                            var row = $("<tr>").appendTo(tbody);
                            $("<td>").text(item.date).appendTo(row);
                            $("<td>").text(item.managerName).appendTo(row);
                            $("<td>").text(item.marketName).appendTo(row);
                            $("<td>").text(item.amount).appendTo(row);

                            totalRevenue += item.amount; // Adding current revenue to total
                        });
                        var totalRow = $("<tr>").appendTo(tbody);
                        $("<td colspan='3'>").text("Итого:").css("text-align", "right").appendTo(totalRow); 
                        $("<td>").text(totalRevenue).css("padding-left", "10px").appendTo(totalRow); 
                    },

                    error: function (xhr, status, error) {
                        console.error("Ошибка:", error);
                    }
                });
            });
        }
    });
});

