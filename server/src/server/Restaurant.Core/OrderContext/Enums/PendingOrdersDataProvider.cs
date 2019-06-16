namespace Restaurant.Core.OrderContext.Enums
{
    public enum PendingOrdersDataProvider
    {
        CsvProvider = 1,
        EntityFrameworkProvider = 2,
        AdoNetProvider = 4
    }
}