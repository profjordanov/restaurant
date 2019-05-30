using System.ComponentModel.DataAnnotations;

namespace Restaurant.Core.OrderContext.HttpRequests
{
    public class MakeOrderRequest
    {
        [Required]
        public int Quantity { get; set; }
    }
}