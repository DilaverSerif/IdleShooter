using System.Collections.Generic;
using UnityEngine;
public static class AttackSystemExtensions
{
    public static T CheckCollisionWithRayCast<T>(this Transform origin, float angle, float length, int rayCount, LayerMask layerMask) where T: MonoBehaviour
    {
        for (int i = 0; i < rayCount; i++)
        {
            float lerpFactor = i / (float)(rayCount - 1);
            float currentAngle = Mathf.Lerp(angle, -angle, lerpFactor);

            var direction = Quaternion.Euler(0, currentAngle, 0) * origin.forward;
            
            RaycastHit hit;
            if (Physics.Raycast(origin.position, direction, out hit, length, layerMask))
            {
                #if UNITY_EDITOR
                Debug.DrawRay(origin.position, direction * length, Color.yellow, 1);
                #endif
                if(hit.transform.TryGetComponent(out T component))
                    return component;
                else Debug.LogError($"Component {typeof(T)} not found on {origin.name}");
            }
            #if UNITY_EDITOR
            else
            {
                Debug.DrawRay(origin.position, direction * length, Color.red, 1);
            }
            #endif
        }

        return null;
    }
    
    
    public static List<T> CheckCollisionWithRayCastArray<T>(this Transform origin, float angle, float length, int rayCount, LayerMask layerMask,bool dontSame = false, Vector3 offset = default) where T: MonoBehaviour
    {
        var components = new List<T>();
        for (int i = 0; i < rayCount; i++)
        {
            float lerpFactor = i / (float)(rayCount - 1);
            float currentAngle = Mathf.Lerp(angle, -angle, lerpFactor);

            var direction = Quaternion.Euler(0, currentAngle, 0) * origin.forward;

            if (Physics.Raycast(origin.position + offset, direction, out var hit, length, layerMask))
            {
                #if UNITY_EDITOR
                Debug.DrawRay(origin.position + offset, direction * length, Color.yellow, 1);
                #endif
                if (hit.transform.TryGetComponent(out T component))
                {
                    if(dontSame)
                        if(components.Contains(component)) continue;
                    components.Add(component);
                }
                Debug.LogWarning($"Component {typeof(T)} not found on {origin.name}");
            }
            #if UNITY_EDITOR
            else
            {
                Debug.DrawRay(origin.position + offset, direction * length, Color.red, 1);
            }
            #endif
        }

        return components.Count > 0 ? components : null;
    }
    
    public static T CheckCollisionWithSphereCast<T>(this Transform origin, 
        float radius, LayerMask layerMask,float distance = 0f,int arraySize = 3) where T: MonoBehaviour
    {
        
        var hits = new RaycastHit[arraySize];
        var hitCount = Physics.SphereCastNonAlloc(origin.position, radius, Vector3.forward, hits, distance, layerMask);

        for (var i = 0; i < hitCount; i++)
        {
            if (hits[i].collider == null) continue;
            if(hits[i].collider.TryGetComponent(out T component))
                return component;
            //else Debug.LogError($"Component {typeof(T)} not found on {origin.name}");
        }        
        
        //Debug.LogWarning($"Component {typeof(T)} not found on {origin.name}");
        return null;
    }
    
    
}