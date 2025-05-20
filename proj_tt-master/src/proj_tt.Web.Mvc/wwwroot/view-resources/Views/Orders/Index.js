(function ($) {
    const _orderService = abp.services.app.order,
        l = abp.localization.getSource('proj_tt'),
        _$modal = $('#OrderDetailModalContainer'),
        _$table = $('#OrdersTable');

    const _permissions = {
        delete: abp.auth.isGranted('Pages.Orders.Delete'),
        restore: abp.auth.isGranted('Pages.Orders.Restore'),
        view: abp.auth.isGranted('Pages.Orders.View'),
        edit: abp.auth.isGranted('Pages.Orders.Update')
    };

    const _$ordersTable = _$table.DataTable({
        paging: true,
        serverSide: true,
        ordering: true,
        processing: true,
        scrollY: '50vh',
        scrollCollapse: true,
        ajax: function (data, callback) {
            const filter = $('#OrderSearchForm').serializeFormToObject(true);
            const sortCol = data.columns[data.order[0].column].data;
            const sortDir = data.order[0].dir;

            _orderService.getAll($.extend({}, filter, {
                skipCount: data.start,
                maxResultCount: data.length,
                sorting: sortCol + ' ' + sortDir
            })).done(function (result) {
                callback({
                    recordsTotal: result.totalCount,
                    recordsFiltered: result.totalCount,
                    data: result.items
                });
            });
        },
        columns: [
            { data: 'id', title: 'ID' },
            { data: 'userName', title: 'Khách hàng' },
            { data: 'userEmail', title: 'Email' },
            { data: 'phoneNumber', title: 'SĐT' },
            {
                data: 'totalAmount',
                title: 'Tổng tiền',
                render: data => data ? Number(data).toLocaleString('vi-VN') + ' đ' : '0 đ'
            },
            {
                data: 'status',
                title: 'Trạng thái',
                render: d => l(d)
            },
            {
                data: 'creationTime',
                title: 'Ngày tạo',
                render: data => new Date(data).toLocaleString('vi-VN')
            },
            {
                data: 'lastModificationTime',
                title: 'Sửa lần cuối',
                render: data => data ? new Date(data).toLocaleString('vi-VN') : ''
            },
            {
                data: null,
                title: 'Thao tác',
                orderable: false,
                render: (data, type, row) => {
                    const actions = [];

                    if (_permissions.view) {
                        actions.push(`<button class="btn btn-sm btn-info view-order" title="Xem chi tiết" data-id="${row.id}">
                            <i class="fas fa-eye"></i></button>`);
                    }

                    if (_permissions.edit) {
                        actions.push(`<button class="btn btn-sm btn-primary edit-order" title="Cập nhật đơn hàng" data-id="${row.id}">
                            <i class="fas fa-edit"></i></button>`);
                    }

                    if (_permissions.restore && row.isDeleted) {
                        actions.push(`<button class="btn btn-sm btn-secondary restore-order" title="Khôi phục đơn hàng" data-id="${row.id}">
                            <i class="fas fa-undo"></i></button>`);
                    }

                    return actions.join(' ');
                }
            }
        ]
    });

    // View modal
    $(document).on('click', '.view-order', function () {
        const id = $(this).data('id');
        $.get(`/admin/orders/view-modal/${id}`, function (html) {
            $('#OrderDetailModalContainer').html(html);
            $('#OrderDetailModal').modal('show');
        }).fail(() => abp.message.error("Không thể tải chi tiết đơn hàng."));
    });
    //Mở modal edit
    $(document).on('click', '.edit-order', function () {
        const id = $(this).data('id');
        if (!id) return abp.message.error("Không có ID đơn hàng.");

        $.get(`/admin/orders/edit-modal/${id}`, function (html) {
            $('#OrderEditModalContainer').html(html);
            $('#OrderEditModal').modal('show');
        }).fail(() => abp.message.error("Không thể tải form chỉnh sửa đơn hàng."));
    });

    // Restore
    $(document).on('click', '.restore-order', function () {
        const id = $(this).data('id');
        abp.message.confirm("Bạn có chắc muốn khôi phục đơn hàng này?", "Xác nhận khôi phục", function (confirmed) {
            if (confirmed) {
                abp.ajax({
                    url: `/admin/orders/restore/${id}`,
                    type: 'POST'
                }).done(() => {
                    abp.notify.success("Đã khôi phục đơn hàng.");
                    _$ordersTable.ajax.reload();
                }).fail(err => {
                    const msg = err.responseJSON?.error?.message || "Có lỗi xảy ra khi khôi phục.";
                    abp.message.error(msg);
                });
            }
        });
    });

    // Tìm kiếm
    $('#OrderSearchForm').on('submit', function (e) {
        e.preventDefault();
        _$ordersTable.ajax.reload();
    });

    $('.btn-search').on('click', () => _$ordersTable.ajax.reload());

    $('.txt-search').on('keypress', function (e) {
        if (e.which === 13) {
            _$ordersTable.ajax.reload();
            return false;
        }
    });

})(jQuery);
