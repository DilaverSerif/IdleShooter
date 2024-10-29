using _BASE_.Scripts.Quest_System;
using UnityEngine;
namespace _GAME_.Quest
{
    [CreateAssetMenu(menuName = "Quest/Create TestQuest", fileName = "TestQuest", order = 0)]
    public class TestQuest: BaseQuest
    {
        public override void UpdateQuest()
        {
            Debug.Log("Update Quest");
        }
    }
}
