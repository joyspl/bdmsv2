/// <reference path="MainAppJs.js" />
var dataCnt = 1;
$(function () {
    // modified 24.12.18

    //$('#btnView').click(function () { 

        $(document).off("click", "#btnView").on("click", "#btnView", function (e) {
        //debugger;
        var CirID = $("#hd_CircleId").val();
        var fromDate = $("#txtStartDate").val();
        var PendingOnly = document.getElementById('chkIsPendingRequire').checked;
        var toDate = $("#txtEndDate").val();

        $("#btnView").prop("disabled", true);

        $.ajax({
            url: "/ChallanReceivedAtCircle/GetChallanViewData",
            type: 'POST',
            data: { startDate: fromDate, endDate: toDate, CircleID: CirID, PendingOnly: PendingOnly },
            cache: false,
            success: function (data) {
                // appendMsg("Please Wait..", "INFO");

                var HtmlItems = "";
                HtmlItems += "<tr>";
                HtmlItems += "  <th style='text-align:Center;width:100px;'>Status<br/><p style='font-size:8px;color:red;' > Click The link below to <br/>View the challan Details</p></th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'><input type='checkbox' onClick='selectall(this)' /> Challan No.<br/><p style='font-size:8px;color:red;' >Click Check box to <br/> Select all Challan</p></th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Comments</th>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Date</th>";
                //HtmlItems += "  <th style='text-align:left;width:200px;'>District Name</th>";
                //HtmlItems += "  <th style='text-align:left;width:200px;'>Circle Name</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Category</th>";
                HtmlItems += "  <th style='text-align:left;width:100px;'>Language</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Transporter</th>";
                HtmlItems += "  <th style='text-align:left;width:150px;'>Consignee No</th>";
                HtmlItems += "  <th style='text-align:left;width:150px;'>Vehicle No</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated By</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated On</th>";
                
                HtmlItems += "</tr>";
               
                $.each(data, function (i, item) {
                    //items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                    // items += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                   
                    var status = "Pending";
                    if (item.ReceivedAtCircle == "1") {
                        status = "Received";
                    }
                    HtmlItems += "<tr>";
                    HtmlItems += "  <td style='text-align:center;'>";
                    HtmlItems += "      <a href='/InvoiceCumChallanReqList/PrintOperation/" + item.ChallanId + "?Command=Print' target='_blank'>" + status + "</a>";
                    HtmlItems += "  </td>";                    
                    if (item.ReceivedAtCircle == "1") {
                        HtmlItems += "  <td style='text-align:left;'>&nbsp;&nbsp;&nbsp;&nbsp;<b><font color='#0d7d14'>" + item.InvoiceCumChallanNo + "</font></b></td>";
                    }
                    else {
                        HtmlItems += "  <td style='text-align:left;'> <input type='checkbox'  id='chk' name='check' value='" + item.ChallanId + "' />&nbsp;" + item.InvoiceCumChallanNo + "</td>";
                    }
                    HtmlItems += "  <td class='tdcmt' style='text-align:left;'><input type='text' class='form-control txtComment' value='" + item.ChallanComment + "' />&nbsp;<button class='btn btn-primary btnComment' onclick='SaveComment(this)' data-id='" + item.ChallanId + "'>Save</button></td>";
                    HtmlItems += "  <td style='text-align:center;'>" + item.InvoiceCumChallanDate + "</td>";
                    //HtmlItems += "  <td style='text-align:left;'>" + item.DistrictName + "</td>";
                    //HtmlItems += "  <td style='text-align:left;'>" + item.CircleName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.CategoryName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Language + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Transporter + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.CONSIGNEE_NO + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.VEHICLE_NO + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.ReceivedBy + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.ReceivedTimeStamp + "</td>";
                    
                    HtmlItems += "</tr>";                    
                });
                //$('#tblInvCumChal').append(HtmlItems);
                $('#tblInvCumChal').html(HtmlItems);

                if (data.length == 0 && dataCnt == 0) {
                    alert("No record found..");
                }
                else {
                    dataCnt = 0;
                }
                //  clearError();
                $("#btnView").prop("disabled", false);
            },
            error: function (data) {
                alert("Some Error Occured");
                $("#btnView").prop("disabled", false);
            }
            
        });
    });
});
function selectall(source) {
    checkboxes = document.getElementsByName('check');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        checkboxes[i].checked = source.checked;
    }
}
function gridTojson() {

    var json = '';
    var $ccol = [];
    checkboxes = document.getElementsByName('check');
    for (var i = 0, n = checkboxes.length; i < n; i++) {
        if (checkboxes[i].checked == true) {
            $ccol.push(checkboxes[i].value);
        }
    }
    json += $ccol.join(",") + '';

    return json;
}

function SaveReceiveChallan() {
    var ReceivedTimeStamp = $("#txtReceiveDate").val();
    var _griddata = gridTojson();
    if (_griddata.trim() == "") {
        alert("Please select atleast one challan from the list.");
        return false;
    }
    if (confirm("Are you sure do you want to Save the records?") == false) {
        return false;
    }
    appendMsg("Please Wait..", "INFO");
    $.ajax({
        url: "/ChallanReceivedAtCircle/ReceiveChallan",
        type: 'POST',
        data: { griddata: _griddata, ReceiveDate: ReceivedTimeStamp },
        cache: false,
        success: function (data) {
            clearError();
            alert(data);
            $('#btnView').click();
        },
        error: function (data) {
            clearError();
        }
    });
    
}


function ExportToExcel() {
    appendMsg("Please Wait..", "INFO");
    // var itemId = $('#InvoiceCumChallanReport').attr('itemid');
    var CirID = $("#hd_CircleId").val();
    var PendingOnly = document.getElementById('chkIsPendingRequire').checked;
    var fromDate = $("#txtStartDate").val();
    var toDate = $("#txtEndDate").val();
    var url = "/InvoiceCumChallanReport/ExportChallanReceivedAtCircle?startDate=" + fromDate + "&endDate=" + toDate + "&CircleID=" + CirID + "&PendingOnly=" + PendingOnly;
    window.location.href = url;
    clearError();
    //window.location.href = '@Url.Action("InvoiceCumChallanReport", "ExportChallanData", new{ appId=d.AppId, userId=d.UserId})'
}

function SaveComment(btn) {
    var postdata = new Object();
    postdata.ChallanID = $(btn).attr("data-id");
    postdata.Comment = $(btn).closest(".tdcmt").find(".txtComment").val().trim();

    $.ajax({
        url: "/ChallanReceivedAtCircle/SaveComment",
        type: 'POST',
        data: JSON.stringify(postdata),
        contentType: "application/json; charset=utf-8",
        cache: false,
        success: function (data) {
            clearError();
            alert(data);
            $('#btnView').click();
        },
        error: function (data) {
            clearError();
        }
    });
}