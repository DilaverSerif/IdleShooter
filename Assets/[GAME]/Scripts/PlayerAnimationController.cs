using _BASE_.Joystick_Pack.Scripts.Base;
using _BASE_.Scripts;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private static readonly int Speed = Animator.StringToHash("Speed");
        private static readonly int Stack = Animator.StringToHash("Stack");
        private static readonly int SideDir = Animator.StringToHash("SideDir");
        private static readonly int Target = Animator.StringToHash("Target");
        private static readonly int Shoot = Animator.StringToHash("Shoot");

        private PlayerBrain _playerBrain;
        private Targeting<EnemyHealth> _targeting;
        private Animator _playerAnimator;
        private Rigidbody _playerRigidbody;
        private PhysicsBasedCharacterController _playerController;
        private PlayerStacker _playerStacker;
        private void Awake()
        {
            _targeting = GetComponent<Targeting<EnemyHealth>>();
            _playerAnimator = GetComponentInChildren<Animator>();
            _playerRigidbody = GetComponent<Rigidbody>();
            _playerController = GetComponent<PhysicsBasedCharacterController>();
            _playerStacker = GetComponent<PlayerStacker>();
        }
    
        private void OnEnable()
        {
            _playerStacker.OnPutStack += OnPutStack;
            PlayerEquipment.OnGunEquipped += OnGunEquipped;
        }
    
        private void OnDisable()
        {
            _playerStacker.OnPutStack -= OnPutStack;
            PlayerEquipment.OnGunEquipped -= OnGunEquipped;
        }

        private void OnGunEquipped(Gun.Gun obj)
        {
            obj.OnFireBullet += () => _playerAnimator.SetTrigger(Shoot);
        }

        private void OnPutStack()
        {
            _playerAnimator.SetBool(Stack,_playerStacker.HoldStack);
        }
    
        // Update is called once per frame
        void Update()
        {
            _playerAnimator.SetBool(Target, _targeting.currentTarget);
            if (_targeting.currentTarget)
            {
                SideDirByTarget();
                return;
            }
            
            var velocity = _playerRigidbody.velocity;
            velocity.y = 0;
            var currentSpeedRelativeToMaxSpeed = velocity.magnitude / _playerController.MaxSpeed;
            _playerAnimator.SetFloat(Speed, currentSpeedRelativeToMaxSpeed);
        }

        private Vector3 _lastDirection;
        private void SideDirByTarget()
        {
            var transformForward = transform.forward;
            var joyStickDirection = Joystick.Instance.InputDirection;
                
            if(joyStickDirection != Vector3.zero)
                _lastDirection = joyStickDirection;
            
            var zAxis = _lastDirection.z;
            var xAxis = _lastDirection.x;

            var direction = new Vector3(
                (0 - transformForward.x) * zAxis + transformForward.z * xAxis,
                0,
                transformForward.x * xAxis + transformForward.z * zAxis);
        
            
            direction *= _playerController.GetSpeedRelativeToMaxSpeed();
            _playerAnimator.SetFloat(SideDir, direction.x);
            _playerAnimator.SetFloat(Speed, direction.z);
        }
    }
}