var filterArr = [];
var filterhtml = [];
//localStorage.setItem("filterArr", JSON.stringify([]));
function bindGridFilter(fn) {
    $("#btnFilter").click(function () {
        filterArr = [];
        filterhtml = [];
        $('#filterctrl').find('.filter-control').each(function () {
            var FieldName = $(this).attr("colName");
            var FieldText = $(this).val();
            var opType = $(this).attr("option-op");
            var ctrltype = $(this).attr("option-type");
            var operation = $(this).attr("operation");
            if (!isNullEmptyUndefined(FieldName)) {
                if (ctrltype == "select") {

                    if ($(this).find("option:selected").length > 0) {
                        if ($(this).attr("multiple") == "multiple") {
                            var vals = $(this).val();
                            var newVals = [];
                            for (var val of vals) {
                                newVals.push(val.split('|')[0]);
                            }
                            FieldText = newVals.join(',');
                        }
                        else
                            FieldText = $(this).find("option:selected").val().split('|')[0];
                    }
                }
                else if (ctrltype == "datepicker" && opType == "Between") {
                    FieldText = "";
                    $(this).find("input").each(function () {
                        var dt = ($(this).val() || "").length > 0 ? moment($(this).val(), "DD/MM/YYYY").format("DD/MMM/YYYY") : $(this).val();
                        FieldText += FieldText == "" ? dt : "-" + dt;
                    });
                }
                else if (ctrltype == "radio" || ctrltype == "checkbox") {
                    FieldText = ($(this).prop("checked")).toString();
                }

                if (operation == "split") {
                    var consider = $(this).attr("consider");
                    var param = $(this).attr("param");
                    FieldText = FieldText.split(param)[consider];
                }
                if ((FieldText || "").length != 0) {
                    filterArr.push({
                        "FieldName": FieldName,
                        "FieldValue": FieldText,
                        "OpType": opType,
                        "DisplayFieldText": (ctrltype == "select" ? $(this).find('option:selected').toArray().map(item => item.text).join() : FieldText),
                        "GrpFieldValue": (vals || [])
                    });
                    filterhtml.push('<div class="float-left add__filter"><span class="">' + (FieldName + ': ' +
                        (ctrltype == "select" ? $(this).find('option:selected').toArray().map(item => item.text).join() : FieldText)) + '</span></div>');
                }

            }
        });
        if ((filterArr || []).length == 0) {
            ShowMessage("Warning", "Please select/enter filter value(s).", "warning", "top", 3000);
        }
        else {
            $("#appliedFilter").empty().html(filterhtml.join(""));
            $(".filter__div").show();
            fn();
        }
        if ("Orders" == window.location.pathname.split("/").pop().replace("#", "") || "Search" == window.location.pathname.split("/").pop().replace("#", "") || "UnClassiFied" == window.location.pathname.split("/").pop().replace("#", "")
            || "Starred" == window.location.pathname.split("/").pop().replace("#", "") || "Deleted" == window.location.pathname.split("/").pop().replace("#", "")
        ) {
            localStorage.setItem(`${window.location.pathname.split("/").pop().replace("#", "")}filterArr`, JSON.stringify([]));
            localStorage.setItem(`${window.location.pathname.split("/").pop().replace("#", "")}filterArr`, JSON.stringify(filterArr));
        }
    });
    $("#btnfilterclear").click(function () {
        $("#clear__filter").click();
    });
    $("#clear__filter").click(function () {
        $(this).parent().hide();
        FilterClear(fn);
    });
    bootstrapClearButton();
    // Filters Start
    $(".filtersbar__close").click(function () {
        $("#filtersbar").removeClass("open");
        $(".filtersbar--overlay").removeClass("open");
    });
    $(".filtersbar--overlay").click(function () {
        $(this).removeClass("open");
        $("#filtersbar").removeClass("open");
    });
}

function FilterClear(fn) {

    filterArr = [];
    localStorage.setItem(`${window.location.pathname.split("/").pop().replace("#", "")}filterArr`, JSON.stringify(filterArr));
    $('#filterctrl').find('.multi-tag-input').val('');
    //$('#filterctrl').find('input, select, textarea').val('').selectpicker("refresh");

    /* Clear All Filter filed Event Start*/
    $('#filterctrl').find("input[type='text']").val("").trigger("change");
    $('.filter-control').val('')
    $('#filterctrl').find('input:checkbox').prop('checked', false);
    $('#filterctrl').find('select').val("").trigger('change');
    $('#filterctrl').find(".tag").remove();
    $('.field__clear').addClass('d-none');
    /* Clear All Filter filed Event End*/
    if (fn)
        fn();
}

function ApplayFilter() {

    if ((JSON.parse(localStorage.getItem(`${window.location.pathname.split("/").pop().replace("#", "")}filterArr`)) || []).length > 0) {
        filterArr = [];
        filterArr = JSON.parse(localStorage.getItem(`${window.location.pathname.split("/").pop().replace("#", "")}filterArr`));
        filterhtml = [];
        filterArr.forEach(function (item, index) {
            $('#filterctrl').find('.filter-control').each(function () {
                var FieldName = $(this).attr("colName");
                var FieldText = $(this).val();
                var opType = $(this).attr("option-op");
                var ctrltype = $(this).attr("option-type");
                var operation = $(this).attr("operation");
                if (item.FieldName == FieldName) {

                    if ($(this).is('select')) {
                        if ($(this).attr("multiple")) {
                            $(this).attr("asp-other-value", (item.GrpFieldValue || []).length > 0 ? item.GrpFieldValue : item.FieldValue);
                            $(this).val(($(this).attr("asp-other-value") || '').split(",")).selectpicker("refresh");
                        }
                        else {
                            $(this).attr("asp-other-value", item.FieldValue);
                            $(this).val(($(this).attr("asp-other-value") || '')).selectpicker("refresh");
                        }

                    }
                    else if (ctrltype == "datepicker" && opType == "Between") {
                        let rn = 1;
                        $(this).find("input").each(function () {
                            if (rn == 1) {
                                $(this).val(moment(item.FieldValue.split('-')[0]).format("DD/MM/YYYY"))
                            }
                            else {
                                $(this).val(moment(item.FieldValue.split('-')[1]).format("DD/MM/YYYY"))
                            }
                            rn++;
                        });
                    }
                    else if ($(this).is('input[type=checkbox]')) {
                        $(this).prop('checked', (item.FieldValue == "true" || item.FieldValue || item.FieldValue == 1) ? true : false)
                    }
                    else if (ctrltype == "radio" || ctrltype == "checkbox") {
                        $(this).prop("checked");
                    }
                    else if ($(this).hasClass('multi-tag-input')) {
                        let id = '#' + $(this).attr('id');
                        $(id).importTags(item.FieldValue);
                        $(id).tagsInput('refresh');
                    }
                    else {
                        $(this).val(item.FieldValue);
                    }
                    filterhtml.push('<div class="float-left add__filter"><span class="">' + (item.FieldName + ': ' +
                        (ctrltype == "select" ? item.DisplayFieldText : item.FieldValue)) + '</span></div>');

                }
            });
        });
        $("#appliedFilter").empty().html(filterhtml.join(""));
        $(".filter__div").show();
    }
}

/* Clear Single Filter filed with X icon Event Start*/
function bootstrapClearButton() {
    $('.form-control').on('keydown focus', function () {
        if ($(this).val().length > 0) {
            $(this).nextAll('.field__clear').removeClass('d-none');
        }
    }).on('keydown keyup blur', function () {
        if ($(this).val().length === 0) {
            $(this).nextAll('.field__clear').addClass('d-none');
        }
    });
    $('.field__clear').on('click', function () {
        $(this).addClass('d-none').prevAll(':input').val('');
    });
}