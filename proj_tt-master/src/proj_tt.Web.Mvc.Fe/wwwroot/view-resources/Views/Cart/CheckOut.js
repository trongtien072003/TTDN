var orderItems = []; 

function renderOrderItems() {
    var $container = $('#orderItemsList');
    $container.empty();
    var subtotal = 0;

    $.each(orderItems, function (index, item) {
        var lineTotal = item.quantity * item.price;
        subtotal += lineTotal;

        var itemHtml = `
                <div class="d-flex justify-content-between mb-2">
                    <div>
                        <strong>${item.productName}</strong><br />
                        $${item.price.toFixed(2)} × ${item.quantity}
                    </div>
                    <div>
                        $${lineTotal.toFixed(2)}
                    </div>
                </div>
            `;
        $container.append(itemHtml);
    });

    var tax = subtotal * 0.08;
    var shipping = 5.99;
    var total = subtotal + tax + shipping;

    $('#subtotalText').text(`$${subtotal.toFixed(2)}`);
    $('#taxText').text(`$${tax.toFixed(2)}`);
    $('#totalText').text(`$${total.toFixed(2)}`);
}

$('#placeOrderBtn').on('click', function () {
    var data = {
        name: $('input[name="Name"]').val(),
        phoneNumber: $('input[name="PhoneNumber"]').val(),
        email: $('input[name="Email"]').val(),
        address: $('textarea[name="Address"]').val(),
        note: $('textarea[name="Note"]').val(),
        items: orderItems
    };

    abp.ajax({
        url: abp.appPath + 'api/services/app/UserOrder/CreateOrderFromCartAsync',
        type: 'POST',
        data: JSON.stringify(data)
    }).done(function () {
        abp.message.success("Đặt hàng thành công!", "Thông báo");
        window.location.href = '/Order/MyOrders';
    });
});

// DEMO - Bạn cần thay bằng dữ liệu thật từ CartAppService

renderOrderItems();