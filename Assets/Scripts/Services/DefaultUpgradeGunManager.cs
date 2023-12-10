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
            public int Cost => _config.costs[Math.Min(_manager.GetLevelGun(_gunSkin), _config.costs.Count - 1)];
            public List<float> Damages => _config.damages;

            public List<float> FireRates =>
                _config.fireRates;

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
            public Dictionary<GunSkin, int> _guns;
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
            public List<float> damages;
            public List<float> fireRates;
            public List<int> costs;
        }

        private void LoadData()
        {
            _data = _dataManager.Get("upgrade_booster_manager",
                new Data
                {
                    _guns = new Dictionary<GunSkin, int>()
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

        public List<GunSkin> AllGuns => UpgradeGunHelper.AllGuns();


        public Dictionary<GunSkin, int> AllGunsAndCurLevel
        {
            get => _data._guns;
        }

        public IGunInfo GetInfo(GunSkin boosterType)
        {
            return new DefaultGunInfo(this, boosterType);
        }


        public void UpgradeGun(GunSkin type, int level)
        {
            if (_data._guns.ContainsKey(type))
            {
                _data._guns[type] += level;
            }
            else
            {
                _data._guns[type] = level;
            }

            DispatchEvent(observer => observer.OnLevelBoosterChanged?.Invoke(type));
            SaveData();
        }

        public int GetLevelGun(GunSkin type)
        {
            if (_data._guns.ContainsKey(type))
            {
                return _data._guns[type];
            }
            else
            {
                return 0;
            }
        }
    }
}