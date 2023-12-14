using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Extension {
    public interface IStoreItem {
        [NotNull]
        string Id { get; }
        
        [NotNull]
        List<(string Id, int Amount)> Costs { get; }
        
        [NotNull]
        List<(string Id, int Amount)> Gains { get; }
    }

    public class StoreObserver {
        [CanBeNull]
        public Action<string, int, int> OnItemBalanceChanged { get; set; }
        
        [CanBeNull]
        public Action<string> OnItemPurchaseStarted { get; set; }
        
        [CanBeNull]
        public Action<string> OnItemPurchaseSucceeded { get; set; }
        
        [CanBeNull]
        public Action<string> OnItemPurchaseFailed { get; set; }
    }

    [Service(typeof(IStoreManager))]
    public interface IStoreManager : IObserverManager<StoreObserver> {
        [NotNull]
        Task Initialize();

        
        [CanBeNull]
        IStoreItem GetItem([NotNull] string id);
        
        bool Purchase([NotNull] string id);
        
        bool CanPurchase([NotNull] string id);
        
        int GetBalance([NotNull] string id);
        
        void AddBalance([NotNull] string id, int amount);
        
        [CanBeNull]
        List<(string Id, int Amount)> GetCosts([NotNull] string id);
        
        [CanBeNull]
        List<(string Id, int Amount)> GetGains([NotNull] string id);
    }
}