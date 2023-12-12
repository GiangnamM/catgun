using UnityEngine;

namespace App
{
    [ExecuteInEditMode]
    public class ParallaxLayer : MonoBehaviour
    {
        public float parallaxFactor;

        public void Move(float delta)
        {
            var trans = transform;
            var newPos = trans.localPosition;
            newPos.x -= delta * parallaxFactor;

            trans.localPosition = newPos;
        }
    }
}