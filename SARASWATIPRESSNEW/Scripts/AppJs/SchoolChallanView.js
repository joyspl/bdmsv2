/// <reference path="MainAppJs.js" />
var dataCnt = 1;
function LoadSchoolDetails() {

    $.ajax({
        url: "/SchoolChallan/GetSchoolDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {

            var HtmlItems = "<option value='-1'><<----All School----->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No School Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<----All School----->></option>";
            }
            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.SchoolID + "'>" + item.School_name + "</option>";
            });
            $('#ddlSchool').html(HtmlItems);
            $('#btnView').click();
        },
        error: function (data) {
            alert(data);
        }
    });

}
$(function () {
    $('#btnView').click(function () {

        var SchID = $("#ddlSchool").val();
        var fromDate = $("#txtStartDate").val();
        var toDate = $("#txtEndDate").val();

        $.ajax({
            url: "/SchoolChallanView/GetReqViewData",
            type: 'POST',
            data: { startDate: fromDate, endDate: toDate, SchoolId: SchID },
            cache: false,
            success: function (data) {
                // appendMsg("Please Wait..", "INFO");

                var HtmlItems = "";
                HtmlItems += "<tr>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Create/View</th>";
                HtmlItems += "  <th style='text-align:Left;width:180px;'>Challan Code</th>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Challan Date</th>";
                HtmlItems += "  <th style='text-align:Left;width:180px;'>Requisiting Code</th>";
                HtmlItems += "  <th style='text-align:Center;width:180px;'>Requisition Date</th>";
                HtmlItems += "  <th style='text-align:left;width:180px;'>School Name</th>";
                HtmlItems += "  <th style='text-align:left;width:180px;'>School Code</th>";
                HtmlItems += "  <th style='text-align:left;width:180px;'>Challan Updated By</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Challan Updated On</th>";
                HtmlItems += "</tr>";
                $.each(data, function (i, item) {
                    HtmlItems += "  <td style='text-align:center;'>";
                    HtmlItems += "     <a href='/SchoolChallanReportPrint/Index?SchChallanId=" + item.SchoolChallanUniqueId + "' target='_blank'>Print</a> &nbsp; |&nbsp;";
                    HtmlItems += "     <a href='/SchoolChallan/CreateSchoolChallan?ReqId=" + item.RequisitionId + "&SchChallanId=" + item.SchoolChallanUniqueId + "'>Edit</a>";
                    HtmlItems += "  </td>";
                    HtmlItems += "  <td style='text-align:Left;'>" + item.SchoolChallanCode + "</td>";
                    HtmlItems += "  <td style='text-align:center;'>" + item.SchoolChallanDate + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.ReqCode + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.RequisitionDate + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.SchoolName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.SchoolCode + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.ChallanUpdatedBy + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.ChallanUpdatedTs + "</td>";
                    HtmlItems += "</tr>";
                });
                $('#tblInvCumChal').html(HtmlItems);
                if (data.length == 0 && dataCnt == 0) {
                    alert("No record found..");
                }
                else {
                    dataCnt = 0;
                }
                //  clearError();
            },
            error: function (data) {
                alert("Some Error Occured");
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

function ExportToExcel() {
    appendMsg("Please Wait..", "INFO");
    // var itemId = $('#InvoiceCumChallanReport').attr('itemid');
    var SchID = $("#ddlSchool").val();
    var fromDate = $("#txtStartDate").val();
    var toDate = $("#txtEndDate").val();

    var url = "/SchoolChallanView/ExportSchChallanData?SchoolId=" + SchID + "&startDate=" + fromDate + "&endDate=" + toDate;
    window.location.href = url;
    clearError();
    //window.location.href = '@Url.Action("InvoiceCumChallanReport", "ExportChallanData", new{ appId=d.AppId, userId=d.UserId})'
}

