using System;
using UnityEngine;

namespace _BASE_.Scripts
{
    public abstract class Manager: MonoBehaviour
    {
        public int priority;

        public virtual void Initialized()
        {
            Debug.Log($"{transform.name} Initialized...");
        }
    }
}