$("#addMarketTable").click(function () {
    // Clear container content before updating
    $(".containerforResult").empty();
    // Create input form
    var form = $("<form>").addClass("market-form").appendTo(".containerforResult");

    // Create containers for each form element and set their width
    var nameGroup = $("<div>").addClass("form-group").appendTo(form);
    var addressGroup = $("<div>").addClass("form-group").appendTo(form);

    $("<label>").text("Имя магазина: ").addClass("control-label").appendTo(nameGroup);
    var nameInput = $("<input>").attr("type", "text").addClass("form-control").appendTo(nameGroup);

    $("<label>").text("Адрес магазина: ").addClass("control-label").appendTo(addressGroup);
    var addressInput = $("<input>").attr("type", "text").addClass("form-control").appendTo(addressGroup);

    var buttonContainer = $("<div>").addClass("button-container").appendTo(form);

    var submitButton = $("<button>").text("Добавить").addClass("btn btn-primary").appendTo(buttonContainer);

    // Event handler for "Add" button
    submitButton.click(function () {
        var shopName = nameInput.val();
        var shopAddress = addressInput.val();
        addMarket(shopName, shopAddress)
        form.remove();
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
                if (data) {
                    alert("Магазин успешно добавлен");
                }
                else {
                    alert("Ошибка")
                }
                
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Добавление отменено");
    }
}
