using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Extension;

namespace App
{
    public class DefaultUpgradeGunManager : ObserverManager<UpgradeGunObserver>, IUpgradeGunManager
    {
        private class DefaultGunInfo : IGunInfo
        {
            private readonly DefaultUpgradeGunManager _manager;
            private readonly GunSkin _gunSkin;
            private readonly SkillConfig _config;

            public GunSkin GunSkin => _gunSkin;
            public int Cost => _config.costs[Math.Min(_manager.GetLevelBooster(_gunSkin), _config.costs.Count - 1)];
            public int Damage => _config.damages[Math.Min(_manager.GetLevelBooster(_gunSkin), _config.costs.Count - 1)];

            public float FireRate =>
                _config.fireRates[Math.Min(_manager.GetLevelBooster(_gunSkin), _config.costs.Count - 1)];

            public int MaxLevel => _config.costs.Count - 1;

            public DefaultGunInfo(DefaultUpgradeGunManager manager, GunSkin gunSkin)
            {
                _manager = manager;
                _gunSkin = gunSkin;
                _config = UpgradeGunHelper.SkinConfigs()[gunSkin];
            }
        }

        [Serializable]
        private class Data
        {
            public Dictionary<GunSkin, int> boosters;
        }

        private readonly IDataManager _dataManager;
        private Task<bool> _initializer;
        private Data _data;

        public Task<bool> Initialize() => _initializer ??= InitializeImpl();

        private async Task<bool> InitializeImpl()
        {
            await _dataManager.Initialize();
            LoadData();
            return true;
        }

        public DefaultUpgradeGunManager(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }

        public class SkillConfig
        {
            public List<int> damages;
            public List<float> fireRates;
            public List<int> costs;
        }

        private void LoadData()
        {
            _data = _dataManager.Get("upgrade_booster_manager",
                new Data
                {
                    boosters = new Dictionary<GunSkin, int>()
                    {
                        { GunSkin.Bazooka, 0 }, { GunSkin.FireBlaster, 0 },
                        { GunSkin.Laser, 0 }
                    }
                });
        }

        private void SaveData()
        {
            _dataManager.Set("upgrade_booster_manager", _data);
        }

        public List<GunSkin> AllBoosters => UpgradeGunHelper.AllBoosters();


        public Dictionary<GunSkin, int> AllBoostersAndCurLevel
        {
            get => _data.boosters;
        }

        public IGunInfo GetInfo(GunSkin boosterType)
        {
            return new DefaultGunInfo(this, boosterType);
        }


        public void UpgradeBooster(GunSkin type, int level)
        {
            if (_data.boosters.ContainsKey(type))
            {
                _data.boosters[type] += level;
            }
            else
            {
                _data.boosters[type] = level;
            }

            DispatchEvent(observer => observer.OnLevelBoosterChanged?.Invoke(type));
            SaveData();
        }

        public int GetLevelBooster(GunSkin type)
        {
            if (_data.boosters.ContainsKey(type))
            {
                return _data.boosters[type];
            }
            else
            {
                return 0;
            }
        }
    }
}