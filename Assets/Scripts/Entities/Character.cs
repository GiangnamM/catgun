using UnityEngine;

namespace App
{
    public class Character : MonoBehaviour
    {
        private const int MaxJump = 1;
        private const float MoveSpeed = 6.1f;
        private const float JumpPower = 14f;

        [SerializeField] private CharacterRenderer _characterRenderer;

        private Rigidbody2D _body;
        private Collider2D _collider;
        private int _jumpCount;
        private bool _isGrounded;
        private Vector2 _direction;
        private Vector2 _lastDirection;
        private bool _invincible;
        private float _horizontal;
        private float _vertical;
        private bool _isDead;

        public bool IsGrounded
        {
            get => _isGrounded;
            private set
            {
                _isGrounded = value;
                if (_isGrounded)
                {
                    _jumpCount = 0;
                }
            }
        }

        public Vector2 Velocity
        {
            get => _body.velocity;
            set
            {
                if (_body.velocity != value)
                {
                    _body.velocity = value;
                }
            }
        }

        public Vector2 Direction
        {
            get => _direction;
            set
            {
                _direction = value;
                if (_direction.x != 0)
                {
                    _lastDirection = _direction;
                }

                UpdateDirection();
                _characterRenderer.Direction = _direction;
            }
        }

        public bool IsStunning { get; private set; }

        private void Awake()
        {
            
        }

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            if (Mathf.Abs(_body.velocity.y) < 0.1f && !IsGrounded)
            {
                IsGrounded = IsOnGround();
            }
        }

        private void FixedUpdate()
        {
            if (_isDead) return;
            Direction = new Vector2(_horizontal, _vertical);
        }

        private void UpdateDirection()
        {
            Move(_direction);
            if (_direction.y > 0f)
            {
                Jump();
            }
        }

        private void Move(Vector2 dir)
        {
            Velocity = new Vector2(dir.x * MoveSpeed, Velocity.y);
        }

        private bool IsOnGround()
        {
            var raycastHit = Physics2D.BoxCast(_collider.bounds.center - new Vector3(0f, 0.2f, 0f),
                _collider.bounds.size, 0, Vector2.down,
                0.1f, LayerMask.GetMask("Wall"));
            return raycastHit.collider != null;
        }

        public void Jump()
        {
            if (_jumpCount == MaxJump || _isDead)
            {
                return;
            }

            _jumpCount++;
            IsGrounded = false;
            Velocity = new Vector2(Velocity.x, JumpPower);
        }

        private void Fire()
        {
            // var gunType = GunType;
            // if (!_gun.Fire()) {
            //     return;
            // }
            // _characterRenderer.Fire();
            // PlayGunSound(gunType);
            // for (var i = 0; i < _gun.Config.Angles.Length; i++) {
            //     var angle = _gun.Config.Angles[i];
            //     var startY = _gun.Config.StartY[i];
            //         SpawnNormalBullet(gunType, angle, 0f, startY);
            // }
        }

        public void Completed()
        {
            Debug.Log("Complete");
            _isDead = true;
            Move(Vector2.zero);
        }
    }
}