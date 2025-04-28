document.addEventListener('DOMContentLoaded', function () {
    const quantityInput = document.getElementById('quantity');
    const increaseBtn = document.getElementById('increase');
    const decreaseBtn = document.getElementById('decrease');
    const addToCartBtn = document.querySelector('.add-to-cart-btn');

    // Tăng số lượng
    increaseBtn?.addEventListener("click", function () {
        let current = parseInt(quantityInput.value) || 1;
        const max = parseInt(quantityInput.max) || 1000;
        if (current < max) {
            quantityInput.value = current + 1;
        }
    });

    // Giảm số lượng
    decreaseBtn?.addEventListener("click", function () {
        let current = parseInt(quantityInput.value) || 1;
        const min = parseInt(quantityInput.min) || 1;
        if (current > min) {
            quantityInput.value = current - 1;
        }
    });

    // Gửi API Thêm vào giỏ
    addToCartBtn?.addEventListener('click', function () {
        const productId = this.dataset.productId;
        const quantity = parseInt(quantityInput.value);

        if (!productId || isNaN(quantity) || quantity < 1) {
            abp.message.warn("Sản phẩm hoặc số lượng không hợp lệ");
            return;
        }

        abp.ajax({
            url: abp.appPath + "api/app/cart/add-to-cart",
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify({
                productId: parseInt(productId),
                quantity: quantity
            })
        }).done(function () {
            abp.notify.success("Đã thêm vào giỏ hàng!");
            // TODO: update badge hoặc redirect nếu cần
            // window.location.href = "/Cart"; // nếu muốn chuyển trang luôn
        }).fail(function (error) {
            const message = error?.responseJSON?.error?.message || "Vui lòng không nhập quá số lượng hàng.";
            abp.message.error(message, "Thêm vào giỏ hàng thất bại");
        });
    });
});
