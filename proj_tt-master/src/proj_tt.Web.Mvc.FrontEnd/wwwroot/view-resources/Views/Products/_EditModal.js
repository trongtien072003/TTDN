//(function ($) {
//    var _productsService = abp.services.app.product,
//        l = abp.localization.getSource('proj_tt'),
//        _$modal = $('#editModal'),
//        _$form = _$modal.find('form');

//    function ImagePreview(input, previewImgSelector) {
//        const file = input.files && input.files[0];
//        if (file) {
//            const reader = new FileReader();
//            reader.onload = function (e) {
//                $(previewImgSelector).attr('src', e.target.result);
//            };
//            reader.readAsDataURL(file);
//            console.log(" reader",reader)
//        }
//    }

//    //Export global (nếu gọi từ nơi khác)
//    window.ImagePreview = ImagePreview;

//    // Gắn sự kiện cho input ảnh
//    $('#image').on('change', function () {
//        ImagePreview(this, '#imagePreview');
//    });
//    function save() {
//        if (!_$form.valid()) {
//            return; // không submit nếu không hợp lệ
//        }

//        var formElement = _$form[0];
//        var formData = new FormData(formElement);

//        console.log("Giá trị categoryId:", formData.get("CategoryId"));

//        if (!formData.get('Discount')) {
//            formData.set('Discount', 0);
//        }
//        abp.ui.setBusy(_$form);

//        $.ajax({
//            url: abp.appPath + 'Product/Update',
//            type: 'POST',
//            data: formData,
//            processData: false, // Không xử lý dữ liệu
//            contentType: false, // Không đặt content-type mặc định
//            success: function () {
//                _$modal.modal('hide');
//                abp.message.success(l('SavedSuccessfully'), l('Success'));
//                abp.event.trigger('product.edited');
//            },
//            error: function (err) {

//                //abp.notify.error('Lỗi khi cập nhật sản phẩm!');
//                abp.message.error(l('SavedFailed'), l('Fail'));
//                console.error(err);
//            },
//            complete: function () {
//                abp.ui.clearBusy(_$form);
//            }
//        });
//    }



//    _$form.closest('div.modal-content').find(".save-button").click(function (e) {
//        e.preventDefault();
//        save();
//    });

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
//                max: 2000000000000,
              
//            },
//            Discount: {
//                number: true,
//                min: 0,
//                max: 100
//            },
//            Stock: {
//                required: true,
//                number: true,
//                min: 0
//            },
//            Description: {
//                maxlength: 1000
//            },
//            ExpiryDate: {
//                date: true,
//                expiryDateCheck: true
//            },
//            ImageUrl: {
//                imageExtension: function () {
//                    return $('#image').val() !== '';
//                },
//                filesize: function () {
//                    return $('#image').val() !== '';
//                }
//            },
//            CategoryId: {
//                required: true
//            }
//        },
//        messages: {
//            Name: {
//                required: "Tên sản phẩm không được để trống",
//                minlength: l("PleaseEnterAtLeastNCharacter"),
//                maxlength: l("PleaseEnterNoMoreThanNCharacter")
//            },
//            Price: {
//                required: "Vui lòng nhập giá",
//                number: "Giá phải là số",
//                min: "Giá phải lớn hơn hoặc bằng 0",
//                max: "Số tiền nhập ko quá 2 tỷ"
//            },
//            Discount: {
//                number: "Giảm giá phải là số",
//                min: "Tối thiểu là 0%",
//                max: "Tối đa là 100%"
//            },
//            Stock: {
//                required: "Vui lòng nhập tồn kho",
//                number: "Tồn kho phải là số",
//                min: "Không được âm"
//            },
//            Description: {
//                maxlength: "Mô tả không được vượt quá 1000 ký tự"
//            },
//            ExpiryDate: {

//                date: "Ngày hết hạn không hợp lệ",
//                expiryDateCheck: "Ngày hết hạn phải từ hôm nay trở đi"
//            },
//            ImageUrl: {
//                required: "Vui lòng chọn ảnh sản phẩm",
//                imageExtension: "Chỉ chấp nhận ảnh JPG, PNG, GIF, BMP",
//                filesize: "Dung lượng ảnh tối đa là 2MB"
//            },
//            CategoryId: {
//                required: "Vui lòng chọn danh mục"
//            }
//        }
//    });


//    // Thêm phương thức kiểm tra size ảnh
//    $.validator.addMethod('filesize', function (value, element, param) {
//        return this.optional(element) || (element.files[0].size <= param);
//    }, 'Dung lượng ảnh vượt quá giới hạn');

//    $.validator.addMethod("imageExtension", function (value, element) {
//        if (element.files.length === 0) return false;
//        var fileName = element.files[0].name;
//        return /\.(jpe?g|png|gif|bmp|webp)$/i.test(fileName);
//    }, "Chỉ chấp nhận ảnh định dạng JPG, PNG, GIF, BMP");



//})(jQuery)