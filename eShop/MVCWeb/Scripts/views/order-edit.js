$.fn.editable.defaults.mode = "inline";

$(document).ready(function () {
    initXEditable();
    initRemoveProductRowButton();
    numberProductRow();
    initNumericTextbox(".unit-price");
    initNumericTextbox(".quantity");
    initNumericTextbox("#discount-value");
    initDiscountTypeOnChange();
    initSearchCustomerTextbox();
    initCustomerTypeToggle();
    initExistingOrder();
    initOrderNoteHint();
    initCategoryListTreeViewButtons();
    initPublicButtons();
});

function initCategoryListTreeViewButtons() {
    $("#category-filter-container button").click(function () {
        loadProductListToSelect($(this).data("category-id"));
        $("#category-filter-container button").removeClass("btn-danger");
        $(this).addClass("btn-danger");
    });
}

function loadProductListToSelect(categoryId)
{
    $.ajax({
        method: "POST",
        url: $("#product-list-to-select-url").val(),
        data: { categoryId },
        beforeSend: function () {
            $("#productSelectLoading").removeClass("hidden");
        },
        success: function (html) {
            $("#product-select-container").html(html);
            $("#product-select-container button").click(function () {
                if ($(this).hasClass("active")) $(this).removeClass("active");
                else $(this).addClass("active");
            });
            $("#productSelectLoading").addClass("hidden");
        }
    });
}

function destroyRegionAreaHint() {
    $("#txtRegion").typeahead("destroy");
    $("#txtArea").typeahead("destroy");
}

function initRegionAreaHint() {
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
}

function initOrderNoteHint() {
    $("#order-note-hint").change(function() {
        $("#txtOrderNote").val($(this).val());
    });
}

function initExistingOrder() {
    $("#txtCustomerNote").summernote({
        toolbar: false,
        height: 90
    });
    if ($("#order-id").val() != "") {
        $(".customer-info").attr("readonly", "readonly");
        $("#chkCustomerType").bootstrapToggle("off");
        $("#chkCustomerType").bootstrapToggle("disable");
        /*$("#txtCustomerNote").summernote("disable");*/
        $(".product-order-row").each(function() {
            calculateCashForProductRow($(this));
        });
        $("#discount-type").val($("#order-discount-type").val());
        $("#discount-value").val($("#order-discount-value").val());
        calculateTotalCash();

        $("#btnComplete").confirmation({
            singleton: true,
            onConfirm: function() {
                $("#orderEditLoading").show();
                $.ajax({
                    url: $("#complete-order-url").val(),
                    data: {
                        id: $("#order-id").val()
                    },
                    beforeSend: function() {
                        $("#orderEditLoading").show();
                    },
                    success: function() {
                        location.reload();
                    }
                });
            },
            placement: "left",
            title: "A completed order can't be edited, are you sure?"
        });

    } else {
        /*$("#txtCustomerNote").summernote("enable");*/
        initRegionAreaHint();
    }
}

function initCustomerTypeToggle() {
    $("#chkCustomerType").bootstrapToggle({
        on: "New customer",
        off: "Existing customer"
    });
    if ($("#customer-id").val() == "") {
        $("#chkCustomerType").bootstrapToggle("disable");
    }
    $("#chkCustomerType").change(function() {
        if ($(this).prop("checked")) {
            $("#customer-id").val("");
            $("#txtCustomerName").val("");
            $("#txtPhoneNo").val("");
            $("#txtEmail").val("");
            $("#txtAddress").val("");
            $("#txtRegion").val("");
            $("#txtArea").val("");
            $("#txtCustomerNote").val("");
            $("#txtCustomerNote").summernote("code","");
            /*$("#txtCustomerNote").summernote("enable");*/
            $(".customer-info").removeAttr("readonly", "readonly");
            $("#chkCustomerType").bootstrapToggle("disable");
            initRegionAreaHint();
        }
    });
}

function initSearchCustomerTextbox() {
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

    $("#txtSearchCustomer").typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        displayKey: "SuggestNameFull",
        name: "customers",
        source: customers
    });
    $("#txtSearchCustomer").bind("typeahead:selected", function(obj, datum) {
        loadCustomerDetail(datum);
        $("#chkCustomerType").bootstrapToggle("enable");
        $("#chkCustomerType").bootstrapToggle("off");
        /*$("#txtCustomerNote").summernote("disable");*/
        $(this).typeahead("val", "");
        destroyRegionAreaHint();
    });

    $("#txtPhoneNo").typeahead({
        hint: true,
        highlight: true,
        minLength: 1
    }, {
        displayKey: "SuggestNameFull",
        name: "customers",
        source: customers
    });
    $("#txtPhoneNo").bind("typeahead:selected", function (obj, datum) {
        loadCustomerDetail(datum);
        $("#chkCustomerType").bootstrapToggle("enable");
        $("#chkCustomerType").bootstrapToggle("off");
        $(this).typeahead("val", datum.PhoneNo);
        destroyRegionAreaHint();
    });
}

function loadCustomerDetail(data) {
    $("#customer-id").val(data.Id);
    $("#txtCustomerName").val(data.CustomerName);
    $("#txtPhoneNo").val(data.PhoneNo);
    $("#txtEmail").val(data.Email);
    $("#txtAddress").val(data.Address);
    $("#txtRegion").val(data.Region);
    $("#txtArea").val(data.Area);
    $("#txtCustomerNote").summernote("code", data.Note);
    $(".customer-info").attr("readonly", "readonly");
}

function initDiscountTypeOnChange() {
    $("#discount-type").change(function() {
        calculateTotalCash();
    });
}

function initNumericTextbox(selector, container) {
    if (typeof container == "undefined") {
        var container = $("body");
    }
    container.find(selector).numeric();
    container.find(selector).keyup(function() {
        var row = $(this).parent().parent();
        calculateCashForProductRow(row);
        calculateTotalCash();
    });
}

function addProductRow(list) {
    var productRow = $("#product-order-row-template").html();
    list.forEach(function(product) {
        var inCart = $(".product-order-row .product-id[value=" + product.ProductId + "]");
        if (inCart.length > 0 && product.ProductId != 0) {
            let inCartProductRow = inCart.closest(".product-order-row");
            let quantityContainer = inCartProductRow.find(".quantity");
            quantityContainer.val(parseInt(quantityContainer.val()) + 1);
            calculateCashForProductRow(inCartProductRow);
        } else {
            let appendRow = $("#tr-for-append");
            appendRow.append(productRow);
            appendRow.removeAttr("id");
            appendRow.addClass("product-order-row");
            appendRow.find(".product-name").attr("data-value", product.ProductId).html(product.ProductName);
            appendRow.find(".product-id").val(product.ProductId);
            appendRow.find(".quantity").val("1");
            appendRow.find(".product-option").html(product.ProductOptions);
            appendRow.find(".unit-price").val(product.UnitPrice);
            appendRow.find(".short-description").html(product.ShortDescription);
            calculateCashForProductRow(appendRow);
            initXEditable(appendRow);
            initRemoveProductRowButton(appendRow);
            initNumericTextbox(".unit-price", appendRow);
            initNumericTextbox(".quantity", appendRow);
            $("#product-order-row-container").append("<tr id=\"tr-for-append\"></tr>");
        }
    });
    numberProductRow();
    calculateTotalCash();
}

function initRemoveProductRowButton(container) {
    if (typeof container == "undefined") {
        var container = $("body");
    }
    container.find(".remove-row").confirmation({
        singleton: true,
        onConfirm: function() {
            $(this).parent().parent().remove();
            numberProductRow();
            calculateTotalCash();
        },
        placement: "left",
        title: "Delete this?"
    });
}

function numberProductRow() {
    var i = 0;
    $(".product-order-row .count-no").each(function () {
        i++;
        $(this).html(i);
    });
}

function initXEditable(container) {
    if (typeof container == "undefined") {
        var container = $("body");
    }
    container.find(".product-name").editable({
        type: "select2",
        select2: {
            placeholder: "Choose product",
            allowClear: true,
            minimumInputLength: 0,
            id: function (item) {
                return item.Id;
            },
            ajax: {
                url: $("#product-name-datasource").val(),
                dataType: "json",
                data: function (term, page) {
                    return { query: term };
                },
                results: function (data, page) {
                    return { results: data };
                }
            },
            formatResult: function (item) {
                return item.ProductName;
            },
            formatSelection: function (item) {
                localStorage.setItem("unitPrice", item.UnitPrice);
                localStorage.setItem("productOptions", item.ProductOptions);
                localStorage.setItem("productId", item.ProductId);
                localStorage.setItem("shortDescription", item.ShortDescription);
                return item.ProductName;
            },
            initSelection: function (element, callback) {
                return $.post($("#product-name-datasource").val(), { id: element.val() }, function (data) {
                    callback(data);
                });
            }
        },
        success: function () {
            var row = $(this).parent().parent();
            row.find(".unit-price").val(localStorage.getItem("unitPrice"));
            row.find(".original-price").val(localStorage.getItem("originalPrice"));
            row.find(".product-id").val(localStorage.getItem("productId"));
            row.find(".short-description").html(localStorage.getItem("shortDescription"));
            calculateCashForProductRow(row);
            calculateTotalCash();
        }
    });
}

function calculateCashForProductRow(container) {
    var unitPrice = container.find(".unit-price");
    var quantity = container.find(".quantity");
    if (quantity.val() == "") {
        quantity.val("1");
    }
    var productCash = container.find(".product-cash");
    var result = parseInt(unitPrice.val()) * parseFloat(quantity.val());
    productCash.data("value", result);
    productCash.val(result.toLocaleString("en"));
}

function calculateTotalCash() {
    var totalCash = 0;
    var discount = 0;
    $("tr.product-order-row").each(function() {
        if ($(this).find(".product-id").val() != "") {
            totalCash = totalCash + parseInt($(this).find(".product-cash").data("value"));
        }
    });
    if ($("#discount-value").val() != "") {
        if ($("#discount-type").val() == "0") {
            discount = totalCash * parseInt($("#discount-value").val()) / 100;
        } else {
            discount = parseInt($("#discount-value").val());
        }
        $("#total-discount").html(discount.toLocaleString("en"));
    }
    $("#total-cash").html(totalCash.toLocaleString("en"));
    $("#final-cash").html((totalCash-discount).toLocaleString("en"));
}

function initPublicButtons() {
    //Submit button
    $("#btnSubmit").click(function () {
        if (validateOrderBeforeSend())
            $("#frmOrderEdit").submit();
    });
    //Show/hide customer-info/quick-add-product panel
    $("#btnOpenCustomerInfoPanel").click(function() {
        $("#divQuickAddProductPanel").addClass("hidden");
        $("#divCustomerInfoPanel").removeClass("hidden");
    });
    $("#btnOpenQuickAddProductPanel").click(function () {
        $("#divQuickAddProductPanel").removeClass("hidden");
        $("#divCustomerInfoPanel").addClass("hidden");
    });
    $("#btnAddProductToOrder").click(function() {
        var list = [];
        $("#product-select-container button.active").each(function() {
            list.push({
                ProductId: $(this).data("product-id"),
                ProductName: $(this).data("product-name"),
                UnitPrice: $(this).data("unit-price"),
                ShortDescription: $(this).data("short-description"),
                ProductOptions: $(this).data("product-options")
            });
        });
        addProductRow(list);
    });
    $("#add-product-row").click(function () {
        var list = [];
        list.push({
            ProductId: 0,
            ProductName: "",
            UnitPrice: 0,
            ProductOptions: "",
            ShortDescription: ""
        });
        addProductRow(list);
    });
}

function validateOrderBeforeSend() {
    if ($("#customer-id").val() == "" && $("#txtCustomerName").val().trim() == "") {
        showMessage("Please input customer!", "error");
        return false;
    }
    $("#order-discount-type").val($("#discount-type").val());
    $("#order-discount-value").val($("#discount-value").val());
    var orderDetails = [];
    $(".product-order-row").each(function() {
        var obj = $(this);
        if (obj.find(".product-id").val() != "" && obj.find(".unit-price").val() != "" && obj.find(".quantity").val() != "") {
            orderDetails.push({
                Id: obj.find(".order-detail-id").val(),
                ProductId: obj.find(".product-id").val(),
                UnitPrice: obj.find(".unit-price").val(),
                Quantity: obj.find(".quantity").val(),
                Note: obj.find(".product-note").val()
            });
        }
    });
    if (orderDetails.length > 0) {
        $("#order-detail-json").val(JSON.stringify(orderDetails));
    } else {
        $("#order-detail-json").val("");
    }
    return true;
}

function orderEditCallBack(result) {
    if (result == "")
        showMessage("Saved order successfully!", "success");
    else {
        $("#order-id").val(result);
        window.location = location.protocol + "//" + location.host + location.pathname + "?id=" + result;
    }
}
