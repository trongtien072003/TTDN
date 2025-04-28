(function ($) {
    var _permissions = {
        create: abp.auth.isGranted('Pages.Products.Create'),
        edit: abp.auth.isGranted('Pages.Products.Edit'),
        delete: abp.auth.isGranted('Pages.Products.Delete')
    };
    var _categoriesService = abp.services.app.categories,
        l = abp.localization.getSource('proj_tt'),
        _$createModal = $('#createModal'),

        _$createForm = _$createModal.find('form'),
        _$table = $('#CategoriesTable');

    var _$categoriesTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        scrollCollapse: true,
        scrollY: '50vh',

        listAction: {
            ajaxFunction: _categoriesService.getAllCategories,
            inputFilter: function () {
                var filter = $('#CategoriesSearchForm').serializeFormToObject(true);
                console.log('Dữ liệu gửi đi:', filter);
                return filter;
            }
        },

        buttons: [
            {
                name: 'refresh',
                text: '<i class="fas fa-redo-alt"></i>',
                action: () => _$categoriesTable.draw(false)
            }
        ],

        responsive: {
            details: {
                type: 'column'
            }
        },
        columnDefs: [
            {
                targets: 0,
                className: 'control',
                defaultContent: '',
            },
            {
                targets: 1,
                data: 'nameCategory',
                sortable: false
            },
            {
                targets: 2,
                data: 'creationTime',
                sortable: false,
            },
            {
                targets: 3,
                data: 'lastModificationTime',
                sortable: false,
            },
            {
                targets: 4,
                data: null,
                sortable: false,
                autoWidth: false,
                defaultContent: '',
                visible:!_permissions.edit? false:true,
                render: (data, type, row, meta) => {
                    return [

                        `   <button type="button" class="btn btn-primary edit-category" data-category-id="${row.id}" data-toggle="modal" data-target="#editModal">`,
                        `   <i class="fas fa-edit"></i>`,
                        '   </button>',
                        `   <button type="button" class="btn btn-danger delete-category" data-category-id="${row.id}" data-category-name="${row.nameCategory}">`,
                        `       <i class="fas fa-trash"></i>`,
                        '   </button>'
                    ].join('');
                }
            }
        ]
    });
    _$createForm.validate({
        rules: {
            NameCategory: {
                required: true,
                minlength: 3,
                maxlength: 100
            }
        },
        messages: {
            NameCategory: {
                required: l("Tên sản phẩm không được để trống"),
                minlength: l("Tên ít nhất 3 ký tự"),
                maxlength: l("Tên tối đa 100 ký tự")
            }
        }
    });

    _$createForm.find('.save-button').on('click', (e) => {
        e.preventDefault();
        if (!_$createForm.valid()) {
            return;
        }
        var category = _$createForm.serializeFormToObject();

        abp.ui.setBusy(_$createModal);
        _categoriesService.create(category).done(function () {
            _$createModal.modal('hide');
            _$createForm[0].reset();
            setTimeout(function () {
                abp.message.success(l('SaveSucessFully'), l('Success'));
            }, 1000);
            _$categoriesTable.ajax.reload();

        }).always(function () {
            abp.ui.clearBusy(_$createModal);
        });
    });


    //$(document).on('click','a[data-ta]')

    $(document).on('click', '.edit-category', function (e) {
        var categoryId = $(this).attr('data-category-id');

        e.preventDefault();
        abp.ajax({
            url: abp.appPath + 'Categories/EditModal?categoryId=' + categoryId,
            type: 'POST',
            dataType: 'html',
            success: function (content) {
                $('#editModal div.modal-content').html(content);
            },
            error: function (e) {

            }
        });
    });


    abp.event.on('category.edited', (data) => {
        _$categoriesTable.ajax.reload();
    });


    $(document).on('click', '.delete-category', function () {
        var categoryId = $(this).attr('data-category-id');
        var categoryName = $(this).attr('data-category-name');

        deleteCategory(categoryId, categoryName);


    });

    function deleteCategory(categoryId, categoryName) {
        abp.message.confirm(
            abp.utils.formatString(l('AreYouSureWantToDelete'), categoryName),
            null,
            (isConfirmed) => {
                if (isConfirmed) {
                    _categoriesService.delete(categoryId)
                        .done(() => {
                            abp.message.success(l('SaveSucessFully'), l('Success'));
                            _$categoriesTable.ajax.reload();
                        })
                        .fail((error) => {
                            let message = l("Xóa danh mục thất bại! Có thể vì danh mục đang chứa sản phẩm.");
                            if (error.responseJSON?.error?.message) {
                                message = error.responseJSON.error.message;
                            }
                            abp.message.error(message, l("Lỗi"));
                        });
                }
            }
        );
    }



    $('.btn-search').on('click', (e) => {
        _$categoriesTable.ajax.reload();
    });

    $('.txt-search').on('keypress', (e) => {
        if (e.which == 13) {
            _$categoriesTable.ajax.reload();
            return false;
        }
    });


})(jQuery);
