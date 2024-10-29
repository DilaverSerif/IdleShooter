using System;
using System.Collections.Generic;
using _BASE_.Scripts.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _BASE_.Scripts.Stat_System
{
    [CreateAssetMenu(menuName = "Create StatMainData", fileName = "StatMainData", order = 0)]
    public class StatMainData: SingletonScriptableObject<StatMainData>
    {
        [Serializable]
        public struct StatData
        {
            public string statName;
            public float minValue;
            public float maxValue;
            [PreviewField]
            public Sprite icon;
        }
    
        public List<StatData> stats;
    
#if UNITY_EDITOR
        [Button]
        public void CreateTags()
        {
            var getQuestsName = stats.ConvertAll(x => x.statName);
            getQuestsName.ToArray().GenerateEnumFile("Tags", "Assets/[BASE]/Scripts/Stat System/Enums/StatEnums.cs", "Stat");
            
            Debug.Log("Quests tags are created");
        }
#endif
    }
}