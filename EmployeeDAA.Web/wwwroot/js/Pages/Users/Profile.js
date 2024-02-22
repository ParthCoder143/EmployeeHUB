$(document).ready(function () {
    getData(ApiUrl + "User/GetUserProfile")
        .then(data => {
            if (!isNullEmptyUndefined(data.Data)) {
                BindFormData(data.Data || []);
            }
        }).catch((error) => {
            ShowMessage("Error", error, "error");
            loaderstop();
        });
    $(document).on("click", '#btnProfile', function (e) {
        SubmitEvent("frmProfile");
    });
});

function SubmitEvent(FormId) {
    try {
        CheckForHtmlTextWholeForm(FormId);
        if ($('#' + FormId + '').valid()) {
            if (($('#' + FormId + '').find('.scriptvalidation').length || 0) == 0) {
                loaderstart();
                //form submit one than submit disabled
                $('#' + FormId + '').find(":submit").prop("disabled", true);
                //--------------------post data in api-----------------------------// 
                postData(ApiUrl + $('#' + FormId + '').attr("apiurl"), JSON.stringify($('#' + FormId + '').serializeObject())).then((response) => {
                    if (response.StatusCode == 200) {
                        ShowMessage("Success", response.Message, "success");
                        $('#' + FormId + '').find(":submit").prop("disabled", false);
                    }
                    else {
                        ShowMessage("Error", response.Message, "error");
                        $('#' + FormId + '').find(":submit").prop("disabled", false);
                    }
                    loaderstop();
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