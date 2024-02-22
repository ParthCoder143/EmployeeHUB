$(document).ready(function () {
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblEmployee").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblEmployee").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Categories/" + $(this).attr("data-id"), "frmDocumentClass", "ManageCategory", reloadGrid);
    });
    $("#tblEmployee").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "Categories", 'ManageCategory', reloadGrid);
    });
    $("#tblEmployee").on("click", ".btn-grid-isactive", function () {
        let id = $(this).attr("data-id");
        UpdateStatus(this, "Categories/UpdateStatus", id, "Are you sure want to update this status?", reloadGrid);
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
            "title": "Category Name",
            "data": "CategoryName"
        },
        {
            "title": "Sort Order",
            "data": "SortOrder",
            'searchable': false,
            'className': 'text-center sort__order',
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
            'className': 'text-center action__3',
            'render': function (data, type, full, meta) {
                var html = '';
                html += GetGridEditButtonHtml(full.Id, "Categories");
                html += GetGridDeleteButtonHtml(full.Id, "Categories");
                return html;
            }
        }


    ];
    $("#tblEmployee").createGrid({
        Url: "Categories/Filters",
        Columns: columns,
        IsDeleteShow: true,
        OnDeleteLabel: "Delete",
        OnDelete: function () {
            DeleteGridData(this, "tblEmployee", "Categories", 'Category', reloadGrid);
            return false;
        },
        SortColumn: '1',
        SortOrder: 'asc',
        SearchParams: filterArr,
        FixedRightColumns: 2,
        LeftButtons: ['csv', CheckAddPermission("Category", "Add") ? {} : {
            'text': 'Add New Category',
            'action': function (e, dt, node) {
                OpenPopup('popmodel', 'popmodelcontent', '', "frmDocumentClass", "ManageCategory", reloadGrid);
            },
            'className': 'btn btn-primary btn-sm',
            'attr': {
                'title': 'Add New Category',
                'id': 'btnAddNew'
            }
        }],
        IsShowGridList: false
    });
}