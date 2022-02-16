
// scripts for create exam page

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
