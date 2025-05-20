let currentPage = 1;
let currentKeyword = '';

const loadProducts = (keyword = '', page = 1) => {
    const pageSize = 9;

    abp.services.app.product.getProductPaged({
        skipCount: (page - 1) * pageSize,
        maxResultCount: pageSize,
        keyword: keyword
    }).done(function (result) {
        const $container = $('#product-list');
        $container.empty().addClass('row');

        $.each(result.items, function (i, product) {
            const originalPrice = Number(product.price);
            const discount = Number(product.discount);
            const finalPrice = discount > 0 ? originalPrice * (1 - discount / 100) : originalPrice;

            const priceHTML = discount > 0
                ? `<p>
                    <span style="text-decoration: line-through; color: #9ca3af;">${originalPrice.toLocaleString()}₫</span> 
                    <span style="color: #ef4444; font-weight: bold;">${finalPrice.toLocaleString()}₫</span> 
                    <span style="color:#10b981;">-${discount}%</span>
                   </p>`
                : `<p style="color: #ef4444; font-weight: bold;">${originalPrice.toLocaleString()}₫</p>`;

            const badgeHTML = product.isBestseller
                ? `<div class="badge bg-success position-absolute top-0 start-0 m-2">Bestseller</div>`
                : product.isNew
                    ? `<div class="badge bg-primary position-absolute top-0 start-0 m-2">New</div>`
                    : discount > 0
                        ? `<div class="badge bg-danger position-absolute top-0 start-0 m-2">-${discount}%</div>`
                        : '';

            const $card = $(`
                <div class="col-md-4 mb-4">
                    <div class="card h-100 position-relative">
                        ${badgeHTML}
                        <div class="card-img-wrapper d-flex justify-content-center align-items-center" style="height: 150px; background-color: #f9fafb;">
                            <img src="${product.imageUrl }"
                                 alt="${product.name}"
                                 style="max-height: 100%; max-width: 100%; object-fit: contain;">
                        </div>
                        <div class="card-body">
                            <h5 class="card-title">${product.name}</h5>
                            <p class="card-text text-muted">${product.description || ''}</p>
                            <div class="text-warning mb-2">★★★★★ <span class="text-secondary small">(${product.reviewCount || 0})</span></div>
                            ${priceHTML}
                           <a href="/Home/Detail/${product.id}" 
   class="view-more text-center d-block mt-2 text-decoration-none text-primary" 
   style="display: none;">Xem thêm</a>
                        </div>
                    </div>
                </div>
            `);

            $card.hover(
                function () {
                    $(this).find('.view-more').show();
                },
                function () {
                    $(this).find('.view-more').hide();
                }
            );

            $container.append($card);
        });

        renderPagination(result.totalCount, page, pageSize);
    });
};

const renderPagination = (totalCount, currentPage, pageSize) => {
    const totalPages = Math.ceil(totalCount / pageSize);
    const $pagination = $("#pagination");
    $pagination.empty();

    if (totalPages <= 1) return;

    const createPageItem = (label, targetPage, isActive = false, isDisabled = false) => {
        const $li = $('<li>').addClass('page-item').toggleClass('active', isActive).toggleClass('disabled', isDisabled);
        const $a = $('<a>').addClass('page-link').attr('href', '#').text(label);
        $a.on('click', function (e) {
            e.preventDefault();
            if (!isDisabled && targetPage !== currentPage) {
                currentPage = targetPage;
                loadProducts(currentKeyword, currentPage);
            }
        });
        return $li.append($a);
    };

    const $ul = $('<ul>').addClass('pagination justify-content-center');

    if (currentPage > 1) {
        $ul.append(createPageItem("Trước", currentPage - 1));
    }

    const maxVisible = 5;
    let start = Math.max(currentPage - Math.floor(maxVisible / 2), 1);
    let end = Math.min(start + maxVisible - 1, totalPages);
    if (end - start < maxVisible - 1) {
        start = Math.max(end - maxVisible + 1, 1);
    }

    for (let i = start; i <= end; i++) {
        $ul.append(createPageItem(i, i, i === currentPage));
    }

    if (currentPage < totalPages) {
        $ul.append(createPageItem("Sau", currentPage + 1));
    }

    $pagination.append($('<nav class="mt-4">').append($ul));
};

$(document).ready(function () {
    const $searchForm = $(".search-box");
    const $searchInput = $("#searchBox");

    $searchForm.on("submit", function (e) {
        e.preventDefault();
        currentKeyword = $searchInput.val().trim();
        currentPage = 1;
        loadProducts(currentKeyword, currentPage);
    });

    loadProducts(); // Load trang đầu tiên
});
