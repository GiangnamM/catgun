using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public enum GunUpgradeInfoView
    {
        Damage,
        FireRate,
    }

    public class InfoGunUpgradeView : MonoBehaviour
    {
        [Serializable]
        private class GunAndInfo
        {
            public GunUpgradeInfoView infoGunUpgradeType;
            public Sprite icon;
            public string name;
        }

        [SerializeField] private GunAndInfo[] _gunAndInfos;

        [SerializeField] private Image _icon;

        [SerializeField] private TextMeshProUGUI _nameText;

        [SerializeField] private TextMeshProUGUI _currentValueUpgradeText;

        [SerializeField] private TextMeshProUGUI _nextValueUpgradeText;

        private GunUpgradeInfoView _viewType;
        private float _currentValue;
        private float _nextValue;
        private bool _isUnlimitedAmmo;

        public float CurrentValue
        {
            get => _currentValue;
            set
            {
                _currentValue = value;
                _currentValueUpgradeText.text = $"{_currentValue}";
            }
        }

        public float NextValue
        {
            get => _nextValue;
            set
            {
                _nextValue = value;
                _nextValueUpgradeText.text = $"{_nextValue}";
            }
        }

        public GunUpgradeInfoView ViewType
        {
            get => _viewType;
            set
            {
                _viewType = value;
                UpdateView();
            }
        }

        private void UpdateView()
        {
            var config = _gunAndInfos.FirstOrDefault(p => p.infoGunUpgradeType == _viewType);
            if (config == null)
            {
                return;
            }

            _icon.sprite = config.icon;
            _nameText.text = config.name;
        }
    }
}