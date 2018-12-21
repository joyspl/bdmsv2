/// <reference path="MainAppJs.js" />
var dataCnt = 1;
function LoadSchoolDetails() {

    $.ajax({
        url: "/SchoolChallan/GetSchoolDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {

            var HtmlItems = "<option value='-1'><<----Select School----->></option>";
            //var HtmlSchCdItems = "<option value='-1'><<----Select School Code----->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No School Aailable</option>';
                //HtmlSchCdItems = '<option>No School Code Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<----Select School----->></option>";
                //HtmlSchCdItems = "<option value='-1'><<----Select School----->></option>";
            }

            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.SchoolID + "'>" + item.School_name + "</option>";
                //HtmlSchCdItems += "<option value='" + item.SchoolID + "'>" + item.School_Code + "</option>";
            });
           
            $('#ddlSchool').html(HtmlItems);
            //$('#ddlSchoolCode').html(HtmlSchCdItems);
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
            url: "/SchoolChallan/GetReqViewData",
            type: 'POST',
            data: { startDate: fromDate, endDate: toDate, SchoolId: SchID },
            cache: false,
            success: function (data) {
                // appendMsg("Please Wait..", "INFO");

                var HtmlItems = "";
                HtmlItems += "<tr>";
                HtmlItems += "  <th style='text-align:Center;width:120px;'>Create</th>";
                HtmlItems += "  <th style='text-align:Left;width:150px;'>Requisition Code</th>";
                HtmlItems += "  <th style='text-align:Center;width:200px;'>Requisition Date</th>";
                HtmlItems += "  <th style='text-align:left;width:100px;'>Language</th>";
                HtmlItems += "  <th style='text-align:left;width:180px;'>Category</th>";                
                HtmlItems += "</tr>";
                $.each(data, function (i, item) {
                    HtmlItems += "  <td style='text-align:center;'><a href='/SchoolChallan/CreateSchoolChallan?ReqId=" + item.RequisitionId + "&SchChallanId=0'>Create</a></td>";
                    HtmlItems += "  <td style='text-align:Left;'>" + item.ReqCode + "</td>";
                    HtmlItems += "  <td style='text-align:center;'>" + item.RequisitionDate + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Language + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Category + "</td>";                   
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
function fun_validation() {
    //$("#btnSaveAsDraft").prop("disabled", true);
    var iCnt = 0;
    $("[id^=QuantityForShipping_]").each(function () {
        if ($(this).val() > 0) {
            iCnt++;
        }
    });


    $("#tblInvCumChal .trdta").each(function () {
        var $tr = $(this);
        var reqQty = ($($tr).find(".reqqty").val() * 1);
        var qtyForShopping = ($($tr).find(".qtyforsh").val() * 1);
        if (qtyForShopping > reqQty) {
            //alert("Quantity for shopping cannot be greater than requisition quantity.");
            //return false;
            $($tr).find(".qtyforsh").val($($tr).find(".reqqty").val());
        }
    });

    if (iCnt == 0) {
        alert("Please enter atleast one Quantity for Shipping..");
        //$("#btnSaveAsDraft").prop("disabled", false);
        return false;
    }
    
    if ($('#txtSchoolChallanDate').val() == "") {
        alert("Enter Challan Date..");
        $('#txtSchoolChallanDate').focus();
        //$("#btnSaveAsDraft").prop("disabled", false);
        return false;
    }
   
    if (confirm("Are you sure to Save the records?") == false) {
        //$("#btnSaveAsDraft").prop("disabled", false);
        return false;
    }
}
function CalcAmount(i) {

    document.getElementById('QuantityForShipping_' + i).value = document.getElementById('QuantityForShipping_' + i).value.replace(/\D/g, '');
    if (document.getElementById('QuantityForShipping_' + i).value == "") {
        document.getElementById('QuantityForShipping_' + i).value = 0;
    }

    //--- This part for more than School challan qty not allowed against requisition qty modified on 14.12.18 --//
    //if (Number(document.getElementById('QuantityForShipping_' + i).value) > (Number(document.getElementById('AvailableStockQuantity_' + i).value) - Number(document.getElementById('AlreadyShippedQuantity_' + i).value))) {
    //    alert("Quantity for Shipping more then Remaining Stock Quantity (Available Stock Quantity - Already Shipped Quantity) can not be allowed...");
    //    document.getElementById('QuantityForShipping_' + i).value = 0;
    //}
    
    return false;
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

