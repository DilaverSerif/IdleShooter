using System;
using System.Linq;
using Eflatun.SceneReference;
using UnityEngine;

namespace _BASE_.Scripts
{
    public abstract class Manager: MonoBehaviour
    {
        public SceneReference[] loadScenes;
        
        public virtual void Initialized()
        {
            var checkScene = loadScenes.FirstOrDefault(scene => scene.Name.Equals(gameObject.scene.path));
            if(checkScene == null) return;
            
            Debug.Log($"{transform.name} Initialized...");
        }
    }
}