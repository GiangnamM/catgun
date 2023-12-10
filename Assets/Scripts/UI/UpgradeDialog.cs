using System;
using System.Linq;
using DG.Tweening;
using Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace App
{
    public enum InventoryState
    {
        Equip,
        Equipped,
        Buy,
    }

    public class UpgradeDialog : MonoBehaviour
    {
        public enum Result
        {
            Close,
        }

        [Serializable]
        private class StateAndButton
        {
            public InventoryState inventoryState;
            public Button button;
        }

        [SerializeField] private RectTransform _dialogLayer;

        [SerializeField] private UpgradeItemViewHelper[] _upgradeItemViews;

        [SerializeField] private StateAndButton[] _stateAndButtons;

        [SerializeField] private SkinGunSelector _gunSelector;

        [SerializeField] private UpgradeInfoTab _upgradeInfoTab;

        [SerializeField] private TextMeshProUGUI _costText;

        [Inject] private IStoreManager _storeManager;
        [Inject] private ISkinGunManager _skinGunManager;
        private bool _initialized;
        private InventoryState _inventoryState;
        private Canvas _canvas;
        private bool _isActive;
        private int _cost;
        private GunSkin _gunSelecting;

        private GunSkin GunSelecting
        {
            get => _gunSelecting;
            set
            {
                _gunSelecting = value;
                UpdateView(value);
            }
        }

        private int Cost
        {
            get => _cost;
            set
            {
                _cost = value;
                _costText.text = $"{_cost}";
            }
        }


        public InventoryState InventoryState
        {
            get => _inventoryState;
            private set
            {
                _inventoryState = value;
                UpdateStateDisplay(value);
            }
        }

        private Action<InventoryState, GunSkin> OnButtonStateCallBack { get; set; }

        public Action<Result> OnDidHide { get; set; }

        public static UpgradeDialog Show(Canvas canvas)
        {
            var prefab = Resources.Load<UpgradeDialog>($"Prefabs/UI/{nameof(UpgradeDialog)}");
            var dialog = Instantiate(prefab, canvas.transform, false);
            dialog._canvas = canvas;
            return dialog;
        }

        private void Awake()
        {
            Initialize();
            OnButtonStateCallBack = OnStateCallBack;
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            InitGunItem();
        }

        private void Hide(Result result)
        {
            DOTween.Sequence()
                .Append(_dialogLayer.DOScale(Vector3.zero, 0.25f).SetEase(Ease.InBack))
                .AppendCallback(() =>
                {
                    Destroy(gameObject);
                    OnDidHide?.Invoke(result);
                });
        }


        private void InitGunItem()
        {
            var entries = new[]
            {
                GunSkin.Bazooka,
                GunSkin.FireBlaster,
                GunSkin.Laser,
            };
            for (var i = 0; i < _upgradeItemViews.Length; i++)
            {
                var view = _upgradeItemViews[i];
                var entry = entries[i];
                var gunInfo = _skinGunManager.GetInfo(entry);
                var isOwned = gunInfo.IsOwned;
                var isSelected = gunInfo.IsSelected;
                view.GunType = entry;
                view.IsUnlock = isOwned;
                view.IsSelected = isSelected;
                if (isSelected)
                {
                    GunSelecting = entry;
                }

                view.OnItemViewPressed = () => { GunSelecting = entry; };
            }
        }


        private void UpdateView(GunSkin gun)
        {
            var gunInfo = _skinGunManager.GetInfo(gun);
            InventoryState = !gunInfo.IsOwned ? InventoryState.Buy :
                gunInfo.IsSelected ? InventoryState.Equipped : InventoryState.Equip;
            Cost = gunInfo.Cost;
            _gunSelector.GunType = gun;
            UpdateItemSelected(gun);
            UpdateUpgradeInfoTab();
        }

        private void UpdateUpgradeInfoTab()
        {
        }
        //


        private void UpdateStateDisplay(InventoryState state)
        {
            foreach (var layer in _stateAndButtons)
            {
                layer.button.gameObject.SetActive(false);
            }

            var view = _stateAndButtons.FirstOrDefault(p => p.inventoryState == state);
            if (view == null)
            {
                return;
            }

            view.button.gameObject.SetActive(true);
        }

        //To do
        private void OnStateCallBack(InventoryState state, GunSkin gunSkin)
        {
            switch (state)
            {
                case InventoryState.Equip:
                    EquipItem(gunSkin);
                    break;
                case InventoryState.Equipped:
                    //Return
                    break;
                case InventoryState.Buy:
                    BuyGun(gunSkin);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void EquipItem(GunSkin gun)
        {
            _skinGunManager.CurrentSkin = gun;
            InitGunItem();
            //To do equip inventory
        }

        private void BuyGun(GunSkin gunSkin)
        {
            if (Cost <= _storeManager.GetBalance(StoreItemId.Gold))
            {
                //Buy success
                _storeManager.AddBalance(StoreItemId.Gold, -Cost);
                var skinInfo = _skinGunManager.GetInfo(gunSkin);
                skinInfo.IsOwned = true;
                EquipItem(gunSkin);
            }
            else
            {
                var dialog = InfoDialog.Show(_canvas);
                dialog.Content = $"Not enough gold";
            }
        }

        private void UpdateItemSelected(GunSkin gun)
        {
            foreach (var view in _upgradeItemViews)
            {
                view.IsSelected = view.GunType == gun;
            }
        }

        public void OnStateInventoryButtonPressed()
        {
            OnButtonStateCallBack?.Invoke(InventoryState, GunSelecting);
        }

        public void OnCloseButtonPressed()
        {
            Hide(Result.Close);
        }

        public void OnSettingsButtonPressed()
        {
        }
    }
}