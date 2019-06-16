using Restaurant.Core.OrderContext.Enums;
using System.ComponentModel.DataAnnotations;

namespace Restaurant.Core.OrderContext.HttpRequests
{
    public class GetPendingOrdersRequest
    {
        public int StartPage { get; set; }

        [Range(0, 20)]
        public int Limit { get; set; }

        public PendingOrdersDataProvider DataProvider { get; set; }
    }
}