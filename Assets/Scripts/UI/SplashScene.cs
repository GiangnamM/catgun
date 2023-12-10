using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Extension;
using UnityEngine;

namespace App
{
    public class SplashScene : MonoBehaviour
    {
        // [Inject] private IAudioManager _audioManager;
        // [Inject] private
        [Inject] private ISceneManager _sceneManager;

        private void Awake()
        {
            Application.targetFrameRate = 60;
            UniTask.Create(async () =>
            {
                await ServiceUtils.Initialize();
                ServiceLocator.Instance.ResolveInjection(this);
                // _audioManager.PlayMusic(Audio.Menu);
                await Task.Delay(3000);
                await TransitionLayer.Fade(0.3f,
                    async () => await _sceneManager.LoadScene<MenuScene>(nameof(MenuScene)));
            });
        }
    }
}