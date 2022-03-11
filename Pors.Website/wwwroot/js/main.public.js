// Mega Menu //
// -----------------------------
$('.mega-menu').HSMegaMenu({
    event: 'hover',
    pageContainer: $('.container'),
    breakpoint: 767.98,
    hideTimeOut: 0,
});

// Exam Form Wizard //
// -----------------------------
$(function () {
    $.validator.messages.required = '(لطفا یک گزینه را انتخاب کنید)';

    var examFormWizard = $(".exam-form-wizard");
    var form = examFormWizard.show();

    examFormWizard.steps({
        headerTag: "h6",
        bodyTag: "fieldset",
        transitionEffect: "fade",
        titleTemplate: '<span class="step">#index#</span>#title#',
        labels: {
            cancel: "انصراف",
            current: "قدم کنونی:",
            pagination: "صفحه بندی",
            finish: "ثبت پاسخ‌ها",
            next: "سوال بعد",
            previous: "سوال قبل",
            loading: "در حال بارگذاری ..."
        },
        onStepChanging: function (event, currentIndex, newIndex) {
            // Allways allow previous action even if the current form is not valid!
            if (currentIndex > newIndex) {
                return true;
            }
            form.validate().settings.ignore = ":disabled,:hidden";
            return form.valid();
        },
        onFinishing: function (event, currentIndex) {
            form.validate().settings.ignore = ":disabled";
            return form.valid();
        },
        onFinished: function (event, currentIndex) {
            $(this).submit();
        }
    });

    examFormWizard.validate({
        // ignore hidden fields
        ignore: 'input[type=hidden]',
        errorClass: 'danger line-height-2',
        successClass: 'success line-height-2',
        //errorLabelContainer: '.answer-error',

        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        },
        unhighlight: function (element, errorClass) {
            $(element).removeClass(errorClass);
        },
        errorPlacement: function (error, element) {
            let errorContainer = $('.answer-error');
            $(error[0]).attr('for', element.attr('name'));
            errorContainer.html(error[0]);
        },
    });

    // add primary btn class
    $('.actions a[role="menuitem"][href="#next"]').addClass("btn primary-solid-btn");
    $('.actions a[role="menuitem"][href="#previous"]').addClass("btn outline-btn");
    $('.actions a[role="menuitem"][href="#finish"]').addClass("btn primary-solid-btn");
    $('.icon-tab [role="menuitem"]').addClass("glow");
});