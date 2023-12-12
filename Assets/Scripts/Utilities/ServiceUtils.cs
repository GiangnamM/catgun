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
            var sceneManager = new SceneManager();
            var gunSkinManager = new DefaultSkinGunManager(dataManager);
            var gunUpgradeManager = new DefaultUpgradeGunManager(dataManager);
            var managers = new object[]
            {
                dataManager,
                storeManager,
                sceneManager,
                gunUpgradeManager,
                gunSkinManager
            };
            await Task.WhenAll(
                dataManager.Initialize(),
                gunSkinManager.Initialize(),
                gunUpgradeManager.Initialize(),
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