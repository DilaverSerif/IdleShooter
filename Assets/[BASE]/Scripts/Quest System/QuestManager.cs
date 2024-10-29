using System;
using System.Collections.Generic;
using System.Linq;
using _BASE_.Scripts.Extensions;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;
namespace _BASE_.Scripts.Quest_System
{
    public class QuestManager : Manager
    {
        public Action<BaseQuest> OnQuestComplete;
        public Action<int> OnQuestIDComplete;
        public Action<string> OnQuestNameComplete;
        public Action<QuestTags> OnTagQuestComplete;


        public Action<BaseQuest> OnQuestStarted;
        public Action<int> OnCurrentQuestProgress;
        public Action<QuestTags,int> OnTagQuestProgress;
        
        public BaseQuest currentQuest;
        [TableList]
        public List<BaseQuest> questList = new List<BaseQuest>();

        public BaseQuest GetQuest(string questName)
        {
            return questList.Find(x => x.name == questName);
        }
        
        public BaseQuest GetQuestByID(int questID)
        {
            return questList.Find(x => x.questID == questID);
        }

        private async UniTaskVoid StartQuest()
        {
            foreach (var quest in questList.Where(quest => !quest.IsComplete))
            {
                currentQuest = quest;
                
                while (!currentQuest.TryStartQuest())
                    await UniTask.WaitForSeconds(1f);
                
                OnQuestStarted?.Invoke(currentQuest);
                currentQuest.StartQuest();
                UpdateQuest().Forget();
            }
        }
        
        private async UniTaskVoid UpdateQuest()
        {
            while (currentQuest && Application.isPlaying)
            {
                currentQuest.UpdateQuest();
                await UniTask.NextFrame();
            }
        }
        
        public void EndQuest(bool startNext = true)
        {
            currentQuest.EndQuest();
            OnQuestComplete?.Invoke(currentQuest);
            OnQuestIDComplete?.Invoke(currentQuest.questID);
            OnQuestNameComplete?.Invoke(currentQuest.questName);
            OnTagQuestComplete?.Invoke(currentQuest.questTag);
            
            currentQuest = null;

            if (IsQuestLastQuest())
            {
                Debug.Log("All Quests are complete");
                return;
            }
            
            if(startNext) StartQuest().Forget();
        }
        private bool IsQuestLastQuest()
        {
            return questList.TrueForAll(x => x.IsComplete);
        }

        public override void Initialized()
        {
            base.Initialized();
            if(Application.isPlaying)
                StartQuest().Forget();
        }
        
        private void OnDisable()
        {
            currentQuest = null;
            Debug.Log("Quest System Disabled");
        }

        #if UNITY_EDITOR
        
        [Button]
        public void ProgressQuest(int value)
        {
            OnCurrentQuestProgress?.Invoke(value);
        }

        [Button]
        public void CreateTags()
        {
            var getQuestsName = questList.ConvertAll(x => x.name);
            getQuestsName.ToArray().GenerateEnumFile("Tags", "Assets/[BASE]/Scripts/Quest System/Enums/QuestEnums.cs", "Quest");
            
            Debug.Log("Quests tags are created");
        }
        
        [Button]
        public void SetTags()
        {
            foreach (var quest in questList)
            {
                quest.questTag = (QuestTags) Enum.Parse(typeof(QuestTags), quest.name.Replace(" ", "_"));
            }
            
            Debug.Log("Quests are set");
        }
        
        [Button]
        public void SetQuestID()
        {
            for (int i = 0; i < questList.Count; i++)
            {
                questList[i].questID = i;
            }
        }
        
        #endif

      
    }
    
    public static class QuestManagerExtensions
    {
        public static void ProgressQuest(this QuestTags tag, int value)
        {
            RedManager.Instance.GetManager<QuestManager>().OnTagQuestProgress?.Invoke(tag,value);
        }
    }
}