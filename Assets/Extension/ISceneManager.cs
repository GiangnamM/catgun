using System.Threading.Tasks;

using JetBrains.Annotations;

using UnityEngine;

namespace Extension {
    [Service(nameof(ISceneManager))]
    public interface ISceneManager {
        [NotNull]
        Task<T> LoadScene<T>([NotNull] string sceneName) where T : MonoBehaviour;
    }
}