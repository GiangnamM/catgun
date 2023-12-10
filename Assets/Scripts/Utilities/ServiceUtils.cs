using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extension;

namespace App
{
    public static class StoreItemId
    {
        public const string Gold = "gold";
    }

    public static class ServiceUtils
    {
        public static async Task Initialize()
        {
            var dataManager = new JsonDataManager();
            var storeManager = CreateStoreManager(dataManager);

            // var characterSkinManager = new DefaultCharacterSkinManager(dataManager);
            var managers = new object[]
            {
                dataManager,
                storeManager,
                // characterSkinManager,
            };
            await Task.WhenAll(
                dataManager.Initialize(),
                storeManager.Initialize()
            );
            foreach (var manager in managers)
                ServiceLocator.Instance.Provide(manager);
        }

        private static IStoreManager CreateStoreManager(IDataManager dataManager)
        {
            return new StoreManager(dataManager, "store_manager", new List<IStoreItem>(),
                new Dictionary<string, int>
                {
                    [StoreItemId.Gold] = Constant.REWARD_COIN_NEW_USER,
                });
        }
    }
}