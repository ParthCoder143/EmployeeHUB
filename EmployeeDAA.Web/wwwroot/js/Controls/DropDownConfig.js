
function DropDownBind(id, isSelectpicker, isgroup, callback) {
    if (id.indexOf('.') == -1)
        id = "#" + id;
    if (!isNullEmptyUndefined($(id).attr("mode"))) {
        var url = (ApiUrl + 'DropDown/GetBindDropDown') + "?Mode=" + $(id).attr("mode");
        GetDropDownData(url, id, isSelectpicker, isgroup, callback);
    }
}
function uniqueGrpByKey(array, key) {
    return [...new Map(array.map((x) => [x[key], x])).values()];
}
function GetDropDownData(url, id, isSelectpicker, isgroup, callback) {
    if (!isSelectpicker)
        isSelectpicker = true;
    if (isNullEmptyUndefined(isgroup))
        isgroup = false;
    $(id).empty();
    if (!isNullEmptyUndefined($(id).attr("title")) && !$(id).attr('multiple'))
        $(id).append(`<option value>${$(id).attr("title")}</option>`);
    else if ($(id).attr('multiple'))
        $(id).attr("data-selected-text-format", "count > 3");
    getData(url)
        .then(data => {
            if ((data.Data || []).length > 0) {
                if (!isgroup) {
                    (data.Data || []).forEach(item => {
                        $(id).append(`<option value="${item.Value}">${item.Text}</option>`);
                    });
                }
                else {
                    var result = [];
                    (data.Data || []).forEach(item => {
                        result.push({ Text: item.Text, Value: item.Value, Group: item.Group.Name });
                    });
                    (uniqueGrpByKey(result, "Group") || []).forEach(function (e) {
                        var GroupTxt = "";
                        (data.Data || []).forEach(function (item) {
                            if ((item.Group.Name || "") == e.Group) {
                                if ($(id).find(`option[value="${item.Value}"]`).length === 0) {
                                    GroupTxt += `<option value="${item.Value}">${item.Text}</option>`;
                                }
                            }
                        });
                        $(id).append(`<optgroup label="${e.Group}">${GroupTxt}</optgroup>`);
                    });
                }

            }
            if (isSelectpicker) {
                if ($(id).attr("multiple")) {
                    $(id).val(($(id).attr("asp-other-value") || '').split(",")).selectpicker("refresh");
                }
                else
                    $(id).val(($(id).attr("asp-other-value") || '')).selectpicker("refresh");
            }
        })
        .catch((error) => {
            ShowMessage("Error", error, "error");
        });
    if (callback)
        callback()
}

