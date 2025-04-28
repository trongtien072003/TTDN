using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Cart.Dto
{
    public class UpdateCartItemInput
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
