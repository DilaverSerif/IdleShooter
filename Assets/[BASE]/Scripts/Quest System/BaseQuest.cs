using Sirenix.OdinInspector;
using UnityEngine;
namespace _BASE_.Scripts.Quest_System
{
    public abstract class BaseQuest : ScriptableObject
    {
        [BoxGroup("Quest Info")]
        public int questID;
        [BoxGroup("Quest Info"),ShowInInspector,ReadOnly]
        public string questName => $"Quest_{name}{questID}";
        [BoxGroup("Quest Info")]
        public int delayStart;
        [ReadOnly,BoxGroup("Debug")]
        public QuestTags questTag;
        [TextArea,BoxGroup("Quest Info")]
        public string questDescription;
        [PreviewField,BoxGroup("Quest Info")]
        public Sprite questIcon;

        [BoxGroup("Quest Info"), GUIColor(0.53f, 0.94f, 1f)]
        public int targetValue = 1;

        [ShowInInspector,BoxGroup("Debug")]
        private int Timer
        {
            get => PlayerPrefs.GetInt(questName + "Timer");
            set => PlayerPrefs.SetInt(questName + "Timer", value);
        }
        
        [ShowInInspector,BoxGroup("Debug")]
        public bool IsComplete
        {
            get => PlayerPrefs.GetInt(questName) == 1;
            set => PlayerPrefs.SetInt(questName, value ? 1 : 0);
        }
        
        [ShowInInspector,BoxGroup("Debug")]
        public int CurrentValue
        {
            get => PlayerPrefs.GetInt(questName + "Current");
            set => PlayerPrefs.SetInt(questName + "Current", value);
        }

        public virtual void StartQuest()
        {
            RedManager.Instance.GetManager<QuestManager>().OnCurrentQuestProgress += OnQuestProgress;
        }
        
        public virtual void EndQuest()
        {
            RedManager.Instance.GetManager<QuestManager>().OnCurrentQuestProgress -= OnQuestProgress;
        }
        
        private void OnQuestProgress(int obj)
        {
            CurrentValue += obj;
            
            if (CurrentValue >= targetValue)
            {
                IsComplete = true;
                IfQuestIsComplete();
                RedManager.Instance.GetManager<QuestManager>().EndQuest();
            }
            else
            {
                IfQuestIsNotComplete();
            }
        }

        public virtual bool TryStartQuest()
        {
            switch (delayStart)
            {
                case 0:
                case > 0 when delayStart < Timer:
                    return true;
                
                default:
                    Timer++;
                    return false;
            }
        }
        
        public abstract void UpdateQuest();
        
        public virtual void IfQuestIsComplete()
        {
            Debug.Log("Quest is complete");
        }
        
        public virtual void IfQuestIsNotComplete()
        {
            Debug.Log("Quest is not complete");
        }
    }
}
