$("#sendReport").click(function () {

    $(".containerforResult").empty();

    // СCreat form
    var form = $("<form>").addClass("market-form").appendTo(".containerforResult");

    // Creat container 
    var nameGroup = $("<div>").addClass("form-group").appendTo(form);
    var addressGroup = $("<div>").addClass("form-group").appendTo(form);
    var revenueGroup = $("<div>").addClass("form-group").appendTo(form);
    var dateGroup = $("<div>").addClass("form-group").appendTo(form);

    $.ajax({
        url: '/Manager/GetFullName',
        type: 'GET',
        success: function (data ) {

            var userName = data.fullName;
            var userId = data.userId
            // Adding input fields and labels for the store name, address, revenue amount and date
            $("<label>").text("ФИО: ").addClass("control-label").appendTo(nameGroup);
            var nameInput = $("<input>").attr("type", "text").addClass("form-control").val(userName).attr("readonly", true).appendTo(nameGroup);

            $("<label>").text("Название магазина: ").addClass("control-label").appendTo(addressGroup);
            var selectInput = $("<select>").addClass("form-control").appendTo(addressGroup);
            $.ajax({
                url: '/Manager/GetAllMarket',
                type: 'GET',
                success: function (markets) {
                    var stores = markets;
                    stores.forEach(function (store) {
                        $("<option>").val(store.name).text(store.name).attr("data-store-id", store.id).appendTo(selectInput);
                    });

                    $("<label>").text("Сумма выручки: ").addClass("control-label").appendTo(revenueGroup);
                    var revenueInput = $("<input>").attr("type", "number").addClass("form-control").appendTo(revenueGroup);

                    var currentDate = new Date().toLocaleDateString('ru-RU'); 
                    $("<label>").text("Дата: ").addClass("control-label").appendTo(dateGroup);
                    var dateInput = $("<input>").attr("value", currentDate).addClass("form-control").appendTo(dateGroup);


                    var buttonContainer = $("<div>").addClass("button-container").appendTo(form);
                    var submitButton = $("<button>").text("Отправить отчет").addClass("btn btn-primary").appendTo(buttonContainer);

                    submitButton.click(function (event) {
                        event.preventDefault(); 

                        var managerId = data.userId;
                        var marketId = $("option:selected", selectInput).data("store-id"); 
                        var revenue = revenueInput.val();
                        var date = dateInput.val();
                        addSales(managerId, marketId, revenue, date);
                    });
                }
            });
        }
    });
});

//Function for add sales
function addSales(managerId, marketId, revenue, date) {
    if (confirm("Уверены ли вы, что хотите отправить отчет?")) {
        $.ajax({
            url: "/Manager/AddSales",
            type: 'POST',
            data: {
                ManagerId: managerId,
                MarketId: marketId,
                Revenue: revenue,
                Date: date
            },
            success: function (response) {
                if (response) {
                    alert("Отчет успешно отправлен");
                    $(".containerforResult").empty();
                }
                else {
                    alert("Некорректно указана дата ." );
                }
               
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Отправление отменено");
    }
}