// Loader //
// -----------------------------

$(window).ready(function () {
    $('#preloader').delay(200).fadeOut('fade');
});

// Hide Header on Scroll //
// -----------------------------

$(window).on('scroll', function () {
    // checks if window is scrolled more than 500px, adds/removes solid class
    if ($(this).scrollTop() > 100) {
        $('.main-header-menu-wrap').addClass('affix')
    } else {
        $('.main-header-menu-wrap').removeClass('affix')
    }
});

// Scroll To Top //
// -----------------------------

$(window).on('scroll', function () {
    if ($(window).scrollTop() > $(window).height()) {
        $('.scroll-to-target').addClass('open');
    } else {
        $('.scroll-to-target').removeClass('open');
    }
    if ($('.scroll-to-target').length) {
        $('.scroll-to-target').on('click', function () {
            var target = $(this).attr('data-target')
            var new_time = new Date()
            if (!this.old_time || new_time - this.old_time > 1000) {
                // animate
                $('html, body').animate(
                    {
                        scrollTop: $(target).offset().top,
                    },
                    800
                )
                this.old_time = new_time
            }
        })
    }
});

// Wow Animate //
// -----------------------------

function wowAnimation() {
    new WOW({
        offset: 100,
        mobile: true,
    }).init()
}

wowAnimation();

// Mega Menu //
// -----------------------------

$('.mega-menu').HSMegaMenu({
    event: 'hover',
    pageContainer: $('.container'),
    breakpoint: 767.98,
    hideTimeOut: 0,
});

// Loading Button //
// -----------------------------

var spinner = '<span class="spinner-wrapper"><i class="spinner-border spinner-border-sm"></i></span>';

function ShowSpinner(target) {
    let $target = $(target);
    let content = `${spinner}${$target.text()}`;

    $target.addClass('disabled');
    $target.html(content);
}

function HideSpinner(target) {
    let $target = $(target);
    let content = $target.text().replace(spinner, '');

    $target.removeClass('disabled');
    $target.html(content);
}

$(function () {
    // Reload Card
    $('[data-action="loading"]:not(.disabled)').on("click", function () {
        ShowSpinner(this);
    })
});

// Exam Form Wizard //
// -----------------------------

$(function () {
    var examFormWizard = $(".exam-form-wizard");

    if (examFormWizard.length) {
        var form = examFormWizard.show();
        $.validator.messages.required = '(لطفا یک گزینه را انتخاب کنید)';

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
    }
});

// Exam Answer Form //
// -----------------------------

$(function () {
    $(document).on('submit', '.exam-comment-form', function (e) {
        e.preventDefault();

        let form = $(this);
        let url = form.attr('action');
        let data = form.serialize();
        let collapse = form.parents('.collapse');
        let btn = form.find('button[type="submit"]');

        $.ajax({
            url: url,
            data: data,
            type: 'post',
        }).done(function () {
            Swal.fire({
                timer: 1500,
                type: 'success',
                showConfirmButton: false,
                title: 'نظر شما با موفقیت ثبت شد',
            });
            collapse.removeClass('show');
        }).fail(function (xhr, exception) {
            Swal.fire({
                type: 'error',
                title: 'خطا',
                confirmButtonText: 'متوجه شدم',
                text: 'خطایی در انجام عملیات اتفاق افتاد!',
            });
        }).always(function () {
            HideSpinner(btn);
        });
    });
});

// Exam Carousel //
// -----------------------------

$('.exams-carousel').owlCarousel({
    loop: true,
    margin: 15,
    nav: false,
    dots: true,
    autoplay: true,
    lazyLoad: true,
    responsiveClass: true,
    autoplayHoverPause: true,
    responsive: {
        0: {
            items: 1,
        },
        500: {
            items: 2,
        },
        600: {
            items: 2,
        },
        800: {
            items: 3,
        },
        1200: {
            items: 3,
        },
    },
});