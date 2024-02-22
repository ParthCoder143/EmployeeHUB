$(document).ready(function () {
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblRole").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblRole").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Role/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageRole", reloadGrid, function () {
            DropDownBind("RoleType");
        });
    });
    $("#tblRole").on("click", ".btn-grid-delete", function () {
        let id = $(this).attr("data-id");
        UpdateStatus(this, "Role/DeleteStatus", id, "Are you sure want to delete this roles??", reloadGrid);
    });
    DropDownBind("ddlRoleType");
});
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
        { "title": "Role", "data": "Name" },
        { "title": "Role Type", "data": "RoleTypeName" },
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
                html += GetGridEditButtonHtml(full.Id);
                html += GetGridDeleteButtonHtml(full.Id);
                return html;
            }
        }
    ];
    $("#tblRole").createGrid({
        Url: "Role/Filters",
        Columns: columns,
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, "tblRole", "Role", 'Roles', reloadGrid);
            return false;
        },
        SortColumn: '1',
        SortOrder: 'asc',
        SearchParams: filterArr,
        FixedRightColumns: 1,
        LeftButtons: ['csv', {
            'text': 'Add New Role',
            'action': function (e, dt, node) {
                OpenPopup('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageRole", reloadGrid, function () {
                    DropDownBind("RoleType");
                });
            },
            'className': 'btn btn-primary btn-sm',
            'attr': {
                'title': 'Add New Role',
                'id': 'btnAddNew'
            }
        }],
        IsShowGridList: false
    });
}