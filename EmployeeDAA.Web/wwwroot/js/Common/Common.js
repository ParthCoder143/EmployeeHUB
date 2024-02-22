function isNullEmptyUndefined(_val) {
    return (_val == undefined || _val == null || _val == "" || _val == "null" || _val == "undefined") ? true : false;
}
function getFloat(_val) {
    return (_val == undefined || _val == null || _val == "") ? 0 : parseFloat(_val);
}
function getInt(_val) {
    return (_val == undefined || _val == null || _val == "") ? 0 : parseInt(_val);
}
function getParameterByName(name, url) {
    if (isNullEmptyUndefined(url))
        url = window.location.href;
    name = name.replace(/[\[\]]/g, '\\$&');
    var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
        results = regex.exec(url);
    if (!results) return null;
    if (!results[2]) return '';
    return decodeURIComponent(results[2].replace(/\+/g, ' '));
}
function convertDateToString(date, format, dateFormat) {
    var convertedDate;
    if (!isNullEmptyUndefined(dateFormat))
        convertedDate = moment(date, dateFormat);
    else
        convertedDate = moment(date);
    if (convertedDate.isValid())
        return convertedDate.format(format);
    else
        return "";
}
// -----------------------------------Form popup--------------------------------------------
function loaderstart() {
    $('.loader-main').show();
    $(".loader-main").css("z-index", "1051");
}

function loaderstop() {
    $('.loader-main').hide();
}

// -----------------------------------Section Loader--------------------------------------------
function sectionloaderstart(pos = 'left') {
    $('.' + pos + '-order-details').show();
    $('.' + pos + '-order-details').css("z-index", "1051");
}

function sectionloaderstop(pos = 'left') {
    $('.' + pos + '-order-details').hide();
}

$(document).ajaxStart(function (event, xhr, settings) {
    loaderstart();
});
$(document).ajaxComplete(function (event, xhr, settings) {
    if (!settings.url.includes('VerifyCaptch'))
        loaderstop();
});
$(document).ajaxError(function (event, xhr, options, exc) {
    // session expire time redirect to login page
    //bhavdip 04/may/2020 13:48
    if (xhr.status == 401) {
        // window.location.href = LoginUrl; 
        ShowMessage("Error", xhr.responseJSON.Message, "error");
    }
    loaderstop();
});
//---------------------------------------------------------------------
// PopUpContent = URL to load popup content
function OpenPopup(modalId, modalContainid, url, FormId, PopUpContent, callback, afterpopup) {
    loaderstart();
    AutoFocusElement(modalId, FormId);
    $('#' + modalContainid + '').empty();
    $('#' + modalContainid).load(PopUpContent, function (response, status, xhr) {
        $(".swal2-close").trigger("click");
        if ((url || "").length > 0) {
            getData(ApiUrl + url)
                .then(data => {
                    if (data.StatusCode != 200) {
                        ShowMessage("Error", data.Message, "error");
                        return;
                    }
                    //----------------------always same in the form ctrl and api model-----------------------------////////////////////              
                    BindFormData((data.Data || []), modalId);
                    afterGet(this, FormId, modalId, callback, afterpopup);
                })
                .catch((error) => {
                    ShowMessage("Error", data.Message, "error");
                    loaderstop();
                });
        }
        else {
            afterGet(this, FormId, modalId, callback, afterpopup);
        }
    });
}

function OpenPopupWithoutForm(modalId, modalContainid, url, FormId, PopUpContent, callback, afterpopup) {
    loaderstart();
    //AutoFocusElement(modalId, FormId);
    $('#' + modalContainid + '').empty();
    $('#' + modalContainid).load(PopUpContent, function (response, status, xhr) {
        $(".swal2-close").trigger("click");
        // Append the submit button to the modal content with styling
        $('#' + modalContainid).append('<div class="text-center mt-3">' +
            '<button type="button" class="btn btn-success" id="submitButton" style="display: none;">Submit</button>' +
            '</div>');
        $('#' + modalContainid).on('change', '.grd-chk', function () {
            var anyCheckboxChecked = $('.grd-chk:checked').length > 0;
            $('#submitButton').toggle(anyCheckboxChecked);
        });
        $('#' + modalId + '').modal('toggle');
        if (callback)
            callback();
    });
}
function BindFormData(data, modalId) {
    for (const [key, value] of Object.entries(data)) {
        if (!$("#" + key).hasClass('NoBind')) {
            if ($("#" + key).is('select'))
                $("#" + key).attr("asp-other-value", value);
            else if ($("#" + key).hasClass('datepicker')) {
                if ((value || '').length != 0)
                    $("#" + key).val(convertDateToString(value, "DD/MM/YYYY"));
            }
            else if ($("#" + key).is('input[type=checkbox]'))
                $("#" + key).prop('checked', (value == "true" || value || value == 1) ? true : false)
            else if ($("#" + key).is('label')) {
                if ($("#" + key).attr('id') == "LastResponse") {
                    if (IsJsonString(value) && (value || "").length > 0) {
                        var rsp = JSON.parse(value);
                        $("#" + key).html(rsp.Message || "No data found.");
                    }
                    else
                        $("#" + key).html(value || "No data found.");
                }
                else if ($("#" + key).attr('id') == "Body") {
                    $("#" + key).html(StringToHTML(value));
                }
                else
                    $("#" + key).html(value);
            }
            else if ($("#" + key).is('table') || $("#" + key).is('div')) {
                if (typeof bindTableWithData === 'function') {
                    bindTableWithData(value, modalId); // this function need to create in your js so their you will get data and render according your need.
                }
            }
            else
                $("#" + key).val(value);
        }
    }
}
function afterGet(obj, FormId, modalId, callback, afterpopup) {
    //------------------------------------------------------------------------------------------------////////////////
    $('#' + modalId + '').modal({
        keyboard: false,
        backdrop: 'static'
    }, 'show');
    if (!isNullEmptyUndefined(FormId)) {
        setTimeout(function () {
            $("input, select, textarea").attr("autocomplete", "off");
            $('#' + FormId + '').validate();
        }, 500);
        //-------------------------------------------------------------------------------------------------/////////////
        bindForm(obj, FormId, modalId, callback);
    }
    if (afterpopup)
        afterpopup()
}
//---------------------------------josn formate convertor---------------------------------------//
$.fn.serializeObject = function () {
    var o = {};
    var a = this.serializeArray();
    $.each(a, function () {
        if (o[this.name]) {
            if (!o[this.name].push) {
                o[this.name] = [o[this.name]];
            }
            if ((this.value || '').length > 0)
                o[this.name].push(this.value || '');
        } else {
            if ((this.value || '').length > 0)
                if ($("#" + this.name).is('select') && $("#" + this.name).attr("multiple")) {
                    o[this.name] = (this.value || '').split(",");
                }
                else if ($("#" + this.name).hasClass('datepicker')) {
                    if ((this.value || '').length == 0)
                        o[this.name] = null
                    else
                        o[this.name] = new Date(moment((this.value || ''), "DD/MM/YYYY").format('YYYY-MM-DD'));
                }
                else if (this.value == "true") o[this.name] = true;
                else if (this.value == "false") o[this.name] = false;
                else
                    o[this.name] = this.value || '';
        }
    });
    return o;
};

function GetFormData(a) {
    var formData = new FormData();
    var fileField = document.querySelector("input[type='file']");
    $.each(a, function () {
        if ((this.value || '').length > 0)
            if ($("#" + this.name).is('select') && $("#" + this.name).attr("multiple")) {
                formData.append(this.name, (this.value || '').split(","));
            }
            else if ($("#" + this.name).hasClass('datepicker')) {
                if ((this.value || '').length == 0)
                    formData.append(this.name, null);
                else
                    formData.append(this.name, new Date(moment((this.value || ''), "DD/MM/YYYY").format('YYYY-MM-DD')));
            }
            else
                formData.append(this.name, this.value || '');
    });
    $('input[type="file"]').each(function () {
        formData.append($(this).attr("id"), $(this)[0].files[0]);
    })
    return formData;
}
function bindForm(dialog, FormId, modalId, callback) {
    //change error placement of select dropdown    
    SelectPickerValidation(FormId);
    $('#' + FormId + '', dialog).submit(function (e) {
        e.preventDefault();
        try {
            CheckForHtmlTextWholeForm(FormId);
            if ($('#' + FormId + '').valid()) {
                if (($('#' + FormId + '').find('.scriptvalidation').length || 0) == 0) {
                    loaderstart();
                    var result;

                    //form submit one than submit disabled
                    if ($('#' + FormId + '').find(":submit").is(':disabled')) {
                        return false;
                    }
                    $('#' + FormId + '').find(":submit").prop("disabled", true);
                    //--------------------post data in api-----------------------------// 
                    if ($(this).attr("enctype") == "multipart/form-data")
                        result = postDataWithImage(ApiUrl + $(this).attr("apiurl"), GetFormData($(this).serializeArray()), $(this).attr("method"));
                    else
                        result = postData(ApiUrl + $(this).attr("apiurl"), JSON.stringify($(this).serializeObject()));

                    result.then((response) => {
                        if (response.StatusCode == 200) {
                            ShowMessage("Success", response.Message, "success");
                            if (FormId != "frmDocumentTypesPermission") {
                                $('#' + modalId + '').modal('toggle');
                            }
                            $('#' + FormId + '').find(":submit").prop("disabled", false);
                            if (callback)
                                callback();
                        }
                        else {
                            loaderstop();
                            ShowMessage("Error", response.Message, "error");
                            $('#' + FormId + '').find(":submit").prop("disabled", false);
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
    });
}
//---------------------------------delete button functionality-------------------------------------------------//
function DeleteGridData(obj, name, DeleteUrl, MessageTitle, reloadGrid) {
    var ids = [];
    $('#' + name).find('input[ChekBoxType="tblcolDeleteCheckBox"]:checked').each(function () {
        ids.push($(this).val());
    });
    if ((ids || []).length == 0) {
        ShowMessage("Info", "Please select at least one.", "info");
        return;
    }
    Delete(ids, obj, DeleteUrl, 'Are you sure want to delete this ' + (MessageTitle || "").toLocaleLowerCase() + '?', 'Yes', 'No', reloadGrid);
}
function SingleDeleteGridData(obj, val, DeleteUrl, MessageTitle, reloadGrid) {
    var ids = [];
    ids.push(val);
    if ((ids || []).length == 0) {
        ShowMessage("Info", "Please select at least one.", "info");
        return;
    }
    Delete(ids, obj, DeleteUrl, 'Are you sure want to delete this ' + (MessageTitle || "").toLocaleLowerCase() + '?', 'Yes', 'No', reloadGrid);
}
function Delete(data, e, url, msg, yes, no, fn_callback) {
    if (!yes) yes = 'Proceed';
    if (!no) no = 'Cancel';
    Swal.fire({
        icon: 'warning',
        title: 'Are you sure?',
        text: msg,
        confirmButtonText: yes,
        cancelButtonText: no,
        showCancelButton: true,
        customClass: {
            actions: 'my-actions',
            cancelButton: 'order-1 right-gap',
            confirmButton: 'order-2',
        }
    }).then((result) => {
        if (result.isConfirmed) {
            loaderstart();
            postData((ApiUrl + url), JSON.stringify(data), 'DELETE').then((response) => {
                if (response.StatusCode) {
                    ShowMessage("Success", response.Message, "success");
                    if (fn_callback)
                        fn_callback();
                }
                else {
                    ShowMessage("Error", response.Message, "error");
                }
                loaderstop();
            });
        }
    });
}
$(document).on("focusout", "input[type='text'],textarea", function (event) {
    CheckForHtmlText(this);
});
function CheckForHtmlText(obj) {
    var regex = /(^((?=.*&#).*)$)|(^((?=.*(<[A-Za-z]|<!|<\?|<\/)).*)$)/;
    var text = $(obj).val().replace(/[\r\n]+(?=[^\r\n])/g, ' ');
    if (regex.test(text)) {
        if (!$(obj).next().hasClass("scriptvalidation")) {
            if (!$(obj).hasClass("removehtmltextcheck")) {
                $("<label class='scriptvalidation text-danger'>Invalid Data.</label>").insertAfter(obj);
            }
        }
    }
    else {
        if ($(obj).next().hasClass("scriptvalidation"))
            $(obj).next().remove();
    }
}
function CheckForHtmlTextWholeForm(formId) {
    $("#" + formId + " input[type='text'],#" + formId + " textarea").each(function () {
        CheckForHtmlText(this);
    });
}
// --------------------------------------------------------------------------------------------------------------------------------

function isNullEmptyUndefined(_val) {
    return (_val == undefined || _val == null || _val == "" || _val == "null" || _val == "undefined") ? true : false;
}

function replaceNullwithBlank(_val) {
    if (isNullEmptyUndefined(_val))
        return "";
    else
        return _val;
}
$(document).on("keypress", ".numberOnly", function (event) {
    if ((event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});
////////////////////////-----------------------Display Swal Message---------------------------------////////////////////////////
function ShowMessage(title, msg, icon, position, timer, fn) {
    if (isNullEmptyUndefined(timer))
        timer = 0;
    if (!position) position = 'top';
    Swal.fire({
        icon: icon,
        //title: 'Success',
        text: msg,
        showConfirmButton: false,
        timer: (timer > 0 ? timer : icon != 'success' ? 0 : 3000),
        timerProgressBar: (timer > 0 ? true : icon != 'success' ? false : true),
        toast: true,
        position: position,
        showCloseButton: true,
        showClass: {
            popup: 'animate__animated animate__fadeInUp'
        },
        hideClass: {
            popup: ''
        }
    }).then(
        function () {
            $(window).resize();
            if (fn)
                fn();
        },
        function (dismiss) {
            $(window).resize();
        });
}
function ShowConfirmation(title, msg, yes, no, fn_yes, fn_no) {
    if (!yes) yes = 'Proceed';
    if (!no) no = 'Cancel';
    Swal.fire({
        icon: 'warning',
        title: title,
        text: msg,
        confirmButtonText: yes,
        cancelButtonText: no,
        showCancelButton: true,
        customClass: {
            actions: 'my-actions',
            cancelButton: 'order-1 right-gap',
            confirmButton: 'order-2',
        }
    }).then((result) => {
        if (result.isConfirmed) {
            ShowMessage("Success", response.Message, "success");
            if (fn_yes)
                fn_yes()
        } else if (result.isDenied) {
            ShowMessage("Info", response.Message, "info");
            if (fn_no)
                fn_no();
        }
    });
}

function UpdateStatus(obj, url, id, MessageTitle, callback) {
    if ((MessageTitle || "").length == 0) {
        loaderstart();
        postData(ApiUrl + url, JSON.stringify(id)).then((response) => {
            if (response.StatusCode == 200) {
                ShowMessage("Success", response.Message, "success");
                if (callback)
                    callback();
            }
            else {
                ShowMessage("Error", response.Message, "error");
            }
            loaderstop();
        });
    }
    else {
        Swal.fire({
            icon: 'warning',
            title: 'Are you sure?',
            text: MessageTitle,
            confirmButtonText: 'Proceed',
            cancelButtonText: 'Cancel',
            showCancelButton: true,
            customClass: {
                actions: 'my-actions',
                cancelButton: 'order-1 right-gap',
                confirmButton: 'order-2',
            }
        }).then((result) => {
            if (result.isConfirmed) {
                loaderstart();
                postData(ApiUrl + url, JSON.stringify(id)).then((response) => {
                    if (response.StatusCode == 200) {
                        ShowMessage("Success", response.Message, "success");
                        if (callback)
                            callback();
                    }
                    else {
                        ShowMessage("Error", response.Message, "error");
                    }
                    loaderstop();
                });
            }
            else {
                $(obj).prop("checked", !$(obj).prop("checked"));
            }
        });
    }
}
function CommonUpdate(url, data, btn, callback) {
    loaderstart();
    postData(ApiUrl + url, JSON.stringify(data)).then((response) => {
        if (response.StatusCode == 200) {
            ShowMessage("Success", response.Message, "success");
            setTimeout(() => {
                if (callback)
                    callback(response);
            }, 3000);
        }
        else {
            ShowMessage("Error", response.Message, "error");
            $(btn).prop("disabled", false);
        }
        loaderstop();
    });
}

function SetUrlOrderDetails() {
    $("#orddtl").attr("href", "ManageOrder?OrderId=" + getParameterByName("OrderId"));
    $("#ordfiles").attr("href", "OrderDocuments?OrderId=" + getParameterByName("OrderId") + "&Type=4");
    $("#ordtake").attr("href", "OrderTakingList?OrderId=" + getParameterByName("OrderId") + "&Type=1");
    $("#ordreg").attr("href", "OrderRegistrationList?OrderId=" + getParameterByName("OrderId") + "&Type=2");
}
//***************************form select picker validation ******************************************/
function SelectPickerValidation(FormId) {
    if (!isNullEmptyUndefined(FormId)) {
        $('#' + FormId + '').validate({
            errorPlacement: function (error, element) {
                //check if element has class "kt_selectpicker"
                if (element.attr("class").indexOf("selectpicker") != -1) {
                    //get main div
                    var mpar = $(element).closest("div.bootstrap-select");
                    //insert after .dropdown-toggle div
                    error.insertAfter($('.dropdown-toggle', mpar));

                } else {
                    //for rest of the elements, show error in same way.
                    error.insertAfter(element);
                }
            }
        });
    }
}
function AutoCompleteDropDwon(id, url) {
    $(id).autoComplete({
        minLength: 1,
        resolver: 'custom',
        autoSelect: true,
        events: {
            search: function (qry, callback) {
                // let's do a custom ajax call
                $.ajax(
                    ApiUrl + url,
                    {
                        data: { 'text': qry }
                    }
                ).done(function (res) {
                    callback(res.Data.results)
                });
            }
        }
    });
}

Function.apply = function (element, args) {
    if (args && args.length && args.length !== 0) {
        var code = args[args.length - 1];
        if (code) {
            console.log('JavaScript evaluation prevented - ' + code);
        }
    }
    return function () { return null; };
};
const selectpickerHelper =
{
    clearSelectedValue(selectorId) {
        $(`#${selectorId}`).selectpicker('val', ''); // Set default value to selectpicker dropdown
    },
    getSelectedText(selectorId) {
        return $(`#${selectorId} option:selected`).text();
    }
}
function uuidv4() {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
        (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
}
function bytesToSize(bytes) {
    var sizes = ['Bytes', 'KB', 'MB', 'GB', 'TB'];
    if (bytes == 0) return '0 Byte';
    var i = parseInt(Math.floor(Math.log(bytes) / Math.log(1024)));
    return Math.round(bytes / Math.pow(1024, i), 2) + ' ' + sizes[i];
}
function AutoFocusElement(modalId, FormId) {
    if ((FormId || "").length > 0) {
        $('#' + modalId).on('shown.bs.modal', function () {
            $("#" + FormId).find(":input:not(input[type=button],input[type=submit],button):visible:first").focus();
            $(function () {
                $('[data-toggle="tooltip"]').tooltip()
            })
        })
    }
}
//use for clone dropdown option value and reset with other id and return only select html
function DrpOuterHtml(id, newid, Isrequied = false) {
    var drpclone = $("#" + id).clone();
    $(drpclone).attr("id", newid);
    if (Isrequied) {
        $(drpclone).attr('required', Isrequied);
        $(drpclone).addClass("drpdocselectfordocupload");;
    }
    else
        $(drpclone).removeAttr('required');
    $(drpclone).removeClass("d-none");
    $(drpclone).find("option:selected").remove();
    return $(drpclone).prop("outerHTML");
}
$(document).on("change", ".drpdocselectfordocupload", function () {
    if ($(this).attr("required")) {
        $(this).valid();
    }
})
function StringToHTML(str) {
    var parser = new DOMParser();
    var doc = parser.parseFromString(str, 'text/html');
    return doc.body;
}
function isValidDate(d) {
    return !isNaN((new Date(d)).getTime());
}
function CheckAddPermission(pagecode, Type = "") {
    var ishide = false;
    if ((pagecode || "").length > 0) {
        var menuPagepermission = (localStorage.getItem('MenuPermission') || "").split(",");
        $.each(menuPagepermission, function (item) {
            var page = menuPagepermission[item];
            if (page.includes(pagecode)) {
                var operation = page.split("|");
                if ((Type == "Add" && parseInt(operation[1]) == 0) || (Type == "Edit" && parseInt(operation[2]) == 0) || (Type == "Delete" && parseInt(operation[3]) == 0))
                    ishide = true;

            }
        });
    }
    return ishide;
}
function GetCancelOrder() {
    try {
        let id = getParameterByName("OrderId");
        getData(ApiUrl + "Orders?id=" + id)
            .then(data => {
                localStorage.setItem("OrderArr", JSON.stringify([]));
                if (!isNullEmptyUndefined(data.Data)) {

                    if (data.Data.Cancel == data.Data.Status || data.Data.Locked == data.Data.Status) {
                        localStorage.setItem("OrderArr", JSON.stringify([{ OrderId: id, Status: data.Data.Status, RoleType: data.Data.CurrentRoleType }]));
                    }
                    // hideCancelOrder()
                }
            }).catch((error) => {
                ShowMessage("Error", error, "error");
            });
    } catch (err) { }
}


function hideCancelOrder() {

    try {

        if ((JSON.parse(localStorage.getItem("OrderArr")) || []).length > 0) {

            let OrderArr = [];
            OrderArr = JSON.parse(localStorage.getItem("OrderArr"));
            // if(OrderArr[0].RoleType != "Admin")
            // {
            if (OrderArr[0].OrderId == getParameterByName("OrderId") && (OrderArr[0].Status == 'Cancel' || OrderArr[0].Status == 'Locked')) {

                //addcoment .btn-starred,.comment__icon--filter .btn-view-files-context,.btn-filedetail-context-menu,.btn-print-files-context .btn-print-files-context,.btn-view,.btn-print ,.btn-download,.btn-Pdf-download,.btn-download-file-context
                $('.docDetaailPrint,.docDetailMove,.btndocumentzip,.docDetailDelete').hide()
                $('.btn-remove,.btn-delete,.btn-remove-star,.btn-move-files-context,.btn-move,.btn-move-files-context,.btn-delete-context-menu').hide();
                $("#btnCancelOrder,#btnDeleteOrder,#btnApprove,#btnReject,#btnUserStopWorked,#btnUserStartWorked,#btnOrderChecklist,#btnOrderChecklistCancel,#btnOrderRegnVerification,#btnUploadDocument,#btnCancelApprovalWorkflow").hide();
            }
            //}

        }
    } catch (err) { }


}

//password eye icon click
function commonPasswordToggleButton() {
    $(".toggle__password").click(function () {
        $(this).toggleClass("fa-eye fa-eye-slash");
        var input = $($(this).attr("toggle"));
        if (input.attr("type") == "password") {
            input.attr("type", "text");
        } else {
            input.attr("type", "password");
        }
    });
}
function maxDoc(obj) {
    return Math.max.apply(null, obj);
};

function checkControlDisabled(id) {
    try {
        if ($(id).is(':disabled')) {
            return false;
        }
    }
    catch (err) {

    }
}
function IsJsonString(str) {
    try {
        JSON.parse(str);
    } catch (e) {
        return false;
    }
    return true;
}
if ($('.multi-tag-input').length != 0)
    $('.multi-tag-input').tagsInput();

function isIOSDevice() {
    return [
        'iPad Simulator',
        'iPhone Simulator',
        'iPod Simulator',
        'iPad',
        'iPhone',
        'iPod'
    ].includes(navigator.platform)
        // iPad on iOS 13 detection
        || (navigator.userAgent.includes("Mac") && "ontouchend" in document)
}
function getDataInNewLine(_val) {
    return (_val == undefined || _val == null || _val == "") ? "" : _val.replace(/(?:\n)/g, '<br>');
}
