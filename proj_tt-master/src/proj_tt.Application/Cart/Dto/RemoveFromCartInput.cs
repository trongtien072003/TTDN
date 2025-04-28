using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;

namespace proj_tt.Cart.Dto
{
    public class RemoveFromCartInput : EntityDto<long>
    {
        public int ProductId { get; set; }
    }
}
