using Abp.Application.Services.Dto;
using proj_tt.Cart.Dto;

namespace proj_tt.Web.Views.Shared.Components.CartMenu
{
    public class CartMenuViewModel
    {
        public ListResultDto<CartDto> Carts { get; set; }
        public int CartItem { get; set; }

        public int UserId
        {
            get; set;
        }
    }
}
