//(function ($) {
//    const _$modal = $('#createModal');
//    const _$form = _$modal.find('form');

//    function ImagePreview(modalSelector) {
//        const $modal = $(modalSelector);

//        $modal.find('#image').on('change', function () {
//            var reader = new FileReader();
//            reader.onload = function (e) {
//                $modal.find('#imagePreview').attr('src', e.target.result).show();
//            };
//            reader.readAsDataURL(this.files[0]);
//        });

//        $modal.on('hidden.bs.modal', function () {
//            $modal.find('#imagePreview').attr('src', '#').hide();
//            $modal.find('#image').val('');
//        });
//    }

//    ImagePreview('#createModal');

//    // Validate
//    _$form.validate({
//        rules: {
//            Name: {
//                required: true,
//                minlength: 3,
//                maxlength: 100
//            },
//            Price: {
//                required: true,
//                number: true,
//                min: 0,
//                max: 2000000000000
//            },
//            Discount: {
//                number: true,
//                min: 0,
//                max: 100
//            },
//            Stock: {
//                required: true,
//                number: true,
//                min: 1
//            },
//            Description: {
//                maxlength: 1000
//            },
//            ExpiryDate: {
//                required: true,
//                date: true,
//                expiryDateCheck: true
//            },
//            ImageUrl: {
//                required: true,
//                imageExtension: true,
//                filesize: 2 * 1024 * 1024
//            },
//            CategoryId: {
//                required: true
//            }
//        },
//        messages: {
//            Name: {
//                required: "Tên sản phẩm không được để trống"
//            },
//            Price: {
//                required: "Vui lòng nhập giá",
//                max: "Số tiền bạn nhập kh quá 2 tỷ"
//            },
//            Discount: {
//                number: "Giảm giá phải là số"
//            },
//            Stock: {
//                required: "Vui lòng nhập tồn kho",
//                min: "Vui lòng nhập số lượng lớn 0"

//            },
//            ExpiryDate: {
//                required: "Vui lòng chon ngày",
//                date: "Ngày không hợp lệ",
//                expiryDateCheck: "Ngày hết hạn phải từ hôm nay trở đi"
//            },
//            ImageUrl: {
//                required: "Chọn ảnh",
//                imageExtension: "Định dạng JPG/PNG/GIF/BMP",
//                filesize: "Tối đa 2MB"
//            },
//            CategoryId: {
//                required: "Chọn danh mục"
//            }
//        }
//    });

//    // Custom methods
//    $.validator.addMethod('filesize', function (value, element, param) {
//        return this.optional(element) || (element.files[0].size <= param);
//    });

//    $.validator.addMethod("imageExtension", function (value, element) {
//        if (element.files.length === 0) return false;
//        var fileName = element.files[0].name;
//        return /\.(jpe?g|png|gif|bmp|webp)$/i.test(fileName);
//    });

//    $.validator.addMethod("expiryDateCheck", function (value, element) {
//        if (this.optional(element)) return true;
//        const today = new Date();
//        today.setHours(0, 0, 0, 0);
//        const inputDate = new Date(value);
//        inputDate.setHours(0, 0, 0, 0);
//        return inputDate >= today;
//    });

//    _$form.find('.save-button').on('click', (e) => {
//        e.preventDefault();

//        if (!_$form.valid()) return;

//        const formData = new FormData(_$form[0]);
//        if (!formData.get('Discount')) {
//            formData.set('Discount', 0);
//        }

//        abp.ui.setBusy(_$modal);

//        $.ajax({
//            url: abp.appPath + 'Product/Create',
//            type: 'POST',
//            data: formData,
//            processData: false,
//            contentType: false,
//            success: function () {
//                _$modal.modal('hide');
//                _$form[0].reset();
//                abp.message.success('Thêm sản phẩm thành công', 'Thành công');
//                $('#ProductsTable').DataTable().ajax.reload();
//            },
//            error: function (err) {
//                abp.notify.error("Thêm sản phẩm thất bại!");
//                console.error(err);
//            },
//            complete: function () {
//                abp.ui.clearBusy(_$modal);
//            }
//        });
//    });

//})(jQuery);
