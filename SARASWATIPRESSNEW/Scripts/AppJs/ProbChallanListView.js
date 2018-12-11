/// <reference path="MainAppJs.js" />

var MsgCnt = 0;
function LoadDistrictDetails()
{
    $.ajax({
        url: "/InvoiceCumChallanReqList/GetDistrictDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {
            var HtmlItems = "<option value='-1'>All District</option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No District Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'>All District</option>";
            }
            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#ddlDistrict').html(HtmlItems);
            $('#ddlCircle').html("<option value='-1'>All Circle</option>");
            $('#btnView').click();
        },
        error: function (data) {
        }
    });

}

$(function () {
    $("#ddlDistrict").change(function () {
        //appendMsg("Please Wait..", "INFO");
        var DisID = $("#ddlDistrict").val();
        $.ajax({
            url: "/InvoiceCumChallanReqList/GetCircleDetailsOfaDistrict",
            type: 'POST',
            data: { DistrictID: DisID },
            cache: false,
            success: function (data) {
                var HtmlItems = "<option value='-1'>All Circle</option>";
                var itemCount = 0;
                if (data.length == 0) {
                    HtmlItems = "<option>No Circle Exist</option>";
                }
                else {
                    HtmlItems = "<option value='-1'>All Circle</option>";
                }
                $.each(data, function (i, item) {
                    HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $('#ddlCircle').html(HtmlItems);
                //clearError();
            },
            error: function (data) {
               // clearError();
            }
        });
    });
});
$(function () {
    $('#btnView').click(function () {
           
        var CirID = $("#ddlCircle").val();
        var DisID = $("#ddlDistrict").val();
        var fromDate = $("#txtStartDate").val();
        var toDate = $("#txtEndDate").val();
        $.ajax({
            url: "/TrxProvisionalChallanView/GetChallanViewData",
            type: 'POST',
            data: { startDate: fromDate, endDate: toDate, CircleID: CirID, DistrictID: DisID },
            cache: false,
            success: function (data) {
               // appendMsg("Please Wait..", "INFO");
                
                var HtmlItems = "";
                HtmlItems += "<tr>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Edit</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'><input type='checkbox' onClick='selectall(this)' /> Chalaan No.</th>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Date</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>District Name</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Circle Name</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Category</th>";
                HtmlItems += "  <th style='text-align:left;width:100px;'>Language</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Transporter</th>";
                HtmlItems += "  <th style='text-align:left;width:150px;'>Consignee No</th>";
                HtmlItems += "  <th style='text-align:left;width:150px;'>Vehicle No</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated By</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated On</th>";

                HtmlItems += "</tr>";
                $.each(data, function (i, item) {
                    HtmlItems += "<tr>";
                    HtmlItems += "  <td style='text-align:center;'>";
                    if (item.Status == "0") {
                        HtmlItems += "      <a href='/TrxProvisionalChallanView/EditOperation/" + item.ChallanId + "?Command=Edit'>Edit</a>";
                    }
                    else {
                        HtmlItems += "      <a href='/TrxProvisionalChallanView/EditOperation/" + item.ChallanId + "?Command=Confirm'>Confirm</a>";
                    }

                    HtmlItems += "  </td>";
                    if (item.Status == "0") {
                        HtmlItems += "  <td style='text-align:left;'> <input type='checkbox'  id='chk' name='check' value='" + item.ChallanId + "' />" + item.InvoiceCumChallanNo + "</td>";
                    }
                    else {
                        HtmlItems += "  <td style='text-align:left;'>" + item.InvoiceCumChallanNo + "</td>";
                    }

                    HtmlItems += "  <td style='text-align:center;'>" + item.InvoiceCumChallanDate + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.DistrictName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.CircleName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.CategoryName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Language + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Transporter + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.CONSIGNEE_NO + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.VEHICLE_NO + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.UpdatedBy + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.UpdatedTimeStamp + "</td>";
                    HtmlItems += "</tr>";
                });
                
                //$('#tblInvCumChal').append(HtmlItems);
                $('#tblInvCumChal').html(HtmlItems);
                if (data.length == 0 && MsgCnt==1) {
                    alert("No record found..");
                }
                MsgCnt = 1;
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
        if (checkboxes[i].checked==true)
        {
            $ccol.push(checkboxes[i].value);
        }
    }       
    json += $ccol.join(",") + '';
    
    return json;
}

function ExportToExcel() {
    appendMsg("Please Wait..", "INFO");
    // var itemId = $('#InvoiceCumChallanReport').attr('itemid');
    var CirID = $("#ddlCircle").val();
    var DisID = $("#ddlDistrict").val();
    var fromDate = $("#txtStartDate").val();
    var toDate = $("#txtEndDate").val();
    //var NestId = $(this).data('id');
    var url = "/InvoiceCumChallanReport/ExportChallanData?startDate=" + fromDate + "&endDate=" + toDate + "&CircleID=" + CirID + "&DistrictID=" + DisID;
    window.location.href = url;
    clearError();    
}
function ConfirmProvisionalChallan() {
    var _griddata = gridTojson();
    if (_griddata.trim() == "") {
        alert("Please select atleast one challan from the list.");
        return false;
    }
    if (confirm("Are you sure do you want to Confirm the challan information?") == false) {
        return false;
    }
    $.ajax({
        url: "/TrxProvisionalChallanView/ConfirmProvisionalChallan",
        type: 'POST',
        data: { griddata: _griddata },
        cache: false,
        success: function (data) {
            $('#btnView').click();
            alert(data);
        },
        error: function (data) {
        }
    });
}
