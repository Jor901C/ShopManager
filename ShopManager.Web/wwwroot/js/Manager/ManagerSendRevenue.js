$("#sendReport").click(function () {
    // Очистка содержимого контейнера перед обновлением
    $(".containerforResult").empty();

    // Создаем форму ввода данных
    var form = $("<form>").addClass("market-form").appendTo(".containerforResult");

    // Создаем контейнеры для каждого элемента формы и устанавливаем им ширину
    var nameGroup = $("<div>").addClass("form-group").appendTo(form);
    var addressGroup = $("<div>").addClass("form-group").appendTo(form);
    var revenueGroup = $("<div>").addClass("form-group").appendTo(form);
    var dateGroup = $("<div>").addClass("form-group").appendTo(form);

    $.ajax({
        url: '/Manager/GetFullName',
        type: 'GET',
        success: function (data ) {
            // Получаем имя, фамилию и отчество пользователя из ASP.NET Identity (замените это на ваш способ получения данных о пользователе)
            var userFullName = data.FullName; // Замените на ваше получение имени, фамилии и отчества пользователя
            var userId = data.UserId;
            // Добавляем поля ввода и метки для имени магазина, адреса, суммы выручки и даты
            $("<label>").text("ФИО: ").addClass("control-label").appendTo(nameGroup);
            var nameInput = $("<input>").attr("type", "text").addClass("form-control").val(userFullName).attr("readonly", true).appendTo(nameGroup);

            $("<label>").text("Название магазина: ").addClass("control-label").appendTo(addressGroup);
            var selectInput = $("<select>").addClass("form-control").appendTo(addressGroup);
            $.ajax({
                url: '/Manager/GetAllMarket',
                type: 'GET',
                success: function (markets) {
                    // Пример вариантов магазинов (замените на свои)
                    var stores = markets;

                    // Добавляем варианты магазинов в элемент select
                    stores.forEach(function (store) {
                        $("<option>").val(store).text(store).appendTo(selectInput);
                    });

                    $("<label>").text("Сумма выручки: ").addClass("control-label").appendTo(revenueGroup);
                    var revenueInput = $("<input>").attr("type", "number").addClass("form-control").appendTo(revenueGroup);

                    var currentDate = new Date().toLocaleDateString('ru-RU'); 
                    $("<label>").text("Дата: ").addClass("control-label").appendTo(dateGroup);
                    var dateInput = $("<input>").attr("value", currentDate).addClass("form-control").appendTo(dateGroup);


                    // Создаем контейнер для кнопки "Отправить отчет"
                    var buttonContainer = $("<div>").addClass("button-container").appendTo(form);
                    // Добавляем кнопку "Отправить отчет" в контейнер
                    var submitButton = $("<button>").text("Отправить отчет").addClass("btn btn-primary").appendTo(buttonContainer);

                    // Обработчик события для кнопки "Отправить отчет"
                    submitButton.click(function (event) {
                        event.preventDefault(); // Предотвращаем обычное поведение отправки формы

                        // Получаем данные из полей ввода
                        var managerId = userId;
                        var marketId = selectInput.val(); // Теперь получаем значение из элемента select
                        var revenue = revenueInput.val();
                        var date = dateInput.val();
                        addSales(managerId, marketName, revenue, date);
                        

                        // Очищаем поля ввода после отправки отчета
                        revenueInput.val("");
                        dateInput.val("");
                    });
                }
            });
        }
    });
});
function addSales(name, marketName, revenue, date) {
    if (confirm("Уверены ли вы, что хотите отправить отчет?")) {
        $.ajax({
            url: "/Manager/AddSales",
            type: 'POST',
            data: {
                ManagerName: name,
                Marketname: marketName,
                Revenue: revenue,
                Date: date
            },
            success: function (data) {
                alert("Отчет успешно отправлен");
            },
            error: function (xhr, status, error) {
                console.error("Error:", error);
            }
        });
    } else {
        alert("Отправление отменено");
    }
}