using System.Threading.Tasks;

using JetBrains.Annotations;

using UnityEngine;

namespace Extension {
    [Service(nameof(ISceneManager))]
    public interface ISceneManager {
        /// <summary>
        /// Loads the specified scene.
        /// </summary>
        [NotNull]
        Task<T> LoadScene<T>([NotNull] string sceneName) where T : MonoBehaviour;
    }
}