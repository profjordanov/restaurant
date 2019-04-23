using Npgsql;
using Restaurant.Domain.Connectors;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Connectors
{
    public class QueryDbConnector : IQueryDbConnector
    {
        public async Task<T> FetchAsync<T>(string sql, Func<DbDataReader, T, Task> mapping, Dictionary<string, object> parameters = null)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters))
            {
                var objectToMap = new T();

                if (!reader.HasRows)
                {
                    return objectToMap;
                }

                while (await reader.ReadAsync())
                {
                    await mapping(reader, objectToMap);
                }

                return objectToMap;
            }
        }

        public async Task<T> FetchAsync<T>(string sql, Action<DbDataReader, T> mapping, Dictionary<string, object> parameters = null)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters))
            {
                var objectToMap = new T();

                if (!reader.HasRows)
                {
                    return objectToMap;
                }

                while (await reader.ReadAsync())
                {
                    mapping(reader, objectToMap);
                }

                return objectToMap;
            }
        }

        public async Task<T> FetchAsync<T>(string sql, Func<DbDataReader, T> mapping, Dictionary<string, object> parameters = null)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters))
            {
                if (reader.HasRows && await reader.ReadAsync())
                {
                    return mapping(reader);
                }

                return new T();
            }
        }

        private static async Task<DbDataReader> GetReaderAsync(NpgsqlConnection connection, NpgsqlCommand command, Dictionary<string, object> parameters)
        {
            await connection.OpenAsync();

            if (parameters == null)
            {
                return await command.ExecuteReaderAsync();
            }

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            return await command.ExecuteReaderAsync();
        }
    }
}