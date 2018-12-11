/// <reference path="MainAppJs.js" />

$(function () {
    $('#btnView').click(function () {       
        var fromDate = $("#txtStartDate").val();
        var toDate = $("#txtEndDate").val();
        $.ajax({
            url: "/InvoiceView/GetInvoiceListData",
            type: 'Get',
            data: { startDate: fromDate, endDate: toDate },
            cache: false,
            success: function (data) {
                // appendMsg("Please Wait..", "INFO");

                var HtmlItems = "";
                HtmlItems += "<tr>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Print/Edit</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Invoice No.</th>";
                HtmlItems += "  <th style='text-align:Center;width:150px;'>Invoice Date</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Category</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Status</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated By</th>";
                HtmlItems += "  <th style='text-align:left;width:200px;'>Last Updated On</th>";
                HtmlItems += "</tr>";
                $.each(data, function (i, item) {                    
                    HtmlItems += "<tr>";
                    if (item.Save_Status == "Confirm") {
                        HtmlItems += "  <td style='text-align:center;'>";
                        HtmlItems += "      <a href='/Invoice/PrintInvoiceAnnexureI/" + item.InvoiceId + "?Command=Print' target='_blank'>Print</a>&nbsp;|&nbsp;";
                        HtmlItems += "      <a href='/Invoice/EditInvoice/" + item.InvoiceId + "?Command=Edit'>View</a>";
                        HtmlItems += "  </td>";
                    }
                    else {
                        HtmlItems += "  <td style='text-align:center;'>";
                        HtmlItems += "      <a href='/Invoice/PrintInvoiceAnnexureI/" + item.InvoiceId + "?Command=Print' target='_blank'>Print</a>&nbsp;|&nbsp;";
                        HtmlItems += "      <a href='/Invoice/EditInvoice/" + item.InvoiceId + "?Command=Edit'>Edit</a>";
                        HtmlItems += "  </td>";
                    }
                    HtmlItems += "  <td style='text-align:left;'>" + item.InvoiceNo + "</td>";
                    HtmlItems += "  <td style='text-align:center;'>" + item.InvoiceDate + "</td>";                   
                    HtmlItems += "  <td style='text-align:left;'>" + item.CategoryName + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.Save_Status + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.UpdatedBy + "</td>";
                    HtmlItems += "  <td style='text-align:left;'>" + item.UpdatedTimeStamp + "</td>";
                    HtmlItems += "</tr>";
                });

                //$('#tblInvCumChal').append(HtmlItems);
                $('#tblInvCumChal').html(HtmlItems);
                if (data.length == 0) {
                    alert("No record found..");
                }
                //  clearError();
            },
            error: function (data) {
                alert("Some Error Occured");
            }
        });
    });
});


function ExportToExcel() {
    appendMsg("Please Wait..", "INFO");
   
    var fromDate = $("#txtStartDate").val();
    var toDate = $("#txtEndDate").val();
    //var NestId = $(this).data('id');
    var url = "/InvoiceView/ExportInvoiceData?startDate=" + fromDate + "&endDate=" + toDate ;
    window.location.href = url;
    clearError();

    //window.location.href = '@Url.Action("InvoiceCumChallanReport", "ExportChallanData", new{ appId=d.AppId, userId=d.UserId})'

}

