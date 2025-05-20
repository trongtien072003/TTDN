$(document).on('submit', '#OrderEditModal form', function (e) {
    e.preventDefault();
    var form = $(this);

    $.ajax({
        url: form.attr('action'),
        type: 'POST',
        data: form.serialize(),
        success: function () {
            alert('Cập nhật thành công!');
            location.reload();
        },
        error: function () {
            alert('Cập nhật thất bại. Vui lòng thử lại.');
        }
    });
});
