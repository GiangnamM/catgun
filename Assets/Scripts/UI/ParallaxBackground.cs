using System.Collections.Generic;
using UnityEngine;

namespace App
{
    public class ParallaxBackground : MonoBehaviour
    {
        private const int MaxBackground = 4;

        [SerializeField] private List<SpriteRenderer> _bg;

        [SerializeField] private Transform _snowLayer;

        [SerializeField] private Transform _fireLayer;

        [SerializeField] private Transform _fallingLeafLayer;

        private bool _hasSnow;
        private bool _hasFire;
        private bool _hasFallingLeaf;

        public bool HasSnow
        {
            get => _hasSnow;
            set
            {
                _hasSnow = value;
                _snowLayer.gameObject.SetActive(_hasSnow);
            }
        }

        public bool HasFire
        {
            get => _hasFire;
            set
            {
                _hasFire = value;
                _fireLayer.gameObject.SetActive(_hasFire);
            }
        }

        public bool HasFallingLeaf
        {
            get => _hasFallingLeaf;
            set
            {
                _hasFallingLeaf = value;
                _fallingLeafLayer.gameObject.SetActive(_hasFallingLeaf);
            }
        }

        public void SetBackground(List<Sprite> bg)
        {
            for (var i = 0; i < bg.Count && i < MaxBackground; i++)
            {
                _bg[i].sprite = bg[i];
                _bg[i].gameObject.SetActive(bg[i] != null);
            }
        }
    }
}