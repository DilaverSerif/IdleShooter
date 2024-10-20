using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _GAME_.Scripts
{
    public class PlayerStacker : MonoBehaviour
    {
        public Transform stackParent;
        public Action OnPutStack;
    
        public bool HoldStack => _stackObjects.Count > 0;
        [ShowInInspector]
        public float StackHeight => _stackObjects.Sum(stackObject => stackObject.GetStackHeight());
        private List<StackObject> _stackObjects = new();
        private CancellationTokenSource _cancellationTokenSource;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out StackObject stackObject))
            {
                if(stackObject.IsStacked) return;
                AddStackHeight(stackObject).Forget();
            }
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel();
        }

        public void JumpAnim()
        {
            if(_stackObjects.Count == 0) return;

            for (int i = 0; i < _stackObjects.Count; i++)
            {
                var stackObject = _stackObjects[i].transform;
                StepEffect(stackObject);
            }
        
            CalculateStackHeight();
        
            //ReadjustItem();
            // for (int i = 0; i < _stackObjects.Count; i++)
            // {
            //     var stackObject = _stackObjects[i].transform;
            //     JumpUp(stackObject).Forget();
            //     
            //     await UniTask.Delay(TimeSpan.FromSeconds(0.1f));
            // }
        }

        private void CalculateStackHeight()
        {
            for (int i = 0; i < _stackObjects.Count; i++)
            {
                var stackObject = _stackObjects[i];
                stackObject.transform.localPosition = new Vector3(0, GetStackHeightBeforeItem(stackObject), 0);
            }
        }
    
        private void ReadjustItem()
        {
            var backup = new List<StackObject>(_stackObjects);
            _stackObjects.Clear();
        
            foreach (var stackObject in backup)
            {
                AddStackHeight(stackObject).Forget();
            }
        }
    
        private float GetStackHeightBeforeItem(StackObject stackObject)
        {
            var index = _stackObjects.IndexOf(stackObject);
            if (index == 0) return 0;
        
            float height = 0;
            for (int i = 0; i < index; i++)
            {
                height += _stackObjects[i].GetStackHeight();
            }

            return height;
        }
    
        private void StepEffect(Transform stackObject)
        {
            var randRotation = UnityEngine.Random.rotation;
            stackObject.localRotation = randRotation;
        }
    
        private async UniTask JumpUp(Transform stackObject)
        {
            var jumpHeight = 1f;
            var jumpSpeed = 10f;
        
            Vector3 startPosition = stackObject.position;
            Vector3 jumpTargetPosition = startPosition + new Vector3(0, jumpHeight, 0);

            // Yukarı doğru zıplama hareketi
            while (Vector3.Distance(stackObject.position, jumpTargetPosition) > 0.01f)
            {
                stackObject.position = Vector3.MoveTowards(stackObject.position, jumpTargetPosition, jumpSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
        
            stackObject.position = jumpTargetPosition;
        
            // Aşağı doğru geri inme hareketi
            while (Vector3.Distance(stackObject.position, startPosition) > 0.01f)
            {
                stackObject.position = Vector3.MoveTowards(stackObject.position, startPosition, jumpSpeed * Time.deltaTime);
                await UniTask.Yield();
            }
        
            stackObject.position = startPosition;
        }
    
    
    
        private async UniTask AddStackHeight(StackObject stackObject)
        {
            var token = _cancellationTokenSource.Token;
            stackObject.SetStacked();
            _stackObjects.Add(stackObject);
            OnPutStack?.Invoke();
        
            var targetPosition = stackParent.position + new Vector3(0, StackHeight, 0);
    
            while (Vector3.Distance(stackObject.transform.position, targetPosition) > 0.01f)
            {
                targetPosition = stackParent.position + new Vector3(0, StackHeight, 0);
                stackObject.transform.position = Vector3.MoveTowards(stackObject.transform.position, targetPosition, Time.deltaTime * 10);
                await UniTask.Yield(cancellationToken:token);
            }
    
            stackObject.transform.SetParent(stackParent);
            stackObject.transform.localPosition = new Vector3(0, StackHeight, 0);
        }
    
#if UNITY_EDITOR
    
        [Button]
        private void JumpEffect()
        {
            JumpAnim();
        }
    
#endif

    }
}