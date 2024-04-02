$(document).ready(function () {
    $("#showRevenue").click(function () {
        $(".containerforResult").empty();

        // Создаем форму фильтров
        var filterForm = $("<form>").addClass("filter-form").appendTo(".containerforResult");

        // Контейнер для фильтра менеджеров
        var managerFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
        $("<label>").text("Менеджеры: ").appendTo(managerFilterContainer);

        // Контейнер для имен менеджеров и чекбоксов
        var managerNamesContainer = $("<div>").addClass("names-container").appendTo(managerFilterContainer);

        // AJAX-запрос для получения списка всех менеджеров
        $.ajax({
            url: "/Admin/GetOnlyManager", // Укажите правильный URL для получения списка менеджеров
            type: "GET",
            success: function (data) {
                // Добавляем имена менеджеров и чекбоксы
                data.forEach(function (manager) {
                    var managerContainer = $("<div>").addClass("manager-container").appendTo(managerNamesContainer);
                    $("<span>").text(manager.name + " " + manager.middleName).appendTo(managerContainer);
                    $("<input>").attr("type", "checkbox").addClass("manager-checkbox").appendTo(managerContainer);
                });
            },
            error: function (xhr, status, error) {
                console.error("Ошибка:", error);
            }
        });

        // Контейнер для фильтра магазинов
        var shopFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
        $("<label>").text("Магазины: ").appendTo(shopFilterContainer);

        // Контейнер для названий магазинов и чекбоксов
        var shopNamesContainer = $("<div>").addClass("names-container").appendTo(shopFilterContainer);

        // AJAX-запрос для получения списка всех магазинов
        $.ajax({
            url: "/Admin/GetAllMarket", // Укажите правильный URL для получения списка магазинов
            type: "GET",
            success: function (data) {
                // Добавляем названия магазинов и чекбоксы
                data.forEach(function (shop) {
                    var shopContainer = $("<div>").addClass("shop-container").appendTo(shopNamesContainer);
                    $("<span>").text(shop.name).appendTo(shopContainer);
                    $("<input>").attr("type", "checkbox").addClass("shop-checkbox").appendTo(shopContainer);
                });
            },
            error: function (xhr, status, error) {
                console.error("Ошибка:", error);
            }
        });

        // Контейнер для фильтрации по дате
        var dateFilterContainer = $("<div>").addClass("filter-container").appendTo(filterForm);
        $("<label>").text("Фильтр по дате: ").appendTo(dateFilterContainer);
        var fromDateInput = $("<input>").attr("type", "date").appendTo(dateFilterContainer);
        var toDateInput = $("<input>").attr("type", "date").appendTo(dateFilterContainer);

        // Кнопка "Показать"
        var showButtonContainer = $("<div>").addClass("show-button-container").appendTo(filterForm);
        var showButton = $("<button>").text("Показать").addClass("btn btn-primary").appendTo(showButtonContainer);

        // Обработчик события для кнопки "Показать"
        showButton.click(function () {
            // Получите выбранные значения фильтров и выполните необходимые действия
        });
    });
});
