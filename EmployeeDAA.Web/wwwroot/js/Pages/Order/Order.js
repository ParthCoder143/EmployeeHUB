$(document).ready(function () {
   
            DropDownBind("ProductId");
        DropDownBind("CategoryId");
    $('#btnAddProduct').on('click', function () {
           
            OpenPopupWithoutForm('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageOrder", reloadGrid
            );
        });


    $('#tblProduct').on('change', '.grd-chk', function () {
        var anyCheckboxChecked = $('.grd-chk:checked').length > 0;

        $('#submitButton').toggle(anyCheckboxChecked);
    });
    function getSelectedProductIds() {
        var selectedProductIds = [];

        $('.grd-chk:checked').each(function () {
            selectedProductIds.push($(this).val());
        });

        return selectedProductIds;
    }

    function loadAndDisplayProducts() {
        var selectedProductIds = getSelectedProductIds();

        $.ajax({
            url: 'https://localhost:7062/api/Products/getByIds',
            type: 'GET',
            data: { productIds: selectedProductIds },
            success: function (data) {
                displayProducts(data);
            },
            error: function (error) {
                console.error(error);
            }
        });
    }

    function displayProducts(products) {
        $('#productContainer').empty();

        for (var i = 0; i < products.length; i++) {
            var product = products[i];
            var productHtml = '<div>' +
                '<p>ID: ' + product.Id + '</p>' +
                '<p>Name: ' + product.ProductName + '</p>' +
                '<p>Price: ' + product.Price + '</p>' +
                '</div>';

            $('#productContainer').append(productHtml);
        }
    }

    $('#popmodelcontent').on('change', '.grd-chk', function () {
        var anyCheckboxChecked = $('.grd-chk:checked').length > 0;
        $('#submitButton').toggle(anyCheckboxChecked);
    });

    $('#popmodelcontent').on('click', '#submitButton', function () {
        loadAndDisplayProducts();
    });

    bindGridFilter(function () {
        $("#tblProduct").DataTable().draw();
    });
    //----------------------------------------------------------edit button-----------------------------------//
    $("#tblProduct").on("click", ".btn-grid-edit", function () {
        OpenPopup('popmodel', 'popmodelcontent', "Products/" + $(this).attr("data-id"), "frmAddEditPopup", "ManageOrder", reloadGrid, function () {
            DropDownBind("CategoryId");
            if (($("#Photo").val() || "").length != 0) {
                $("div.PrvPhoto").removeClass("d-none");
                var url = ApiUrl + "Products/GetProductDocument?filename=" + $("#Photo").val();
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
        { "title": "Product Name", "data": "ProductName" },
        { "title": "Unit Price", "data": "Price" },
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
                OpenPopup('popmodel', 'popmodelcontent', "", 'frmAddEditPopup', "ManageOrder", reloadGrid, function () {
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







//$(document).ready(function () {

//    function updateCategoryBasedOnProduct(productId, currentCategoryId) {
//        debugger;
//        $.ajax({
//            url: ApiUrl + `Categories/GetAll?productId=${productId}`,
//            type: 'GET',
//            success: function (data) {
//                debugger

//                var categoryIdDropdown = $('#CategoryId');
//                categoryIdDropdown.empty();

//                // Add options to the dropdown
//                (data || []).forEach(item => {
//                    categoryIdDropdown.append(`<option value="${item.Id}">${item.CategoryName}</option>`);
//                });

//                // Refresh the selectpicker
//                categoryIdDropdown.selectpicker('refresh');

//                // Automatically select the current category if it exists in the data
//                var selectedCategory = data.find(category => category.Id === currentCategoryId);
//                if (selectedCategory) {
//                    categoryIdDropdown.val(selectedCategory.Id);
//                    categoryIdDropdown.selectpicker('render'); // Use render instead of refresh for Bootstrap Select
//                }
//            },
//            error: function (error) {
//                console.error('Error fetching category:', error);
//            }
//        });
//    }

//    DropDownBind("ProductId"); // Ensure this is called after initializing the product dropdown

//    $('#ProductId').on('change', function () {
//        debugger
//        var selectedProductId = $(this).val();
//        var currentCategoryId = $('#CategoryId').val(); // Get the current category ID if it exists
//        updateCategoryBasedOnProduct(selectedProductId, currentCategoryId);
//    });

//    DropDownBind("CategoryId"); // Initialize the category dropdown

//    updateCategoryBasedOnProduct(0);
//});




//$(document).ready(function () {


//    function updateCategoryBasedOnProduct(productId) {
//        debugger;
//        $.ajax({
//            url: ApiUrl + `Categories/GetAll?productId=${productId.join(',')}`,
//            type: 'GET',
//            success: function (data) {
//                debugger

//                var categoryIdDropdown = $('#CategoryId');

//                categoryIdDropdown.selectpicker('refresh');
//                categoryIdDropdown.empty();

//                (data || []).forEach(item => {
//                    debugger
//                    categoryIdDropdown.append(`<option value="${item.Id}">${item.CategoryName}</option>`);
//                });

//                if (data.length > 0) {
//                    categoryIdDropdown.val(data[0].Id);
//                }
//                categoryIdDropdown.selectpicker('refresh');
//            },
//            error: function (error) {
//                console.error('Error fetching category:', error);
//            }
//        });
//    }


//    $('#ProductId').on('change', function () {
//        debugger
//        var selectedProductId = $(this).val();

//        updateCategoryBasedOnProduct(selectedProductId);
//    });

//    DropDownBind("ProductId");
//    DropDownBind("CategoryId");
//    updateCategoryBasedOnProduct(0);
//});




//$(document).ready(function () {
//    DropDownBind("ProductId");
//    DropDownBind("CategoryId");
//});







//$(document).ready(function () {
//    $('#frmAddEditPopup').submit(function (event) {
//        event.preventDefault();

//        var formData = {
//            CustomerId: $('#CustomerId').val(),
//            CustomerName: $('#CustomerName').val(),
//            DateOfBirth: $('#DateOfBirth').val(),
//            MobileNo: $('#MobileNo').val(),
//            EmailAddress: $('#EmailAddress').val(),
//            UnitNo: $('#UnitNo').val(),
//            Block: $('#Block').val(),
//            Street: $('#Street').val(),
//            BuildingName: $('#BuildingName').val(),
//            Country: $('#Country').val(),
//            PostalCode: $('#PostalCode').val(),
//        };

//        $.ajax({
//            url: 'ApiUrl/Order',
//            method: 'POST',
//            contentType: 'application/json',
//            data: JSON.stringify(formData),
//            success: function (data) {
//                console.log('Data saved successfully:', data);
//            },
//            error: function (error) {
//                console.error('Error saving data:', error);
//            }
//        });
//    });
//});
