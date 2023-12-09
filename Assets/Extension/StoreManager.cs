using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;


namespace Extension
{
    public class StoreItem : IStoreItem
    {
        public string Id { get; }
        public List<(string Id, int Amount)> Costs { get; } = new();
        public List<(string Id, int Amount)> Gains { get; } = new();

        public StoreItem([NotNull] string id)
        {
            Id = id;
        }

        public StoreItem AddCost([NotNull] string id, int amount)
        {
            if (id == Id)
            {
                throw new Exception("Cannot reference the same store item ID");
            }

            Costs.Add((id, amount));
            return this;
        }

        public StoreItem AddGain([NotNull] string id, int amount)
        {
            if (id == Id)
            {
                throw new Exception("Cannot reference the same store item ID");
            }

            Gains.Add((id, amount));
            return this;
        }
    }

    public class StoreManager : ObserverManager<StoreObserver>, IStoreManager
    {
        [Serializable]
        private class Data
        {
            public Dictionary<string, int> balances;
        }

        [NotNull] private readonly IDataManager _dataManager;

        [NotNull] private readonly string _dataKey;

        [NotNull] private readonly Dictionary<string, int> _defaultBalances;

        [NotNull] private readonly Dictionary<string, IStoreItem> _items;
        [CanBeNull] private Task<bool> _initializer;

        private Data _data;

        public StoreManager(
            [NotNull] IDataManager dataManager,
            [NotNull] string dataKey,
            [ItemNotNull] [NotNull] List<IStoreItem> items,
            [NotNull] Dictionary<string, int> defaultBalances
        )
        {
            _dataManager = dataManager;
            _dataKey = dataKey;
            _defaultBalances = defaultBalances;
            _items = items.ToDictionary(it => it.Id);
        }

        public Task Initialize() => _initializer ??= InitializeImpl();

        private async Task<bool> InitializeImpl()
        {
            await _dataManager.Initialize();
            Load();
            return true;
        }

        private void Load()
        {
            _data = _dataManager.Get(_dataKey,
                new Data
                {
                    //
                    balances = _defaultBalances,
                });
        }

        private void Save()
        {
            _dataManager.Set(_dataKey, _data);
        }

        public bool RegisterItem(IStoreItem item)
        {
            if (_items.ContainsKey(item.Id))
            {
                return false;
            }

            _items.Add(item.Id, item);
            return true;
        }

        public IStoreItem GetItem(string id)
        {
            return _items.TryGetValue(id, out var result) ? result : null;
        }

        public bool Purchase(string id)
        {
            return Purchase(id, 1);
        }

        private bool Purchase([NotNull] string id, int amount)
        {
            DispatchEvent(observer => observer.OnItemPurchaseStarted?.Invoke(id));
            if (!CanPurchase(id, amount))
            {
                DispatchEvent(observer => observer.OnItemPurchaseFailed?.Invoke(id));
                return false;
            }

            Purchase(_items[id], amount);
            DispatchEvent(observer => observer.OnItemPurchaseSucceeded?.Invoke(id));
            return true;
        }

        private void Purchase([NotNull] IStoreItem item, int amount)
        {
            AddBalance(item.Id, amount);
            item.Gains.ForEach(entry =>
            {
                var (entryId, entryAmount) = entry;
                if (_items.TryGetValue(entryId, out var entryItem))
                {
                    // Nested store item.
                    Purchase(entryItem, entryAmount * amount);
                }
                else
                {
                    AddBalance(entryId, entryAmount * amount);
                }
            });
        }

        public bool CanPurchase(string id)
        {
            return CanPurchase(id, 1);
        }

        private bool CanPurchase([NotNull] string id, int amount)
        {
            return _items.TryGetValue(id, out var item) && CanPurchase(item, amount);
        }

        private bool CanPurchase([NotNull] IStoreItem item, int amount)
        {
            return item.Costs.All(entry =>
            {
                var (entryId, entryAmount) = entry;
                if (_items.TryGetValue(entryId, out var entryItem))
                {
                    // Nested store item.
                    return CanPurchase(entryItem, entryAmount * amount);
                }

                // Currency.
                return GetBalance(entryId) >= entryAmount * amount;
            });
        }

        public int GetBalance(string id)
        {
            return _data.balances.TryGetValue(id, out var result) ? result : 0;
        }

        public void AddBalance(string id, int amount)
        {
            var balance = GetBalance(id);
            balance += amount;
            DispatchEvent(observer => observer.OnItemBalanceChanged?.Invoke(id, balance, amount));
            _data.balances[id] = balance;
            Save();
        }

        public List<(string Id, int Amount)> GetCosts(string id)
        {
            return GetItem(id)?.Costs;
        }

        public List<(string Id, int Amount)> GetGains(string id)
        {
            return GetItem(id)?.Gains;
        }
    }
}