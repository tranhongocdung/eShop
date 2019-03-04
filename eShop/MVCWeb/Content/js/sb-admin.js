$(function() {

    $('#side-menu').metisMenu();

});

//Loads the correct sidebar on window load,
//collapses the sidebar on window resize.
$(function() {
    $(window).bind("load resize", function() {
        var width = (this.window.innerWidth > 0) ? this.window.innerWidth : this.screen.width;
        if (width < 768) {
            $("div.sidebar-collapse").addClass("collapse");
        } else {
            $("div.sidebar-collapse").removeClass("collapse");
        }
    });
    $(".clear-text").click(function () {
        $(this).parent().parent().children("input").first().val("");
    });
    UILoad.initDatePicker(".datepicker-control");
    UILoad.initMVCValidationForBootstrap();
});

var UILoad = {
    initDatePicker: function (selector, funcOnDateChanged) {
        $(selector).each(function () {
            var that = $(this);
            that.datepicker({
                format: "dd/mm/yyyy",
                language: "vi",
                autoclose: true,
                orientation: "bottom auto",
                todayHighlight: true
            }).on("changeDate", funcOnDateChanged);
            $(this).parent().find(".remove-date").click(function() {
                that.val("");
            });
        });
    },
    initMVCValidationForBootstrap: function() {
        var defaultOptions = {
            validClass: "has-success",
            errorClass: "has-error",
            highlight: function (element, errorClass, validClass) {
                $(element).closest(".form-group")
                    .removeClass(validClass)
                    .addClass(errorClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).closest(".form-group")
                .removeClass(errorClass)
                .addClass(validClass);
            }
        };

        $.validator.setDefaults(defaultOptions);

        $.validator.unobtrusive.options = {
            errorClass: defaultOptions.errorClass,
            validClass: defaultOptions.validClass
        };
    }
}