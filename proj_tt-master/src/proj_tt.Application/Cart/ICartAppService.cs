
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
        Task<List<CartItemDto>> GetCartItemsByUserIdAsync();
        Task AddToCartAsync(AddToCartInput input);
        Task UpdateCartItemQuantityAsync(UpdateCartItemInput input);
        Task RemoveCartItemAsync(RemoveFromCartInput input);
        Task ClearCartAsync();
        Task<int> CountCartItemsAsync();
        //Task<int> CountCartAsync();
    }
}
