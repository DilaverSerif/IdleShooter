using System;
using UnityEngine;

[Serializable]
public class StepCounter
{
    public Action Step;
    public float minStepDistance = 0.1f;  // Minimum adım mesafesi
    private Vector3 lastPosition;         // Karakterin bir önceki pozisyonu
    private bool isStepping = false;      // Karakter adım atıyor mu?
    private Transform transform;
    
    public void Start(Transform transform)
    {
        this.transform = transform;
        lastPosition = transform.position;
    }
   

    public void Update()
    {
        DetectStep();
    }

    // Adım algılamayı yapan fonksiyon
    private void DetectStep()
    {
        // Şu anki pozisyon ile önceki pozisyon arasındaki mesafeyi hesapla
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);

        if (distanceMoved > minStepDistance)
        {
            if (!isStepping)
            {
                isStepping = true;
                OnStepStart(); // Adım atma başladığında tetiklenecek fonksiyon
            }

            // Pozisyonu güncelle
            lastPosition = transform.position;
        }
        else
        {
            if (isStepping)
            {
                isStepping = false;
                OnStepStop(); // Adım durduğunda tetiklenecek fonksiyon
            }
        }
    }

    // Karakter adım atmaya başladığında yapılacaklar
    private void OnStepStart()
    {
        Step?.Invoke();
        Debug.Log("Karakter adım attı.");
        // Burada adım sesi veya animasyonu tetikleyebilirsiniz.
    }

    // Karakter adım atmayı durdurduğunda yapılacaklar
    private void OnStepStop()
    {
        Debug.Log("Karakter adım atmayı durdurdu.");
        // Adım durduğunda ses veya animasyonu durdurabilirsiniz.
    }
}