// Loader //
// -----------------------------

$(window).ready(function () {
    $('#preloader').delay(200).fadeOut('fade');
});

// Hide Header on Scroll //
// -----------------------------

$(window).on('scroll', function () {
    // checks if window is scrolled more than 500px, adds/removes solid class
    //if ($(this).scrollTop() > 100) {
    //    $('.main-header-menu-wrap').addClass('affix');
    //} else {
    //    $('.main-header-menu-wrap').removeClass('affix');
    //}

    if ($(this).scrollTop() > 100) {
        $('.header-top-bar').addClass('affix');
        $('.main-header-menu-wrap').addClass('affix');
    } else {
        $('.header-top-bar').removeClass('affix');
        $('.main-header-menu-wrap').removeClass('affix');
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

// Mega Menu //
// -----------------------------

$(function () {
    $('.mega-menu').HSMegaMenu({
        event: 'click',
        pageContainer: $('.container'),
        //breakpoint: 767.98,
        hideTimeOut: 0,
    });
});

// Select2 //
// -----------------------------

$(function () {
    try {
        $(".select2").select2({
            dir: "rtl",
            width: '100%',
            theme: 'bootstrap4'
        });
    }
    catch {
    }
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

// Exam Rating //
// -----------------------------

$(function () {
    var ratingTarget = $('.barrating');
    if (ratingTarget.length) {
        ratingTarget.barrating('show', {
            theme: 'bars-pill',
            showValues: true,
            hoverState: false,
            fastClicks: false,
            allowEmpty: false,
            deselectable: true,
            showSelectedRating: false,
            emptyValue: '-- no rating selected --',
            onSelect: function (value, text, event) {

                var answerId = $(event.target)
                    .parents('.br-wrapper')
                    .children('select')
                    .data('answer-id');

                $.ajax({
                    type: 'post',
                    url: '/exams/comment',
                    data: {
                        Id: answerId,
                        Status: value
                    },
                }).done(function () {
                    Swal.fire({
                        timer: 1500,
                        type: 'success',
                        showConfirmButton: false,
                        title: 'نظر شما با موفقیت ثبت شد',
                    });
                }).fail(function (xhr, exception) {
                    Swal.fire({
                        type: 'error',
                        title: 'خطا',
                        confirmButtonText: 'متوجه شدم',
                        text: 'خطایی در انجام عملیات اتفاق افتاد!',
                    });
                }).always(function () {

                });
            }
        });
    }
});

// Exam Carousel //
// -----------------------------

$(function () {
    var examCarouselTarget = $('.exams-carousel');
    if (examCarouselTarget.length) {
        examCarouselTarget.owlCarousel({
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
    }
});

// Charts //
// -----------------------------

let Charts = (function () {

    let mode = 'light';

    let fonts = {
        base: 'IRANSans'
    }
    let colors = {
        black: '#12263F',
        white: '#FFFFFF',
        transparent: 'transparent',
        gray: {
            100: '#f6f9fc',
            200: '#e9ecef',
            300: '#dee2e6',
            400: '#ced4da',
            500: '#adb5bd',
            600: '#8898aa',
            700: '#525f7f',
            800: '#32325d',
            900: '#212529'
        },
        theme: {
            'info': '#11cdef',
            'default': '#172b4d',
            'primary': '#5e72e4',
            'success': '#2dce89',
            'danger': '#f5365c',
            'warning': '#fb6340',
            'secondary': '#f4f5f7',
        },
    };

    // Chart.js global options
    function chartOptions() {
        let options = {
            defaults: {
                global: {
                    responsive: true,
                    maintainAspectRatio: false,
                    defaultColor: (mode == 'dark') ? colors.gray[700] : colors.gray[600],
                    defaultFontColor: (mode == 'dark') ? colors.gray[700] : colors.gray[600],
                    defaultFontFamily: fonts.base,
                    defaultFontSize: 13,
                    layout: {
                        padding: 0
                    },
                    legend: {
                        rtl: true,
                        display: false,
                        position: 'bottom',
                        labels: {
                            padding: 40,
                            usePointStyle: true,
                        }
                    },
                    elements: {
                        point: {
                            radius: 0,
                            backgroundColor: colors.theme['primary']
                        },
                        line: {
                            tension: .4,
                            borderWidth: 4,
                            borderColor: colors.theme['primary'],
                            backgroundColor: colors.transparent,
                            borderCapStyle: 'rounded'
                        },
                        rectangle: {
                            backgroundColor: colors.theme['warning']
                        },
                        arc: {
                            backgroundColor: colors.theme['primary'],
                            borderColor: (mode == 'dark') ? colors.gray[800] : colors.white,
                            borderWidth: 4
                        }
                    },
                    tooltips: {
                        rtl: true,
                        enabled: true,
                        mode: 'index',
                        textDirection: 'rtl',
                        callbacks: {
                            title: function (item, data) {
                                return data['labels'][item[0]['index']];
                            },
                            label: function (item, data) {
                                let value = item.yLabel;
                                let label = data.datasets[item.datasetIndex].label || '';

                                if (label) {
                                    return `${label}: ${value}`;
                                }

                                return value;
                            },
                        }
                    }
                },
                doughnut: {
                    cutoutPercentage: 83,
                    tooltips: {
                        callbacks: {
                            title: function (item, data) {
                                let title = data.labels[item[0].index];
                                return title;
                            },
                            label: function (item, data) {
                                let value = data.datasets[0].data[item.index];
                                let content = '';

                                content += '<span class="popover-body-value">' + value + '</span>';
                                return content;
                            }
                        }
                    },
                    legendCallback: function (chart) {
                        let data = chart.data;
                        let content = '';

                        data.labels.forEach(function (label, index) {
                            let bgColor = data.datasets[0].backgroundColor[index];

                            content += '<span class="chart-legend-item">';
                            content += '<i class="chart-legend-indicator" style="background-color: ' + bgColor + '"></i>';
                            content += label;
                            content += '</span>';
                        });

                        return content;
                    }
                }
            }
        }

        // yAxes
        Chart.scaleService.updateScaleDefaults('linear', {
            gridLines: {
                borderDash: [2],
                borderDashOffset: [2],
                color: (mode == 'dark') ? colors.gray[900] : colors.gray[300],
                drawBorder: false,
                drawTicks: false,
                lineWidth: 0,
                zeroLineWidth: 0,
                zeroLineColor: (mode == 'dark') ? colors.gray[900] : colors.gray[300],
                zeroLineBorderDash: [2],
                zeroLineBorderDashOffset: [2]
            },
            ticks: {
                beginAtZero: true,
                padding: 10,
                callback: function (value) {
                    if (value % 1 === 0) {
                        return value;
                    }
                }
            }
        });

        // xAxes
        Chart.scaleService.updateScaleDefaults('category', {
            gridLines: {
                drawBorder: false,
                drawOnChartArea: false,
                drawTicks: false
            },
            ticks: {
                padding: 20
            },
            maxBarThickness: 10
        });

        return options;

    }

    // Parse global options
    function parseOptions(parent, options) {
        for (let item in options) {
            if (typeof options[item] !== 'object') {
                parent[item] = options[item];
            } else {
                parseOptions(parent[item], options[item]);
            }
        }
    }

    if (window.Chart) {
        parseOptions(Chart, chartOptions());
    }

    return {
        mode: mode,
        fonts: fonts,
        colors: colors,
    };

})();

$(function () {
    let ExamAnswersChart = (function () {
        let chart;
        let chartTarget = $('#exam-answers-chart');
        let questionIdTarget = $('.chart-question-select');

        function GetChartData() {
            let data;
            $.ajax({
                async: false,
                url: "/exams/getQuestionAnswersChartData",
                type: 'post',
                datatype: 'json',
                data: {
                    QuestionId: questionIdTarget.val(),
                },
                success: function (result) {
                    data = result;
                }
            });
            return data;
        }

        function Init() {
            var chartData = GetChartData();
            chart = new Chart(chartTarget, {
                type: 'bar',
                data: chartData,
            });
        };

        function Update() {
            chart.data = GetChartData();
            chart.update();
        }

        if (chartTarget.length) {
            Init();
        }

        $('.chart-question-select').on('change', function () {
            Update();
        });
    })();

    let ExamAnswersAccuracyChart = (function () {
        let chart;
        let chartTarget = $('#exam-answers-accuracy-chart');
        let questionIdTarget = $('.chart-question-select');

        function GetChartData() {
            let data;
            $.ajax({
                async: false,
                url: "/exams/getQuestionAnswersAccuracyChartData",
                type: 'post',
                datatype: 'json',
                data: {
                    QuestionId: questionIdTarget.val(),
                },
                success: function (result) {
                    data = result;
                }
            });
            return data;
        }

        function Init() {
            var chartData = GetChartData();
            chart = new Chart(chartTarget, {
                type: 'bar',
                data: {
                    labels: chartData.labels,
                    datasets: [
                        {
                            fill: true,
                            data: chartData.datasets[0].data,
                            stack: chartData.datasets[0].stack,
                            label: chartData.datasets[0].label,
                            backgroundColor: '#2dce89',
                        },
                        {
                            fill: true,
                            data: chartData.datasets[1].data,
                            stack: chartData.datasets[1].stack,
                            label: chartData.datasets[1].label,
                            backgroundColor: '#f5365c',
                        },
                        {
                            fill: true,
                            data: chartData.datasets[2].data,
                            stack: chartData.datasets[2].stack,
                            label: chartData.datasets[2].label,
                            backgroundColor: '#5e72e4',
                        },
                    ],
                },
                options: {
                    legend: {
                        display: true,
                        labels: {
                            fontColor: '#000',
                        },
                    },
                }
            });
        };

        function Update() {
            var chartData = GetChartData();
            chart.data = {
                labels: chartData.labels,
                datasets: [
                    {
                        fill: true,
                        data: chartData.datasets[0].data,
                        stack: chartData.datasets[0].stack,
                        label: chartData.datasets[0].label,
                        backgroundColor: '#2dce89',
                    },
                    {
                        fill: true,
                        data: chartData.datasets[1].data,
                        stack: chartData.datasets[1].stack,
                        label: chartData.datasets[1].label,
                        backgroundColor: '#f5365c',
                    },
                    {
                        fill: true,
                        data: chartData.datasets[2].data,
                        stack: chartData.datasets[2].stack,
                        label: chartData.datasets[2].label,
                        backgroundColor: '#5e72e4',
                    },
                ],
            };
            chart.update();
        }

        if (chartTarget.length) {
            Init();
        }

        $('.chart-question-select').on('change', function () {
            Update();
        });
    })();
});

// User Login //
// -----------------------------
$(function () {
    $(document).on('submit', '#login-form', function (e) {
        e.preventDefault();
        let form = $(e.target);
        let url = form.attr('action');
        let formData = form.serialize();
        let validationWrapper = $('#login-error');
        let submitButton = form.find('button[type="submit"]');
        let recaptchaResponse = $('#g-recaptcha-response');
        let participantId = $('input[name="ParticipantId"]');

        ShowSpinner(submitButton);
        validationWrapper.removeClass('d-none');

        $.ajax({
            'url': url,
            'type': 'post',
            //'data': formData
            'data': {
                ParticipantId: participantId.val(),
                GoogleRecaptchaResponse: recaptchaResponse.val(),
            }
        }).done(function () {
            location.reload();
        }).fail(function (xhr, exception) {
            var response = xhr.responseJSON;
            if (response && response.length) {
                validationWrapper.html(response[0]);
            }
        }).always(function () {
            HideSpinner(submitButton);
        });
    });
});