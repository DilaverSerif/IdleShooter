using System;
using _BASE_.Scripts;
using _GAME_.Scripts;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelBar : MainMenu
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI nameText;
    [SerializeField]
    private TextMeshProUGUI levelText;
    [SerializeField]
    private TextMeshProUGUI progressText;

    [SerializeField]
    private CanvasGroup canvasGroup;
    
    private void UpdateBar(CharacterLevel characterLevel)
    {
        var level = characterLevel.Level;
        var experience = characterLevel.Experience;
        var neededExperience = characterLevel.NeededExperience(level);
        
        slider.value = experience / neededExperience;
        nameText.text = "Player";
        levelText.text = level.ToString();
        progressText.text = $"{experience}/{neededExperience}";
    }
    
    public override Tween OpenMenu()
    {
        return canvasGroup.DOFade(1, 0.5f);
    }
    public override Tween CloseMenu()
    {
        return canvasGroup.DOFade(0, 0.5f);
    }

    void OnEnable()
    {
        PlayerLevel.OnPlayerExpChanged.AddListener(UpdateBar);
    }

    void OnDisable()
    {
        PlayerLevel.OnPlayerExpChanged.RemoveListener(UpdateBar);
    }
}
