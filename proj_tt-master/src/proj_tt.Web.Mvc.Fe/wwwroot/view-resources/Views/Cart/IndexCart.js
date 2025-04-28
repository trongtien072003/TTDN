(function ($) {
    var _cartService = abp.services.app.cart;

    function updateCartTotal() {
        let total = 0;

        $('.cart-item').each(function () {
            const quantity = parseInt($(this).find('.quantity-input').val());
            const unitPrice = parseFloat($(this).find('.total-price').data('unit-price'));

            if (!isNaN(quantity) && !isNaN(unitPrice)) {
                total += quantity * unitPrice;
            }
        });

        $('.cart-grand-total').text(`$${total.toFixed(2)}`);
    }

    function updateCartItem(productId, quantity, unitPrice, $totalElement) {
        _cartService.updateCartItem({
            productId: parseInt(productId),
            quantity: parseInt(quantity)
        }).done(function () {
            const total = unitPrice * quantity;
            $totalElement.text(`$${total.toFixed(2)}`);
            updateCartTotal();
        }).fail(function (error) {
            const msg = error?.responseJSON?.error?.message || "Không thể cập nhật số lượng.";
            abp.message.error(msg);
        });
    }

    $('.decrease-btn').on('click', function () {
        const productId = $(this).data('productId');
        const $input = $(`.quantity-input[data-product-id="${productId}"]`);
        let quantity = parseInt($input.val());
        if (quantity > 1) {
            quantity--;
            $input.val(quantity);
            const $totalEl = $(`#total-price-${productId}`);
            const unitPrice = parseFloat($totalEl.data('unit-price'));
            updateCartItem(productId, quantity, unitPrice, $totalEl);
        }
    });

    $('.increase-btn').on('click', function () {
        const productId = $(this).data('productId');
        const $input = $(`.quantity-input[data-product-id="${productId}"]`);
        let quantity = parseInt($input.val());
        quantity++;
        $input.val(quantity);
        const $totalEl = $(`#total-price-${productId}`);
        const unitPrice = parseFloat($totalEl.data('unit-price'));
        updateCartItem(productId, quantity, unitPrice, $totalEl);
    });

})(jQuery);
