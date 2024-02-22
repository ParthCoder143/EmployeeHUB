$(document).ready(function () {
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblDepartment").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblDepartment").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Department/" + $(this).attr("data-id"), "frmDocumentClass", "ManageDepartment", reloadGrid);
    });
    $("#tblDepartment").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "Department", 'ManageDepartment', reloadGrid);
    });
    $("#tblDepartment").on("click", ".btn-grid-isactive", function () {
        let id = $(this).attr("data-id");
        UpdateStatus(this, "Department/UpdateStatus", id, "Are you sure want to update this status?", reloadGrid);
    });

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
            'className': 'text-center',
            "render": function (data, type, full, meta) {
                return `<label class="checkbox__wrap row--select-checkbox">
                            <input type="checkbox"  ChekBoxType="tblcolDeleteCheckBox" class="grd-chk"  value = "${full.Id}" check-for="cls">
                            <span class="checkmark"></span>
                        </label>`;
            }
        },
        {
            "title": "EmployeeId", "data": "EmployeeId"
        },
        {
            "title": "DepartmentName", "data": "DepartmentName"
        },
        {
            "title": "DepartmentCode", "data": "DepartmentCode"
        },
        {
            'title': 'Status',
            'orderable': false,
            "data": "IsActive",
            'visible': true,
            'className': 'text-center status__col',
            'render': function (data, type, full, meta) {
                return GetCheckboxHtml(full.Id, full.IsActive, 'btn-grid-isactive');
            }
        },
        {
            'title': 'Action',
            "data": "",
            'IsHideFromSelection': true,
            'searchable': false,
            'visible': true,
            'orderable': false,
            'className': 'text-center',
            'render': function (data, type, full, meta) {
                var html = '';
                html += GetGridEditButtonHtml(full.Id, "Employee");
                html += GetGridDeleteButtonHtml(full.Id, "Employee");
                return html;
            }
        }

    ];
    $("#tblDepartment").createGrid({
        Url: "Department/Filters",
        Columns: columns,
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, "tblDepartment", "Department", 'Department', reloadGrid);
            return false;
        },
        SortColumn: '1',
        SortOrder: 'asc',
        SearchParams: filterArr,
        FixedRightColumns: 2,
        LeftButtons: ['csv', CheckAddPermission("Department", "Add") ? {} : {
            'text': 'Add New Department',
            'action': function (e, dt, node) {
                OpenPopup('popmodel', 'popmodelcontent', '', "frmDocumentClass", "ManageDepartment", reloadGrid);
            },
            'className': 'btn btn-primary btn-sm',
            'attr': {
                'title': 'Add New Department',
                'id': 'btnAddNew'
            }
        }],
        IsShowGridList: false
    });
}