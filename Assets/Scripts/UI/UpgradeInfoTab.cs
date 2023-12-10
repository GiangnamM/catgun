using System;
using System.Collections.Generic;
using System.Linq;

using Extension;

using TMPro;

using UnityEngine;

namespace App {
    public class UpgradeInfoTab : MonoBehaviour {
        private readonly List<GunUpgradeInfoView> _entries = new() {
            { GunUpgradeInfoView.Damage },
            { GunUpgradeInfoView.Ammo },

        };
        
        [SerializeField]
        private DefaultConfigManager _configManager;

        [SerializeField]
        private InfoGunUpgradeView[] _infoGunUpgradeViews;
        
        [SerializeField]
        private GameObject[] _layerMaxLayerHide;

        [SerializeField]
        private TextMeshProUGUI _currentLevelUpgradeText;

        [SerializeField]
        private TextMeshProUGUI _nextLevelUpgradeText;

        [SerializeField]
        private TextMeshProUGUI _costText;

        [SerializeField]
        private GameObject _buttonUpgrade;

        private bool _enableUpgrade;
        private int _level;
        private bool _initialized;
        private string _id;
        private int _cost;
        private bool _isUnLock;
        private bool _isMaxLevel;

        public int Cost {
            get => _cost;
            set {
                _cost = value;
                _costText.text = $"{value}";
            }
        }
        
        public Action OnUpgradeButtonCallback { get; set; }
        
        public bool IsMaxLevel {
            get => _isMaxLevel;
            set {
                _isMaxLevel = value;
                foreach (var layer in _layerMaxLayerHide) {
                    layer.SetActive(!value);
                }
            }
        }

        public int Level {
            get => _level;
            set {
                _level = value;
                _currentLevelUpgradeText.text = $"Lv {value + 1}";
                _nextLevelUpgradeText.text = $"Lv {value + 2}";
                UpdateView(_id);
            }
        }

        private void Awake() {
            Initialize();
        }

        private void Initialize() {
            if (_initialized) {
                return;
            }
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
        }

        private void UpdateView(string id) {
            // var (gun, fireRate) = _configGunItemViews[id];
            // var gunConfigs = _configManager.GunConfigs.FirstOrDefault(p => p.GunType == gun);
            // if (gunConfigs == null) {
            //     return;
            // }
            // var gunInfoConfig = new List<string> {
            //     gunConfigs.Damage.ToString(),
            //     gunConfigs.MaxBullet.ToString(),
            //     fireRate,
            //     gunConfigs.DamePerSecond.ToString(),
            //     gunConfigs.ReloadTime.ToString(),
            // };
            // var isUnlimitedAmmo = gunConfigs.UnlimitedBullet;
            // for (var i = 0; i < _infoGunUpgradeViews.Length; i++) {
            //     var type = _entries[i];
            //     var item = _infoGunUpgradeViews[i];
            //     item.ViewType = type;
            //     var itemEnableUpgrade = _infoGunUpgradeViews[i].EnableUpgrade =
            //         EnableUpgrade && _configsItemEnableUpgrades[type];
            //     item.CurrentValue = gunInfoConfig[i];
            //     if (isUnlimitedAmmo) {
            //         if (type is GunUpgradeInfoView.Ammo) {
            //             item.IsUnlimitedAmmo = true;
            //         }
            //     }
            //     if (type is GunUpgradeInfoView.CoolDown) {
            //         item.gameObject.SetActive(!isUnlimitedAmmo);
            //     }
            //     if (!itemEnableUpgrade) {
            //         continue;
            //     }
            //     var (dameList, bulletList) = _configManager.AllUpgradeGunTuples[gun];
            //     switch (type) {
            //         case GunUpgradeInfoView.Ammo:
            //             item.CurrentValue =
            //                 (gunConfigs.MaxBullet + UpgradeHelper.GetBulletAtLevel(_level, bulletList)).ToString();
            //             item.NextValue =
            //                 (gunConfigs.MaxBullet + UpgradeHelper.GetBulletAtLevel(_level + 1, bulletList))
            //                 .ToString();
            //             break;
            //         case GunUpgradeInfoView.Damage:
            //             //Hard
            //             item.CurrentValue =
            //                 (UpgradeHelper.GetDameAtLevel(_level, dameList) + gunConfigs.Damage).ToString();
            //             item.NextValue = (gunConfigs.Damage + UpgradeHelper.GetDameAtLevel(_level + 1, dameList))
            //                 .ToString();
            //             break;
            //         default:
            //             throw new ArgumentOutOfRangeException();
            //     }
                //
            // }
        }

        public void OnButtonUpgradePressed() {
            OnUpgradeButtonCallback?.Invoke();
        }
    }
}