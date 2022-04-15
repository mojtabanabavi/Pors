
// Navbar //
// -----------------------------

'use strict';

let Navbar = (function () {

    // letiables

    let $nav = $('.navbar-nav, .navbar-nav .nav');
    let $collapse = $('.navbar .collapse');
    let $dropdown = $('.navbar .dropdown');

    // Methods

    function accordion($this) {
        $this.closest($nav).find($collapse).not($this).collapse('hide');
    }

    function closeDropdown($this) {
        let $dropdownMenu = $this.find('.dropdown-menu');

        $dropdownMenu.addClass('close');

        setTimeout(function () {
            $dropdownMenu.removeClass('close');
        }, 200);
    }


    // Events

    $collapse.on({
        'show.bs.collapse': function () {
            accordion($(this));
        }
    })

    $dropdown.on({
        'hide.bs.dropdown': function () {
            closeDropdown($(this));
        }
    })

})();

let NavbarCollapse = (function () {

    // letiables

    let $nav = $('.navbar-nav'),
        $collapse = $('.navbar .collapse');


    // Methods

    function hideNavbarCollapse($this) {
        $this.addClass('collapsing-out');
    }

    function hiddenNavbarCollapse($this) {
        $this.removeClass('collapsing-out');
    }


    // Events

    if ($collapse.length) {
        $collapse.on({
            'hide.bs.collapse': function () {
                hideNavbarCollapse($collapse);
            }
        })

        $collapse.on({
            'hidden.bs.collapse': function () {
                hiddenNavbarCollapse($collapse);
            }
        })
    }

})();

// Sidebar Auto Collapse //
// -----------------------------

$(function () {
    $('.nav-link[data-toggle="collapse"].active').each(function () {
        var $this = $(this);
        var $target = $(`.collapse${$this.attr('href')}`);
        $target.collapse('show');
    });
});

// Set Default Image //
// -----------------------------

$(function () {
    function SetDefaultImages() {
        $('img').each(function () {
            let $this = $(this);
            let $src = $this.attr('src');
            if (!$src || $src == '/') {
                $this.attr('src', '/img/defaults/nopicture.jpg');
            }
        });
    }

    SetDefaultImages();
});

// Image Preview //
// -----------------------------

$(function () {
    function PreviewImage(input, output) {
        if (input.files && input.files[0]) {
            let reader = new FileReader();
            reader.onload = function (e) {
                output.attr('src', e.target.result);
            }
            reader.readAsDataURL(input.files[0]);
        }
    }

    let imagePreviewClear = $('#image-preview-clear');
    let imagePreviewInput = $('#image-preview-input');
    let imagePreviewOutput = $('#image-preview-output');

    $(imagePreviewInput).change(function () {
        PreviewImage(this, imagePreviewOutput);
        //let parent = $(this).parent();
        //if (parent.has('#image-preview-clear').length === 0) {
        //    parent.append('<button type="button" id="image-preview-clear" class="btn btn-danger mr-2 disabled">حذف تصویر</button>');
        //}
    });
});

// Select2 //
// -----------------------------

try {
    $(".select2").select2({
        dir: "rtl",
        width: '100%',
        theme: 'bootstrap4'
    });
}
catch {
}

// Form Repeater //
// -----------------------------

$(function () {
    let repeaterTarget = $('.form-repeater');

    if (repeaterTarget.length) {
        repeaterTarget.repeater({
            show: function () {
                $(this).slideDown();
            },
            hide: function (deleteElement) {
                if (confirm('آیا از حذف این آیتم مطمئن هستید؟')) {
                    $(this).slideUp(deleteElement);
                }
            }
        });
    }
});

// Ck Editor //
// -----------------------------

$(function () {
    var targets = $(".ck-classic-editor");
    if (targets.length) {
        targets.each(function () {
            let item = $(this)[0];
            var itemHeight = $(item).attr('height');
            ClassicEditor
                .create(item, {
                    language: 'fa',
                })
                .then(editor => {
                    editor.editing.view.change(writer => {
                        writer.setStyle('height', itemHeight, editor.editing.view.document.getRoot());
                    });
                })
                .catch(error => {
                    console.error(error);
                });
        });
    }
});

// Confirm Delete Alert //
// -----------------------------

$(function () {
    $(document).on('click', '[data-delete-btn]', function (e) {
        e.preventDefault();
        Swal.fire({
            type: 'warning',
            title: 'آیا اطمینان دارید؟',
            text: "این عمل قابل بازگشت نخواهد بود!",
            buttonsStyling: false,
            showCancelButton: true,
            cancelButtonText: 'انصراف',
            confirmButtonText: 'بله، حذف شود!',
            confirmButtonClass: 'btn btn-danger',
            cancelButtonClass: 'btn btn-primary mr-3',

        }).then((result) => {
            if (result.value) {
                let url = '';
                let isLink = $(e.target).is('a, a *');

                if (isLink) {
                    url = $(this).attr('href');
                }
                else {
                    url = $(this).data('data-delete-url');
                }

                fetch(url)
                    .then(response => {
                        if (!response.ok) {
                            Swal.fire({
                                type: 'error',
                                title: 'خطا',
                                confirmButtonText: 'متوجه شدم',
                                text: 'خطایی در انجام عملیات اتفاق افتاد!',
                            });
                        }
                        else {
                            // reload datatable
                            $(document).find("table").dataTable().api().ajax.reload();

                            Swal.fire({
                                timer: 1500,
                                type: 'success',
                                showConfirmButton: false,
                                title: 'عملیات با موفقیت انجام شد',
                            });
                        }
                    });
            }
        })
    });
});

// Select All Button //
// -----------------------------

$(function () {
    $('#select-all-permissions').on('change', function () {
        let $this = $(this);
        let targets = $('input[name="permissionIds"]');
        targets.prop('checked', $this.prop("checked"));
    });
});


// Data Table //
// -----------------------------

$(function () {
    function initDataTable(target, options) {

        if (typeof DataTable !== 'undefined') {
            options.filter = true;
            options.paging = true;
            options.ordering = true;
            options.processing = true;
            options.serverSide = true;
            options.orderMulti = false;
            if (typeof dataTablesPersianLanguage !== 'undefined') {
                options.language = dataTablesPersianLanguage;
            }
            let table = $(target).on('init.dt', function () {
                $('div.dataTables_length select').removeClass('custom-select custom-select-sm')
            }).DataTable(options);
            return table;
        }
    }

    // Target Options
    let examsDataTableOptions = {
        'ajax': {
            'url': '/admin/exam/GetExams',
            'type': 'post',
            'datatype': 'json'
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
            },
            {
                'name': 'status',
                'autoWidth': true,
                'searchable': false,
                'render': function (data, type, row) {
                    return `<p class="badge badge-primary">${row.status}</p>`
                }
            },
            {
                'name': 'createdBy',
                'data': 'createdBy',
                'autoWidth': true,
                'orderable': false,
                'searchable': false,
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/exam/report/${row.id}">مشاهده‌ی گزارش</a>
                            <a class="dropdown-item" href="/admin/question/index/${row.id}">لیست ‌سوالات</a>
                            <a class="dropdown-item" href="/admin/participant/index/${row.id}">لیست ‌شرکت‌کنندگان</a>
                            <a class="dropdown-item" href="/admin/exam/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/exam/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let questionDataTableOptions = {
        'ajax': {
            'url': '/admin/question/GetQuestions',
            'type': 'post',
            'datatype': 'json',
            'data': function (params) {
                params.ExamId = $('#exam-id').val()
            },
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'examId',
                'data': 'examId',
                'autoWidth': true,
                'searchable': false
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/option/index/${row.id}">لیست گزینه‌ها</a>
                            <a class="dropdown-item" href="/admin/answer/index/${row.id}">لیست ‌پاسخ‌ها</a>
                            <a class="dropdown-item" href="/admin/option/create/${row.id}">افزودن گزینه</a>
                            <a class="dropdown-item" href="/admin/question/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/question/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let rolesDataTableOptions = {
        'ajax': {
            'url': '/admin/role/GetRoles',
            'type': 'post',
            'datatype': 'json'
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
            },
            {
                'name': 'description',
                'data': 'description',
                'autoWidth': true,
                'orderable': false,
                'searchable': false,
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/permission/update/${row.id}">مدیریت دسترسی</a>
                            <a class="dropdown-item" href="/admin/role/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/role/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let usersDataTableOptions = {
        'ajax': {
            'url': '/admin/user/GetUsers',
            'type': 'post',
            'datatype': 'json'
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'firstName',
                'data': 'firstName',
                'autoWidth': true,
            },
            {
                'name': 'lastName',
                'data': 'lastName',
                'autoWidth': true,
            },
            {
                'name': 'email',
                'data': 'email',
                'autoWidth': true,
            },
            {
                'name': 'isActive',
                'autoWidth': true,
                'searchable': false,
                'render': function (data, type, row) {
                    let content = '';

                    if (row.isActive)
                        content += '<span class="badge badge-primary">فعال</span>';
                    else
                        content += '<span class="badge badge-danger">غیرفعال</span>';

                    return content;
                }
            },
            {
                'name': 'registerDateTime',
                'data': 'registerDateTime',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/user/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/user/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let optionsDataTableOptions = {
        'ajax': {
            'url': '/admin/option/GetOptions',
            'type': 'post',
            'datatype': 'json',
            'data': function (params) {
                params.questionId = $('#question-id').val();
            },
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'questionId',
                'data': 'questionId',
                'autoWidth': true,
                'searchable': false
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
            },
            {
                'name': 'image',
                'data': 'image',
                'autoWidth': true,
                'searchable': false,
                'render': function (data, type, row) {
                    let content = '';

                    if (row.image)
                        content += '<span class="badge badge-primary">دارد</span>';
                    else
                        content += '<span class="badge badge-danger">ندارد</span>';

                    return content;
                }
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/option/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/option/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let answersDataTableOptions = {
        'ajax': {
            'url': '/admin/answer/GetAnswers',
            'type': 'post',
            'datatype': 'json',
            'data': function (params) {
                params.questionId = $('#question-id').val();
            },
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'attemptId',
                'data': 'attemptId',
                'autoWidth': true,
            },
            {
                'name': 'optionId',
                'data': 'optionId',
                'autoWidth': true,
                'searchable': false
            },
            {
                'name': 'status',
                'autoWidth': true,
                'orderable': true,
                'searchable': false,
                'render': function (data, type, row) {
                    let content = '';

                    if (row.status == 1)
                        content += '<span class="badge badge-dark text-white">نامشخص</span>';
                    else if (row.status == 2)
                        content += '<span class="badge badge-primary">صحیح</span>';
                    else
                        content += '<span class="badge badge-danger">غلط</span>';

                    return content;
                }
            },
            {
                'name': 'hasDescription',
                'autoWidth': true,
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content = '';

                    if (row.hasDescription)
                        content += '<span class="badge badge-primary">بله</span>';
                    else
                        content += '<span class="badge badge-danger">خیر</span>';

                    return content;
                }
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/answer/details/${row.id}">جزئیات</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    let attemptsDataTableOptions = {
        'ajax': {
            'url': '/admin/participant/GetAttempts',
            'type': 'post',
            'datatype': 'json',
            'data': function (params) {
                params.examId = $('#exam-id').val();
            },
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'examId',
                'data': 'examId',
                'autoWidth': true,
                'searchable': false,
            },
            {
                'name': 'ipAddress',
                'data': 'ipAddress',
                'autoWidth': true,
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false,
            },
        ]
    };
    let faqsDataTableOptions = {
        'ajax': {
            'url': '/admin/faq/GetFaqs',
            'type': 'post',
            'datatype': 'json',
        },
        'columns': [
            {
                'name': 'id',
                'data': 'id',
                'autoWidth': true,
            },
            {
                'name': 'question',
                'data': 'question',
                'autoWidth': true,
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false,
            },
            {
                'orderable': false,
                'searchable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/faq/update/${row.id}">ویرایش</a>
                            <a class="dropdown-item" href="/admin/faq/delete/${row.id}" data-delete-btn>حذف</a>
                          </div>
                      </div>
                    </div>`;

                    return content;
                }
            }
        ]
    };
    // Table Targets
    let targets = [
        {
            target: $('#exams-datatable'),
            options: examsDataTableOptions,
        },
        {
            target: $('#questions-datatable'),
            options: questionDataTableOptions,
        },
        {
            target: $('#roles-datatable'),
            options: rolesDataTableOptions,
        },
        {
            target: $('#users-datatable'),
            options: usersDataTableOptions,
        },
        {
            target: $('#options-datatable'),
            options: optionsDataTableOptions,
        },
        {
            target: $('#answers-datatable'),
            options: answersDataTableOptions,
        },
        {
            target: $('#attempts-datatable'),
            options: attemptsDataTableOptions,
        },
        {
            target: $('#faqs-datatable'),
            options: faqsDataTableOptions,
        },
    ];

    // Init Tables
    targets.forEach(function (item) {
        if (item.target && item.options) {
            initDataTable(item.target, item.options);
        }
    });
});

// Charts //
// -----------------------------

let Charts = (function () {

    let mode = 'light';

    let fonts = {
        base: 'yekan'
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
    let ExamVisitsChart = (function () {
        let chart;
        let chartTarget = $('#exam-visits-chart');
        let examIdTarget = $('.chart-exam-id');

        function GetChartData() {
            let data;
            $.ajax({
                async: false,
                url: "/admin/exam/getGetExamVisitsChartData",
                type: 'post',
                datatype: 'json',
                data: {
                    ExamId: examIdTarget.val(),
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
                type: 'line',
                data: chartData,
                options: {
                    responsive: true,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value) {
                                    if (value % 1 === 0) {
                                        return value;
                                    }
                                }
                            }
                        }]
                    },
                }
            });
        };

        if (chartTarget.length) {
            Init();
        }
    })();

    let ExamsVisitsChart = (function () {
        let chart;
        let chartTarget = $('#exams-visits-chart');

        function GetChartData() {
            let data;
            $.ajax({
                async: false,
                url: "/admin/exam/getGetExamsVisitsChartData",
                type: 'post',
                datatype: 'json',
                success: function (result) {
                    data = result;
                }
            });
            return data;
        }

        function Init() {
            var chartData = GetChartData();
            chart = new Chart(chartTarget, {
                type: 'line',
                data: chartData,
                options: {
                    responsive: true,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value) {
                                    if (value % 1 === 0) {
                                        return value;
                                    }
                                }
                            }
                        }]
                    },
                }
            });
        };

        if (chartTarget.length) {
            Init();
        }

    })();

    let ExamAnswersChart = (function () {
        let chart;
        let chartTarget = $('#exam-answers-chart');
        let questionIdTarget = $('.chart-question-select');

        function GetChartData() {
            let data;
            $.ajax({
                async: false,
                url: "/admin/exam/getQuestionAnswersChartData",
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
                options: {
                    responsive: true,
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value) {
                                    if (value % 1 === 0) {
                                        return value;
                                    }
                                }
                            }
                        }]
                    },
                }
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
                url: "/admin/exam/getQuestionAnswersAccuracyChartData",
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
                    responsive: true,
                    tooltips: {
                        rtl: true,
                    },
                    legend: {
                        rtl: true,
                        display: true,
                        position: 'bottom',
                        labels: {
                            padding: 40,
                            fontColor: '#fff',
                        },
                    },
                    scales: {
                        yAxes: [{
                            ticks: {
                                beginAtZero: true,
                                callback: function (value) {
                                    if (value % 1 === 0) {
                                        return value;
                                    }
                                }
                            }
                        }]
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
            },
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