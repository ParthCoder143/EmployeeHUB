var ViewPage = [""];
var EditPage = ["Employee", "Category", "Product", "User", "Role", ];
var AddPage = ["Employee", "Category", 'Product', 'User', "Role", ];
var DeletePage = ["Employee", "Category", 'Product', 'User', "Role", "Permission"];
$(document).ready(function () {
    DropDownBind("DrpRole");
});
$(document).on("click", "#permissonSearch", function () {
    if ($("#DrpRole").val() > 0) {
        CallFormPartialView($("#DrpRole").val())
    }
    else {
        ShowMessage('Warning', 'Please select a role', 'warning');
    }
});
$(document).on("click", "#btnPermissionclear", function () {
    $("#DrpRole").val('').selectpicker("refresh");
    $("#Permissiontbl").empty().html(`<tr>  
    <td colspan=6>
        No Data Found.
    </td>    
    </tr>`);
});
function CallFormPartialView(roleid) {
    CheckAddPermission("Permission", "Edit") ? $("#btnsavepermission").hide() : $("#btnsavepermission").show();
    getData(ApiUrl + 'Permission/GetPermissions?roleid=' + roleid)
        .then(data => {
            $("#Permissiontbl").empty();
            $("#PermissionDt").removeClass("d-none");
            (data.Data.PermissionList || []).forEach(function (item) {
                $("#Permissiontbl").append(BindPermissionRaw(item));
            });

        })
        .catch((error) => {
            console.error('Error:', error);
        });
}

function BindPermissionRaw(item) {
    return `<tr>
    <input type="hidden" id="Id" Name="Id" value="${item.PermissionId}" />
    <input type="hidden" id="RoleId" Name="RoleId" value="${item.RoleId}" />
    <input type="hidden" id="PageId" Name="PageId" value="${item.PageId}"/>
    <td>
        <label id="ModuleName">${item.ModuleName}</label>
    </td>
    <td>
        <label id="PageName">${item.PageName}</label>
    </td>
    ${ViewPage.indexOf(item.PageName) == -1 ?
            `<td>
        <div class="ui checkbox">
            <label class="checkbox__wrap">
                <input type="checkbox" id="viewid"  class="chkView" ${item.IsView ? "checked" : ""}>
                <span class="checkmark"></span>
                <span class="check__text">Yes</span>
            </label>           
        </div>
    </td>`: '<td></td>'
        }
    ${AddPage.indexOf(item.PageName) == -1 ?
            `<td>
             <div class="ui checkbox">
                <label class="checkbox__wrap">
                    <input type="checkbox" id="addid" Name="addid" class="chkView" ${item.IsAdd ? "checked" : ""}>
                    <span class="checkmark"></span>
                    <span class="check__text">Yes</span>
                </label> 
            </div>
        </div>
    </td>`: '<td></td>'
        }
    ${EditPage.indexOf(item.PageName) == -1 ?
            `<td>
        <div class="ui checkbox">
            <label class="checkbox__wrap">
                <input type="checkbox" id="editid" class="chkView" ${item.IsEdit ? "checked" : ""}>
                <span class="checkmark"></span>
                <span class="check__text">Yes</span>
            </label>             
        </div>
    </td>`: '<td></td>'
        }
    ${DeletePage.indexOf(item.PageName) == -1 ? `<td>
        <div class="ui checkbox">
            <label class="checkbox__wrap">
                <input type="checkbox" id="deleteid" class="chkView" ${item.IsDelete ? "checked" : ""}>
                <span class="checkmark"></span>
                <span class="check__text">Yes</span>
            </label>             
        </div>
    </td>`: '<td></td>'
        }
</tr>`;
}

$(document).on("click", "#btnsavepermission", function () {
    if (parseInt($("#DrpRole").val()) > 0)
        postCheckListData();
    else
        ShowMessage("Warning", "Please select at least one role type.", "warning", "top", 3000);
});
function postCheckListData() {
    loaderstart();
    postData(ApiUrl + "Permission/SavePermission", JSON.stringify(getTableDataAsJson())).then((response) => {
        if (response.StatusCode == 200) {
            $("#Permissiontbl").html('');
            $("#permissonSearch").trigger("click");
            loaderstop();
            ShowMessage("Success", response.Message, "success");
        }
        else {
            loaderstop();
            ShowMessage("Error", response.Message, "error");
        }
    });
}
function getTableDataAsJson() {
    let permissiondata = [];
    $('#permissiontbl > tbody > tr').each(function (index, curCell) {
        permissiondata.push(
            {
                'Id': $(curCell).find("input[name='Id']").val(),
                'RoleId': $(curCell).find("input[name='RoleId']").val(),
                'PageId': $(curCell).find("input[name='PageId']").val(),
                'IsAdd': $(curCell).find("#addid").prop('checked'),
                'IsEdit': $(curCell).find("#editid").prop('checked'),
                'IsDelete': $(curCell).find("#deleteid").prop('checked'),
                'IsView': $(curCell).find("#viewid").prop('checked'),
            });
    });
    return permissiondata;
}
$("#selectallview,#selectalladd,#selectalledit,#selectalldelete").click(function () {
    var id = $(this).attr("id") == "selectallview" ? "viewid" :
        $(this).attr("id") == "selectalladd" ? "addid" :
            $(this).attr("id") == "selectalledit" ? "editid" : "deleteid";
    if (id == "viewid" && !$(this).prop("checked")) {
        $("#selectalladd,#selectalledit,#selectalldelete").prop("checked", $(this).prop("checked"));
        $('#permissiontbl > tbody > tr').find(`input:checkbox[id!=AllCheckBox]`).prop("checked", $(this).prop("checked"));
    }
    else
        $('#permissiontbl > tbody > tr').find(`input:checkbox[id=${id}]`).prop("checked", $(this).prop("checked"));
})
$(document).on("click", "#viewid", function () {
    var index = $(this).closest("tr").index();
    $(`#permissiontbl > tbody > tr:eq(${index})`).find(`input:checkbox[id!=viewid]`).attr("disabled", false);
    if (!$(this).prop("checked")) {
        $("#selectallview").prop("checked", $(this).prop("checked"));
        $(`#permissiontbl > tbody > tr:eq(${index})`).find(`input:checkbox[id!=viewid]`).attr("disabled", true);
    }
    $(`#permissiontbl > tbody > tr:eq(${index})`).find(`input:checkbox`).prop("checked", $(this).prop("checked"));
})