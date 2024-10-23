using _BASE_.Joystick_Pack.Scripts.Base;
using UnityEngine;
namespace _BASE_.Scripts
{
    public class SimpleBasedCharacterController : MonoBehaviour
    {
        public float acceleration;
        public float deceleration;
        public float gravity = -9.8f;
        public float rotationSpeed = 10f;
        
        private Rigidbody _characterController;
        private Vector3 _targetPos;
        void Awake()
        {
            _characterController = GetComponent<Rigidbody>();
        }

        private void FixedUpdate()
        {
            if (Joystick.Instance.Moved)
                JoyStickMove();
            else
                Locomotion();
        }

        private float Gravity()
        {
            return gravity * Time.deltaTime;
        }

        private void JoyStickMove()
        {
            var direction = Joystick.Instance.Direction;
            var distance = Joystick.Instance.DistanceJoyStick();
            
            SetRotation(direction);

            _targetPos = transform.forward * (distance * acceleration * Time.deltaTime);
            _targetPos.y = Gravity();
            _characterController.velocity = _targetPos;
        }
        
        private void Locomotion()
        {
            _targetPos = Vector3.Lerp(_targetPos, Vector3.zero, Time.deltaTime * deceleration);
            _characterController.velocity = _targetPos;
        }

        private void SetRotation(Vector3 move)
        {
            float angleA = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
            
            var transformRotation = transform.rotation;
            var targetRotation = Quaternion.Euler(0f, angleA, 0f);
            transform.rotation = Quaternion.Lerp(transformRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }
}

