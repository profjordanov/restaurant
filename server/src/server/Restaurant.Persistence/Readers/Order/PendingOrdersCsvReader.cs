using Restaurant.Domain.FileLoaders;
using Restaurant.Domain.Readers.Order;
using Restaurant.Domain.Views.Meal;
using Restaurant.Domain.Views.Order;
using Restaurant.Persistence.FileLoaders;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Readers.Order
{
    public class PendingOrdersCsvReader : IPendingOrdersReader
    {
        public PendingOrdersCsvReader()
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory + "PendingOrders.csv";
            FileLoader = new CsvFileLoader(filePath);
        }

        public ICsvFileLoader FileLoader { get; set; }

        public async Task<IEnumerable<PendingOrderView>> PendingOrdersAsync(Guid userId, int limit, int startPage, CancellationToken cancellationToken)
        {
            var fileData = await FileLoader.LoadFileAsync();

            return ParseDataString(fileData);
        }

        private static IEnumerable<PendingOrderView> ParseDataString(string csvData)
        {
            var people = new List<PendingOrderView>();
            var lines = csvData.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (var line in lines)
            {
                try
                {
                    people.Add(ParsePendingOrderString(line));
                }
                catch (Exception e)
                {
                    // Skip the bad record, log it, and move to the next record
                    Debug.Fail($"Unable to parse record: {line}." +
                               $"Exception: {e.Message}.");
                }
            }

            return people;
        }

        private static PendingOrderView ParsePendingOrderString(string pendingOrderData)
        {
            var elements = pendingOrderData.Split(',');
            var person = new PendingOrderView
            {
                Id = Guid.Parse(elements[0]),
                Quantity = int.Parse(elements[1]),
                CreatedOn = DateTime.Parse(elements[2]),
                Meal = new MealView
                {
                    Id = Guid.Parse(elements[3]),
                    Name = elements[4],
                    Price = decimal.Parse(elements[5])
                }
            };
            return person;
        }
    }
}