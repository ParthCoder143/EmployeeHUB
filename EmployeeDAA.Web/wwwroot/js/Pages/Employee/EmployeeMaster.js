$(document).ready(function () {
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblEmployee").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblEmployee").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Employees/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageEmployee", reloadGrid);
    });
    $("#tblEmployee").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "Employees", 'ManageEmployee', reloadGrid);
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
            "title": "Name", "data": "Name"
        },
        {
            "title": "Address", "data": "Address"
        },
        {
            "title": "EmailId", "data": "EmailId"
        },
        {
            "title": "PhoneNo", "data": "PhoneNo"
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
    $("#tblEmployee").createGrid({
        Url: "Employees/Filters",
        Columns: columns,
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, "tblEmployee", "Employees", 'Employee', reloadGrid);
            return false;
        },
        SortColumn: '1',
        SortOrder: 'asc',
        SearchParams: filterArr,
        //FixedRightColumns: 2,
        LeftButtons: ['csv', CheckAddPermission("Employee", "Add") ? {} : {
            'text': 'Add New Employee',
            'action': function (e, dt, node) {
                OpenPopup('popmodel', 'popmodelcontent', '', "frmAddEditPopup", "ManageEmployee", reloadGrid);
            },
            'className': 'btn btn-primary btn-sm',
            'attr': {
                'title': 'Add New Employee',
                'id': 'btnAddNew'
            }
        }],
        IsShowGridList: false
    });
}