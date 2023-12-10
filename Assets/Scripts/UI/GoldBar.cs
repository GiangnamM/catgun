using System;

using DG.Tweening;

using Extension;
using TMPro;

using UnityEngine;

namespace App {
    public class GoldBar : MonoBehaviour {
        [SerializeField]
        private TextMeshProUGUI _amountText;

        [Inject] private IStoreManager _storeManager;
        private ObserverHandle _handle;
        private bool _initialized;
        private int _amount;
        private Tween _tween;

        public Action GoldBarPressedCallback = () => { };
        public Action PlusPressedCallback = () => { };

        public int Amount {
            get => _amount;
            set {
                Initialize();
                if (_amount == value) {
                    return;
                }
                _amount = value;
                UpdateAmount();
            }
        }

        private void Awake() {
            Initialize();
            UpdateAmount();
        }

        private void OnDestroy() {
            _handle.Dispose();
            _tween?.Kill();
            _tween = null;
        }

        private void Initialize() {
            if (_initialized) {
                return;
            }
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            _handle = new ObserverHandle();
            _handle.AddObserver(_storeManager, new StoreObserver() {
                OnItemBalanceChanged = OnItemBalanceChanged,
            });
            Amount = _storeManager.GetBalance(StoreItemId.Gold);
        }

        private void UpdateAmount() {
            _amountText.text = $"{Amount}";
        }

        public void OnGoldBarButtonPressed() {
            GoldBarPressedCallback?.Invoke();
        }
        public void OnPlusButtonPressed() {
            PlusPressedCallback?.Invoke();
        }

        private void OnItemBalanceChanged(string item, int balance, int amount) {
            if (item != StoreItemId.Gold) {
                return;
            }
            _tween?.Kill();
            _tween = DOTween.To(
                    () => Amount,
                    x => Amount = x,
                    balance,
                    1)
                .OnUpdate(UpdateAmount)
                .OnComplete(UpdateAmount);
        }
    }
}