using Restaurant.Domain.Readers.Order;
using Restaurant.Domain.Views.Order;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.Domain.Decorators.Order
{
    public class PendingOrdersCachingReader : IPendingOrdersReader
    {
        private readonly IPendingOrdersReader _wrappedReader;
        private readonly TimeSpan _cacheDuration = new TimeSpan(0, 0, 30);

        private IEnumerable<PendingOrderView> _cachedItems;
        private DateTime _dataDateTime;

        public PendingOrdersCachingReader(IPendingOrdersReader wrappedReader)
        {
            _wrappedReader = wrappedReader;
        }

        private bool IsCacheValid
        {
            get
            {
                if (_cachedItems == null)
                {
                    return false;
                }

                var cacheAge = DateTime.Now - _dataDateTime;
                return cacheAge < _cacheDuration;
            }
        }

        public async Task<IEnumerable<PendingOrderView>> PendingOrdersAsync(
            Guid userId,
            int limit,
            int startPage,
            CancellationToken cancellationToken)
        {
            await ValidateCacheAsync(userId, limit, startPage, cancellationToken);
            return _cachedItems;
        }

        private async Task ValidateCacheAsync(
            Guid userId,
            int limit,
            int startPage,
            CancellationToken cancellationToken)
        {
            if (IsCacheValid)
            {
                return;
            }

            try
            {
                _cachedItems = await _wrappedReader.PendingOrdersAsync(
                    userId, limit, startPage, cancellationToken);

                _dataDateTime = DateTime.Now;
            }
            catch (Exception)
            {
                InvalidateCache();

                throw;
            }
        }

        private void InvalidateCache()
        {
            _dataDateTime = DateTime.MinValue;
        }
    }
}