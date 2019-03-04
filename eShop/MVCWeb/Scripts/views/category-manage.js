$(document).ready(function () {
    initCategoryManageButton();
});

function initCategoryManageButton() {
    $(".manage-category").click(function () {
        $.ajax({
            method: "GET",
            url: $("#category-manage-url").val(),
            success: function (html) {
                $("#modal-content").html(html);
                $.validator.unobtrusive.parse("#frmCategoryEdit");
                $("#modal-content").modal();
                initCategoryListTreeViewButtons();
                initCategoryEditFormButtons();
            }
        });
    });
}

function initCategoryListTreeViewButtons() {
    $("#category-treeview-container button").click(function () {
        loadCategoryEditForm($(this).data("category-id"));
        $("#category-treeview-container button").removeClass("btn-warning");
        $(this).addClass("btn-warning");
    });
}

function initCategoryEditFormButtons() {
    $("#btnCancelEditCategory").click(function () {
        loadCategoryEditForm(0);
        $("#category-treeview-container button").removeClass("btn-warning");
    });
    $("#btnDeleteCategory").confirmation({
        singleton: true,
        onConfirm: function () {
            $.ajax({
                method: "POST",
                url: $("#category-delete-url").val(),
                data: {
                    id: $("#ObjId").val()
                },
                beforeSend: function () {
                    categoryEditBegin();
                },
                success: function (data) {
                    categoryEditCallBack(data);
                }
            });
        },
        placement: "top",
        title: "Xóa nhóm này?"
    });
}

function categoryEditBegin() {
    setEditProgressBar("on");
}

function categoryEditCallBack(data) {
    if (data.Success) {
        showModalMessage(data.Message, "success");
        $("#category-treeview-container").html(data.Data);
        initCategoryListTreeViewButtons();
        $("#btnCancelEditCategory").click();
    }
    else showModalMessage(data.Message, "danger");
    setEditProgressBar("off");
}

function loadCategoryEditForm(id) {
    $.ajax({
        method: "GET",
        url: $("#category-edit-url").val(),
        data: {
            id: id
        },
        beforeSend: function () {
            $("#editFormLoading").show();
        },
        success: function (html) {
            $("#category-edit-container").html(html);
            initCategoryEditFormButtons();
        }
    });
}