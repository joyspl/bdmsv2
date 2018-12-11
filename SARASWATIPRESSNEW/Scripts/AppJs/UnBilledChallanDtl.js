function LoadDistrictDetails() {
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

function ExportToExcel() {
    appendMsg("Please Wait..", "INFO");
    var DisID = $("#ddlDistrict").val();
    var fromDate = $("#txtStartDate").val();
    var toDate = $("#txtEndDate").val();
    var url = "/UnBilledChallanDtlByDistrict/ExportUnBilledData?startDate=" + fromDate + "&endDate=" + toDate + "&DistrictID=" + DisID;
    window.location.href = url;
    clearError();
}