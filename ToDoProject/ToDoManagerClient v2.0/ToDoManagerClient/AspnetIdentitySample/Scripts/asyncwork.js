$(document).ready(function () {

    $("#changing-button").click(function(event)
    {
        event.preventDefault();
        var toDoData = { "Description": $("#Description").val(), "IsDone": $("#IsDone").val() };
        var form = $("#toDo-form");
        var url = form.attr("action");
        ToDoProj.onchange(toDoData, url);
    });

    $(".toDo-link").click(function(event)
    {
        event.preventDefault();
        var url = $(this).attr("href");
        ToDoProj.get(url);
    });
});

var ToDoProj = ToDoProj || {}
    
ToDoProj.onchange =
    function(toDoData, targetURL, isCreating) {
        $.ajax
        ({
            cache: false,
            type: "POST",
            url: targetURL,
            dataType: "json",
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify(toDoData),
            async: true,
            beforeSend: function()
            {
                $("#img-ajax").remove();
                $("#changing-button").after(" <span id='img-ajax'><img src='/Content/ok.png' /> Data will be changed soon...</span>");
            },
            error: function()
            {
                $("#img-ajax").remove();
                $("body").prepend(" <p id='img-ajax'><img src='/Content/error.png' /> Server error! Data hasn't been changed...</p>");
            }
        });
    }

ToDoProj.get =
    function(targetURL) {
        $.ajax
        ({
            cache: false,
            type: "GET",
            url: targetURL,
            async: true,
            beforeSend: function()
            {
                $("#img-ajax").remove();
                $("body").prepend("<img id='img-ajax' src='/Content/loading.gif' />");
            },
            success: function(result)
            {
                $("#img-ajax").remove();
                $("body").html(result);
                history.pushState(null, null, targetURL);
            },
            error: function()
            {
                $("#img-ajax").remove();
                $("body").prepend("<p id='img-ajax'><img  src='/Content/error.png' /> Server error! Data is not available...</p>");
            }
        });
    }