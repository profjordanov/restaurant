using System;
using System.Globalization;

namespace Restaurant.Domain.Extensions
{
    public class CustomParser
    {
        public static int ParseInteger(object property)
        {
            if (property == null)
            {
                return 0;
            }

            return int.TryParse(property.ToString(), out var intValue) ? intValue : 0;
        }

        public static int? ParseIntegerNullable(object property)
        {
            if (property == null)
            {
                return null;
            }

            return int.TryParse(property.ToString(), out var intValue) ? (int?) intValue : null;
        }

        public static decimal ParseDecimal(object property)
        {
            if (property == null)
            {
                return 0;
            }

            return decimal.TryParse(property.ToString(), out var decimalValue) ? decimalValue : 0;
        }

        public static decimal? ParseDecimalNullable(object property)
        {
            if (property == null)
            {
                return null;
            }

            return decimal.TryParse(property.ToString(), out var decimalValue) ? (decimal?) decimalValue : null;
        }

        public static DateTime ParseDateTime(object property)
        {
            if (property == null)
            {
                throw new FormatException();
            }

            return DateTime.Parse(property.ToString(), CultureInfo.CurrentCulture);
        }

        public static DateTime? ParseDateTimeNullable(object property)
        {
            if (property == null)
            {
                return null;
            }

            try
            {
                if (DateTime.TryParse(property.ToString(), out var dateValue))
                {
                    return dateValue;
                }
            }
            catch (Exception)
            {
                return DateTime.MinValue;
            }

            return null;
        }
    }
}