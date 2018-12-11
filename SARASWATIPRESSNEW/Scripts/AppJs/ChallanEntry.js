/// <reference path="MainAppJs.js" />
function LoadMasterDetails() {
    LoadTransporterDetails(); 
}

function LoadTransporterDetails() {
    $.ajax({
        url: "/InvoiceCumChallan/GetTransporterDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {
            var HtmlItems = "<option value='-1'><<--Select Transporter-->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No Transporter Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<--Select Transporter-->></option>";
            }
            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#ddlTransporterID').html(HtmlItems);
            var TransporterID = $("#hd_TransporterID").val()
            if (TransporterID > 0) {
                $("#ddlTransporterID").val($("#hd_TransporterID").val());
            }
            
        },
        error: function (data) {
        },
    }).done(function () {
    });
}


function GetReqBookDtl() {   
    var CirID = $("#hd_CircleId").val();
    var DisID = $("#hd_DistrictId").val();
    var CatId = $("#hd_CategoryId").val();
    var langId = $("#hd_LanguageId").val();
    var ChallanId = $("#hd_ChallanId").val();

    $.ajax({
        url: "/InvoiceCumChallan/GetBooksReqDetails",
        type: 'POST',
        data: { District: DisID, CircleId: CirID, categoryId: CatId, languageId: langId, ChallanId: ChallanId },
        cache: false,
        success: function (data) {
            var HtmlItems = "";
            if (data.InvoiceCumChallanCollection == null) {
                $('#tblInvCumChal').html("<tr><td style='color:red;'>No record found.....</td></tr>");
            }
            for (var i = 0; i < data.InvoiceCumChallanCollection.length; i++) {
                HtmlItems += "<tr>";
                HtmlItems += "    <td style='text-align:left;width:10%;'>";
                HtmlItems += "      <input class='form-control' id='Book_Code_" + i + "' name='InvoiceCumChallanCollection[" + i + "].Book_Code' type='hidden' value='" + data.InvoiceCumChallanCollection[i].Book_Code + "' />";
                HtmlItems += "      " + data.InvoiceCumChallanCollection[i].ClassName + "";
                HtmlItems += "       <input class='form-control' id='ClassName"+ i +"' name='InvoiceCumChallanCollection["+ i +"].ClassName' readonly='readonly' style='width:90px;color:black;visibility:hidden;' type='text' value='" + data.InvoiceCumChallanCollection[i].ClassName + "' />";
                HtmlItems += "    </td>";               
                HtmlItems += "    <td style='text-align:left;width:10%;'>" + data.InvoiceCumChallanCollection[i].Book_Code + "</td>";
                HtmlItems += "    <td style='text-align:left;width:22%;'>" + data.InvoiceCumChallanCollection[i].Book_Name + "</td>";
                HtmlItems += "    <td style='text-align:left;width:11%;'>";
                HtmlItems += "        <input class='form-control' id='NetReqQty_" + i + "' name='InvoiceCumChallanCollection[" + i + "].NetReqQty' readonly='readonly' style='width:90px;color:black;' type='text' value='" + data.InvoiceCumChallanCollection[i].NetReqQty + "' />";
                HtmlItems += "        <p style='font-size:10px;color:red;' >" + data.InvoiceCumChallanCollection[i].BookSurplusQty + "</p>";
                HtmlItems += "    </td>";
                HtmlItems += "    <td style='text-align:center;width:12%;'>";
                HtmlItems += "        <input class='form-control' data-val='true' data-val-number='The field Already Shipped Qty must be a number.' data-val-required='The Already Shipped Qty field is required.' id='AlreadyShippedQty_" + i + "' name='InvoiceCumChallanCollection[" + i + "].AlreadyShippedQty' readonly='readonly' style='width:90px;color:black;' type='text' value='" + data.InvoiceCumChallanCollection[i].AlreadyShippedQty + "' />";
                HtmlItems += "        <input class='form-control' id='AlreadyShippedQty1_" + i + "' name='InvoiceCumChallanCollection[" + i + "].AlreadyShippedQty' readonly='readonly' style='width:90px;color:black;visibility:hidden;' type='text' value='" + data.InvoiceCumChallanCollection[i].AlreadyShippedQty + "' />";
                HtmlItems += "    </td>";
                HtmlItems += "    <td style='text-align:center;width:12%;'>";
                HtmlItems += "        <input class='form-control' data-val='true' data-val-number='The field RemainBal must be a number.' data-val-required='The RemainBal field is required.' id='RemainBal_" + i + "' name='InvoiceCumChallanCollection[" + i + "].RemainBal' readonly='readonly' style='width:90px;color:black;' type='text' value='" + data.InvoiceCumChallanCollection[i].RemainBal + "' />";
                HtmlItems += "    </td>";
                HtmlItems += "    <td style='text-align: center;width:10%;'>";
                HtmlItems += "        <input class='form-control numeric' data-val='true' data-val-number='The field QtyShipped must be a number.' data-val-required='The QtyShipped field is required.' id='QtyShipped_" + i + "' name='InvoiceCumChallanCollection[" + i + "].QtyShipped' onchange='CalcAmount("+ i +")' style='width:90px;color:black;' type='text' value='" + data.InvoiceCumChallanCollection[i].QtyShipped + "' />";
                HtmlItems += "    </td>";
                HtmlItems += "    <td style='text-align: left;width:13%;'>";
                HtmlItems += "        <input class='form-control' id='Cartoon_" + i + "' name='InvoiceCumChallanCollection[" + i + "].Cartoon' style='width:120px;color:black;' type='text' value='" + data.InvoiceCumChallanCollection[i].Cartoon + "' />";
                HtmlItems += "    </td>";
                HtmlItems += "</tr>";
            }
            $('#tblInvCumChal').html(HtmlItems);
        },
        error: function (data) {
            alert("Some Error Occured");
        }
    });
}
function fun_validation() {
    var iCnt = 0;
    $("[id^=QtyShipped_]").each(function () {
            if ($(this).val() > 0) {
                iCnt++;
            }
        }
    );
    if (iCnt == 0) {
        alert("Please enter atleast one Qty. for shipping..");
        return false;
    }
    if ($('#txtInvoiceCumChallanNo').val() == "") {
        return false;
    }

    if ($('#InvoiceCumChallanDate').val() == "") {
        alert("Enter Challan Date..");
        $('#InvoiceCumChallanDate').focus();
        return false;
    }
    if (document.getElementById("ddlTransporterID").selectedIndex == 0) {
        alert("Select Transporter..");
        $('#ddlTransporterID').focus();
        return false;
    }
    if (confirm("Are you sure do you want to Save the records?") == false) {
        return false;
    }
}
function CalcAmount(i) {
    document.getElementById('QtyShipped_' + i).value = document.getElementById('QtyShipped_' + i).value.replace(/\D/g, '');
    if (document.getElementById('QtyShipped_' + i).value == "")
    {
        document.getElementById('QtyShipped_' + i).value = 0;
    }
    if (Number(document.getElementById('QtyShipped_' + i).value) > Number(document.getElementById('NetReqQty_' + i).value) - Number(document.getElementById('AlreadyShippedQty1_' + i).value)) {
        alert("Quantity for Shipping more then Net Quantity For Shipped...");
        document.getElementById('QtyShipped_' + i).value = 0;
    }
    document.getElementById('RemainBal_' + i).value = ((document.getElementById('NetReqQty_' + i).value - document.getElementById('AlreadyShippedQty_' + i).value) - document.getElementById('QtyShipped_' + i).value);

    if (Number(document.getElementById('RemainBal_' + i).value) < 0) {
        document.getElementById('RemainBal_' + i).value = 0;
    }
    return false;
}



