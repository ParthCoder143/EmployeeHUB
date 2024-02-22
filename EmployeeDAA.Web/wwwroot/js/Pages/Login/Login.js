$(document).ready(function () {
    Refreshchange();
    commonPasswordToggleButton();
    $(document).on("click", '#btnLogin', function (e) {
        if ($('#frmLogin').valid()) {
            //var url = LoginUrl + "/VerifyCaptch?CaptchaCode=" + $("#CaptchaCode").val();
            //$.getJSON(url, function (data) {
            //    if ((data.success || 0) == 200)
            SubmitEvent("frmLogin");
            //    else {
            //        ShowMessage("Error", data.returnMsg, "error");
            //        loaderstop();
            //    }
            //});
        }
    });
    $("#CaptchaCode").on('keyup', function (e) {
        if (e.key === 'Enter' || e.keyCode === 13) {
            $("#btnLogin").trigger("click");
        }
    });
});
function Refreshchange() {
    localStorage.clear();
    $.getJSON(ApiUrl + 'login/GetCaptcha', function (data) {
        if ((data.StatusCode || 0) == 200) {
            $('#CaptchaImage').attr('src', 'data:image/png;base64,' + data.Data.Img);
            $("#CaptchaToken").val(data.Data.Token);
        }
        else {
            ShowMessage("Error", data.returnMsg, "error");
            loaderstop();
        }
    });
}
function SubmitEvent(FormId) {
    localStorage.clear();
    try {
        if ($('#' + FormId + '').valid()) {
            if (($('#' + FormId + '').find('.scriptvalidation').length || 0) == 0) {
                loaderstart();
                //form submit one than submit disabled
                $('#' + FormId + '').find(":submit").prop("disabled", true);
                //--------------------post data in api-----------------------------// 
                postData(ApiUrl + $('#' + FormId + '').attr("apiurl"), JSON.stringify($('#' + FormId + '').serializeObject())).then((response) => {
                    if (response.StatusCode == 200) {
                        localStorage.setItem('MenuPermission', response.Data.Permissions);
                        localStorage.setItem('UserToken', response.Data.Token);
                        localStorage.setItem('UserName', response.Data.UserName);
                        localStorage.setItem('ShortName', response.Data.ShortName);
                        localStorage.setItem('Role', response.Data.Role);
                        window.location.href = LoginUrl.replace("Login", "Home/Index");
                    }
                    else {
                        ShowMessage("Error", response.Message, "error");
                        $('#' + FormId + '').find(":submit").prop("disabled", false);
                        loaderstop();
                    }
                });
                //------------------------------------------------------------//
            }
            else
                return false;
        }
    } catch (e) {
        return false;
    }
}
function loaderstart() {
    $('.loader-main').show();
    $(".loader-main").css("z-index", "1051");
}

function loaderstop() {
    $('.loader-main').hide();
}
$(document).on("click", ".captcha__refresh", function () {
    Refreshchange();
})