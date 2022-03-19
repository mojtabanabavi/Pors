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

// Data Table //
// -----------------------------
$(function () {
    function initDataTable(target, options) {

        if (typeof DataTable !== 'undefined') {
            options.filter = true;
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
                'searchable': false
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
                'searchable': true
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
                'searchable': false
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
                'render': function (data, type, row) {
                    let content =
                        `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
                            <a class="dropdown-item" href="/admin/exam/report/${row.id}">مشاهده‌ی گزارش</a>
                            <a class="dropdown-item" href="/admin/question/index/${row.id}">لیست ‌سوالات</a>
                            <a class="dropdown-item" href="/admin/attempt/index/${row.id}">لیست ‌شرکت‌کنندگان</a>
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
                'searchable': false
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
                'searchable': true
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
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
                'searchable': false
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
                'searchable': false
            },
            {
                'name': 'description',
                'data': 'description',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': false
            },
            {
                'orderable': false,
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
                'searchable': false
            },
            {
                'name': 'firstName',
                'data': 'firstName',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'lastName',
                'data': 'lastName',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'email',
                'data': 'email',
                'autoWidth': true,
                'searchable': true
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
                'searchable': false
            },
            {
                'name': 'questionId',
                'data': 'questionId',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'title',
                'data': 'title',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'image',
                'data': 'image',
                'autoWidth': true,
                'searchable': true,
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
                'searchable': false
            },
            {
                'name': 'attemptId',
                'data': 'attemptId',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'optionId',
                'data': 'optionId',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'isCorrect',
                'autoWidth': true,
                'searchable': false,
                'render': function (data, type, row) {
                    let content = '';

                    if (row.isActive)
                        content += '<span class="badge badge-primary">بله</span>';
                    else
                        content += '<span class="badge badge-danger">خیر</span>';

                    return content;
                }
            },
            {
                'name': 'hasDescription',
                'autoWidth': true,
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
            'url': '/admin/attempt/GetAttempts',
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
                'searchable': false
            },
            {
                'name': 'examId',
                'data': 'examId',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'ipAddress',
                'data': 'ipAddress',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': true
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
                'searchable': false
            },
            {
                'name': 'question',
                'data': 'question',
                'autoWidth': true,
                'searchable': true
            },
            {
                'name': 'createdAt',
                'data': 'createdAt',
                'autoWidth': true,
                'searchable': true
            },
            {
                'orderable': false,
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

$(function () {

    if (typeof Chart != 'undefined') {
        Chart.defaults.global.defaultFontFamily = 'yekan';
    }

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
                data: {
                    labels: chartData.labels,
                    datasets: [{
                        label: 'answers',
                        data: chartData.dataSet,
                    }]
                },
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
            var chartData = GetChartData();
            chart.data.labels = chartData.labels;
            chart.data.datasets[0].data = chartData.dataSet;
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
                    datasets: [{
                        label: 'answers',
                        data: chartData.dataSet,
                    }]
                },
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
            var chartData = GetChartData();
            chart.data.labels = chartData.labels;
            chart.data.datasets[0].data = chartData.dataSet;
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