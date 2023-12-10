using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Extension;
using TMPro;

namespace App
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Transform _btnPlayLayer;

        [SerializeField] private Transform _btnWatchAdsLayer;

        [SerializeField] private Transform _skinWarningLayer;

        [SerializeField] private TextMeshProUGUI _rewardText;

        private bool _initialized;

        [Inject] private ISceneManager _sceneLoader;

        // [Inject] private ILevelService _levelService;
        [Inject] private IStoreManager _storeManager;


        private List<Tween> _tweens;

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
        }

        private void Awake()
        {
            Initialize();
            // _tweens = new List<Tween>();
            // var tween = DOTween.Sequence()
            //     .Append(_btnPlayLayer.DOScale(new Vector3(0.9f, 0.9f, 1f), 1f)).SetEase(Ease.OutSine)
            //     .Append(_btnPlayLayer.DOScale(Vector3.one, 1f)).SetEase(Ease.InSine)
            //     .SetLoops(-1);
            // _tweens.Add(tween);
            // tween = DOTween.Sequence()
            //     .Append(_btnWatchAdsLayer.DOScale(new Vector3(0.9f, 0.9f, 1f), 1f)).SetEase(Ease.OutSine)
            //     .Append(_btnWatchAdsLayer.DOScale(Vector3.one, 1f)).SetEase(Ease.InSine)
            //     .SetLoops(-1);
            // _tweens.Add(tween);
            // AnimWarningSkin();
        }

        private void OnDestroy()
        {
        }


        public void OnPlayButtonPressed()
        {
            // _audioManager.PlaySound(Audio.Button);
            UniTask.Create(async () =>
            {
                await TransitionLayer.Fade(0.3f,
                    async () => await _sceneLoader.LoadScene<LevelScene>(nameof(LevelScene)));
            });
        }

        public void OnSettingsButtonPressed()
        {
            // _logManager.Log();
            // _analyticsManager.LogEvent(new ClickEvent {
            //     Button = "settings",
            // });
            // _audioManager.PlaySound(Audio.Button);
            // UniTask.Create(async () => {
            //     var dialog = SettingsDialog.Create(_canvas);
            //     await dialog.Show();
            // });
        }

        public void OnBonusButtonPressed()
        {
            // _logManager.Log();
            // _analyticsManager.LogEvent(new ClickEvent {
            //     Button = "bonus",
            // });
            // _audioManager.PlaySound(Audio.Button);
            // UniTask.Create(async () => {
            //     var (result, message) = await _adsManager.ShowRewardedAd();
            //     if (result == AdResult.Completed) {
            //         _storeManager.AddBalance(StoreItemId.Gold, Constant.REWARD_WATCH_ADS);
            //     } else {
            //         var dialog = InfoDialog.Create(_canvas);
            //         dialog.Content = message;
            //         await dialog.Show();
            //     }
            // });
        }

        public void OnShopButtonPressed()
        {
            // _audioManager.PlaySound(Audio.Button);
            // _analyticsManager.LogEvent(new ClickEvent {
            //     Button = "shop",
            // });
            // UniTask.Create(async () => {
            //     var dialog = ShopDialog.Create(_canvas);
            //     dialog.SelectedTab = ShopDialog.TabType.Gold;
            //     await dialog.Show();
            // });
        }
    }
}