$(document).ready(function() {
    initCustomerEditButton();
});

$(document).on("click", "#btnSubmit", function () {
    $("#page").val("1");
    $("#frmCustomerManage").submit();
});

function initCustomerEditButton() {
    $(".edit-customer").click(function() {
        $.ajax({
            method: "GET",
            url: $("#edit-customer-url").val(),
            data: { id: $(this).data("customer-id") },
            success: function (html) {
                $("#modal-content").html(html);
                $.validator.unobtrusive.parse("#frmCustomerEdit");
                $("#modal-content").modal();

                $("#txtNote").summernote({
                    toolbar: false,
                    height: 90
                });

                var region = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace("region"),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    identify: function (obj) { return obj.region; },
                    prefetch: "../content/json/region.js"
                });

                function regionsWithDefaults(q, sync) {
                    if (q === "") {
                        sync(region.get("Bình Thạnh"));
                    }

                    else {
                        region.search(q, sync);
                    }
                }

                $("#txtRegion").typeahead({
                    minLength: 0,
                    highlight: true
                },
                {
                    name: "region",
                    display: "region",
                    source: regionsWithDefaults
                });

                var area = new Bloodhound({
                    datumTokenizer: Bloodhound.tokenizers.obj.whitespace("area"),
                    queryTokenizer: Bloodhound.tokenizers.whitespace,
                    identify: function (obj) { return obj.area; },
                    prefetch: "../content/json/area.js"
                });

                function areasWithDefaults(q, sync) {
                    if (q === "") {
                        sync(area.get("TP.HCM"));
                    }

                    else {
                        area.search(q, sync);
                    }
                }

                $("#txtArea").typeahead({
                    minLength: 0,
                    highlight: true
                },
                {
                    name: "area",
                    display: "area",
                    source: areasWithDefaults
                });

                initEditButtons();
            }
        });
    });
}

function initEditButtons() {
    $("#btnSave").click(function () {
        $("#frmCustomerEdit").submit();
    });
}

function customerManageCallBack(result) {
    $("#manager-content").html(result);
    initCustomerEditButton();
}

function customerEditBegin() {
    setEditProgressBar("on");
}

function customerEditCallBack(data) {
    if (data.Success) {
        showModalMessage(data.Message, "success");
        $("#ObjId").val(data.Data);
        reloadCurrentPage();
    }
    else showModalMessage(data.Message, "danger");
    setEditProgressBar("off");
}

function reloadCurrentPage() {
    var currentPage = $("#current-page").val();
    goToPage(currentPage); //inside pager.cshtml
}