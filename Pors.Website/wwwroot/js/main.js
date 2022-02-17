
/* ############### create exam ############### */

function readURL(input, output) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $(output).attr('src', e.target.result);
        }

        reader.readAsDataURL(input.files[0]);
    }
}

$('#exam-image-input').change(function () {
    readURL(this, '#exam-image-preview');
});

/* ############### exam list datatable ############### */

$(document).ready(function () {
    let options = {
        'filter': true,  
        'ordering': true,
        'processing': true,    
        'serverSide': true, 
        'orderMulti': false,
        'language': dataTables_persian_language,
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
                    let content = '';
                    content += '<a class="btn btn-sm btn-info ml-3" href="exam/update/' + row.id + '">ویرایش</a>';
                    content += '<a class="btn btn-sm btn-danger" href="exam/delete/' + row.id + '">حذف</a>';

                    return content;
                }
            }
        ]
    };
    var table = $('#exams-datatable').on('init.dt', function () {
        $('div.dataTables_length select').removeClass('custom-select custom-select-sm')
    }).DataTable(options);
});

/* ############### question list datatable ############### */

$(document).ready(function () {
    let options = {
        'filter': true,
        'ordering': true,
        'processing': true,
        'serverSide': true,
        'orderMulti': false,
        'language': dataTables_persian_language,
        'ajax': {
            'url': '/admin/question/GetQuestions',
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
                    let content = '';
                    content += '<a class="btn btn-sm btn-info ml-3" href="question/update/' + row.id + '">ویرایش</a>';
                    content += '<a class="btn btn-sm btn-danger" href="question/delete/' + row.id + '">حذف</a>';

                    return content;
                }
            }
        ]
    };
    var table = $('#questions-datatable').on('init.dt', function () {
        $('div.dataTables_length select').removeClass('custom-select custom-select-sm')
    }).DataTable(options);
});