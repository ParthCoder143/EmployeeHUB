$(document).ready(function () {
    DropDownBind("DrpRoleId");
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblUser").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblUser").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "User/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageUsers", reloadGrid, function () {
            commonPasswordToggleButton();
            DropDownBind("RoleId");
            DropDownBind("UserTypeId");
            //if (parseInt($("#IsAdLogin").val()) > 0 && $("#IsAdLogin").prop("checked")) {
            //    $("#adnoncomppass").hide();
            //    $("#adnoncompconfirmpass").hide();
            //}
            if (/*parseInt($("#IsAdLogin").val()) > 0 && $("#IsAdLogin").prop("checked") ||*/ parseInt($("#Id").val() || "0") > 0) {

                $("#Password,#ConfirmPassword").val("");
            }
            else {
                $("#Password").prop('required', false);
                $("#ConfirmPassword").prop('required', false);
                $("#Password").val("");
            }
            if (($("#Photo").val() || "").length != 0) {
                $("div.PrvPhoto").removeClass("d-none");
                var url = ApiUrl + "User/GetUserProfileDocument?filename=" + $("#Photo").val();
                //Get pdf viewr
                fetch(url, {
                    headers: new Headers({
                        'Authorization': 'Bearer ' + localStorage.getItem("UserToken")
                    })
                }).then(response => {
                    if (!response.ok)
                        return null;
                    else {
                        return response.blob().then(blob => {
                            return {
                                contentType: response.headers.get("Content-Type"),
                                raw: blob
                            }
                        })
                    }
                })
                    .then(data => {
                        if (data != null) {
                            const file = new Blob([data.raw], {
                                type: data.contentType,
                            });
                            $('#PrvPhoto').attr('src', URL.createObjectURL(file));
                        }
                    });
            }

        });
    });

    $(document).on("click", "#OpenImgUpload", function () {
        $("#Imgupload").click();
    });
    $(document).on("change", "#Imgupload", function () {
        encodeImageFileAsURL();
    });
   
    $("#tblUser").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "User", 'Users', reloadGrid);
    });
    $("#tblUser").on("click", ".btn-grid-isactive", function () {
        let id = $(this).attr("data-id");
        UpdateStatus(this, "User/UpdateStatus", id, "Are you sure want to update this status?", reloadGrid);
    });
    //$(document).on("click", "#IsAdLogin", function () {
    //    var IsAdLogin = document.getElementById("IsAdLogin");
    //    // $("#divConfirmPassword").hide();
    //    // $("#divPassword").hide();
    //    $("#adnoncomppass").hide();
    //    $("#adnoncompconfirmpass").hide();
    //    $("#Password").prop('required', false);
    //    $("#ConfirmPassword").prop('required', false);
    //    if (!IsAdLogin.checked == true) {
    //        // $("#divConfirmPassword").show();
    //        // $("#divPassword").show();
    //        $("#adnoncomppass").show();
    //        $("#adnoncompconfirmpass").show();
    //        $("#Password").prop('required', true);
    //        $("#ConfirmPassword").prop('required', true);
    //    }
    //});
    
});
function encodeImageFileAsURL() {
    var filesSelected = document.getElementById("Imgupload").files;
    if (filesSelected.length > 0) {
        $("div.PrvPhoto").removeClass("d-none");
        var fileToLoad = filesSelected[0];
        var fileReader = new FileReader();
        $('#Photo').val(fileToLoad.name);
        fileReader.onload = function (fileLoadedEvent) {
            var srcData = fileLoadedEvent.target.result;
            var newImage = document.createElement('img');
            newImage.src = srcData;
            $('#PrvPhoto').attr('src', srcData);

        }
        fileReader.readAsDataURL(fileToLoad);
    }
}
function reloadGrid() {
    var columns = [
        {
            "title": '<label class="checkbox__wrap row--select-checkbox"><input type="checkbox" chk-delete="1" class="grd-chk-all" check-for="cls"><span class="checkmark"></span></label>',
            "data": "",
            'searchable': false,
            'IsHideFromSelection': true,
            'visible': true,
            'orderable': false,
            "sWidth": "100px",
            'className': 'text-center checkbox__table',
            "render": function (data, type, full, meta) {
                return `<label class="checkbox__wrap row--select-checkbox">
                            <input type="checkbox"  ChekBoxType="tblcolDeleteCheckBox" class="grd-chk"  value = "${full.Id}" check-for="cls">
                            <span class="checkmark"></span>
                        </label>`;
            }
        },
        { "title": "First Name", "data": "FirstName" },
        { "title": "Last Name", "data": "LastName" },
        { "title": "Email", "data": "Email" },
        { "title": "Mobile", "data": "Mobile" },
        { "title": "User Name", "data": "UserName" },
        { "title": "Role", "data": "Role" },
        {
            'title': 'Status',
            'orderable': false,
            "data": "IsActive",
            'className': 'text-center status__col',
            'render': function (data, type, full, meta) {
                return GetCheckboxHtml(full.Id, full.IsActive, "btn-grid-isactive");
            }
        },
        {
            'title': 'Action',
            "data": "",
            'IsHideFromSelection': true,
            'searchable': false,
            'visible': true,
            'orderable': false,
            'className': 'text-center action__3',
            'render': function (data, type, full, meta) {
                var html = '';
                html += GetGridEditButtonHtml(full.Id, "User");
                html += GetGridDeleteButtonHtml(full.Id, "User");
                return html;
            }
        }
    ];
    $("#tblUser").createGrid({
        Url: "User/Filters",
        Columns: columns,
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, "tblUser", "User", 'Users', reloadGrid);
            return false;
        },
        SortColumn: '1',
        SortOrder: 'asc',
        SearchParams: filterArr,
        FixedRightColumns: 2,
        LeftButtons: ['csv', CheckAddPermission("User", "Add") ? {} : {
            'text': 'Add New User',
            'action': function (e, dt, node) {
                OpenPopup('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageUsers", reloadGrid, function () {
                    commonPasswordToggleButton();
                    DropDownBind("RoleId");
                    DropDownBind("UserTypeId");
                    //if (parseInt($("#IsAdLogin").val()) == 1) {
                    //    $("#Password").prop('required', true);
                    //    $("#ConfirmPassword").prop('required', true);
                    //}
                });
            },
            'className': 'btn btn-primary btn-sm',
            'attr': {
                'title': 'Add New User',
                'id': 'btnAddNew'
            }
        }],
        IsShowGridList: false
    });
}
$(document).on("click", '#saveuser', function (e) {
    //if (parseInt($("#Id").val() || "0") > 0) {
    //    var IsAdLogin = document.getElementById("IsAdLogin");
    //    var tempAD = $("#TempIsAdLogin").val() == "true" ? true : false;
    //    if (tempAD != IsAdLogin.checked) {
    //        if (!IsAdLogin && (isNullEmptyUndefined($("#TempPassword").val()))) {
    //            $("#Password").prop('required', true);
    //            $("#ConfirmPassword").prop('required', true);
    //        }
    //    }
    //}
    if (parseInt($("#Id").val() || "0") > 0 && $("#divPassword").is(':visible') == false) {
        $("#Password,#ConfirmPassword").val("");
    }
    $("#frmAddEditPopup").submit();
});