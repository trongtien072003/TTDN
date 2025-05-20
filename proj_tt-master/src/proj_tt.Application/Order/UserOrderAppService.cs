using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Linq.Extensions;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using proj_tt.Cart;
using proj_tt.Order.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Abp.Authorization;


namespace proj_tt.Order
{
    public class UserOrderAppService : ApplicationService, IUserOrderAppService
    {
        private readonly IRepository<Orders, long> _orderRepository;
        private readonly IRepository<OrderItem, long> _orderItemRepository;
        private readonly IRepository<Carts, long> _cartRepository;
        private readonly IRepository<CartItem, long> _cartItemRepository;

        public UserOrderAppService(
            IRepository<Orders, long> orderRepository,
            IRepository<OrderItem, long> orderItemRepository,
            IRepository<Carts, long> cartRepository,
            IRepository<CartItem, long> cartItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
        }
        /// </summary>
        public async Task CreateOrderAsync(CreateOrderInput input)
        {
            var userId = AbpSession.UserId ?? throw new AbpAuthorizationException("Chưa đăng nhập");

            var cart = await _cartRepository.GetAllIncluding(c => c.Items)
                        .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || cart.Items == null || !cart.Items.Any())
                throw new UserFriendlyException("Giỏ hàng trống");

            var order = new Orders
            {
                UserId = userId,
                UserName = input.UserName,
                UserEmail = input.UserEmail,
                PhoneNumber = input.PhoneNumber,
                Address = input.Address,
                Note = input.Note,
                Status = OrderStatus.Pending,
                TotalAmount = 0,
                OrderItems = new List<OrderItem>()
            };

            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    Price = item.UnitPrice,
                    CreationTime = Clock.Now
                };

                order.TotalAmount += orderItem.TotalPrice;
                order.OrderItems.Add(orderItem);
            }

            await _orderRepository.InsertAsync(order);
            await _cartItemRepository.DeleteAsync(i => i.CartId == cart.Id);
            await _cartRepository.DeleteAsync(cart);
        }
        /// <summary>
        /// Lấy danh sách đơn hàng của người dùng hiện tại với hỗ trợ lọc, tìm kiếm và phân trang.
        /// Trả về danh sách đơn hàng kèm các sản phẩm trong đơn.
        /// </summary>
        public async Task<PagedResultDto<OrderDto>> GetMyOrdersPagedAsync(PagedOrderRequestDto input)
        {
            var userId = AbpSession.UserId;
            if (!userId.HasValue)
                throw new AbpAuthorizationException("Bạn chưa đăng nhập.");

            var query = _orderRepository.GetAll()
                .Include(o => o.OrderItems)
                .AsNoTracking()
                .Where(o => o.UserId == userId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Keyword),
                    o => o.UserName.Contains(input.Keyword) || o.OrderItems.Any(i => i.ProductName.Contains(input.Keyword)))
                .WhereIf(input.Status.HasValue, o => o.Status == input.Status)
                .WhereIf(input.MinDate.HasValue, o => o.CreationTime >= input.MinDate.Value)
                .WhereIf(input.MaxDate.HasValue, o => o.CreationTime <= input.MaxDate.Value)
                .WhereIf(input.MinTotal.HasValue, o => o.TotalAmount >= input.MinTotal.Value)
                .WhereIf(input.MaxTotal.HasValue, o => o.TotalAmount <= input.MaxTotal.Value);

            var totalCount = await query.CountAsync();

            var orders = await query
                .OrderByDescending(o => o.CreationTime)
                .PageBy(input)
                .Select(o => new OrderDto
                {
                    Id = o.Id,
                    UserName = o.UserName,
                    UserEmail = o.UserEmail,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status,
                    Note = o.Note,
                    PhoneNumber = o.PhoneNumber,
                    Address = o.Address,
                    OrderItems = o.OrderItems.Select(i => new OrderItemDto
                    {
                        ProductId = i.ProductId,
                        ProductName = i.ProductName,
                        Quantity = i.Quantity,
                        Price = i.Price
                    }).ToList()
                })
                .ToListAsync();

            return new PagedResultDto<OrderDto>(totalCount, orders);
        }

        /// <summary>
        /// Lấy thông tin chi tiết của một đơn hàng cụ thể.
        /// Kiểm tra quyền truy cập của người dùng trước khi trả dữ liệu.
        /// </summary>
        public async Task<OrderDto> GetMyOrderDetailAsync(long orderId)
        {
            var userId = AbpSession.UserId;
            if (!userId.HasValue)
                throw new AbpAuthorizationException("Bạn chưa đăng nhập.");

            var order = await _orderRepository.GetAllIncluding(o => o.OrderItems)
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null)
                throw new UserFriendlyException("Không tìm thấy đơn hàng hoặc bạn không có quyền truy cập.");

            return ObjectMapper.Map<OrderDto>(order);
        }

        /// <summary>
        /// Hủy một đơn hàng nếu trạng thái hiện tại là Pending.
        /// Kiểm tra quyền sở hữu đơn và cập nhật trạng thái + thời gian sửa đổi.
        /// </summary>
        public async Task CancelMyOrderAsync(long orderId)
        {
            var userId = AbpSession.UserId;
            if (!userId.HasValue)
                throw new AbpAuthorizationException("Bạn chưa đăng nhập.");

            var order = await _orderRepository.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
            if (order == null)
                throw new UserFriendlyException("Không tìm thấy đơn hàng.");

            if (order.Status != OrderStatus.Pending)
                throw new UserFriendlyException("Chỉ có thể hủy đơn hàng ở trạng thái Chờ xử lý.");

            order.Status = OrderStatus.Cancelled;
            order.LastModificationTime = Clock.Now;
        }
    }

}


