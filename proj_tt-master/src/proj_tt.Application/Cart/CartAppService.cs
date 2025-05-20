using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using proj_tt.Cart.Dto;
using proj_tt.Products;

namespace proj_tt.Cart
{
    [Authorize]
    public class CartAppService : ApplicationService, ICartAppService
    {
        private readonly IRepository<Carts, long> _cartRepository;
        private readonly IRepository<CartItem, long> _cartItemRepository;
        private readonly IRepository<Product, int> _productRepository;

        public CartAppService(
            IRepository<Carts, long> cartRepository,
            IRepository<CartItem, long> cartItemRepository,
            IRepository<Product, int> productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        }

        public async Task<List<CartItemDto>> GetCartItemsByUserIdAsync()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart == null)
                return new List<CartItemDto>();

            var items = await _cartItemRepository.GetAllListAsync(x => x.CartId == cart.Id);
            return items.Select(MapCartItemDto).ToList();
        }

        public async Task AddToCartAsync(AddToCartInput input)
        {
            if (input.Quantity <= 0)
                throw new UserFriendlyException("Số lượng phải lớn hơn 0.");

            var userId = GetCurrentUserId();
            var cartId = await EnsureCartExistsAndGetId(userId);

            var product = await _productRepository.FirstOrDefaultAsync(p => p.Id == input.ProductId);
            if (product == null)
                throw new UserFriendlyException("Sản phẩm không tồn tại.");

            var unitPrice = product.Price; // ✅ lấy từ DB

            var existingItem = await _cartItemRepository.FirstOrDefaultAsync(
                x => x.CartId == cartId && x.ProductId == product.Id);

            if (existingItem != null)
            {
                existingItem.Quantity += input.Quantity;
                existingItem.UnitPrice = unitPrice; 
                existingItem.TotalPrice = existingItem.Quantity * unitPrice;
                existingItem.ProductName = product.Name;
              
                await _cartItemRepository.UpdateAsync(existingItem);
            }
            else
            {
                var newItem = new CartItem
                {
                    CartId = cartId,
                    ProductId = product.Id,
                    ProductName = product.Name,
                    Quantity = input.Quantity,
                    UnitPrice = unitPrice,
                    ProductImage = product.ImageUrl,
                    TotalPrice = input.Quantity * unitPrice
                };
                await _cartItemRepository.InsertAsync(newItem);
            }
        }


        public async Task UpdateCartItemQuantityAsync(UpdateCartItemInput input)
        {
            if (input.Quantity < 0)
                throw new UserFriendlyException("Số lượng không hợp lệ.");

            var item = await _cartItemRepository.FirstOrDefaultAsync(x => x.Id == input.CartItemId);
            if (item == null)
                throw new UserFriendlyException("Không tìm thấy sản phẩm trong giỏ hàng.");

            if (input.Quantity == 0)
            {
                await _cartItemRepository.DeleteAsync(item);
            }
            else
            {
                item.Quantity = input.Quantity;
                item.TotalPrice = input.Quantity * item.UnitPrice;
                await _cartItemRepository.UpdateAsync(item);
            }
        }

        public async Task RemoveCartItemAsync(RemoveFromCartInput input)
        {
            var item = await _cartItemRepository.FirstOrDefaultAsync(x => x.Id == input.CartItemId);
            if (item != null)
            {
                await _cartItemRepository.DeleteAsync(item);
            }
        }

        public async Task ClearCartAsync()
        {
            var userId = GetCurrentUserId();
            var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null)
            {
                await _cartItemRepository.DeleteAsync(x => x.CartId == cart.Id);
                await _cartRepository.DeleteAsync(cart.Id);
            }
        }

        private long GetCurrentUserId()
        {
            return AbpSession.UserId ?? throw new AbpAuthorizationException("Bạn cần đăng nhập.");
        }

        private async Task<long> EnsureCartExistsAndGetId(long userId)
        {
            var cart = await _cartRepository.FirstOrDefaultAsync(c => c.UserId == userId);
            if (cart != null) return cart.Id;

            var newCart = new Carts { UserId = userId };
            return await _cartRepository.InsertAndGetIdAsync(newCart);
        }

        private CartItemDto MapCartItemDto(CartItem x)
        {
            return new CartItemDto
            {
                Id = x.Id,
                CartId = x.CartId,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                Quantity = x.Quantity,
                UnitPrice = x.UnitPrice,
                TotalPrice = x.TotalPrice
            };
        }
    }
}
