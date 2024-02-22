

$(document).ready(function () {
    getData(ApiUrl + "User/GetUserProfile")
        .then(data => {
        }).catch((error) => {
        });
});