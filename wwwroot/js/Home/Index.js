"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

connection.on("ReceiveMessage", function (user, message) {
    $("#messages").append(`<p>${user} : ${message}</p>`);
    $("#messages")[0].scrollTo(0, $("#messages")[0].scrollHeight);
});

connection.start().then(function () {
    document.getElementById("message").addEventListener("keypress", function (event) {
        if (event.keyCode == 13) {
            var user = document.getElementById("username").value;
            var message = document.getElementById("message").value;
            connection.invoke("SendMessage", user, message).then(function () {
                document.getElementById("message").value = "";
            }).catch (function (err) {
                return console.error(err.toString());
            });
        }
    });
}).catch(function (err) {
    return console.error(err.toString());
});