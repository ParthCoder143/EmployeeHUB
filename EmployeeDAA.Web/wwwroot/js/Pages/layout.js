var fileExtension = ['jpeg', 'jpg', 'png', 'bmp', "pdf", "xlsx", "xls", "docx", "doc", "pptx", "ppt"];
var menupermission;
$(document).ready(function () {
    $(".userdrop__icon").text((localStorage.getItem('ShortName') || ""));
    $("#GlbUserName").text((localStorage.getItem('UserName') || ""));
    menupermission = (localStorage.getItem('MenuPermission') || "");

    $(".dropdown-menu .dropdown-item").click(function () {
        if (!$(this).hasClass('NoBind')) {
            if ($(".greedy-nav li[data-main-menu='" + $(this).attr("data-menu") + "'] a:not([style*='display: none'])[href]:eq(0)").attr("href") != undefined)
                window.location.href = $(".greedy-nav li[data-main-menu='" + $(this).attr("data-menu") + "'] a:not([style*='display: none'])[href]:eq(0)").attr("href");
        }
    });

    $(".greedy-nav a").each(function () {
        loaderstart()
        if ($(this).attr("href")) {
            //--------------hide menu base on role permission-----------------------         
            if (!menupermission.includes($(this).attr("pagecode")) && $(this).attr("pagecode")) {
                $(this).hide();
                if ($(this).attr("tabid"))
                    $("#" + $(this).attr("tabid")).closest("li").hide()
            }
            if (window.location.href.indexOf($(this).attr("href")) == window.location.href.length - $(this).attr("href").length) {
                var main = $(this).closest("li").attr("data-main-menu");
                $(this).addClass("active");
                $(this).closest("li.has__submenu").find(".submenu").addClass("active");
                var parEle = $(".dropdown-menu .dropdown-item[data-menu='" + main + "']");
                $(".greedy-nav li[data-main-menu]").addClass("d-none");
                $(".greedy-nav li[data-main-menu='" + main + "']").removeClass("d-none");
                $("#ddlMainMenu").text($(parEle).text());
                $(".dropdown-menu .dropdown-item").removeClass("active");
                $(parEle).addClass("active");
            }
        }
        loaderstop()
    });
    $('.greedy-nav ul > li').each(function () {
        if ($(this).hasClass("has__submenu")) {
            if ($(this).find('a:not([style*="display: none"])').length == 1)
                $(this).hide();
        }
    });
    $(".greedy-nav").removeClass("d-none");
});
// the function using this page Unclassified , Order Document page,Start,delete and search
function SetViewFileRedirectionLink(full,CmtType = 1) {
    
    var versionName = getInt(full.Version) >= 0 ? "Version" : "Latest";
    var version = getInt(full.DocumentTypeId) > 0 ? (getInt(full.Version) >= 0 ? full.Version : -full.Version) : "-";
    var extention = full.OriginalFilename.substring(full.OriginalFilename.lastIndexOf('.') + 1).toLowerCase();
    if ($.inArray(extention, fileExtension) != -1)
        return `<a id=${full.Id} href="../OrderDocument/ViewDocumentDetail?id=${full.EncrId}&Type=${CmtType}" target="_blank" class="table__link">
    <text>
        <i class="fas fa-file-pdf"></i>
        <strong>${full.OriginalFilename}</strong>
    </text>
    <label class="label">${versionName} (${version})</label>
    </a>
    <button class="btn ver__hist--btn left__context float-right" data-CmtType='${CmtType}' data-filename='${full.OriginalFilename}' value =${full.Id} data-show-classified=${((full.AutolineOrderId || '').length > 0 && (full.AutoLineQMagic || '').length > 0 && (full.DocumentType || '').length > 0) ? false : true} data-order-id=${full.OrderId}><i class="fas fa-ellipsis-v"></i></button>`;
    else
        return `<a id=${full.Id} data-CmtType='${CmtType}' class="table__link">
    <text>
        <i class="fas fa-file-pdf"></i>
        <strong>${full.OriginalFilename}</strong>
    </text>
    <label class="label">${versionName} (${version})</label>
    </a>
    <button class="btn ver__hist--btn left__context float-right" data-CmtType='${CmtType}' data-filename='${full.OriginalFilename}' value =${full.Id} data-show-classified=${((full.AutolineOrderId || '').length > 0 && (full.AutoLineQMagic || '').length > 0 && (full.DocumentType || '').length > 0) ? false : true} data-order-id=${full.OrderId}><i class="fas fa-ellipsis-v"></i></button>`;
}
//the function using this page Unclassified & Order Document page use only
function DisplayGridOrderQNo(full) {
    if ((full.AutolineOrderId || '').length > 0 || (full.AutoLineQMagic || '').length > 0) {
        if ((full.EncrOrderId || '').length == 0)
            return `<strong data-autoid=${full.OrderAutolineOrderId} data-qmagic=${replaceNullwithBlank(full.OrderAutoLineQMagic)}>${full.AutolineOrderId + ' / ' + replaceNullwithBlank(full.AutoLineQMagic)}</strong><small>${replaceNullwithBlank(full.CustomerName)}</small>`;
        else
            return `<a href="${pathRoot}/Orders/ManageOrder?OrderId=${full.EncrOrderId}" class="table__link"><strong data-autoid=${full.OrderAutolineOrderId} data-qmagic=${replaceNullwithBlank(full.OrderAutoLineQMagic)}>${replaceNullwithBlank(full.AutolineOrderId)}/${replaceNullwithBlank(full.AutoLineQMagic)}</strong><small>${replaceNullwithBlank(full.CustomerName)}</small></a>`;
    }
    else {
        return `<button type='button'class="table__link btnUnclassifiedPopup" data-id =${full.Id} data-filename='${full.OriginalFilename} data-order-id=${full.OrderId}' data-autoid=${full.OrderAutolineOrderId} data-qmagic=${replaceNullwithBlank(full.OrderAutoLineQMagic)}>
        <i class="fas fa-question-circle"></i>
        </button>`
    }
}
//the function using this page Unclassified & Order Document page use only
function DisplayGridOrderDocumentType(full) {
    if ((full.DocumentTypeId || 0) > 0) {
        var clsname = 'doctyperow' + full.DocumentTypeId;
        //return '<strong class="DocRow ' + clsname + '" data-doc-id=' + full.Id + ' data-doctype-id=' + full.DocumentTypeId + '> ' + replaceNullwithBlank(full.DocumentClass) + '</strong><small>' + full.DocumentType + '</small>';
        return '<small class="DocRow ' + clsname + '" data-doc-id=' + full.Id + ' data-doctype-id=' + full.DocumentTypeId + '> ' + replaceNullwithBlank(full.DocumentClass) + '</small><strong>' + full.DocumentType + '</strong>';
    }
    else {
        return `<button type='button'class="table__link btnUnclassifiedPopup" data-id =${full.Id} data-filename='${full.OriginalFilename}' data-order-id=${full.OrderId}>
            <i class="fas fa-question-circle"></i>
        </button>`
    }
}

$("#Logout").click(function () {

    loaderstart();
    getData(ApiUrl + "Login/Logout")
        .then(data => {
            localStorage.clear();
            window.location.href = LogoutUrl;
        })
        .catch((error) => {
            ShowMessage("Error", error, "error");
            loaderstop();
            window.location.href = LoginUrl;
        });
});