using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using DG.Tweening;
using Extension;

namespace App
{
    public class MenuScene : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private Transform _btnPlayLayer;
        
        private bool _initialized;

        [Inject] private ISceneManager _sceneLoader;

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
            _tweens = new List<Tween>();
            var tween = DOTween.Sequence()
                .Append(_btnPlayLayer.DOScale(new Vector3(0.9f, 0.9f, 1f), 1f)).SetEase(Ease.OutSine)
                .Append(_btnPlayLayer.DOScale(Vector3.one, 1f)).SetEase(Ease.InSine)
                .SetLoops(-1);
            _tweens.Add(tween);
        }

        private void OnDestroy()
        {
            foreach (var tween in _tweens)
            {
                tween?.Kill();
            }
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
            // UniTask.Create(async () => {
            //     var dialog = SettingsDialog.Create(_canvas);
            //     await dialog.Show();
            // });
        }


        public void OnUpgradeButtonPressed()
        {
            var dialog = UpgradeDialog.Show(_canvas);
        }
    }
}