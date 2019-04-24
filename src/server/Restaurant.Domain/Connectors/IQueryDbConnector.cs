using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Connectors
{
    public interface IQueryDbConnector
    {
        Task<T> FetchAsync<T>(
            string sql,
            Func<DbDataReader, T, Task> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new();

        Task<T> FetchAsync<T>(
            string sql,
            Action<DbDataReader, T> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new();

        Task<T> FetchAsync<T>(
            string sql,
            Func<DbDataReader, T> mapping,
            Dictionary<string, object> parameters = null,
            CancellationToken cancellationToken = default)
            where T : new();
    }
}
