using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace proj_tt.Cart.Dto
{
    public class CheckoutInput
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string ShippingAddress { get; set; }
    }
}
