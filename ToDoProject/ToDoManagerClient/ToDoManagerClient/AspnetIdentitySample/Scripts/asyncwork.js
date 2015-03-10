$(document).ready(function () {
    $('#create').click(function () {
        var data = { 'Description': $('#Description').val(), 'IsDone': $('#IsDone').val() };
        $.ajax
        ({
            cache: false,
            type: 'POST',
            url: '/ToDo/Create',
            dataType: 'json',
            contentType: 'application/json; charset=utf-8',
            data: JSON.stringify(data),
            async: true,
            success: function(result) { alert("OK"); } // temp
        })
    });
});