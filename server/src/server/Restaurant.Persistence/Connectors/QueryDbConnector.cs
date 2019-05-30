using Npgsql;
using Restaurant.Domain.Connectors;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Persistence.Connectors
{
    public class QueryDbConnector : IQueryDbConnector
    {
        public async Task<T> FetchAsync<T>(
            string sql,
            Func<DbDataReader, T, Task> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters, cancellationToken))
            {
                var objectToMap = new T();

                if (!reader.HasRows)
                {
                    return objectToMap;
                }

                while (await reader.ReadAsync(cancellationToken))
                {
                    await mapping(reader, objectToMap);
                }

                return objectToMap;
            }
        }

        public async Task<T> FetchAsync<T>(
            string sql,
            Action<DbDataReader, T> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters, cancellationToken))
            {
                var objectToMap = new T();

                if (!reader.HasRows)
                {
                    return objectToMap;
                }

                while (await reader.ReadAsync(cancellationToken))
                {
                    mapping(reader, objectToMap);
                }

                return objectToMap;
            }
        }

        public async Task<T> FetchAsync<T>(
            string sql,
            Func<DbDataReader, T> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new()
        {
            using (var connection = ConnectionManagerBase.GetConnection())
            using (var command = new NpgsqlCommand(sql, connection))
            using (var reader = await GetReaderAsync(connection, command, parameters, cancellationToken))
            {
                if (reader.HasRows && await reader.ReadAsync(cancellationToken))
                {
                    return mapping(reader);
                }

                return new T();
            }
        }

        private static async Task<DbDataReader> GetReaderAsync(
            NpgsqlConnection connection,
            NpgsqlCommand command,
            Dictionary<string, object> parameters,
            CancellationToken cancellationToken = default)
        {
            await connection.OpenAsync(cancellationToken);

            if (parameters == null)
            {
                return await command.ExecuteReaderAsync(cancellationToken);
            }

            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Key, parameter.Value);
            }

            return await command.ExecuteReaderAsync(cancellationToken);
        }
    }
}