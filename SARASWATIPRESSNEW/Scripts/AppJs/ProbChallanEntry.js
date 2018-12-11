/// <reference path="MainAppJs.js" />
function LoadMasterDetails() {
    LoadDistrictDetails();
    LoadChallanCategoryDetails();
    LoadLanguageDetails();
    LoadTransporterDetails();
 
}
function LoadDistrictDetails() {
    $.ajax({
        url: "/InvoiceCumChallan/GetDistrictDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {
            var HtmlItems = "<option value='-1'><<----Select District----->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No District Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<----Select District----->></option>";
            }
            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#ddlDistrict').html(HtmlItems);
            var DistrictId = $("#hd_DistrictId").val()
            if (DistrictId > 0) {
                $("#ddlDistrict").val($("#hd_DistrictId").val());
                $('#ddlDistrict').change();
            }
        },
        error: function (data) {
        }
    });

}
function LoadChallanCategoryDetails() {
    $.ajax({
        url: "/InvoiceCumChallan/GetChallanCategoryDetails",
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
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#ddlCategoryID').html(HtmlItems);
            var CategoryId = $("#hd_CategoryId").val()
            if (CategoryId > 0) {
                $("#ddlCategoryID").val($("#hd_CategoryId").val());
            }
        },
        error: function (data) {
        }
    }).done(function () {
       
    });
}
function LoadLanguageDetails() {
    $.ajax({
        url: "/InvoiceCumChallan/GetChallanLanguageDetails",
        type: 'POST',
        data: {},
        cache: false,
        success: function (data) {
            var HtmlItems = "<option value='-1'><<----Select Language----->></option>";
            var itemCount = 0;
            if (data.length == 0) {
                HtmlItems = '<option>No Language Aailable</option>';
            }
            else {
                HtmlItems = "<option value='-1'><<----Select Language----->></option>";
            }
            $.each(data, function (i, item) {
                HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
            });
            $('#ddlLanguageId').html(HtmlItems);
            var LanguageId = $("#hd_LanguageId").val()
            if (LanguageId > 0) {
                $("#ddlLanguageId").val($("#hd_LanguageId").val());
            }           
        },
        error: function (data) {
        }
    }).done(function () {
       
    });
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
function SaveAddress() {
    if (document.getElementById("ddlDistrict").selectedIndex == 0) {    
        alert("Select District..");
        $('#ddlDistrict').focus();
        return false;
    }
    if (document.getElementById("ddlCircle").selectedIndex == 0) {
        alert("Select Circle..");
        $('#ddlCircle').focus();
        return false;
    }
    if ($("#txtCircleAddress").val()=="") {
        alert("Enter Circle Address..");
        $('#txtCircleAddress').focus();
        return false;
    }
    if ($("#txtCirclePinCode").val() == "") {
        alert("Enter Circle Pincode..");
        $('#txtCirclePinCode').focus();
        return false;
    }
    if (confirm("Are you sure do you want to update circle Address!") == false) {
        return false;
    }
    var CirID = $("#ddlCircle").val();
    var DisID = $("#ddlDistrict").val();
    $.ajax({
        url: "/InvoiceCumChallan/SaveAddress",
        type: "POST",
        data: { "CircleId": CirID, "SchooldAddress": $("#txtCircleAddress").val(), "Pincode": $('#txtCirclePinCode').val(), "InspectorName": $('#txtInspectorName').val(), "InspectorPhoneNo": $('#txtInspectorPhoneNo').val(), "InspectorEmailId": $('#txtInspectorEmailId').val() },
        success: function (mydata) {
            alert("Information Updated successfully");
        },
        error: function (error) {
            alert('failed');
            alert(error);
        }
    });
}
$(function () {
    $("#ddlDistrict").change(function () {
        var DisID = $("#ddlDistrict").val();
        $.ajax({
            url: "/InvoiceCumChallan/GetCircleDetailsOfaDistrict",
            type: 'POST',
            data: { DistrictID: DisID },
            cache: false,
            success: function (data) {
                var HtmlItems = "<option value='-1'><<----Select Circle----->></option>";
                var itemCount = 0;
                if (data.length == 0) {
                    HtmlItems = "<option>No Circle Exist</option>";
                }
                else {
                    HtmlItems = "<option value='-1'><<----Select Circle----->></option>";
                }
                $.each(data, function (i, item) {
                    HtmlItems += "<option value='" + item.Value + "'>" + item.Text + "</option>";
                });
                $('#ddlCircle').html(HtmlItems);
                var CircleId = $("#hd_CircleId").val()
                if (CircleId > 0) {
                    $("#ddlCircle").val($("#hd_CircleId").val());
                    $("#ddlCircle").change();                }               
            },
            error: function (data) {
            }
        });
    });
});
$(function () {
    $("#ddlCircle").change(function () {
        var CirID = $("#ddlCircle").val();
        $.ajax({
            url: "/InvoiceCumChallan/GetCircleAddressDetails",
            type: 'POST',
            data: { CircleID: CirID },
            cache: false,
            success: function (data) {
                $("#txtCircleAddress").val(data.CircleAddress);
                $("#txtCirclePinCode").val(data.CirclePinCode);
                $("#txtInspectorName").val(data.InspectorName);
                $("#txtInspectorPhoneNo").val(data.InspectorPhoneNo);
                $("#txtInspectorEmailId").val(data.InspectorEmailId);
            },
            error: function (data) {
            }
        });
    });
});


function fun_validation() {
    if ($('#InvoiceCumChallanDate').val() == "") {
        alert("Enter Challan Date..");
        $('#InvoiceCumChallanDate').focus();
        return false;
    }
    if (document.getElementById("ddlDistrict").selectedIndex == 0) {
        alert("Select District..");
        $('#ddlDistrict').focus();
        return false;
    }
    if (document.getElementById("ddlCircle").selectedIndex == 0) {
        alert("Select Circle..");
        $('#ddlCircle').focus();
        return false;
    }
    if (document.getElementById("ddlLanguageId").selectedIndex == 0) {
        alert("Select Language..");
        $('#ddlLanguageId').focus();
        return false;
    }
    if (document.getElementById("ddlTransporterID").selectedIndex == 0) {
        alert("Select Transporter..");
        $('#ddlTransporterID').focus();
        return false;
    }
    if (document.getElementById("txtVehicleNo").value == "") {
        alert("Enter vehicle number");
        $('#txtVehicleNo').focus();
        return false;
    }
    if (document.getElementById("txtConsigneeNo").value == "") {
        alert("Enter consignee number");
        $('#txtConsigneeNo').focus();
        return false;
    }
    if (confirm("Are you sure do you want to Save the records?") == false) {
        return false;
    }
}
