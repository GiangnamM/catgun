using System;
using System.Collections;
using System.Collections.Generic;
using App;
using UnityEngine;

namespace App
{
    public class CharacterRenderer : MonoBehaviour
    {
        private const int MoveTrack = 2;
        private const int FireTrack = 1;

        private enum AnimState
        {
            IDLE,
            MOVE,
            DIE,
            JUMP,
            FIRE,
        }

        [SerializeField] private SkeletonAnimHelper _animHelper;

        [SerializeField] private Character _character;

        private bool _initialized;
        private Vector2 _direction;
        private AnimState _animState;
        private IDictionary<AnimState, string> _skinAndName;
        private bool _isSmile;
        private string _skinName;

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                var lastDir = _direction;
                _direction = value;
                UpdateDirection(lastDir);
                _animHelper.Direction = _direction;
            }
        }

        private AnimState State
        {
            get => _animState;
            set
            {
                var lastState = _animState;
                _animState = value;
                if (_animState != lastState)
                {
                    AnimStateChanged();
                }
            }
        }

        private void Awake()
        {
            Initialize();
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
        }

        private void Initialize()
        {
            if (_initialized)
            {
                return;
            }

            _initialized = false;
            _direction = Vector2.zero;
            _skinAndName = new Dictionary<AnimState, string>()
            {
                { AnimState.IDLE, "" },
                { AnimState.MOVE, "" },
                { AnimState.JUMP, "" },
                { AnimState.FIRE, "" },
                { AnimState.DIE, "" },
            };
            UpdateGunType();
        }

        private void AnimStateChanged()
        {
            switch (_animState)
            {
                case AnimState.IDLE:
                    Idle();
                    break;
                case AnimState.MOVE:
                    Move();
                    break;
                case AnimState.DIE:
                    Die();
                    break;
                case AnimState.JUMP:
                    Jump();
                    break;
                case AnimState.FIRE:
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private void UpdateGunType()
        {
            _skinAndName[AnimState.IDLE] = $"GUN 02/IDLE";
            _skinAndName[AnimState.MOVE] = $"GUN 02/RUN";
            _skinAndName[AnimState.JUMP] = $"GUN 02/JUMP";
            _skinAndName[AnimState.FIRE] = $"GUN 02/ATTACK GUN";
            _skinAndName[AnimState.DIE] = $"GUN 02/DIE GUN";
        }

        private void Jump()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.JUMP], false);
            _animHelper.Animation.AnimationState.ClearTrack(MoveTrack);
        }

        private void Move()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.MOVE], true, null, MoveTrack);
        }

        private void Idle()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.IDLE], true);
            _animHelper.Animation.AnimationState.ClearTrack(MoveTrack);
        }

        private void Die()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.DIE], false);
        }

        public void Fire()
        {
            _animHelper.PlayAnimByName(_skinAndName[AnimState.FIRE], false,
                () => { _animHelper.Animation.AnimationState.ClearTrack(FireTrack); }, FireTrack);
        }

        private void UpdateDirection(Vector2 lastDir)
        {
            if (!_character.IsGrounded)
            {
                State = AnimState.JUMP;
                return;
            }
            State = _direction.x != 0 ? AnimState.MOVE : AnimState.IDLE;
        }
    }
}