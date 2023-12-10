using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Extension;

namespace App {
    public class DefaultSkinGunManager : ISkinGunManager {
        private class DefaultSkinInfo : ISkinInfo {
            private readonly DefaultSkinGunManager _gunManager;
            private readonly GunSkin _skin;
            private readonly SkinConfig _config;

            public GunSkin Skin => _skin;
            public int Cost => _config.cost;

            public bool IsOwned {
                get => _gunManager.IsOwned(_skin);
                set => _gunManager.SetOwned(_skin, value);
            }

            public bool IsSelected => _gunManager.CurrentSkin == _skin;
            
            public DefaultSkinInfo(DefaultSkinGunManager gunManager, GunSkin skin) {
                _gunManager = gunManager;
                _skin = skin;
                _config = SkinGunHelper.SkinConfigs()[skin];
            }
        }

        public class SkinConfig {
            public int cost;
        }

        [Serializable]
        private class Data {
            public GunSkin currentGunSkin;
            public List<GunSkin> ownedSkins;
        }

        private readonly IDataManager _dataManager;
        private Task<bool> _initializer;
        private Data _data;

        public DefaultSkinGunManager(
            IDataManager dataManager) {
            _dataManager = dataManager;
        }

        public Task<bool> Initialize() => _initializer ??= InitializeImpl();

        public void Destroy() { }

        private async Task<bool> InitializeImpl() {
            await _dataManager.Initialize();
            LoadData();
            return true;
        }

        private void LoadData() {
            _data = _dataManager.Get("skin_gun_data",
                new Data {
                    currentGunSkin = GunSkin.Bazooka,
                    ownedSkins = new List<GunSkin>() {GunSkin.Bazooka,},
                });
        }

        private void SaveData() {
            _dataManager.Set("skin_data", _data);
        }

        public GunSkin CurrentSkin {
            get => _data.currentGunSkin;
            set {
                if (_data.currentGunSkin == value) {
                    return;
                }
                _data.currentGunSkin = value;
                SaveData();
            }
        }

        public List<GunSkin> AllSkins => SkinGunHelper.AllSkins();

        public ISkinInfo GetInfo(GunSkin gunSkin) {
            return new DefaultSkinInfo(this, gunSkin);
        }
        
        private bool IsOwned(GunSkin gunSkin) {
            return _data.ownedSkins.IndexOf(gunSkin) != -1;
        }

        public void SetOwned(GunSkin gunSkin, bool owned) {
            if (IsOwned(gunSkin) == owned) {
                return;
            }
            if (owned) {
                _data.ownedSkins.Add(gunSkin);
            } else {
                _data.ownedSkins.Remove(gunSkin);
            }
            SaveData();
        }
    }
}