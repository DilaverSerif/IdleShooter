using System;
using _BASE_.Joystick_Pack.Scripts.Base;
using _BASE_.Scripts.Stat_System;
using _GAME_.Scripts;
using UnityEngine;

namespace _BASE_.Scripts
{
    public class PhysicsBasedCharacterController : MonoBehaviour, IStatOwner
    {
        [SerializeField] private PlayerBrain _playerBrain;
        private Rigidbody _rb;
        private Vector3 _gravitationalForce;
        private Vector3 _rayDir = Vector3.down;
        private Vector3 _previousVelocity = Vector3.zero;
        private Vector2 _moveContext;

        [Header("Other:")]
        [SerializeField] private bool _adjustInputsToCameraAngle = false;
        [SerializeField] private LayerMask _terrainLayer;
        // [SerializeField] private ParticleSystem _dustParticleSystem;

        private bool _shouldMaintainHeight = true;

        [Header("Height Spring:")]
        [SerializeField] private float _rideHeight = 1.75f;
        [SerializeField] private float _rayToGroundLength = 3f;
        [SerializeField] public float _rideSpringStrength = 50f;
        [SerializeField] private float _rideSpringDamper = 5f;
        //[SerializeField] private Oscillator _squashAndStretchOcillator;

        public enum lookDirectionOptions { velocity, acceleration, moveInput,targetDirection };
        private Quaternion _uprightTargetRot = Quaternion.identity;
        private Quaternion _lastTargetRot;
        private Vector3 _platformInitRot;
        private bool didLastRayHit;

        [Header("Upright Spring:")]
        [SerializeField] private lookDirectionOptions _characterLookDirection = lookDirectionOptions.velocity;
        [SerializeField] private float _uprightSpringStrength = 40f;
        [SerializeField] private float _uprightSpringDamper = 5f;

        private Vector3 _moveInput;
        private float _speedFactor = 1f;
        private float _maxAccelForceFactor = 1f;
        private Vector3 _m_GoalVel = Vector3.zero;

        [Header("Movement:")]
        [SerializeField] private float _maxSpeed = 8f;
        [SerializeField] private float _acceleration = 200f;
        [SerializeField] private float _maxAccelForce = 150f;
        [SerializeField] private float _leanFactor = 0.25f;
        [SerializeField] private AnimationCurve _accelerationFactorFromDot;
        [SerializeField] private AnimationCurve _maxAccelerationForceFactorFromDot;
        [SerializeField] private Vector3 _moveForceScale = new Vector3(1f, 0f, 1f);

        private Vector3 _jumpInput;
        private float _timeSinceJumpPressed = 0f;
        private float _timeSinceUngrounded = 0f;
        private float _timeSinceJump = 0f;
        private bool _jumpReady = true;
        private bool _isJumping = false;

        [Header("Jump:")]
        [SerializeField] private float _jumpForceFactor = 10f;
        [SerializeField] private float _riseGravityFactor = 5f;
        [SerializeField] private float _fallGravityFactor = 10f;
        [SerializeField] private float _lowJumpFactor = 2.5f;
        [SerializeField] private float _jumpBuffer = 0.15f;
        [SerializeField] private float _coyoteTime = 0.25f;
        public float MaxSpeed => _maxSpeed;

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            _gravitationalForce = Physics.gravity * _rb.mass;
        }

        private void Update()
        {
            _moveContext = Joystick.Instance.Direction;
            _jumpInput = new Vector3(0, Input.GetAxis("Jump"), 0);
        }

        public void SetLookDirection(lookDirectionOptions lookDirection)
        {
            _characterLookDirection = lookDirection;
        }
        
        private bool CheckIfGrounded(bool rayHitGround, RaycastHit rayHit)
        {
            bool checkIfGrounded;
            if (rayHitGround)
            {
                checkIfGrounded = rayHit.distance <= _rideHeight * 1.3f;
            }
            else
            {
                checkIfGrounded = false;
            }
            return checkIfGrounded;
        }

        private Vector3 GetLookDirection(lookDirectionOptions lookDirectionOption)
        {
            Vector3 lookDirection;
        
            switch (lookDirectionOption)
            {
                case lookDirectionOptions.velocity:
                case lookDirectionOptions.acceleration:
                {
                    Vector3 velocity = _rb.velocity;
                    velocity.y = 0f;
                    if (lookDirectionOption == lookDirectionOptions.velocity)
                    {
                        lookDirection = velocity;
                    }
                    else
                    {
                        Vector3 deltaVelocity = velocity - _previousVelocity;
                        _previousVelocity = velocity;
                        Vector3 acceleration = deltaVelocity / Time.fixedDeltaTime;
                        lookDirection = acceleration;
                    }

                    break;
                }
                case lookDirectionOptions.moveInput:
                    lookDirection = _moveInput;
                    break;
                case lookDirectionOptions.targetDirection:
                    if(_playerBrain.Targeting.currentTarget == null)
                    {
                        lookDirection = Vector3.zero;
                        break;
                    }
                    lookDirection = (_playerBrain.Targeting.currentTarget.transform.position - transform.position).normalized;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(lookDirectionOption), lookDirectionOption, null);
            }
            return lookDirection;
        }

        public bool grounded;
        private void FixedUpdate()
        {
            _moveInput = new Vector3(_moveContext.x, 0, _moveContext.y);
            
            (var rayHitGround, RaycastHit rayHit) = RaycastToGround();
            // SetPlatform(rayHit);

            grounded = CheckIfGrounded(rayHitGround, rayHit);
            
            if (grounded)
            {
                _timeSinceUngrounded = 0f;

                if (_timeSinceJump > 0.2f)
                {
                    _isJumping = false;
                }
            }
            else
            {
                _timeSinceUngrounded += Time.fixedDeltaTime;
            }

            CharacterMove(_moveInput);
            CharacterJump(_jumpInput, grounded, rayHit);

            if (rayHitGround && _shouldMaintainHeight)
            {
                MaintainHeight(rayHit);
            }

            Vector3 lookDirection = GetLookDirection(_characterLookDirection);
            MaintainUpright(lookDirection, rayHit);
        }

        private (bool, RaycastHit) RaycastToGround()
        {
            Ray rayToGround = new Ray(transform.position, _rayDir);
            bool rayHitGround = Physics.Raycast(rayToGround, out var rayHit, _rayToGroundLength, _terrainLayer.value);
            return (rayHitGround, rayHit);
        }

        private void MaintainHeight(RaycastHit rayHit)
        {
            Vector3 vel = _rb.velocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;
            if (hitBody != null)
            {
                otherVel = hitBody.velocity;
            }
            float rayDirVel = Vector3.Dot(_rayDir, vel);
            float otherDirVel = Vector3.Dot(_rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float currHeight = rayHit.distance - _rideHeight;
            float springForce = (currHeight * _rideSpringStrength) - (relVel * _rideSpringDamper);
            Vector3 maintainHeightForce = - _gravitationalForce + springForce * Vector3.down;
            _rb.AddForce(maintainHeightForce);

            if (hitBody != null)
            {
                hitBody.AddForceAtPosition(-maintainHeightForce, rayHit.point);
            }
        }

        private void CalculateTargetRotation(Vector3 yLookAt, RaycastHit rayHit = new())
        {
            if (didLastRayHit)
            {
                _lastTargetRot = _uprightTargetRot;
                try
                {
                    _platformInitRot = transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    _platformInitRot = Vector3.zero;
                }
            }
            didLastRayHit = rayHit.rigidbody == null;

            if (yLookAt != Vector3.zero)
            {
                _uprightTargetRot = Quaternion.LookRotation(yLookAt, Vector3.up);
                _lastTargetRot = _uprightTargetRot;
                try
                {
                    _platformInitRot = transform.parent.rotation.eulerAngles;
                }
                catch
                {
                    _platformInitRot = Vector3.zero;
                }
            }
            else
            {
                try
                {
                    Vector3 platformRot = transform.parent.rotation.eulerAngles;
                    Vector3 deltaPlatformRot = platformRot - _platformInitRot;
                    float yAngle = _lastTargetRot.eulerAngles.y + deltaPlatformRot.y;
                    _uprightTargetRot = Quaternion.Euler(new Vector3(0f, yAngle, 0f));
                }
                catch
                {
                    // ignored
                }
            }
        }

        private void MaintainUpright(Vector3 yLookAt, RaycastHit rayHit = new())
        {
            CalculateTargetRotation(yLookAt, rayHit);

            Quaternion currentRot = transform.rotation;
            Quaternion toGoal = MathsUtils.ShortestRotation(_uprightTargetRot, currentRot);

            Vector3 rotAxis;
            float rotDegrees;

            toGoal.ToAngleAxis(out rotDegrees, out rotAxis);
            rotAxis.Normalize();

            float rotRadians = rotDegrees * Mathf.Deg2Rad;

            _rb.AddTorque((rotAxis * (rotRadians * _uprightSpringStrength)) - (_rb.angularVelocity * _uprightSpringDamper));
        }
    

        private void CharacterMove(Vector3 moveInput)
        {
            Vector3 mUnitGoal = moveInput;
            Vector3 unitVel = _m_GoalVel.normalized;
            float velDot = Vector3.Dot(mUnitGoal, unitVel);
            float accel = _acceleration * _accelerationFactorFromDot.Evaluate(velDot);
            Vector3 goalVel = mUnitGoal * (_maxSpeed * _speedFactor);
 
            _m_GoalVel = Vector3.MoveTowards(_m_GoalVel,
                goalVel,
                accel * Time.fixedDeltaTime);
            Vector3 neededAccel = (_m_GoalVel - _rb.velocity) / Time.fixedDeltaTime;
            float maxAccel = _maxAccelForce * _maxAccelerationForceFactorFromDot.Evaluate(velDot) * _maxAccelForceFactor;
            neededAccel = Vector3.ClampMagnitude(neededAccel, maxAccel);
            _rb.AddForceAtPosition(Vector3.Scale(neededAccel * _rb.mass, _moveForceScale), transform.position + new Vector3(0f, transform.localScale.y * _leanFactor, 0f));
        }

        private void CharacterJump(Vector3 jumpInput, bool grounded, RaycastHit rayHit)
        {
            _timeSinceJumpPressed += Time.fixedDeltaTime;
            _timeSinceJump += Time.fixedDeltaTime;
        
            switch (_rb.velocity.y)
            {
                case < 0:
                {
                    _shouldMaintainHeight = true;
                    _jumpReady = true;
                    if (!grounded)
                    {
                        _rb.AddForce(_gravitationalForce * (_fallGravityFactor - 1f));
                    }

                    break;
                }
                case > 0:
                {
                    if (!grounded)
                    {
                        if (_isJumping)
                        {
                            _rb.AddForce(_gravitationalForce * (_riseGravityFactor - 1f));
                        }
                        if (jumpInput == Vector3.zero)
                        {
                            _rb.AddForce(_gravitationalForce * (_lowJumpFactor - 1f));
                        }
                    }

                    break;
                }
            }

            if (!(_timeSinceJumpPressed < _jumpBuffer)) return;
            if (!(_timeSinceUngrounded < _coyoteTime)) return;
            if (!_jumpReady) return;
        
            _jumpReady = false;
            _shouldMaintainHeight = false;
            _isJumping = true;
            _rb.velocity = new Vector3(_rb.velocity.x, 0f, _rb.velocity.z);
            if (rayHit.distance != 0)
            {
                _rb.position = new Vector3(_rb.position.x, _rb.position.y - (rayHit.distance - _rideHeight), _rb.position.z);
            }
            _rb.AddForce(Vector3.up * _jumpForceFactor, ForceMode.Impulse);
            _timeSinceJumpPressed = _jumpBuffer;
            _timeSinceJump = 0f;
        }

        public float GetSpeedRelativeToMaxSpeed()
        {
            var velocity = _rb.velocity;
            return velocity.magnitude / _maxSpeed;
        }

        public StatOwner StatOwner { get; set; }
        public void UpdateStat(Stat stat)
        {
            if (stat.statTag == StatTags.BaseSpeed)
                _maxSpeed += stat.value;
        }
    }
}