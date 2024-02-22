$(document).ready(function () {
    reloadGrid();
    
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblOrderInfo").DataTable().draw();
    });
    //----------------------------------------------------------Grid Edit and delete button-----------------------------------//
    $("#tblOrderInfo").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Order/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageOrderInfo", reloadGrid);
    });
    $("#tblOrderInfo").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "Order", 'ManageOrderInfo', reloadGrid);
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
                'className': 'text-center checkbox__table',
                "render": function (data, type, full, meta) {
                    return `<label class="checkbox__wrap row--select-checkbox">
                            <input type="checkbox"  ChekBoxType="tblcolDeleteCheckBox" class="grd-chk"  value = "${full.Id}" check-for="cls">
                            <span class="checkmark"></span>
                        </label>`;
                }
            },
            { "title": "Customer Id", "data": "CustomerId" },
            { "title": "Customer Name", "data": "CustomerName" },
            { "title": "Date of Birth", "data": "DateOfBirth" },
            { "title": "Mobile No", "data": "MobileNo" },
            { "title": "Email Address", "data": "EmailAddress" },
            { "title": "Unit No", "data": "UnitNo" },
            { "title": "Block", "data": "Block" },
            { "title": "Street", "data": "Street" },
            { "title": "Building Name", "data": "BuildingName" },
            { "title": "Country", "data": "Country" },
            { "title": "Postal Code", "data": "PostalCode" },
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
        $("#tblOrderInfo").createGrid({
            Url: "Order/Filters",
            Columns: columns,
            IsDeleteShow: true,
            OnDeleteLabel: "Delete",
            OnDelete: function () {
                DeleteGridData(this, "tblOrderInfo", "Order", 'Order', reloadGrid);
                return false;
            },
            SortColumn: '1',
            SortOrder: 'asc',
            SearchParams: filterArr,
            FixedRightColumns: 1,
            LeftButtons: ['csv', CheckAddPermission("Customer", "Add") ? {} : {
                'text': 'Add New Order',
                //'action': function (e, dt, node) {
                //    OpenPopup('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageOrderInfo", reloadGrid);
                //},
                'action': function () {
                         window.location.href = '/Order/Order';
                },

                'className': 'btn btn-primary btn-sm',
                'attr': {
                    'title': 'Add New Customer Details',
                    'id': 'btnAddNew'
                }
            }],
            IsShowGridList: false
        });
    }