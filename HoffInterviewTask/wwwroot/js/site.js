$("#btnRate").click(function (elem) {
    var messageElement = $("#message");
    messageElement.addClass("invisible");

    var btn = $(this);
    btn.attr("disabled", "disabled");

    var showMessage = (msg, isError) => {
        var classToAdd = "alert-success";
        var classToRemove = "alert-danger";

        if (isError === true) {
            var temp = classToAdd;
            classToAdd = classToRemove;
            classToRemove = temp;
        }

        messageElement
            .removeClass("invisible")
            .removeClass(classToRemove)
            .addClass("visible")
            .addClass(classToAdd)
            .html(msg);

        btn.removeAttr("disabled");
    };

    var isValidCoordinate = (val) => {
        var parsed = parseFloat(val);

        return isNaN(parsed);
    }

    var x = $("#xCoord").val();
    var y = $("#yCoord").val();

    if (isValidCoordinate(x) || isValidCoordinate(y)) {
        showMessage("Введены некорректные координаты. Допускаются положительные и отрицательные числа. В качестве разделителя нужно использовать точку. Пример, -1.72", true);
        return;
    }

    $.get(`/rates/coordinate/${x}/${y}`, function (res) {
            var date = new Date(res.date);
            showMessage(`Курс для валюты с кодом ${res.currency} за дату ${date.toLocaleDateString("ru-RU")} равен ${res.rate}`);
        })
        .fail(function(res) {
            showMessage(res.responseText, true);
        });
});