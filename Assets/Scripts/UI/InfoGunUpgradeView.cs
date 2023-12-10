using System;
using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace App {
    public enum GunUpgradeInfoView {
        Damage,
        Ammo,
        FireRate,
        DamePerSecond,
        CoolDown,
    }

    public class InfoGunUpgradeView : MonoBehaviour {
        [Serializable]
        private class GunAndInfo {
            public GunUpgradeInfoView infoGunUpgradeType;
            public Sprite icon;
            public string name;
        }

        [SerializeField]
        private GunAndInfo[] _gunAndInfos;

        [SerializeField]
        private Image _icon;

        [SerializeField]
        private TextMeshProUGUI _nameText;

        [SerializeField]
        private TextMeshProUGUI _currentValueUpgradeText;

        [SerializeField]
        private TextMeshProUGUI _nextValueUpgradeText;

        [SerializeField]
        private TextMeshProUGUI _valueText;

        [SerializeField]
        private GameObject _layerShowUpgrade;

        [SerializeField]
        private GameObject _layerShowValue;

        [SerializeField]
        private GameObject _layerUnlimitedAmmo;

        [SerializeField]
        private GameObject _layerValue;

        private bool _enableUpgrade;
        private GunUpgradeInfoView _viewType;
        private string _currentValue;
        private string _nextValue;
        private bool _isUnlimitedAmmo;

        public bool EnableUpgrade {
            get => _enableUpgrade;
            set {
                _enableUpgrade = value;
                _layerShowUpgrade.SetActive(value);
                _layerShowValue.SetActive(!value);
            }
        }

        public string CurrentValue {
            get => _currentValue;
            set {
                _currentValue = value;
                _currentValueUpgradeText.text = $"{_currentValue}";
                _valueText.text = $"{_currentValue}";
            }
        }

        public string NextValue {
            get => _nextValue;
            set {
                _nextValue = value;
                _nextValueUpgradeText.text = $"{_nextValue}";
            }
        }

        public bool IsUnlimitedAmmo {
            get => _isUnlimitedAmmo;
            set {
                _isUnlimitedAmmo = value;
                _layerUnlimitedAmmo.SetActive(value);
                _layerValue.SetActive(!value);
            }
        }

        public GunUpgradeInfoView ViewType {
            get => _viewType;
            set {
                _viewType = value;
                UpdateView();
            }
        }

        private void UpdateView() {
            var config = _gunAndInfos.FirstOrDefault(p => p.infoGunUpgradeType == _viewType);
            if (config == null) {
                return;
            }
            _icon.sprite = config.icon;
            _nameText.text = config.name;
        }
    }
}