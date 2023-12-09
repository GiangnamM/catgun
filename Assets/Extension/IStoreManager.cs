using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using JetBrains.Annotations;

namespace Extension {
    public interface IStoreItem {
        /// <summary>
        /// Gets the ID of this item.
        /// </summary>
        [NotNull]
        string Id { get; }

        /// <summary>
        /// i.e. cost items.
        /// </summary>
        [NotNull]
        List<(string Id, int Amount)> Costs { get; }

        /// <summary>
        /// i.e. gained items.
        /// </summary>
        [NotNull]
        List<(string Id, int Amount)> Gains { get; }
    }

    public class StoreObserver {
        /// <summary>
        /// Occurs when an item balance is changed.
        /// </summary>
        [CanBeNull]
        public Action<string, int, int> OnItemBalanceChanged { get; set; }

        /// <summary>
        /// Occurs when an item is about to be purchased.
        /// </summary>
        [CanBeNull]
        public Action<string> OnItemPurchaseStarted { get; set; }

        /// <summary>
        /// Occurs when an item is purchased.
        /// </summary>
        [CanBeNull]
        public Action<string> OnItemPurchaseSucceeded { get; set; }

        /// <summary>
        /// Occurs when an item is failed to purchase.
        /// </summary>
        [CanBeNull]
        public Action<string> OnItemPurchaseFailed { get; set; }
    }

    [Service(typeof(IStoreManager))]
    public interface IStoreManager : IObserverManager<StoreObserver> {
        [NotNull]
        Task Initialize();

        /// <summary>
        /// Gets store item for the specified item ID.
        /// </summary>
        /// <param name="id">The desired item ID.</param>
        /// <returns>The corresponding store item if found, null otherwise.</returns>
        [CanBeNull]
        IStoreItem GetItem([NotNull] string id);

        /// <summary>
        /// Purchases the specified item.
        /// </summary>
        /// <param name="id">The ID of the desired item.</param>
        /// <returns>Whether the purchase was successful.</returns>
        bool Purchase([NotNull] string id);

        /// <summary>
        /// Checks whether the specified item can be purchased.
        /// </summary>
        /// <param name="id">The ID of the desired item.</param>
        /// <returns>True if the item can be purchased, false otherwise.</returns>
        bool CanPurchase([NotNull] string id);

        /// <summary>
        /// Gets the current balance of the specified item.
        /// </summary>
        /// <param name="id">The ID of the desired item.</param>
        /// <returns>Current balance.</returns>
        int GetBalance([NotNull] string id);

        /// <summary>
        /// Adds an amount to the specified item.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <param name="amount">The desired amount.</param>
        void AddBalance([NotNull] string id, int amount);

        /// <summary>
        /// Gets the costs for the specified store item.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>Costs, can be null.</returns>
        [CanBeNull]
        List<(string Id, int Amount)> GetCosts([NotNull] string id);

        /// <summary>
        /// Gets the gains for the specified store item.
        /// </summary>
        /// <param name="id">The ID of the item.</param>
        /// <returns>Gains, can be null.</returns>
        [CanBeNull]
        List<(string Id, int Amount)> GetGains([NotNull] string id);
    }
}