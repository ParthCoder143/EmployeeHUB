var DataTablePLength = 25;
var filterArr = [];
(function ($) {

    if ($.fn.DataTable.isDataTable(this)) {
        $(this).dataTable().fnDestroy();
    }
    $.fn.dataTable.ext.errMode = function (settings, helpPage, message) {
    };

    $.fn.createGrid = function (options) {
        var defaults = {
            PagerInfo: true,
            LengthChange: true,
            SearchParams: {},
            RecordPerPage: DataTablePLength,
            DataType: 'POST',
            Columns: [],
            Mode: '',
            FixClause: '',
            SortColumn: '',
            SortOrder: 'asc',
            ExportIcon: true,
            ExportFileName: "",
            IsWordExport: true,
            ColumnSelection: true,
            IsDeleteShow: false,
            IsPaging: true,
            OnDelete: fnDelete,
            IsShowFilter: true,
            OnDeleteLabel: '',
            FixedRightColumns: 0,
            FixedLeftColumns: 0,
            Dom: 'rt<"bottom"li<"export-btns">p><"clear">',
            LeftButtons: [],
            RightButtons: [],
            DrawCallback: fn_drawCallback,
            RowCallback: fn_rowCallback,
            CreatedRow: fn_createdRow,
            OnFilter: fn_filter,
            IsShowGridList: true,
            OnShowGridList: fn_gridlist,
            GlobalSearch: "",
            scrollY: 'calc(100vh - 310px)',
            scrollCollapse: true,
            scrollX: true,
            CheckBoxSelectWithPaging: false,
            CheckBoxSelectWithAttribute: "",
            language: {
                processing: "Loading Data...",
                zeroRecords: "No matching records found",
                paginate: {
                    next: '<i class="fas fa-chevron-right">',
                    previous: '<i class="fas fa-chevron-left">'
                },
                infoFiltered: ""
            },
        };
        settings = $.extend({}, defaults, options);

        $.fn.DataTable.ext.pager.numbers_length = 5;
        self = this;

        var tblIds = $(this).attr("id");
        var chkhdnid = tblIds + "_checkhidden";
        var table = $(this).DataTable({
            "processing": false,
            "destroy": true,
            "serverSide": true,
            "language": settings.language,
            "order": [
                [settings.SortColumn, '' + settings.SortOrder + '']
            ],
            "ajax": {
                "type": settings.DataType,
                "url": (ApiUrl + settings.Url),
                'contentType': "application/json",
                'headers': {
                    'Authorization': 'Bearer ' + localStorage.getItem("UserToken")
                },
                "data": function (d) {
                    delete d.SearchParams;
                    d.SearchParams = filterArr;
                    return JSON.stringify(d)
                },

                'dataType': 'json',
                'dataFilter': function (data) {
                    var json = jQuery.parseJSON(data);
                    json.recordsTotal = ((json.Data || []).RecordsTotal || 0);
                    json.recordsFiltered = ((json.Data || []).RecordsTotal || 0);
                    json.data = ((json.Data || []).Data || []);
                    return JSON.stringify(json); // return JSON string
                },
                "error": function (xhr, error, thrown) {
                    if (typeof xhr.responseJSON !== 'undefined') {
                        ShowMessage("Error", (xhr.responseJSON.Message || "error"), "error");
                    }
                },
                'cache': false
            },
            'buttons': settings.LeftButtons,
            "columns": settings.Columns,
            // "searching": settings.IsShowFilter,
            "bPaginate": settings.IsPaging,
            "lengthChange": settings.LengthChange,
            "pageLength": settings.RecordPerPage,
            "scrollX": true,
            "scrollY": settings.scrollY,
            "scrollCollapse": true,
            "fixedColumns": {
                rightColumns: settings.FixedRightColumns,
                leftColumns: settings.FixedLeftColumns
            },
            "bStateSave": true,
            "bInfo": settings.PagerInfo ? true : false,
            "pagingType": "simple_numbers",
            "dom": settings.Dom,
            "drawCallback": function (settings) {
                CardViewDisplayEvent(this);
                if (fn_drawCallback)
                    setTimeout(function () {
                        fn_drawCallback(settings.DrawCallback);
                        $('.dataTables_scrollBody').on('show.bs.dropdown', function () {
                            $('.dataTables_scrollBody').addClass("more-row");
                        });

                        $('.dataTables_scrollBody').on('hide.bs.dropdown', function () {
                            $('.dataTables_scrollBody').removeClass("more-row");
                        });
                        $(window).resize();
                        $(this).tooltip('hide');
                    }, 300);
                // Start check letter table td text increase add class 
                $("table.table-bordered tr td").each(function () {
                    if ($(this).text().trim().length > 30) {
                        //$(this).addClass('line-break');
                        $(this).addClass('line-break-active');
                    } else {
                        $(this).removeClass('line-break-active');
                    }
                });
                // End check letter table td text increase add class 
                //Checkbox logic
                SelectCheckBoxBaseToHiddenFiled(tblIds, chkhdnid);
            },
            "rowCallback": function (row, data) {
                settings.RowCallback();
            },
            "createdRow": function (row, data, dataIndex) {
                settings.CreatedRow(row, data, dataIndex);
            },
            initComplete: function (settingsData) {
                if (typeof BindExtraValue === 'function') {
                    BindExtraValue(settingsData.json.ExtraData);
                }
                if (typeof GetCancelOrder === 'function') {
                    hideCancelOrder()
                }
                var table = $('#' + $(this).attr("id") + '').DataTable();
                var s1 = '';
                if (settings.ExportIcon && table.data().count() != 0) {
                    s1 += "<iframe name='exportFrame' id='exportFrame'></iframe>&nbsp;<button data-name='" + $(this).attr("id") + "' data-type='1' data-tooltip=\"true\" title=\"Export to excel\" class='btn btn-sm btn-icon btn-secondary btn-export'><i class='fas fa-file-excel'></i></button>";
                    if (settings.IsWordExport)
                        s1 += "&nbsp;<button data-name='" + $(this).attr("id") + "' data-type='2' data-tooltip=\"true\" title=\"Export to pdf\" class='btn btn-sm btn-icon btn-secondary btn-export'><i class='fas fa-file-pdf'></i></button>";
                }
                if (settings.ColumnSelection) {
                    if (settings.ExportIcon && table.data().count() != 0) {
                        s1 += '&nbsp;<div class="dropup"><button data-tooltip=\"true\" title="Columns" id="dLabel1" class="btn btn-sm btn-icon btn-secondary"><i class="fas fa-columns"></i></button>';
                        s1 += '<ul class="dropdown-menu column-selection" aria-labelledby="dLabel1">';
                        for (var i = 0; i < table.columns().count(); i++) {
                            var c = table.column(i).visible() ? "checked" : "";
                            if (!($(table.column(i).header()).text().match("Id")) && !table.columns().column(i).context[0].aoColumns[i].IsHideFromSelection) {
                                //s1 += "<li class=\"mt-checkbox-list\"><div class=\"checkbox-fade fade-in-primary\"><label> <input id=\"chk_select_column_" + i + "\" type=\"checkbox\" " + c + " class='data-table-column-selection' tbl='" + $(this).attr("id") + "' coldata='" + $(table.column(i).header()).text() + "' /><span class=\"cr checkmark mright-5\"><i class=\"cr-icon icofont icofont-ui-check txt-primary\"></i></span><span class=\"checkbox-content\">" + $(table.column(i).header()).text() + "</span></label></div></li>";
                                s1 += "<label class='checkbox__wrap'><input type='checkbox' id='chk_select_column_" + i + "' class='data-table-column-selection' " + c + " tbl='" + $(this).attr("id") + "' coldata='" + $(table.column(i).header()).text() + "'><span class='checkmark'></span> <span class='check__text'>" + $(table.column(i).header()).text() + "</span></label>";
                            }
                        }
                        s1 += "</ul></div>";
                    }
                }
                s1 += "&nbsp;<button data-tooltip=\"true\" title=\"Reload Data\" class='btn btn-sm btn-icon btn-secondary btn-datatable-reload' dtname=" + tblIds + " SortColumn=" + settings.SortColumn + " SortOrder=" + settings.SortOrder + "><i class='fas fa-undo'></i></button>";
                $("#" + $(this).attr("id") + "").parent().parent().parent().find("div.export-btns").html(s1);
                $("#dLabel1").on("click", function (event) {
                    event.stopPropagation();
                    $(".column-selection").slideToggle("fast");
                    $(this).tooltip('hide');
                });

                $(".column-selection").on("click", function (event) {
                    event.stopPropagation();
                });

                $(document).on("click", function () {
                    $(".column-selection").hide();
                });

                if ($("#dLabel1").length > 0) {
                    $(".mt-checkbox-list .checkmark").on("click", function (e) {
                        e.stopPropagation();
                    });
                }
                $("#" + $(this).attr("id") + " tr th").each(function (e, index) {
                    $("#" + $(self).attr("id") + " tr td:nth-child(" + (e + 1) + ")").attr('title', $(this).text());
                });
                setTimeout(function () {
                    $('[data-tooltip="true"]').tooltip({
                        container: 'body',
                        placement: 'auto',
                        trigger: 'hover'
                    });
                }, 100);

                if (settings.DrawCallback)
                    settings.DrawCallback(settings);
            }
        }).order([settings.SortColumn, '' + settings.SortOrder + '']);
        if (settings.CheckBoxSelectWithPaging) {
            $("#" + tblIds + "_wrapper").before(`<input type="hidden" id=${chkhdnid} ></input>`);
        }
        if (settings.IsShowGridList) {
            settings.RightButtons.push('csv', {
                'text': '<i class="grd-filter fas fa-th-list" aria-hidden="true"></i>',
                'action': function (e, dt, node) {
                    settings.OnShowGridList(e, dt, node);
                },
                'className': 'btn-sm btn-secondary btnAutoChange',
                'attr': {
                    'title': 'Change views',
                    'id': 'btnAutoChange'
                }
            });
        }
        if (settings.IsShowFilter) {
            settings.RightButtons.push('csv', {
                'text': '<i class="fas fa-filter"></i>',
                'action': function (e, dt, node) {
                    settings.OnFilter(e, dt, node);
                },
                'className': 'btn-sm btn-secondary filter__icon',
                'attr': {
                    'title': 'Filter data'
                }
            });
        }

        if (settings.IsDeleteShow) {
            settings.LeftButtons.push('csv', {
                'text': '<i class="far fa-trash-alt"></i> ' + settings.OnDeleteLabel,
                'action': function (e, dt, node) {
                    settings.OnDelete();
                },
                'className': 'btn btn-secondary btn-sm btn-delete hide',
                'attr': {
                    'title': 'Delete',
                    'id': 'btnGridDelete'
                }
            });
        }
        new $.fn.dataTable.Buttons(table, {
            buttons: settings.RightButtons
        }).container().insertBefore($('#grdLstToggle'));
        new $.fn.dataTable.Buttons(table, {
            buttons: settings.LeftButtons
        }).container().insertBefore($('#top__actions').find('.mob__hidden--btns'));

        $("#grdLstToggle,#top__actions").find('.dt-buttons').removeClass('btn-group');

        $('div.dataTables_filter').addClass('xs-mb-5');
        $('div.dataTables_filter input').addClass('form-control input-sm input-small input-inline');
        $('div.dataTables_length select').addClass('form-control input-sm input-xsmall input-inline');
        $("#" + $(this).attr("id") + "").on('order.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        $("#" + $(this).attr("id") + "").on('length.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        $("#" + $(this).attr("id") + "").on('page.dt', function () {
            //for uncheck checkall btn when page change
            $('#flowcheckall').prop('checked', false);

            if (settings.DrawCallback) {
                setTimeout(function () {

                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        $("#" + $(this).attr("id") + "").on('search.dt', function () {
            if (settings.DrawCallback) {
                setTimeout(function () {
                    settings.DrawCallback(settings);
                }, 300);
            }
        });
        $("#" + $(this).attr("id") + "_wrapper").on('change', '.grd-chk-all', function () {
            $(this).closest('.dataTables_wrapper').find('td input[type="checkbox"][check-for="' + $(this).attr("check-for") + '"]').prop("checked", $(this).prop("checked")).change();
            if ($(this).attr("chk-delete") == "1")
                HideShowDeleteButton(this);
            FilterActiontoggleAction($(this).prop("checked") ? 1 : 0, this);
        });
        $("#" + $(this).attr("id") + "_wrapper").on('change', '.grd-chk', function () {
            if ($(this).prop("checked")) {
                $(this).closest('tr').addClass("selected");
                if (settings.CheckBoxSelectWithPaging) {
                    CheckBoxCreateHidden(chkhdnid, GetCheckBoxAttrValue(this), true);
                }
            }
            else {
                $(this).closest('tr').removeClass("selected");
                if (settings.CheckBoxSelectWithPaging) {
                    CheckBoxCreateHidden(chkhdnid, GetCheckBoxAttrValue(this), false);
                }
            }
            if ($(this).closest(".dataTables_wrapper").find('.grd-chk-all[check-for="' + $(this).attr("check-for") + '"]').length > 0) {
                var ischeck = true;
                $(this).closest('.dataTables_wrapper').find('td input[type="checkbox"][check-for="' + $(this).attr("check-for") + '"]').each(function () {
                    if (!$(this).prop("checked")) {
                        ischeck = false;
                    }
                });
                $(this).closest(".dataTables_wrapper").find('.grd-chk-all[check-for="' + $(this).attr("check-for") + '"]').prop("checked", ischeck);
            }
            HideShowDeleteButton(this);
            FilterActionBtn($(this));
        });
        $("#" + $(this).attr("id") + "_wrapper").on('change', '.data-table-column-selection', function () {
            var table = $('#' + $(this).attr("tbl") + '').DataTable();
            for (var i = 0; i < table.columns().count(); i++) {
                if ($(table.column(i).header()).text() == $(this).attr("coldata")) {
                    table.column(i).visible(this.checked)
                }
            }
            fn_autoAdjustColumn();
        });
        $("#" + $(this).attr("id") + "_wrapper").on('click', '.btn-export', function () {
            var fileName = "";
            if ((settings.ExportFileName || "").length !== 0)
                fileName = `${settings.ExportFileName}_${moment().format("DD_MM_YYYY_HHmm")}`;
            else fileName = uuidv4();
            Export(parseInt($(this).attr("data-type") || 0), tblIds, fileName);
        });
    }
}(jQuery));
//------------------------------------------------------------select checkbox of grid base on hidden filed------------------------------------------------------------------------------
function CheckBoxCreateHidden(hid, val, check) {

    var vl = "," + $("#" + hid).val() + ",";
    if (($("#" + hid).val() || "").length == 0)
        $("#" + hid).val(val);
    else if (check) {
        if (!vl.includes(`,${val},`))
            $("#" + hid).val($("#" + hid).val() + "," + val);
    }
    else if (!check) {
        $("#" + hid).val(((vl.replace(`,${val},`, ',') || "").slice(1) || "").slice(0, -1));
    }
}
function GetCheckBoxAttrValue(obj) {
    var attrvalue = [];
    attrvalue.push($(obj).val());
    if ((settings.CheckBoxSelectWithAttribute || "").length != 0) {
        $.each(settings.CheckBoxSelectWithAttribute.split(","), function (index, value) {
            if (value == "data-filename")
                attrvalue.push($(obj).attr(value).replace(/,/g, ""));
            else
                attrvalue.push($(obj).attr(value));
        });
    }
    return attrvalue.join("|");
}
function SelectCheckBoxBaseToHiddenFiled(dttblIds, hid) {
    if (($("#" + hid).val() || "").length != 0) {
        dttblIds = "#" + dttblIds + "_wrapper";
        $(dttblIds).find('.grd-chk').each(function () {
            var vl = ("," + $("#" + hid).val() + "," || "");
            if (vl.includes(`,${GetCheckBoxAttrValue(this)},`)) {
                $(this).prop("checked", true);
                $(this).closest('tr').addClass("selected");
            }

        });
        FilterActionBtn(dttblIds);
    }
}
//------------------------------------------------select checkbox of grid base on hidden filed----------------------------------------------//
function HideShowDeleteButton(obj) {
    var isHide = true;
    $(obj).closest('.dataTables_wrapper').find('td input[type="checkbox"][check-for="' + $(obj).attr("check-for") + '"]').each(function () {
        if ($(this).prop("checked")) {
            $("#top__actions").find("#btnGridDelete").show();
            isHide = false;
        }
    });
    if (isHide)
        $("#top__actions").find("#btnGridDelete").hide();
}

// -------------------------------------------Extra Function List -----------------------------------------------------------------//
function CardViewDisplayEvent(obj) {
    var api = obj.api();
    var $table = $(api.table().node());

    if ($table.hasClass('cards')) {

        $('.cards').parents('.dataTables_scroll').addClass('cards__wrap');

        // Create an array of labels containing all table headers
        var labels = [];
        $('thead th', $table).each(function () {
            labels.push($(this).text());
        });

        // Add data-label attribute to each cell
        $('tbody tr', $table).each(function () {
            $(this).find('td').each(function (column) {
                $(this).attr('data-label', labels[column]);
            });
        });

        var max = 0;
        $('tbody tr', $table).each(function () {
            max = Math.max($(this).height(), max);
        }).height(max);

    } else {

        $('.dataTables_scroll').removeClass('cards__wrap');

        // Remove data-label attribute from each cell
        $('tbody td', $table).each(function () {
            $(this).removeAttr('data-label');
        });

        $('tbody tr', $table).each(function () {
            $(this).height('auto');
        });
    }
}
function HideLoading() {
    $(".dataTables_processing").hide();
}

function fn_drawCallback() {
    $('#flowcheckall').prop('checked', false);
}

function fn_rowCallback() {

}

function fn_createdRow() {

}

function fn_autoAdjustColumn() {
    if ($.isFunction($.fn.dataTable)) {
        $($.fn.dataTable.tables(true)).DataTable().columns.adjust();
    }
}

function fnDelete() {

}
function fn_filter(e, dt, node) {
    $("#filtersbar").toggleClass("open");
    $(".filtersbar--overlay").toggleClass("open");
}
function fn_gridlist(e, dt, node) {
    $(dt.table().node()).toggleClass('cards');
    $("#grdLstToggle .grd-filter").toggleClass(['fa-th-large', 'fa-th-list']);
    $(dt.table().header()).toggleClass('hide');
    dt.columns.adjust();
}

// Auto Change list to grid while comes in mobile screen Start
var flag = 0;
$(window).on("resize", function () {
    $('[data-tooltip="true"]').tooltip({
        container: 'body',
        placement: 'auto',
        trigger: 'hover'
    });
    if ($(window).width() <= 767) {
        if (flag == 0) {
            $("#btnAutoChange").trigger('click');
            flag = 1;
        }
    }
    else {
        if (flag == 1) {
            $("#btnAutoChange").trigger('click');
            flag = 0
        }
    }
});

$(window).on("load", function () {
    if ($(window).width() <= 767) {
        if (flag == 0) {
            $("#btnAutoChange").trigger('click');
            flag = 1;
        }
    }
});
// Auto Change list to grid while comes in mobile screen End

function GetCheckboxHtml(Id, Val, clsevnt) {
    if (isNullEmptyUndefined(clsevnt))
        clsevnt = 'chkCondition';
    return `<div class="d-inline-block align-checklistgrpitems-center justify-content-center">
    <label class="switch small-switch mb-0">
    <input type="checkbox" data-id="${Id}"  class=${clsevnt} ${Val ? "checked" : ""}>
    <span class="switch-slider"></span>
    </label>
    </div>`;
}
function GetGridEditButtonHtml(Id, Pagecode = '') {
    return CheckAddPermission(Pagecode, "Edit") ? '' : `<button class="btn btn-sm btn-secondary btn-grid-edit" data-id=${Id} data-tooltip="true" title="Edit"><i class="far fa-edit"></i></button>&nbsp;`;
}
function GetGridDeleteButtonHtml(Id, Pagecode = '') {
    return CheckAddPermission(Pagecode, "Delete") ? '' : `<button class="btn btn-sm btn-secondary btn-grid-delete" data-id=${Id} data-tooltip="true" title="Delete"><i class="far fa-trash-alt"></i></button>&nbsp;`;
}
//***************************Filter Button (View,Print,Delete,Restore,Classified) ******************************************************************/
function FilterActionBtn(obj) {
    var totalSelected = $(obj).closest('.dataTables_wrapper').find('.grd-chk:checked').length;
    $('.total-selected').text(totalSelected)
    FilterActiontoggleAction(totalSelected, obj)
    return parseInt(totalSelected);
}
function FilterActiontoggleAction(no, obj) {
    if (typeof SingleCheckBoxViewButton === 'function') {
        if (no == 1) {
            SingleCheckBoxViewButton(obj);
            $('.mob__btns--opener').removeClass("hide");
            $('.mob__btns--opener').text("Action");
        }
        else if (no > 1) {
            if (typeof ContextMenuAction === 'function') {
                ContextMenuAction();
            }
            $('.mob__btns--opener').text("Bulk Actions");
            $('.mob__btns--opener').removeClass("hide");

        }
        else {
            $('.mob__btns--opener').addClass("hide");
            $('.mob__hidden--btns').removeClass("open");

            if (typeof CloseViewButton === 'function') {
                CloseViewButton();
            }
            else
                $("#top__actions").find(":button:not(.back__btn,.btn-not-hide)").hide()
        }
        if (isIOSDevice()) {
            $(".btn-view").hide();
        }
    }
}


/****************************** Export Column *******************************************************/
function Export(type, tbl, fileName) {
    var table = $('#' + tbl.trim() + '').DataTable();
    if (!table.data().any()) {
        ShowMessage("Info", "No data found.", "info", "top", 3000);
    }
    else {
        loaderstart();
        var table = jQuery('#' + tbl + '').DataTable();
        var params1 = JSON.parse(table.ajax.params());
        var columns = table.settings().init().columns;
        (params1.columns || []).forEach(function (item, index) {
            if (table.column(index).visible() === false) {
                $(this).remove();
            }
            else if ((item.data || "").length > 0) {
                item.name = columns[index].title;
            }
        });
        delete params1.SearchParams;
        params1.SearchParams = filterArr;
        params1.ExportType = type;
        params1.Start = 0;
        params1.TimeZone = "+05:30";
        if ((params1.order[0].column || 0) == 0 && (params1.columns[0].data || '') == '') {
            delete params1.order;
            params1.order = [{
                "column": settings.SortColumn,
                "dir": settings.SortOrder,
            }];
        }
        FileDownloadData((ApiUrl + settings.Url), JSON.stringify(params1))
            .then(resp => {
                if (resp == null)
                    ShowMessage("Error", "Document file not found..", "error");
                else {
                    // set the blog type to final pdf
                    const file = new Blob([resp.raw], {
                        type: type == 2
                            ? 'application/pdf'
                            : 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet'
                    });
                    // process to auto download it
                    const fileURL = URL.createObjectURL(file);
                    const link = document.createElement('a');
                    link.href = fileURL;
                    link.download = fileName + ((file.type || "").includes("/pdf") ? ".pdf" : ".xlsx");
                    link.click();
                }
                loaderstop();
            });
    }
}
$(document).on("click", ".btn-datatable-reload", function () {
    $('#' + $(this).attr("dtname")).DataTable().order([parseInt($(this).attr("SortColumn")), $(this).attr("SortOrder")]).draw();
});