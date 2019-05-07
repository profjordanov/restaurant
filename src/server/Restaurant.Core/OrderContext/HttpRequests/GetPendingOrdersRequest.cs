using System.ComponentModel.DataAnnotations;

namespace Restaurant.Core.OrderContext.HttpRequests
{
    public class GetPendingOrdersRequest
    {
        public int StartPage { get; set; }

        [Range(2, 10)]
        public int Limit { get; set; }
    }
}