using Extension;
using UnityEngine;

namespace App
{
    public class Character : MonoBehaviour
    {
        private const int MaxJump = 1;
        private const float MoveSpeed = 6.1f;
        private const float JumpPower = 14f;
        private const float BulletSpeed = 15f;

        [SerializeField] private CharacterRenderer _characterRenderer;

        [SerializeField] private DefaultConfigManager _configManager;

        [SerializeField] private Bullet _bullet;

        [Inject] private ISkinGunManager _skinGunManager;
        private Rigidbody2D _body;
        private Collider2D _collider;
        private int _jumpCount;
        private bool _isGrounded;
        private Vector2 _direction;
        private Vector2 _lastDirection;
        private float _horizontal;
        private float _vertical;
        private bool _isDead;
        private Gun _gun;
        private GunSkin _gunSkin;
        private bool _initialized;

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

        public GunSkin GunSkin
        {
            get => _gunSkin;
            set
            {
                _gunSkin = value;
                Initialize();
                UpdateGunConfig();
            }
        }


        public bool IsStunning { get; private set; }

        private void Awake()
        {
            Initialize();
            GunSkin = GunSkin.Bazooka;
        }

        private void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            ServiceLocator.Instance.ResolveInjection(this);
            UpdateSkin();
            _gun = GetComponent<Gun>();
        }

        void Start()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<Collider2D>();
            _lastDirection = Vector2.right;
        }

        // Update is called once per frame
        void Update()
        {
            _horizontal = Input.GetAxisRaw("Horizontal");
            _vertical = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }

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

        private void UpdateSkin()
        {
            
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
            var gunType = GunSkin;
            if (!_gun.Fire())
            {
                return;
            }

            _characterRenderer.Fire();
            SpawnNormalBullet(gunType);
        }

        private void UpdateGunConfig()
        {
            _gun.FireRate = _configManager.GunBaseInfo[_gunSkin].Item2;
        }

        private void SpawnNormalBullet(GunSkin gunType, float startX = 0f, float startY = 0f)
        {
            var bullet = Instantiate(_bullet);
            var trans = bullet.transform;
            trans.localPosition = _gun.GunPos + new Vector3(startX, startY, -0.01f);
            bullet.Direction = _lastDirection;
            bullet.Speed = BulletSpeed;
            bullet.Damage = _configManager.GunBaseInfo[gunType].Item1;
            bullet.Skin = gunType;
        }

        public void Completed()
        {
            Debug.Log("Complete");
            _isDead = true;
            Move(Vector2.zero);
        }
    }
}