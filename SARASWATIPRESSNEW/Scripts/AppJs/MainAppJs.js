

//displayMessageOnUI
function appendMsg(varMsg, actionType) {

    var contId = $("#ErrorMsgContainer");
    $(contId).empty();
    $(contId).html(varMsg);
    addErrorClass(contId, actionType);
    UpScroll();
}

function UpScroll() {
    $('html, body').animate({
        scrollTop: (0)
    }, 500);
}
function clearError() {
    var contId = $("#ErrorMsgContainer");
    $(contId).empty();
    $(contId).html("");
    addErrorClass(contId, "CLEAR");
}
function addErrorClass(contId, actionType) {
    if ($.trim($(contId).html()) != "") {
        if (actionType == "WARNING") {
            if (!$(contId).hasClass("alert alert-warning")) {
                $(contId).addClass("alert alert-warning");
            }
        }
        else if (actionType == "ERROR") {
            if (!$(contId).hasClass("alert alert-danger")) {
                $(contId).addClass("alert alert-danger");
            }
        }
        else if (actionType == "SUCCESS") {
            if (!$(contId).hasClass("alert alert-success")) {
                $(contId).addClass("alert alert-success");
            }
        }
        else if (actionType == "INFO") {
            if (!$(contId).hasClass("alert alert-info")) {
                $(contId).addClass("alert alert-info");
            }
        }
        else {
            $(contId).removeClass("alert alert-danger alert-success alert-warning alert-info", 200);
        }
    }
    else {
        $(contId).removeClass("alert alert-danger alert-success alert-warning alert-info", 200);

    }
}
