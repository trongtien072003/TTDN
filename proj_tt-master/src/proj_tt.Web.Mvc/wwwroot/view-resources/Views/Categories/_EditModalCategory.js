(function ($) {
    var _categoriesService = abp.services.app.categories,
        l = abp.localization.getSource('proj_tt'),
        _$editModal = $('#editModal'),
        _$editForm = _$editModal.find('form');

    // Gán validate cho đúng form: _$editForm
    _$editForm.validate({
        rules: {
            NameCategory: {
                required: true,
                minlength: 3,
                maxlength: 100
            }
        },
        messages: {
            NameCategory: {
                required: l("Tên danh mục không được để trống"),
                minlength: l("Tên ít nhất 3 ký tự"),
                maxlength: l("Tên tối đa 100 ký tự")
            }
        }
    });

    function save() {
        // Sửa ở đây: validate đúng form _$editForm
        if (!_$editForm.valid()) {
            return;
        }

        var categories = _$editForm.serializeFormToObject();
        console.log('Dữ liệu categories gửi đi:', categories);

        abp.ui.setBusy(_$editForm);
        _categoriesService.update(categories).done(function () {
            _$editModal.modal('hide');
            setTimeout(function () {
                abp.message.success(l('SaveSucessFully'), l('Success'));
            }, 1000);
            abp.event.trigger('category.edited', categories);
        }).always(function () {
            abp.ui.clearBusy(_$editForm);
        });
    }

    _$editForm.closest('div.modal-content').find(".save-button").click(function (e) {
        e.preventDefault();
        save();
    });

})(jQuery);
