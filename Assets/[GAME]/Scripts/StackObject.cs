using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class StackObject : MonoBehaviour
{
    private bool _isStacked;
    private Renderer _renderer;
    
    [ShowInInspector]
    public bool IsStacked => _isStacked;

    private void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }
    
    public float GetStackHeight()
    {
        Bounds meshBounds = _renderer.bounds;
        Vector3 center = meshBounds.center;

        // Y eksenindeki en üst ve en alt noktaları hesapla
        float topY = center.y + meshBounds.extents.y;
        float bottomY = center.y - meshBounds.extents.y;

        return topY - bottomY;
    }
    
    public void SetStacked(bool isStacked = true)
    {
        _isStacked = isStacked;
    }
    
    private void OnDrawGizmosSelected()
    {
        if(_renderer == null)
            _renderer = GetComponentInChildren<Renderer>();
        
        Bounds meshBounds = _renderer.bounds;

        Vector3 center = meshBounds.center;

        // Y eksenindeki en üst ve en alt noktaları hesapla
        float topY = center.y + meshBounds.extents.y;
        float bottomY = center.y - meshBounds.extents.y;

        // En üst ve en alt noktaların dünya pozisyonları
        Vector3 topPoint = new Vector3(center.x, topY, center.z);
        Vector3 bottomPoint = new Vector3(center.x, bottomY, center.z);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(bottomPoint, 0.1f);
        Gizmos.DrawSphere(topPoint, 0.1f);
    }
}