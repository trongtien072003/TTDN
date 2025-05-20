$(function () {
    // Thiết lập Anti-Forgery nếu cần
    var antiForgeryToken = $('input[name="__RequestVerificationToken"]').val();
    if (antiForgeryToken) {
        $.ajaxSetup({
            headers: { 'RequestVerificationToken': antiForgeryToken }
        });
    }

    function showMessage(message, type) {
        if (window.abp && abp.notify) {
            if (type === 'success') abp.notify.success(message);
            else if (type === 'error') abp.notify.error(message);
            else abp.notify.info(message);
        } else {
            alert(message);
        }
    }

    // 1. Cập nhật số lượng sản phẩm qua nút reload ↻
    $('form[asp-action="UpdateQuantity"]').on('submit', function (e) {
        e.preventDefault();
        const $form = $(this);
        const cartItemId = $form.find('input[name="CartItemId"]').val();
        const quantity = $form.find('input[name="Quantity"]').val();

        $.post('/cart/update-quantity', {
            CartItemId: cartItemId,
            Quantity: quantity
        }).done(function () {
            showMessage('✔️ Cập nhật số lượng thành công.', 'success');
            location.reload();
        }).fail(function () {
            showMessage('❌ Không thể cập nhật số lượng.', 'error');
        });
    });

    // 2. Xóa sản phẩm qua nút ×
    $('form[asp-action="RemoveItem"]').on('submit', function (e) {
        e.preventDefault();
        if (!confirm('Bạn có chắc muốn xóa sản phẩm này?')) return;

        const $form = $(this);
        const cartItemId = $form.find('input[name="CartItemId"]').val();

        $.post('/cart/remove-item', {
            CartItemId: cartItemId
        }).done(function () {
            showMessage('🗑️ Đã xóa sản phẩm.', 'success');
            location.reload();
        }).fail(function () {
            showMessage('❌ Không thể xóa sản phẩm.', 'error');
        });
    });

    // 3. Xóa toàn bộ giỏ hàng
    $('form[asp-action="Clear"]').on('submit', function (e) {
        e.preventDefault();
        if (!confirm('Bạn có chắc muốn xóa toàn bộ giỏ hàng?')) return;

        $.post('/cart/clear').done(function () {
            showMessage('🧹 Đã xóa toàn bộ giỏ hàng.', 'success');
            location.reload();
        }).fail(function () {
            showMessage('❌ Không thể xóa giỏ hàng.', 'error');
        });
    });
});