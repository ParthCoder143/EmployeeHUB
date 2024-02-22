
var TotalCommentPerPage = 5;
var CurrentDocComment = 1;
var AllCommentForDoc = 2;
var OrderTakingPage = 1;
var RegistrationPage = 2;
var PageIndex = 0;
//-----------------------------------------Comment------------------------------------------------------------------------//
//when click on "No Commnet" column in  grid view page.
$(document).on("click", ".comment__icon--filter", function () {

    var pagetype = (getParameterByName("Type") || OrderTakingPage);
    $('#CommentPageType').val(pagetype).selectpicker("refresh");
    $("#Comment").val("");
    $('#addcoment').hide();
    $("#OriginalFilename").val($(this).attr("data-file-name"));
    $("#DocId").val($(this).attr("data-docid"));
    //Clear Current Document comment 
    $("#commentofcurrrentdoc").empty();

    $("#commentofalldoc").empty();
    $('#this-order-comments-tab').addClass('active')
    $('#all-comments-tab').removeClass('active')
    $('#this-order-comments').addClass('active').addClass('show');

    //$('#this-order-comments-tab')
    //"#commentofalldoc":"#commentofcurrrentdoc"
    //pass Page Type 1 is Order Taking 2 is Registration and 3 is other     
    $("#PageType").val(pagetype);
    GetCommentList(CurrentDocComment, PageIndex, function () {
        $("#filtersbar4").toggleClass("open");
        $(".filtersbar--overlay").toggleClass("open");
        if (typeof ShowCommentAddBox === 'function') {
            ShowCommentAddBox();
            $("#frmComment").find(":input:not(input[type=button],input[type=submit],button):visible:first").focus();
        }
    });
});
$(document).on("change", "#CommentPageType", function () {
    $("#commnetaddbox").removeClass("d-none");
    if (parseInt($(this).val()) == 5) {
        $("#commnetaddbox").addClass("d-none");
        $("#Comment").val('');
    }
    $("#CommentType").val((parseInt($(this).val()) || OrderTakingPage));
    $('#PageType').val((parseInt($(this).val()) || OrderTakingPage));

});
//-------------------close comment popup close button event--------------------------------
$(document).on("click", ".filtersbar__close", function () {
    $("#filtersbar4").removeClass("open");
    $(".filtersbar--overlay").removeClass("open");
});
$(document).on("click", ".filtersbar--overlay", function () {
    $(this).removeClass("open");
    $("#filtersbar4").removeClass("open");
});
//---------------click on "This File" and "All file" button click event on comment popup-------------------------------
$(document).on("click", "#all-comments-tab,#this-order-comments-tab", function () {

    //if comment already exists then no need to call api
    var id = $(this).attr("id") == "all-comments-tab" ? "#commentofalldoc" : "#commentofcurrrentdoc";
    if ($(`${id}`).find('.comments-block').length == 0)
        GetCommentList(($(this).attr("id") == "all-comments-tab" ? AllCommentForDoc : CurrentDocComment), 0);
});
//--------------------"Add" button click event to Comment box----------------------------------------------
$(document).on("click", "#addcoment", function () {

    var FormId = "frmComment";
    if ($('#' + FormId + '').valid()) {
        if (($('#' + FormId + '').find('.scriptvalidation').length || 0) == 0) {
            loaderstart();
            //form submit one than submit disabled
            $('#' + FormId + '').find(":submit").prop("disabled", true);
            //--------------------post data in api-----------------------------// 
            postData(ApiUrl + $('#' + FormId + '').attr("apiurl"), JSON.stringify($('#' + FormId + '').serializeObject())).then((response) => {
                if (response.StatusCode == 200) {

                    var cmtcnt = $("#Comment_" + $("#DocId").val()).text();
                    if (parseInt(cmtcnt))
                        $("#Comment_" + $("#DocId").val()).text(parseInt(cmtcnt) + 1);
                    else
                        $("#Comment_" + $("#DocId").val()).text(1);

                    var data = [];
                    data.push(response.Data);

                    GenerateCommentText(data, CurrentDocComment, response.Data, PageIndex, -1);
                    $('#' + FormId + '')[0].reset();
                    $("#addcoment").hide();
                    ShowMessage("Success", response.Message, "success");
                    $('#' + FormId + '').find(":submit").prop("disabled", false);
                }
                else {
                    ShowMessage("Error", response.Message, "error");
                    $('#' + FormId + '').find(":submit").prop("disabled", false);
                }
                loaderstop();
            });
            //------------------------------------------------------------//
        }
        else
            return false;
    }
});
//---------------------------- "View More Comments" button click event----------------------
$(document).on("click", ".more-comment-link", function () {
    var CommentListType = $(this).closest(".tab-pane").attr("id") == "all-comments" ? AllCommentForDoc : CurrentDocComment;
    show_comment_loader(this);
    GetCommentList(CommentListType, $(this).attr("data-stage"), function () {
        hide_comment_loader(this);
    });
})
//-----------------------------------------------Api call for Get Comment list-----------------------------------------------------------------------------------------------
function GetCommentList(type, pageindex, callback) {        
    if ((window.location.href || "").indexOf("ViewDocumentDetail") !== -1 && type == 1) {
        type = 3;
    }
    loaderstart();
    var url = `Comment/${parseInt($("#DocId").val())}/${(parseInt($("#PageType").val()) || OrderTakingPage)}/${parseInt(type)}/${parseInt(pageindex)}`
    getData(ApiUrl + url)
        .then(data => {

            if (data.StatusCode == 200) {
                if (type == 3) //it's special condition for show all comment base on version
                {
                    type = 1;
                }
                (uniqueByKey(data.Data.Data, "CommentOn") || []).forEach(function (e) {
                    GenerateCommentText((data.Data.Data || []), type, e, parseInt((data.Data || []).PageIndex + parseInt(TotalCommentPerPage)), (data.Data || []).RecordsTotal);
                });
            }
            else
                ShowMessage("Error", data.Message, "error");
            if (callback)
                callback();
            loaderstop();
        })
}
//-------------------------------arrange comment in date wise--------------------------
function uniqueByKey(array, key) {
    return [...new Map(array.map((x) => [convertDateToString(x[key], "DDMMMYYYY"), x])).values()];
}
//-------------------------check date wise comment box exists or not---------------------------------
function CheckCommentBoxExists(divid, dtformate, CommentType, TotalCmt) {

    if ($("#" + divid).find("#" + dtformate.format("DDMMMYYYY")).length == 0) {
        var commentbox = ` <div class="commnet__date--sep"><span>${CommentType == OrderTakingPage ? "Order Verification on" : CommentType == RegistrationPage ? "Regn Verification on" : ""} ${dtformate.format("DD MMM YYYY")}</span></div><ul id=${dtformate.format("DDMMMYYYY")} TotatCnt=${TotalCmt} >
        </ul>
       `;
        if (TotalCmt == -1) {
            $("#commentofcurrrentdoc").prepend(commentbox);
            if ($("#commentofalldoc").find("#" + dtformate.format("DDMMMYYYY")).length == 0)
                $("#commentofalldoc").prepend(commentbox);
        }
        else
            $("#" + divid).append(commentbox);
    }
}
//--------------------generate Comment text box with date and time and comment by and comment text wise-------------------------------
function GenerateCommentText(data, type, currentdate, CurrentPageCmtCount, TotalCommentCount) {
    var dtformate = moment(currentdate.CommentOn);
    //generate id for div all comment and only doc wise
    var divid = type == 1 ? "commentofcurrrentdoc" : "commentofalldoc";
    //create ul id

    CheckCommentBoxExists(divid, dtformate, currentdate.CommentType, TotalCommentCount);
    //get unique date wise comment
    $.each(data.filter(function (i, n) {
        return convertDateToString(i.CommentOn, "DDMMMYYYY") === dtformate.format("DDMMMYYYY");
    }), function (index, curData) {
        //------------------generate comment text li------------------        
        var dt = moment(curData.CommentOn);
        var commenttext = `<li class='comments-block'>
            <span class="com__icon">${curData.CommentBy.substring(0, 2)}</span>
            <div class="com__desc">
                ${"<h6>" + curData.OriginalFilename + "</h6>"}
                <strong>${curData.CommentBy}</strong>
                <strong>(${curData.CommentTypeStatus})</strong>
                <span>${dt.format("DD MMM YYYY")} | ${dt.format("hh:mm a")}</span>
                <p><b>${getDataInNewLine(curData.Comment)}</b></p>
            </div>
        </li>`;

        //------------------------------------------------------------this conditio use for add comment after add succesfully-----------------
        if (TotalCommentCount == -1) {
            $("#" + divid).find("#" + dtformate.format("DDMMMYYYY")).prepend(commenttext);

            if ($("#commentofalldoc").find('.comments-block').length != 0)
                $("#commentofalldoc").find("#" + dtformate.format("DDMMMYYYY")).prepend(commenttext);
        }
        else
            $("#" + divid).find("#" + dtformate.format("DDMMMYYYY")).append(commenttext);
    });

    if (TotalCommentCount == -1) {
        //this block special use for  when add new comment that time also give effect on all doc comment
        if ($("#commentofalldoc").find('.comments-block').length != 0) {
            var TotalCommentCountForAllDoc = 0;
            if (parseInt($("#commentofalldoc").find(".comments-block").length) > TotalCommentPerPage) {
                TotalCommentCountForAllDoc = parseInt($("#commentofalldoc").find(".more-comment-link").attr("data-cnt") || TotalCommentPerPage) + 1;
                $("#commentofalldoc").find("#" + dtformate.format("DDMMMYYYY")).find('li:last-child').remove();
                $("#commentofalldoc").append(GenerateButtonForLoadMoreComment("commentofalldoc", $("#commentofalldoc").find(".comments-block").length, TotalCommentCountForAllDoc));
            }
        }
        // //------------------------------------------------------------------------------------------------ 
        if (parseInt($("#commentofcurrrentdoc").find(".comments-block").length) > TotalCommentPerPage) {
            var TotalCommentCount = parseInt($("#commentofcurrrentdoc").find(".more-comment-link").attr("data-cnt") || TotalCommentPerPage) + 1;
            $("#commentofcurrrentdoc").find("#" + dtformate.format("DDMMMYYYY")).find('li:last-child').remove();
            $("#commentofcurrrentdoc").append(GenerateButtonForLoadMoreComment("commentofcurrrentdoc", $("#commentofcurrrentdoc").find(".comments-block").length, TotalCommentCount));
        }

    }
    else {
        if (TotalCommentCount != $("#" + divid).find(".comments-block").length)
            $("#" + divid).append(GenerateButtonForLoadMoreComment(divid, CurrentPageCmtCount, TotalCommentCount));
        else if (TotalCommentCount == $("#" + divid).find(".comments-block").length)
            $("#" + divid).find(".more-comments").hide();
        else
            $("#" + divid).find(".CommentPageRow").text(`(${$("#" + divid).find(".comments-block").length} of ${TotalCommentCount})`)
    }
}
//---------------------generate load "View More Comments" button for comment------------------------------
function GenerateButtonForLoadMoreComment(divid, CurrentPageCmtCount, TotalCommentCount) {
    $("#" + divid).find(".more-comments").remove();
    return `<div class="more-comments">
    <button type="button" class="btn btn-secondary btn-sm w-100 more-comment-link" data-cnt="${TotalCommentCount}" data-stage="${CurrentPageCmtCount}">
        <span>View more comments <span class="CommentPageRow">(${$("#" + divid).find(".comments-block").length} of ${TotalCommentCount})</span></span>
        <img src="/images/loader_gray.gif" width="14px" class="comment-loader ml-2 hide-block">
    </button>
</div> `;

}
//-------------hide show loader----------------------------------
function show_comment_loader(obj) {
    $(obj).parent().find('.comment-loader').show();
}
function hide_comment_loader(obj) {
    setTimeout(function () {
        $('.comment-loader').hide();
    }, 1000);
}
//------------------------------comment box empty than add button hide------------------------------------
$("#Comment").keyup(function () {
    if ($(this).val() == "") {
        $("#addcoment").hide();
    } else {
        $("#addcoment").show();
    }
});
$("#Comment").focusout(function () {
    if ($(this).val() == "") {
        $("#addcoment").hide();
    } else {
        $("#addcoment").show();
    }
});
$(document).ready(function () {
    $('#addcoment').hide();
});
//---------------------------------------------------comment---------------------------------------------------//
$(document).on("change", "#CommentPageType", function () {
    $("#commentofcurrrentdoc,#commentofalldoc").empty();
    $("#PageType").val(($(this).val() || 1));
    if ($("#this-order-comments-tab").hasClass('active'))
        $("#this-order-comments-tab").click();
    else
        $("#all-comments-tab").click();
});