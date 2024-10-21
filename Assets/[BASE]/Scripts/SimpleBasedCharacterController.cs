using _BASE_.Joystick_Pack.Scripts.Base;
using UnityEngine;
namespace _BASE_.Scripts
{
    public class SimpleBasedCharacterController : MonoBehaviour
    {
        private CharacterController _characterController;
        private Vector3 _targetPos;
        public float maxSpeed;
        public float gravity = -9.8f;
        public float rotationSpeed = 10f;
        void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            if (Joystick.Instance.Moved)
                JoyStickMove();
            else
                Locomotion();
        }

        private void JoyStickMove()
        {
            var direction = Joystick.Instance.Direction;
            var distance = Joystick.Instance.DistanceJoyStick();
            
            SetRotation(Joystick.Instance.Direction);

            _targetPos = transform.forward * (Joystick.Instance.DistanceJoyStick() * maxSpeed * Time.deltaTime);
            _targetPos.y = gravity * Time.deltaTime;
            _characterController.Move(_targetPos);
        }
        
        private void Locomotion()
        {
            _targetPos = Vector3.Lerp(_targetPos, Vector3.zero, Time.deltaTime * maxSpeed);
            _characterController.Move(_targetPos);
        }

        private void SetRotation(Vector3 move)
        {
            float angleA = Mathf.Atan2(move.x, move.y) * Mathf.Rad2Deg;
            
            var transformRotation = transform.rotation;
            var targetRotation = Quaternion.Euler(0f, angleA, 0f);
            var shortestRotation = ShortestRotation(transformRotation, targetRotation);
            transform.rotation = Quaternion.Lerp(transformRotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
        
        public static Quaternion ShortestRotation(Quaternion a, Quaternion b)
        {
            if (Quaternion.Dot(a, b) < 0)
            {
                return a * Quaternion.Inverse(Multiply(b, -1));
            }

            else return a * Quaternion.Inverse(b);
        }
        
        public static Quaternion Multiply(Quaternion input, float scalar)
        {
            return new Quaternion(input.x * scalar, input.y * scalar, input.z * scalar, input.w * scalar);
        }

    }
}

