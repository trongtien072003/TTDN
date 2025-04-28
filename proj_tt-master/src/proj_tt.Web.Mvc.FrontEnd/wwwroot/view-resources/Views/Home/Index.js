document.addEventListener("DOMContentLoaded", function () {
    const productCards = document.querySelectorAll(".product-card");

    productCards.forEach(card => {
        card.addEventListener("click", function () {
            const productId = card.getAttribute("data-id");
            if (!productId) return;

            fetch(`/Home/ModalDetail/${productId}`)
                .then(res => res.text())
                .then(html => {
                    const modalContent = document.querySelector("#productDetailModal .modal-content");
                    modalContent.innerHTML = html;

                    const modal = new bootstrap.Modal(document.getElementById("productDetailModal"));
                    modal.show();
                })
                .catch(err => {
                    console.error("Lỗi khi tải chi tiết sản phẩm:", err);
                    alert("Không thể hiển thị thông tin sản phẩm.");
                });
        });
    });
});