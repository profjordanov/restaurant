using System;
using System.Globalization;

namespace Restaurant.Business.Extensions
{
    public class CustomParser
    {
        public static int ParseInteger(object property)
        {
            if (property == null)
            {
                return 0;
            }

            int intValue;
            if (int.TryParse(property.ToString(), out intValue))
            {
                return intValue;
            }

            return 0;
        }

        public static int? ParseIntegerNullable(object property)
        {
            if (property == null)
            {
                return null;
            }

            int intValue;
            if (int.TryParse(property.ToString(), out intValue))
            {
                return intValue;
            }

            return null;
        }

        public static decimal ParseDecimal(object property)
        {
            if (property == null)
            {
                return 0;
            }

            decimal decimalValue;
            if (decimal.TryParse(property.ToString(), out decimalValue))
            {
                return decimalValue;
            }

            return 0;
        }

        public static decimal? ParseDecimalNullable(object property)
        {
            if (property == null)
            {
                return null;
            }

            decimal decimalValue;
            if (decimal.TryParse(property.ToString(), out decimalValue))
            {
                return decimalValue;
            }

            return null;
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

            DateTime dateValue;

            try
            {
                if (DateTime.TryParse(property.ToString(), out dateValue))
                {
                    return dateValue;
                }
            }
            catch (Exception)
            {
                return dateValue = DateTime.MinValue;
            }

            return null;
        }
    }
}