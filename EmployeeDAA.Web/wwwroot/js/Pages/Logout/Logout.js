
    $("#LoginBtn").click(function () {
        loaderstart();
        getData(ApiUrl + "Login/Logout")
            .then(data => {
                window.location.href = LoginUrl + (!(data.Data.IsAdLogin || false) ? "/Custom" : "");
            })
            .catch((error) => {
                ShowMessage("Error", error, "error");
                loaderstop();
                window.location.href = LoginUrl;
            });
    });
