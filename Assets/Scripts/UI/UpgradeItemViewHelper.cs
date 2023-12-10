using System;

using System.Linq;

using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace App {
    public class UpgradeItemViewHelper : MonoBehaviour {
        [Serializable]
        private class GunAndInfo {
            public GunSkin gunType;
            public Sprite nameBar;
            public Sprite iconGun;
            public string nameGun;
        }

        [SerializeField]
        private GunAndInfo[] _gunAndInfos;

        [SerializeField]
        private GameObject _layerSelected;

        [SerializeField]
        private GameObject _layerLock;
        
        [SerializeField]
        private TextMeshProUGUI _levelText;

        [SerializeField]
        private TextMeshProUGUI _nameText;

        [SerializeField]
        private Image _iconGun;

        [SerializeField]
        private Image _nameBar;

        private GunSkin _gunType;
        private bool _isUnlock;
        private bool _isSelected;
        private int _level;

        public bool IsSelected {
            get => _isSelected;
            set {
                _isSelected = value;
                _layerSelected.SetActive(value);
            }
        }

        public bool IsUnlock {
            get => _isUnlock;
            set {
                _isUnlock = value;
                _layerLock.SetActive(!value);
            }
        }
        
        public int Level {
            get => _level;
            set {
                _level = value;
                _levelText.text = $"Lv: {value}";
            }
        }

        public GunSkin GunType {
            get => _gunType;
            set {
                _gunType = value;
                UpdateDisplay(value);
            }
        }

        public Action OnItemViewPressed { get; set; }

        private void UpdateDisplay(GunSkin gun) {
            var config = _gunAndInfos.FirstOrDefault(p => p.gunType == gun);
            if (config == null) {
                return;
            }
            _iconGun.sprite = config.iconGun;
            _nameText.text = $"{config.nameGun}";
            _nameBar.sprite = config.nameBar;
        }

        public void OnButtonItemViewPressed() {
            OnItemViewPressed?.Invoke();
        }
    }
}