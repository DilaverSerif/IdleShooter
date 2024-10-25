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
        private static readonly int ShootIndex = Animator.StringToHash("ShootIndex");
        private static readonly int WeaponType = Animator.StringToHash("WeaponType");

        private PlayerBrain _playerBrain;
        private Targeting<Damageable> _targeting;
        private Animator _playerAnimator;
        private Rigidbody _playerRigidbody;
        private PhysicsBasedCharacterController _playerController;
        private PlayerStacker _playerStacker;

        
        public Transform leftHintTransform;
        public Transform leftTargetTransform;
        
        public Transform rightHintTransform;
        public Transform rightTargetTransform;
        
        private void Awake()
        {
            _targeting = GetComponent<Targeting<Damageable>>();
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

        private void OnGunEquipped(Gun.Gun gun)
        {
            _playerAnimator.SetInteger(WeaponType, (int)gun.weaponType);
            
            Debug.Log("OnGunEquipped");
            gun.OnFireBullet += OnFireBullet;

            leftHintTransform.SetLocalPositionAndRotation(gun.leftHandIK.HintPosition,
                Quaternion.Euler(gun.leftHandIK.HintRotation));
            leftTargetTransform.SetLocalPositionAndRotation(gun.leftHandIK.TargetPosition,
                Quaternion.Euler(gun.leftHandIK.TargetRotation));
            
            rightHintTransform.SetLocalPositionAndRotation(gun.rightHandIK.HintPosition,
                Quaternion.Euler(gun.rightHandIK.HintRotation));
            rightTargetTransform.SetLocalPositionAndRotation(gun.rightHandIK.TargetPosition,
                Quaternion.Euler(gun.rightHandIK.TargetRotation));
        }

        private void OnFireBullet(int obj)
        {
            _playerAnimator.ResetTrigger(Shoot);
            _playerAnimator.SetTrigger(Shoot);
            _playerAnimator.SetInteger(ShootIndex, obj);
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