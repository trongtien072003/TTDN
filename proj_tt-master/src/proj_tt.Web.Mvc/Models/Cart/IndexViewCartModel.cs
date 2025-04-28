using System.Collections.Generic;

namespace proj_tt.Web.Models.Cart
{
    public class IndexViewCartModel
    {
        public List<IndexViewCartItemModel> Items { get; set; }
        public decimal TotalPrice { get; set; }
        public long UserId { get; set; }

        public IndexViewCartModel()
        {
            Items = new List<IndexViewCartItemModel>();
        }
    }

    public class IndexViewCartItemModel
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string ImageUrl { get; set; }
    }
}
