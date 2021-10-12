$(document).ready(function () {
    $("#connect").on("click", event => {
        $("#username-result").html("");
        $("#password-result").html("");
        $.ajax({
            url: `/Home/Login`,
            method: "POST",
            data: {username: $("#username").val(), password: $("#password").val()}
        }).done(data => {
            console.log(data);
            if (data == "user_not_found") {
                $("#username-result").html("Utilisateur introuvable");
            } else if (data == "wrong_password") {
                $("#password-result").html("Mot de pass incorrect");
            } else {
                window.location.href = "/Home/Index";
            }
        });
    });
});