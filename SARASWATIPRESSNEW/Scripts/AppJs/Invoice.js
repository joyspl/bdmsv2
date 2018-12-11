/// <reference path="MainAppJs.js" />
var dataCnt = 1;
function LoadCategory() {

    $.ajax({
        url: "/Invoice/GetChallanCategoryDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {

            var HtmlItems = "<option value='-1'><<----Select Category----->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No Category Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<----Select Category----->></option>";
            }
            $.each(data, function (i, item) {
                debugger;
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
           
            $('#ddlCategory').html(HtmlItems);
            $("#txtChallanNo").focus();
        },
        error: function (data) {
            alert(data);
        }
    });

}
function LoadInvoiceDataForEdit() {
    var _InvoiceId = $("#txtInvoiceId").val();
    $.ajax({
        url: "/Invoice/GetInvoiceDetailForEdit",
        type: 'POST',
        data: { InvoiceId: _InvoiceId },
        cache: false,
        success: function (data) {
            $("#txtInvoiceNo").val(data.InvoiceNo);
            $("#ddlCategory").val(data.CategoryId);
            $("#txtInvoiceDate").val(data.InvoiceDate);
            $("#txtManualInvoiceNo").val(data.ManualInvoiceNo);
            if (data.SaveStatus == "1") {
                document.getElementById("btnSaveAsDraft").style.display = "None";//Block
                document.getElementById("btnConfirm").style.display = "None";
                $("#txtChallanNo").prop('disabled', true);
                document.getElementById("btnAdd").style.display = "None";
                
            }                    
            $("#txtInvoiceNo").prop('disabled', true);           
            $("#ddlCategory").prop('disabled', true);
            $('#btnAdd').click();
        },
        error: function (data) {
            alert(data);
        }
    });

}
function DeleteChallanIdFromInvoice(Id) {
    var _InvoiceId = Id;
    $.ajax({
        url: "/Invoice/DeleteChallanIdInInvoice",
        type: 'POST',
        data: { InvoiceId: _InvoiceId },
        cache: false,
        success: function (data) {
            
            $('#btnAdd').click();
            alert("Selected Challan deleted.");
        },
        error: function (data) {
            alert(data);
        }
    });

}

function UpdatedInInvoice(SaveStatus) {
    
    var _ManualInvoiceNo = $("#txtManualInvoiceNo").val();
    var _InvoiceDate = $("#txtInvoiceDate").val();
    var _InvoiceId = $("#txtInvoiceId").val();
    var _SaveStatus = SaveStatus;
    $.ajax({
        url: "/Invoice/UpdatedInInvoice",
        type: 'POST',
        data: { ManualInvoiceNo: _ManualInvoiceNo, InvoiceDate: _InvoiceDate, SaveStatus: _SaveStatus, InvoiceId: _InvoiceId },
        cache: false,
        success: function (data) {
            if (SaveStatus == "1") {
                document.getElementById("btnSaveAsDraft").style.display = "None";//Block
                document.getElementById("btnConfirm").style.display = "None";
                $("#txtChallanNo").prop('disabled', true);
                document.getElementById("btnAdd").style.display = "None";
            }
            alert("Information Saved Successfully.");
        },
        error: function (data) {
            alert(data);
        }
    });

}

$(function () {
    $('#btnAdd').click(function () {

        if (document.getElementById("ddlCategory").selectedIndex == 0) {
            alert("Select Category..");
            $('#ddlCategory').focus();
            return false;
        }
      //  appendMsg("Please Wait..", "INFO");
        var _ChallanNo = $("#txtChallanNo").val();
        var _InvoiceNo = $("#txtInvoiceNo").val();
        var _ManualInvoiceNo = $("#txtManualInvoiceNo").val();
        var _InvoiceDate = $("#txtInvoiceDate").val();
        var _InvoiceId = $("#txtInvoiceId").val();
        var _CategoryId = $("#ddlCategory").val();
        var contId = $("#ErrorMsgContainer");
        //clearError();
        $.ajax({
            url: "/Invoice/InvoiceUpdate",
            type: 'POST',
            data: { ChallanNo: _ChallanNo, InvoiceNo: _InvoiceNo,ManualInvoiceNo:_ManualInvoiceNo, InvoiceDate: _InvoiceDate, CategoryId: _CategoryId, InvoiceId: _InvoiceId },
            cache: false,
            success: function (data) {                
                if (data.UpdateCode == "ERROR") {
                    appendMsg(data.UpdateMsg, "INFO");
                    UpScroll();
                }
                else {
                    if (data.UpdateCode == "NEW") {
                        $("#txtInvoiceNo").val(data.InvoiceNo);                        
                        $("#txtInvoiceId").val(data.InvoiceId);
                        $("#txtInvoiceNo").prop('disabled', true);
                        $("#txtInvoiceDate").prop('disabled', true);
                        $("#ddlCategory").prop('disabled', true);
                    }
                   
                    var HtmlItems = "";
                    HtmlItems += "<tr>";
                    if (data.SaveStatus == "0") {
                        HtmlItems += "    <th style='text-align:left;width:200px;'>Updated/Delete From Invoice</th>";
                    }
                    HtmlItems += "    <th style='text-align:left;width:200px;'>Chalaan No.</th>";
                    HtmlItems += "    <th style='text-align:center;width:150px;'>Date</th>";
                    HtmlItems += "    <th style='text-align:left;width:200px;'>District Name</th>";
                    HtmlItems += "    <th style='text-align:left;width:200px;'>Circle Name</th>";
                    HtmlItems += "    <th style='text-align:left;width:200px;'>Category</th>";
                    HtmlItems += "    <th style='text-align:left;width:100px;'>Language</th>";
                    HtmlItems += "    <th style='text-align:left;width:200px;'>Transporter</th>";
                    HtmlItems += "    <th style='text-align:left;width:150px;'>Consignee No</th>";
                    HtmlItems += "    <th style='text-align:left;width:150px;'>Vehicle No</th>";
                    HtmlItems += "</tr>";
                    for (var i = 0; i < data.InvoiceChallanDtlCollection.length; i++) {
                        HtmlItems += "<tr>";
                        if (data.SaveStatus == "0") {
                            HtmlItems += "    <td style='text-align:left'>";
                            HtmlItems += "      <a href='/InvoiceCumChallanReqList/EditOperation/" + data.InvoiceChallanDtlCollection[i].ChallanId + "?Command=Edit'>Edit</a>&nbsp;|&nbsp;";
                            HtmlItems += "      <a href='#' onclick='DeleteChallanIdFromInvoice(" + data.InvoiceChallanDtlCollection[i].InvoiceDtlId + ")' >Delete</a>";
                            HtmlItems += "    </td>";
                        }
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].ChallanNo + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].ChallanDate + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].DistrictName + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].CircleName + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].CategoryName + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].Language + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].Transporter + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].CONSIGNEE_NO + "</td>";
                        HtmlItems += "    <td style='text-align:left'>" + data.InvoiceChallanDtlCollection[i].VEHICLE_NO + "</td>";
                        HtmlItems += "/<tr>";
                    }
                    $('#tblInvCumChal').html(HtmlItems);
                    appendMsg(data.UpdateMsg, "SUCCESS");
                    UpScroll();

                   
                }
                $("#txtChallanNo").val("");
                $('#txtChallanNo').focus();
            },
            error: function (data) {
                alert("Some Error Occured");
            }
        });
    });

    $('#txtChallanNo').keypress(function (e) {
        clearError();
        if (e.which == 13) {
            $('#btnAdd').click();
        }
    });

    $("#ddlCategory").change(function () {
        $("#txtChallanNo").val("");
        $('#txtChallanNo').focus();
    });
});
