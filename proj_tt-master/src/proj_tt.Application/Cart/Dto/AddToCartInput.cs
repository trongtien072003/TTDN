using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace proj_tt.Cart.Dto
{
    [AutoMapFrom(typeof(Carts))]
    public class AddToCartInput : EntityDto<long>
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
