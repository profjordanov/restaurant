using System;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Restaurant.Domain.Extensions
{
    public static class DbReaderExtensions
    {
        /// <summary>
        /// Assumes that the DB column types match the property types. Will not work for complex types.
        /// </summary>
        public static T Fill<T>(this DbDataReader reader, T obj)
        {
            var columns = Enumerable.Range(0, reader.FieldCount).Select(reader.GetName).ToList();

            foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(obj))
            {
                if (prop.IsReadOnly)
                    continue;
                if (!reader.HasColumn(prop.Name))
                    continue;
                if (!reader.HasRows)
                    continue;

                var value = reader[ prop.Name ];
                prop.SetValue(obj, value is DBNull ? null : value);
            }

            return obj;
        }

        /// <inheritdoc cref="Fill{T}"/>
        public static T Get<T>(this DbDataReader reader)
            where T : new() =>
            Fill<T>(reader, new T());

        /// <summary>
        /// Case-insensitive.
        /// </summary>
        public static bool HasColumn(this IDataRecord dr, string columnName)
        {
            for (var i = 0; i < dr.FieldCount; i++)
            {
                if (dr.GetName(i).ToLower().Equals(columnName.ToLower(), StringComparison.InvariantCultureIgnoreCase))
                    return true;
            }

            return false;
        }

        public static Guid SafeGetGuid(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? reader.GetGuid(reader.GetOrdinal(columnName)) : Guid.Empty;

        public static bool? SafeGetNullableBool(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (bool?)reader.GetBoolean(reader.GetOrdinal(columnName)) : null;

        public static DateTime? SafeGetNullableDateTime(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (DateTime?)reader.GetDateTime(reader.GetOrdinal(columnName)) : null;

        public static Guid? SafeGetNullableGuid(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (Guid?)reader.GetGuid(reader.GetOrdinal(columnName)) : null;

        public static int? SafeGetNullableInt(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (int?)reader.GetInt32(reader.GetOrdinal(columnName)) : null;

        public static long? SafeGetNullableLong(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (long?)reader.GetInt64(reader.GetOrdinal(columnName)) : null;

        public static double? SafeGetNullableDouble(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? (double?)reader.GetDouble(reader.GetOrdinal(columnName)) : null;

        public static int GetIntOrDefault(this DbDataReader reader, string columnName) =>
            SafeGetNullableInt(reader, columnName) ?? default(int);

        public static string SafeGetString(this DbDataReader reader, string columnName) =>
            ColumnContainsValue(reader, columnName) ? reader.GetString(reader.GetOrdinal(columnName)) : string.Empty;

        private static bool ColumnContainsValue(IDataRecord reader, string columnName) =>
            reader.HasColumn(columnName) && !reader.IsDBNull(reader.GetOrdinal(columnName));

    }
}
