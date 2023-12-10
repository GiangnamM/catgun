using System;

using System.Linq;

using Extension;

using UnityEngine;
using UnityEngine.UI;

namespace App {
    public enum InventoryState {
        Equip,
        Equipped,
        Buy,
    }

    public class UpgradeDialog : MonoBehaviour {
        
        [Serializable]
        private class StateAndButton {
            public InventoryState inventoryState;
            public Button button;
        }

        [SerializeField]
        private RectTransform _dialogLayer;

        [SerializeField]
        private UpgradeItemViewHelper[] _upgradeItemViews;

        [SerializeField]
        private StateAndButton[] _stateAndButtons;

        [SerializeField]
        private SkinGunSelector _gunSelector;

        [SerializeField]
        private UpgradeInfoTab _upgradeInfoTab;
        
        [Inject] private IStoreManager _storeManager;
        private bool _initialized;
        // private ObserverHandle _handle;
        private InventoryState _inventoryState;

        private string IdSelecting { get; set; }

        public int Cost {
            get => _upgradeInfoTab.Cost;
            set => _upgradeInfoTab.Cost = value;
        }

        
        public InventoryState InventoryState {
            get => _inventoryState;
            private set {
                _inventoryState = value;
                UpdateStateDisplay(value);
            }
        }

        private Action<InventoryState, string> OnButtonStateCallBack { get; set; }

        private void Awake() {
            Initialize();
            OnButtonStateCallBack = OnStateCallBack;
        }

        private void Initialize() {
            if (_initialized) {
                return;
            }
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            InitGunItem();
            UpdateDefaultDisplay();
            // _handle = new ObserverHandle();
            // _handle.AddObserver(_storeManager, new StoreManagerObserver {
            //     OnItemBalanceChanged = OnItemBalanceChanged,
            // });
        }
        
        private void InitGunItem() {
            // for (var i = 0; i < _upgradeItemViews.Length; i++) {
            //     var view = _upgradeItemViews[i];
            //     var idInventoryConfig = _configsIds[i];
            //     var (gun, upgradeItemId, _, enableUpgrade) = UpgradeHelper.ConfigInventories[idInventoryConfig];
            //     var isUnlock = _inventoryManager.IsOwned(idInventoryConfig);
            //     view.GunType = gun;
            //     view.IsUnlock = isUnlock;
            //     view.EnableUpgrade = enableUpgrade;
            //     if (enableUpgrade) {
            //         var upgradeItem = _upgradeManager.GetItem(upgradeItemId);
            //         if (upgradeItem != null) {
            //             view.Level = upgradeItem.Level + 1;
            //         }
            //     }
            //     var itemEquipped = _inventoryManager.GetEquippedItem(CategoryGun);
            //     var isEquipped = itemEquipped != null && itemEquipped.Id == idInventoryConfig;
            //     view.OnItemViewPressed = () => {
            //         //Update inventory
            //         IdSelecting = idInventoryConfig;
            //         InventoryState = !isUnlock ? InventoryState.Buy :
            //             isEquipped ? InventoryState.Equipped : InventoryState.Equip;
            //         if (view.IsSelected) {
            //             return;
            //         }
            //         _gunSelector.GunType = gun;
            //         UpdateItemSelected(view);
            //         UpdateUpgradeInfoTab(idInventoryConfig);
            //     };
            // }
        }

        // private int GetCostUpgradeItem(string upgradeItemId, string storeItemId) {
        //     
        // }

        // private void UpdateUpgradeInfoTab(string idInventoryConfig) {
        //    
        // }
        //
        // private int GetLevelUpgrade(string id) {
        //     
        // }

        private void OnButtonUpgradeCallBack(string upgradeItemId) {
            
        }

        private void UpdateStateDisplay(InventoryState state) {
            foreach (var layer in _stateAndButtons) {
                layer.button.gameObject.SetActive(false);
            }
            var view = _stateAndButtons.FirstOrDefault(p => p.inventoryState == state);
            if (view == null) {
                return;
            }
            view.button.gameObject.SetActive(true);
        }

        //To do
        private void OnStateCallBack(InventoryState state, string id) {
            switch (state) {
                case InventoryState.Equip:
                    EquipItem(id);
                    break;
                case InventoryState.Equipped:
                    //Return
                    break;
                case InventoryState.Buy:
                    OpenUnlockGunDialog(id);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        private void EquipItem(string id) {
            //To do equip inventory
            
        }

        private void OpenUnlockGunDialog(string id) {
            
        }

        private void UpdateDefaultDisplay() {
           
        }

        private void UpdateItemSelected(UpgradeItemViewHelper item) {
            foreach (var view in _upgradeItemViews) {
                view.IsSelected = false;
            }
            item.IsSelected = true;
        }

        private void OnItemBalanceChanged(IStoreItem item, int balance, int amount) {
            
        }

        public void OnStateInventoryButtonPressed() {
            
        }

        public void OnCloseButtonPressed() {
            
        }

        public void OnSettingsButtonPressed() {
            
        }

        public void CheatPieceUpgrade(GunSkin gun, int amount) {
            
        }
    }
}