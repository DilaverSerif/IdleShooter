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
    private PlayerSave _playerSave;

    void OnDisable()
    {
        _playerSave.playerLevel.OnExperienceChanged -= UpdateBar;
    }

    private void UpdateBar()
    {
        if (_playerSave.playerLevel == null)
        {
            Debug.LogError("PlayerSave is null");
            return;
        }
        var level = _playerSave.playerLevel.Level;
        var experience = _playerSave.playerLevel.Experience;
        var neededExperience = _playerSave.playerLevel.NeededExperience(level);
        
        slider.value = experience / neededExperience;
        nameText.text = "Player";
        levelText.text = level.ToString();
        progressText.text = $"{experience}/{neededExperience}";
    }
    
    public override Tween OpenMenu()
    {
        _playerSave = RedManager.Instance.GetManager<SaveManager>().playerSave;
     

        _playerSave.playerLevel.OnExperienceChanged += UpdateBar;
        UpdateBar();
        
        
        return canvasGroup.DOFade(1, 0.5f);
    }
    public override Tween CloseMenu()
    {
        return canvasGroup.DOFade(0, 0.5f);
    }
}
