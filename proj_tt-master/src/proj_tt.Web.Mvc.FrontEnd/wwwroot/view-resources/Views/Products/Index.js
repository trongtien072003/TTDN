//(function ($) {
//    var _productService = abp.services.app.product,
//        l = abp.localization.getSource('proj_tt'),
//        _$modal = $('#createModal'),

//        _$form = _$modal.find('form'),
//        _$table = $('#ProductsTable');

    
//    var _permissions = {
//        create: abp.auth.isGranted('Pages.Products.Create'),
//        edit: abp.auth.isGranted('Pages.Products.Edit'),
//        delete: abp.auth.isGranted('Pages.Products.Delete')
//    };


//    var _$productsTable = _$table.DataTable({
//        paging: true,
//        serverSide: true,
//        ordering: true,
//        pageLength: 15,
//        lengthMenu: [15, 20, 25],
//        scrollCollapse: true,
//        scrollY: '50vh',
//        processing: true,
//        ajax: function (data, callback, settings) {
//            var filter = $('#ProductsSearchForm').serializeFormToObject(true);
//            var filterDate = $('#ProductSearchByExpiry').serializeFormToObject(true);
//            console.log("startDate", filterDate.startDate)
//            console.log("endDate", filterDate.endDate)
//            console.log("filter", filter)
//            var sortColumn = data.columns[data.order[0].column]?.data || 'creationTime'; 
           
//            var sortDirection = data.order[0].dir;

//            _productService.getProductPaged($.extend({}, filter, {
//                skipCount: data.start,
//                maxResultCount: data.length,
//                sorting: sortColumn + ' ' + sortDirection,
//                startDate: filterDate.startDate,
//                endDate: filterDate.endDate,


//            }))
//                .done(function (result) {
//                    console.log("result", result)

//                    callback({
//                        recordsTotal: result.totalCount,
//                        recordsFiltered: result.totalCount,
//                        data: result.items
//                    });
//                });
//        },

//        buttons: [
//            {
//                name: 'refresh',
//                text: '<i class="fas fa-redo-alt"></i>',
//                action: function () {
//                    _$productsTable.ajax.reload(null, true); 
//                },
//            }
//        ],
//        responsive: {
//            details: {
//                type: 'column'
//            }
//        },
//        columnDefs: [
//            {
//                targets: 0,
//                className: 'control',
//                defaultContent: '',
//                orderable: false
//            },
//            { targets: 1, data: 'name', orderable: true },
//            { targets: 2, data: 'price', orderable: true, render: data => data ? Number(data).toLocaleString("vi-VN") + ' VND' : '0' },
//            { targets: 3, data: 'discount', orderable: true },
//            { targets: 4, data: 'imageUrl', orderable: false, render: data => data ? `<img src="${data}" style="width:60px; height:60px; border-radius:8px; object-fit:cover;">` : '' },
//            { targets: 5, data: 'nameCategory', orderable: false },
//            { targets: 6, data: 'description', orderable: false },
//            { targets: 7, data: 'stock', orderable: true },
//            {
//                targets: 8,
//                data: 'expiryDate',
//                orderable: true,
//                render: function (data) {
//                    if (!data) return '';
//                    return new Date(data).toLocaleDateString('vi-VN');
//                }
//            },

//            {
//                targets: 9,
//                data: 'creationTime',
//                render: data => data ? new Date(data).toLocaleString('vi-VN') : ''
//            },
//            {
//                targets: 10,
//                data: 'lastModificationTime',
//                render: data => data ? new Date(data).toLocaleString('vi-VN') : ''
//            },
//            {
//                targets: 11,
//                data: null,
//                orderable: false,
//                visible: !_permissions.edit?false:true,
//                render: (data, type, row) => {
//                      return [
//                        `   <button type="button" class="btn btn-primary edit-product" data-product-id="${row.id}" data-toggle="modal" data-target="#editModal">`,
//                        `   <i class="fas fa-edit"></i>`,
//                        '   </button>',
//                        `   <button type="button" class="btn btn-danger delete-product" data-product-id="${row.id}" data-product-name="${row.name}" data-toggle="modal" data-target="#deleteModal">`,
//                        `       <i class="fas fa-trash"></i>`,
//                        '   </button>'
//                    ].join('');
//                }
//            }
//        ]

//    });
//    $(document).on('click', '.edit-product', function (e) {
//        e.preventDefault();
        
         
        
//        var productId = $(this).attr('data-product-id');
//        console.log('productId ', productId);
//        abp.ajax({
//            url: abp.appPath + 'Product/EditModal?productId=' + productId, 
//            type: 'POST',
//            dataType: 'html',
//            success: function (content) {
//                //console.log('content:', content); 
//                $('#editModal div.modal-content').html(content); // add cái form của editmodal vào index
//                ImagePreview('#editModal');//xem lại
//            },
//            error: function (e) {
                
//            }
//        });
//    });


//    abp.event.on('product.edited', (data) => {
//        _$productsTable.ajax.reload();
//    });

//    $(document).on('click', '.delete-product', function () {
//        var productId = $(this).attr('data-product-id');
//        var productName = $(this).attr('data-product-name');
//        deleteProduct(productId, productName);


//    });

//    function deleteProduct(productId, productName) {
//        abp.message.confirm(          
//            abp.utils.formatString( 
//                l('AreYouSureWantToDelete'),
//                productName),
//            "Xác nhận xóa sản phẩm",
//            (isConfirmed) => {
//                if (isConfirmed) {
//                    _productService.delete(productId).done(() => {
//                        abp.message.success(l('SuccessfullyDeleted'), l('Success'));
//                        _$productsTable.ajax.reload();
//                    });
//                }
//            }
//        );
//    }
//    $('#ProductSearchByExpiry').on('submit', function (e) {
//        e.preventDefault();
//        _$productsTable.ajax.reload();
//    });

    

//    $('.btn-search').on('click', (e) => {
//        _$productsTable.ajax.reload();
//    });

//    $('.txt-search').on('keypress', (e) => {
//        if (e.which == 13) {
//            _$productsTable.ajax.reload();
//            return false;
//        }
//    });


//})(jQuery);
