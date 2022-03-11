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
    errorLabelContainer: '.answer-error',

    highlight: function (element, errorClass) {
        $(element).removeClass(errorClass);
    },
    unhighlight: function (element, errorClass) {
        $(element).removeClass(errorClass);
    },
    errorPlacement: function (error, element) {
        error.insertAfter(element);
    },
    messages: {
        answer: "(لطفا یک گزینه را انتخاب کنید)",
    }
});

// add primary btn class
$('.actions a[role="menuitem"][href="#next"]').addClass("btn primary-solid-btn");
$('.actions a[role="menuitem"][href="#previous"]').addClass("btn outline-btn");
$('.actions a[role="menuitem"][href="#finish"]').addClass("btn primary-solid-btn");
$('.icon-tab [role="menuitem"]').addClass("glow");



//$(document).on('submit', '#exam-form-wizard', function (e) {
//    e.preventDefault();

//    console.log('prevented');

//    const data = new FormData(e.target);

//    const formJSON = Object.fromEntries(data.entries());

//    console.log(JSON.stringify(formJSON, null, 2));
//});


//jQuery.extend(jQuery.validator.messages, {
//    required: "وارد کردن این فیلد الزامی است.",
//    remote: "لطفا این فیلد را اصلاح کنید.",
//    email: "لطفا یک آدرس ایمیل معتبر وارد کنید.",
//    url: "لطفا یک URL معتبر وارد کنید.",
//    date: "لطفا یک تاریخ معتبر وارد کنید.",
//    dateISO: "لطفا یک تاریخ معتبر وارد کنید (ISO).",
//    number: "لطفا یک شماره معتبر وارد کنید.",
//    digits: "لطفا فقط عدد وارد کنید.",
//    creditcard: "لطفا یک شماره کارت معتبر وارد کنید.",
//    equalTo: "لطفا همان مقدار را دوباره وارد کنید.",
//    accept: "لطفا یک مقدار با پسوند معتبر وارد کنید.",
//    maxlength: jQuery.validator.format("لطفا بیشتر از {0} حرف وارد نکنید."),
//    minlength: jQuery.validator.format("لطفا حداقل {0} حرف وارد کنید."),
//    rangelength: jQuery.validator.format("لطفا مقداری به طول بین {0} و {1} وارد کنید."),
//    range: jQuery.validator.format("لطفا مقداری بین {0} و {1} وارد کنید."),
//    max: jQuery.validator.format("لطفا مقداری کمتر یا مساوی {0} وارد کنید."),
//    min: jQuery.validator.format("لطفا مقداری بیشتر یا مساوی {0} وارد کنید.")
//});