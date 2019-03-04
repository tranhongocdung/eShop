var table;

var tableDefOptions = {
    "bLengthChange": false,
    "info": false,
    "bFilter": false,
    "bDestroy": true,
    "order": false,
    "bPaginate": false,
    "dom": "<\"search\"f><\"top\"l>rt<\"bottom\"ip><\"clear\">",
    "aoColumnDefs": [
            { 'bSortable': false, 'aTargets': [0, 1, 2, 3, 4, 5, 6, 7, 8, 9] }],
    "fnDrawCallback": function (o) {
        $(".dataTables_scrollBody").scrollTop(0);
    }
};

$(document).ready(function () {
    initSearchBox();
    var options = $.extend({}, tableDefOptions);
    table = $("#order-table").DataTable(options);
    initOrderActionButtons();
});

$(document).on("click", "#order-table td.details-control", function() {
    var tr = $(this).closest("tr");
    var row = table.row(tr);

    if (row.child.isShown()) {
        row.child.hide();
        tr.removeClass("shown");
    } else {
        var extendTable = $("#extend-for-search").clone();
        extendTable.removeAttr("id");
        extendTable.removeClass("hidden");
        $("tbody tr[data-order-id!='" + tr.data("order-id") + "']", extendTable).remove();
        row.child(extendTable[0].outerHTML).show();
        tr.addClass("shown");
    }
}).on("click", "#btnSubmit", function () {
    $("#page").val("1");
    $("#frmOrderManage").submit();
}).on("click", "#btnSubmitStatistic", function () {
    var fromDate = $("#txtFromDate").val();
    var toDate = $("#txtToDate").val();
    var customerIds = $("#txtCustomerIds").val();
    if (fromDate != "" || toDate != "" || customerIds != "") {
        var text = "Thông tin bán hàng ";
        if (fromDate != "") {
            text += "từ ngày <strong>" + fromDate + "</strong> ";
        }
        if (toDate != "") {
            text += "đến hết ngày <strong>" + toDate + "</strong>";
        }
        if (customerIds != "") {
            var strArr = [];
            $(".bootstrap-tagsinput .label-info").each(function() {
                if ($(this) != "undefined") {
                    strArr.push($(this).text());
                }
            });
            text += "cho khách hàng <strong>" + strArr.join(", ") + "</strong>";
        }
        $("#divStatisticInfo").removeClass("hidden");
        $("#divStatisticInfo").html(text);
    } else {
        $("#divStatisticInfo").addClass("hidden");
    }
    
    $("#txtFromDateStatistic").val(fromDate);
    $("#txtToDateStatistic").val(toDate);
    $("#txtCustomerIdsStatistic").val(customerIds);
    

    $("#txtStatusIdStatistic").val($("#ddlStatusId").val());
    $("#frmOrderStatistic").submit();
});

function initOrderActionButtons() {
    $(".complete-order").confirmation({
        singleton: true,
        onConfirm: function () {
            var that = $(this);
            $.ajax({
                url: $("#complete-order-url").val(),
                data: {
                    id: that.closest("tr").data("order-id")
                },
                beforeSend: function () {
                    that.find("span").remove();
                    that.find("img").show();
                },
                success: function () {
                    that.closest("tr").addClass("table-success");
                    that.closest(".order-btn-group").find(".cancel-order").remove();
                    that.remove();
                }
            });
        },
        placement: "left",
        title: "Hoàn tất đơn này?"
    });
    $(".cancel-order").confirmation({
        singleton: true,
        onConfirm: function () {
            var that = $(this);
            $.ajax({
                url: $("#cancel-order-url").val(),
                data: {
                    id: that.closest("tr").data("order-id")
                },
                beforeSend: function () {
                    that.find("span").remove();
                    that.find("img").show();
                },
                success: function () {
                    reloadCurrentPage();
                }
            });
        },
        placement: "left",
        title: "Hủy đơn này?"
    });
    $(".restore-order").confirmation({
        singleton: true,
        onConfirm: function () {
            var that = $(this);
            $.ajax({
                url: $("#restore-order-url").val(),
                data: {
                    id: that.closest("tr").data("order-id")
                },
                beforeSend: function () {
                    that.find("span").remove();
                    that.find("img").show();
                },
                success: function () {
                    reloadCurrentPage();
                }
            });
        },
        placement: "left",
        title: "Khôi phục đơn này?"
    });
    $(".open-payment-history").click(function() {
        $.ajax({
            method: "GET",
            url: $("#payment-history-url").val(),
            data: { id: $(this).closest("tr").data("order-id") },
            success: function (html) {
                $("#modal-content").html(html);
                $("#modal-content").modal();
            }
        });
    });
}

function initSearchBox() {
    var customers = new Bloodhound({
        datumTokenizer: function (datum) {
            return Bloodhound.tokenizers.whitespace(datum.SuggestNameFull);
        },
        queryTokenizer: Bloodhound.tokenizers.whitespace,
        remote: {
            wildcard: "%QUERY",
            url: $("#customer-suggestion-datasource").val() + "?query=%QUERY",
            transform: function (customers) {
                return $.map(customers, function (customer) {
                    return customer;
                });
            }
        }
    });
    $("#txtCustomerIds").tagsinput({
        itemValue: "Id",
        itemText: "CustomerName",
        typeaheadjs: [{
            hint: true,
            highlight: true,
            minLength: 1
        }, {
        displayKey: "SuggestNameFull",
        name: "customers",
        source: customers
        }]
    });
}

function orderManageCallBack(result) {
    $("#manager-content").html(result);
    var options = $.extend({}, tableDefOptions);
    table = $("#order-table").DataTable(options);
    initOrderActionButtons();
}

function orderStatisticCallBack(result) {
    $("#statistic-content").html(result);;
}

function reloadCurrentPage() {
    $("#page").val($("#current-page").val());
    $("#frmOrderManage").submit();
}