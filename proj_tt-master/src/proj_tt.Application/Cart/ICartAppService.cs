using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services;
using proj_tt.Cart.Dto;

namespace proj_tt.Cart
{
    public interface ICartAppService : IApplicationService
    {
        Task<CartDto> GetCartAsync();
        Task AddToCart(AddToCartInput input);
        Task UpdateCartItem(UpdateCartItemInput input);
        Task RemoveFromCart(RemoveFromCartInput input);
        Task Checkout();
    }
}
