using System;
using System.Collections.Generic;
using Extension;
using TMPro;
using UnityEngine;

namespace App
{
    public class UpgradeInfoTab : MonoBehaviour
    {
        private readonly List<GunUpgradeInfoView> _entries = new()
        {
            { GunUpgradeInfoView.Damage },
            { GunUpgradeInfoView.FireRate },
        };

        [SerializeField] private DefaultConfigManager _configManager;

        [SerializeField] private InfoGunUpgradeView[] _infoGunUpgradeViews;

        [SerializeField] private GameObject[] _layerMaxLayerHide;

        [SerializeField] private TextMeshProUGUI _currentLevelUpgradeText;

        [SerializeField] private TextMeshProUGUI _nextLevelUpgradeText;

        [SerializeField] private TextMeshProUGUI _costText;

        [Inject] private IUpgradeGunManager _upgradeGunManager;
        [Inject] private IStoreManager _storeManager;
        private int _level;
        private bool _initialized;
        private int _cost;
        private bool _isUnLock;
        private bool _isMaxLevel;
        private GunSkin _gunSkin;

        public Canvas Canvas { get; set; }

        public GunSkin GunSkin
        {
            get => _gunSkin;
            set
            {
                _gunSkin = value;
                Initialize();
                UpdateView(value);
            }
        }

        public int Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                _costText.text = $"{value}";
            }
        }

        public Action OnUpgradeButtonCallback { get; set; }

        public bool IsMaxLevel
        {
            get => _isMaxLevel;
            set
            {
                _isMaxLevel = value;
                foreach (var layer in _layerMaxLayerHide)
                {
                    layer.SetActive(!value);
                }
            }
        }

        public int Level
        {
            get => _level;
            set
            {
                _level = value;
                _currentLevelUpgradeText.text = $"Lv {value + 1}";
                _nextLevelUpgradeText.text = $"Lv {value + 2}";
            }
        }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            ServiceLocator.Instance.ResolveInjection(this);
            _initialized = true;
        }

        private void UpdateView(GunSkin gunSkin)
        {
            var gunUpgradeItemInfo = _upgradeGunManager.GetInfo(gunSkin);
            var level = _upgradeGunManager.GetLevelGun(gunSkin);
            var maxLevel = gunUpgradeItemInfo.MaxLevel;
            var cost = gunUpgradeItemInfo.Cost;
            Level = level;
            IsMaxLevel = level == maxLevel;
            Cost = cost;
            OnUpgradeButtonCallback = () =>
            {
                if (cost <= _storeManager.GetBalance(StoreItemId.Gold))
                {
                    //Upgrade Success
                    _storeManager.AddBalance(StoreItemId.Gold, -cost);
                    _upgradeGunManager.UpgradeGun(gunSkin, 1);
                    UpdateView(gunSkin);
                }
                else
                {
                    var dialog = InfoDialog.Show(Canvas);
                    dialog.Content = $"Not enough gold";
                }
            };
            // Damage, FireRate
            var upgradeConfigs = new List<List<float>>()
            {
                _configManager.AllUpgradeGunTuples[gunSkin].Item1,
                _configManager.AllUpgradeGunTuples[gunSkin].Item2,
            };
            var baseConfigs = new[]
            {
                _configManager.GunBaseInfo[gunSkin].Item1,
                _configManager.GunBaseInfo[gunSkin].Item2,
            };
            for (var i = 0; i < _infoGunUpgradeViews.Length; i++)
            {
                var type = _entries[i];
                var item = _infoGunUpgradeViews[i];
                var upgradeConfig = upgradeConfigs[i];
                var baseConfig = baseConfigs[i];
                item.ViewType = type;
                item.CurrentValue = ValueAtIndex(upgradeConfig, level) + baseConfig;
                item.IsMaxLevel = maxLevel == level;
                if (maxLevel == level) continue;
                item.NextValue = ValueAtIndex(upgradeConfig, level + 1) + baseConfig;
            }
        }

        private float ValueAtIndex(List<float> arr, int index)
        {
            float sum = 0;
            for (int i = 0; i <= index; i++)
            {
                sum += arr[i];
            }

            return sum;
        }

        public void OnButtonUpgradePressed()
        {
            OnUpgradeButtonCallback?.Invoke();
        }
    }
}