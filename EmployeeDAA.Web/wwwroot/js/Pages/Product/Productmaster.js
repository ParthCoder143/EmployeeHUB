$(document).ready(function () {
    DropDownBind("DrpCategoryId");
    reloadGrid();
    //-----------------------filter data---------------------------------------------//
    bindGridFilter(function () {
        $("#tblProduct").DataTable().draw();
    });
    //----------------------------------------------------------edit button-----------------------------------//
    $("#tblProduct").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Products/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageProduct", reloadGrid, function () {
            DropDownBind("CategoryId");
            if(($("#Photo").val() || "").length!=0)        
            {
                $("div.PrvPhoto").removeClass("d-none");
                var url = ApiUrl + "Products/GetProductDocument?filename=" + $("#Photo").val() ;    
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
                    if(data!=null)
                    {
                        const file = new Blob([data.raw], {
                            type: data.contentType,
                        });                    
                        $('#PrvPhoto').attr('src', URL.createObjectURL(file));
                    }
                });
            }

        });
      
    });
    $("#tblProduct").on("click", ".btn-grid-delete", function () {
        SingleDeleteGridData(this, $(this).attr("data-id"), "Products", 'ManageProduct', reloadGrid);
    });
    $("#tblProduct").on("click", ".btn-grid-isactive", function () {
        let id = $(this).attr("data-id");
        UpdateStatus(this, "Products/UpdateStatus", id, "Are you sure want to update this status?", reloadGrid);
    });
    $(document).on("click", "#OpenImgUpload", function () {
        $("#Imgupload").click();
    });
    $(document).on("change", "#Imgupload", function () {
        encodeImageFileAsURL();
    });
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
            { "title": " Category Name", "data": "CategoryName" },
            { "title": "Product Name", "data": "ProductName" },
            { "title": "Price", "data": "Price" },
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
                    html += GetGridEditButtonHtml(full.Id, "Products");
                    html += GetGridDeleteButtonHtml(full.Id, "Products");
                    return html;
                }
            }
        ];
        $("#tblProduct").createGrid({
            Url: "Products/Filters",
            Columns: columns,
            IsDeleteShow: true,
            OnDeleteLabel: "Delete",
            OnDelete: function () {
                DeleteGridData(this, "tblProduct", "Products", 'Products', reloadGrid);
                return false;
            },
            SortColumn: '1',
            SortOrder: 'asc',
            SearchParams: filterArr,
            FixedRightColumns: 2,
            LeftButtons: ['csv', CheckAddPermission("Product", "Add") ? {} : {
                'text': 'Add New Product',
                'action': function (e, dt, node) {
                    OpenPopup('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageProduct", reloadGrid, function () {
                        DropDownBind("CategoryId");
                    });
                },
                'className': 'btn-sm btn-primary',
                'attr': {
                    'title': 'Add Product',
                    'id': 'btnAddNew'
                }
            }],
            IsShowGridList: false
        });
    }
    document.addEventListener("click", function (e) {
        closeAllLists(e.target);
    });
function closeAllLists(elmnt) {
    var x = document.getElementsByClassName("autocomplete-items");
    for (var i = 0; i < x.length; i++) {
        if (elmnt != x[i] && elmnt != inp) {
            x[i].parentNode.removeChild(x[i]);
        }
    }
}

//$('#modalDocumentTypes').on('shown.bs.modal', function () {
//    if (($("#Id").val() || '0') == 0) {
//        $("#IsCompulsoryStatus").val('3').selectpicker("refresh");
//    }
//    var grp = ($("#Status").val() || "").split(",");
//    if ((grp || []).length > 0) {
//        $("#ddlStatus").val(grp).selectpicker("refresh");
//    }
//})