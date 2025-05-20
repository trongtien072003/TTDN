$(document).ready(function () {
    const $quantityInput = $('#quantity');
    const $increaseBtn = $('#increase');
    const $decreaseBtn = $('#decrease');
    const $addToCartBtn = $('.add-to-cart-btn');
    const $priceDisplay = $('#total-price');
    const unitPrice = parseFloat($priceDisplay.data('unit-price')) || 0;

    function updateTotalPrice() {
        const quantity = parseInt($quantityInput.val()) || 1;
        const total = unitPrice * quantity;
        $priceDisplay.text(total.toLocaleString('vi-VN', { style: 'currency', currency: 'VND' }));
    }

    // Tăng số lượng
    $increaseBtn.on('click', function () {
        let current = parseInt($quantityInput.val()) || 1;
        const max = parseInt($quantityInput.attr('max')) || 1000;
        if (current < max) {
            $quantityInput.val(current + 1);
            updateTotalPrice();
        }
    });

    // Giảm số lượng
    $decreaseBtn.on('click', function () {
        let current = parseInt($quantityInput.val()) || 1;
        const min = parseInt($quantityInput.attr('min')) || 1;
        if (current > min) {
            $quantityInput.val(current - 1);
            updateTotalPrice();
        }
    });

    // Thay đổi số lượng thủ công
    $quantityInput.on('input', function () {
        updateTotalPrice();
    });

    // Thêm vào giỏ hàng
    $addToCartBtn.on('click', function () {
        const productId = $(this).data('product-id');
        const quantity = parseInt($quantityInput.val());

        if (!productId || isNaN(quantity) || quantity < 1) {
            abp.message.warn("Sản phẩm hoặc số lượng không hợp lệ");
            return;
        }

        abp.services.app.cart.addToCart({
            productId: parseInt(productId),
            quantity: quantity
        }).done(function () {
            abp.notify.success("Đã thêm vào giỏ hàng!");
        }).fail(function (error) {
            const message = error?.responseJSON?.error?.message || "Vui lòng thử lại sau.";
            abp.message.error(message, "Thêm vào giỏ hàng thất bại");
        });
    });

    // Khởi tạo giá khi load trang
    updateTotalPrice();
});
