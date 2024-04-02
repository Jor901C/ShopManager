$("#addMarketTable").click(function () {
    // Очистка содержимого контейнера перед обновлением
    $(".containerforResult").empty();

    // Создаем форму ввода данных
    var form = $("<form>").addClass("market-form").appendTo(".containerforResult");

    // Создаем контейнеры для каждого элемента формы и устанавливаем им ширину
    var nameGroup = $("<div>").addClass("form-group").appendTo(form);
    var addressGroup = $("<div>").addClass("form-group").appendTo(form);

    // Добавляем поля ввода и метки для имени магазина и адреса
    $("<label>").text("Имя магазина: ").addClass("control-label").appendTo(nameGroup);
    var nameInput = $("<input>").attr("type", "text").addClass("form-control").appendTo(nameGroup);

    $("<label>").text("Адрес магазина: ").addClass("control-label").appendTo(addressGroup);
    var addressInput = $("<input>").attr("type", "text").addClass("form-control").appendTo(addressGroup);

    // Создаем контейнер для кнопки "Добавить"
    var buttonContainer = $("<div>").addClass("button-container").appendTo(form);
    // Добавляем кнопку "Добавить" в контейнер
    var submitButton = $("<button>").text("Добавить").addClass("btn btn-primary").appendTo(buttonContainer);

    // Обработчик события для кнопки "Добавить"
    submitButton.click(function () {
        var shopName = nameInput.val();
        var shopAddress = addressInput.val();
        addMarket(shopName, shopAddress)
        // После добавления магазина можно скрыть форму или выполнить другие действия
        form.remove(); // Удаляем форму после добавления магазина
    });
});


function addMarket(shopName, shopAddress) {
    if (confirm("Уверены ли вы, что хотите добавить новый магазин?")) {
        $.ajax({
            url: "/Admin/AddMarket",
            type: 'POST',
            data: {
                MarketName: shopName,
                MarketAddress: shopAddress
            },
            success: function (data) {
                alert("Магазин успешно добавлен");
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Добавление отменено");
    }
}
