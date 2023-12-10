using System;
using System.Collections.Generic;

using Extension;

using TMPro;

using UnityEngine;

namespace App {
    public class UpgradeInfoTab : MonoBehaviour {
        private readonly List<GunUpgradeInfoView> _entries = new() {
            { GunUpgradeInfoView.Damage },
            { GunUpgradeInfoView.FireRate },
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

        private void UpdateView() {
            
            for (var i = 0; i < _infoGunUpgradeViews.Length; i++) {
                var type = _entries[i];
                var item = _infoGunUpgradeViews[i];
                item.ViewType = type;
                // item.CurrentValue = 
                
            }
        }

        public void OnButtonUpgradePressed() {
            OnUpgradeButtonCallback?.Invoke();
        }
    }
}