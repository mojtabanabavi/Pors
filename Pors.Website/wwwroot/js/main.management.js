/* ############### navbar collapse ############### */

$('.nav-link[data-toggle="collapse"].active').each(function () {
    var $this = $(this);
    var $target = $(`.collapse${$this.attr('href')}`);
    $target.collapse('show');
});

/* ############### default image ############### */

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

/* ############### sweet alerts ############### */

// delete alert
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

/* ############### image previews ############### */

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

/* ############### question options ############### */

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

/* ############### datatables ############### */

function initDataTable(target, options) {

    let dataTablesPersianLanguage = {
        "emptyTable": "هیچ داده‌ای در جدول وجود ندارد",
        "info": "نمایش _START_ تا _END_ از _TOTAL_ ردیف",
        "infoEmpty": "نمایش 0 تا 0 از 0 ردیف",
        "infoFiltered": "(فیلتر شده از _MAX_ ردیف)",
        "infoThousands": ",",
        "lengthMenu": "نمایش _MENU_ ردیف",
        "processing": "در حال پردازش...",
        "search": "جستجو:",
        "zeroRecords": "رکوردی با این مشخصات پیدا نشد",
        "paginate": {
            "next": "بعدی",
            "previous": "قبلی",
            "first": "ابتدا",
            "last": "انتها"
        },
        "aria": {
            "sortAscending": ": فعال سازی نمایش به صورت صعودی",
            "sortDescending": ": فعال سازی نمایش به صورت نزولی"
        },
        "autoFill": {
            "cancel": "انصراف",
            "fill": "پر کردن همه سلول ها با ساختار سیستم",
            "fillHorizontal": "پر کردن سلول به صورت افقی",
            "fillVertical": "پرکردن سلول به صورت عمودی"
        },
        "buttons": {
            "collection": "مجموعه",
            "colvis": "قابلیت نمایش ستون",
            "colvisRestore": "بازنشانی قابلیت نمایش",
            "copy": "کپی",
            "copySuccess": {
                "1": "یک ردیف داخل حافظه کپی شد",
                "_": "%ds ردیف داخل حافظه کپی شد"
            },
            "copyTitle": "کپی در حافظه",
            "excel": "اکسل",
            "pageLength": {
                "-1": "نمایش همه ردیف‌ها",
                "_": "نمایش %d ردیف"
            },
            "print": "چاپ",
            "copyKeys": "برای کپی داده جدول در حافظه سیستم کلید های ctrl یا ⌘ + C را فشار دهید",
            "csv": "فایل CSV",
            "pdf": "فایل PDF",
            "renameState": "تغییر نام",
            "updateState": "به روز رسانی"
        },
        "searchBuilder": {
            "add": "افزودن شرط",
            "button": {
                "0": "جستجو ساز",
                "_": "جستجوساز (%d)"
            },
            "clearAll": "خالی کردن همه",
            "condition": "شرط",
            "conditions": {
                "date": {
                    "after": "بعد از",
                    "before": "بعد از",
                    "between": "میان",
                    "empty": "خالی",
                    "equals": "برابر",
                    "not": "نباشد",
                    "notBetween": "میان نباشد",
                    "notEmpty": "خالی نباشد"
                },
                "number": {
                    "between": "میان",
                    "empty": "خالی",
                    "equals": "برابر",
                    "gt": "بزرگتر از",
                    "gte": "برابر یا بزرگتر از",
                    "lt": "کمتر از",
                    "lte": "برابر یا کمتر از",
                    "not": "نباشد",
                    "notBetween": "میان نباشد",
                    "notEmpty": "خالی نباشد"
                },
                "string": {
                    "contains": "حاوی",
                    "empty": "خالی",
                    "endsWith": "به پایان می رسد با",
                    "equals": "برابر",
                    "not": "نباشد",
                    "notEmpty": "خالی نباشد",
                    "startsWith": "شروع  شود با",
                    "notContains": "نباشد حاوی",
                    "notEnds": "پایان نیابد با",
                    "notStarts": "شروع نشود با"
                },
                "array": {
                    "equals": "برابر",
                    "empty": "خالی",
                    "contains": "حاوی",
                    "not": "نباشد",
                    "notEmpty": "خالی نباشد",
                    "without": "بدون"
                }
            },
            "data": "اطلاعات",
            "logicAnd": "و",
            "logicOr": "یا",
            "title": {
                "0": "جستجو ساز",
                "_": "جستجوساز (%d)"
            },
            "value": "مقدار",
            "deleteTitle": "حذف شرط فیلتر",
            "leftTitle": "شرط بیرونی",
            "rightTitle": "شرط داخلی"
        },
        "select": {
            "cells": {
                "1": "1 سلول انتخاب شد",
                "_": "%d سلول انتخاب شد"
            },
            "columns": {
                "1": "یک ستون انتخاب شد",
                "_": "%d ستون انتخاب شد"
            },
            "rows": {
                "1": "1ردیف انتخاب شد",
                "_": "%d  انتخاب شد"
            }
        },
        "thousands": ",",
        "searchPanes": {
            "clearMessage": "همه را پاک کن",
            "collapse": {
                "0": "صفحه جستجو",
                "_": "صفحه جستجو (٪ d)"
            },
            "count": "{total}",
            "countFiltered": "{shown} ({total})",
            "emptyPanes": "صفحه جستجو وجود ندارد",
            "loadMessage": "در حال بارگیری صفحات جستجو ...",
            "title": "فیلترهای فعال - %d",
            "showMessage": "نمایش همه"
        },
        "loadingRecords": "در حال بارگذاری...",
        "datetime": {
            "previous": "قبلی",
            "next": "بعدی",
            "hours": "ساعت",
            "minutes": "دقیقه",
            "seconds": "ثانیه",
            "amPm": [
                "صبح",
                "عصر"
            ],
            "months": {
                "0": "ژانویه",
                "1": "فوریه",
                "10": "نوامبر",
                "2": "مارچ",
                "4": "می",
                "6": "جولای",
                "8": "سپتامبر",
                "11": "دسامبر",
                "3": "آوریل",
                "5": "جون",
                "7": "آست",
                "9": "اکتبر"
            },
            "unknown": "-",
            "weekdays": [
                "یکشنبه",
                "دوشنبه",
                "سه‌شنبه",
                "چهارشنبه",
                "پنجشنبه",
                "جمعه",
                "شنبه"
            ]
        },
        "editor": {
            "close": "بستن",
            "create": {
                "button": "جدید",
                "title": "ثبت جدید",
                "submit": "ایجــاد"
            },
            "edit": {
                "button": "ویرایش",
                "title": "ویرایش",
                "submit": "به‌روزرسانی"
            },
            "remove": {
                "button": "حذف",
                "title": "حذف",
                "submit": "حذف",
                "confirm": {
                    "_": "آیا از حذف %d خط اطمینان دارید؟",
                    "1": "آیا از حذف یک خط اطمینان دارید؟"
                }
            },
            "multi": {
                "restore": "واگرد",
                "noMulti": "این ورودی را می توان به صورت جداگانه ویرایش کرد، اما نه بخشی از یک گروه"
            }
        },
        "decimal": ".",
        "stateRestore": {
            "creationModal": {
                "button": "ایجاد",
                "columns": {
                    "search": "جستجوی ستون",
                    "visible": "وضعیت نمایش ستون"
                },
                "name": "نام:",
                "order": "مرتب سازی",
                "paging": "صفحه بندی",
                "search": "جستجو",
                "select": "انتخاب",
                "title": "ایجاد وضعیت جدید",
                "toggleLabel": "شامل:"
            },
            "emptyError": "نام نمیتواند خالی باشد.",
            "removeConfirm": "آیا از حذف %s مطمئنید؟",
            "removeJoiner": "و",
            "removeSubmit": "حذف",
            "renameButton": "تغییر نام",
            "renameLabel": "نام جدید برای $s :"
        }
    }

    options.filter = true;
    options.ordering = true;
    options.processing = true;
    options.serverSide = true;
    options.orderMulti = false;
    options.language = dataTablesPersianLanguage;

    let table = $(target).on('init.dt', function () {
        $('div.dataTables_length select').removeClass('custom-select custom-select-sm')
    }).DataTable(options);

    return table;
}

// exams dataTable
let examsDataTableTarget = $('#exams-datatable');
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
                            <a class="dropdown-item" href="/admin/question/index/${row.id}">لیست ‌سوالات</a>
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

if (examsDataTableTarget.length) {
    initDataTable(examsDataTableTarget, examsDataTableOptions);
}

// questions dataTable

let questionsDataTableTarget = $('#questions-datatable');
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
                            <a class="dropdown-item" href="/admin/option/create/${row.id}">افزودن گزینه</a>
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

if (questionsDataTableTarget.length) {
    initDataTable(questionsDataTableTarget, questionDataTableOptions);
}

// roles dataTable
let rolesDataTableTarget = $('#roles-datatable');
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
            'orderable': false,
            'render': function (data, type, row) {
                let content =
                    `<div class="btn-group ">
                          <button type="button" class="btn btn-sm btn-icon-only text-light dropdown-toggle" data-toggle="dropdown">
                            <i class="fas fa-ellipsis-v"></i>
                          </button>
                          <div class="dropdown-menu">
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

if (rolesDataTableTarget.length) {
    initDataTable(rolesDataTableTarget, rolesDataTableOptions);
}

// users dataTable
let usersDataTableTarget = $('#users-datatable');
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

if (usersDataTableTarget.length) {
    initDataTable(usersDataTableTarget, usersDataTableOptions);
}

// options dataTable
let optionsDataTableTarget = $('#options-datatable');
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

if (optionsDataTableTarget.length) {
    initDataTable(optionsDataTableTarget, optionsDataTableOptions);
}
