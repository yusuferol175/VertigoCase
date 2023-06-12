using System;
using System.Text.RegularExpressions;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager Instance;

    public Button[] Buttons => _buttons;

    [SerializeField] private Button[] _buttons;
    [SerializeField] private RewardController _rewardController;
    [SerializeField] private TMP_Text _reviveInfoText;
    [SerializeField] private TMP_Text _collectText;

    private ButtonType[] _buttonTypeValues;
    private int _revivePrice = 25;
    [field: SerializeField] public bool AdState {get; set;}
    
    private void Awake() => Instance = this;

    private void Start()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            Button button = _buttons[i];
            var index = i;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => OnClick(index));
        }
        
        AdState = true;
        
        _reviveInfoText.SetText(_revivePrice.ToString() + " REVIVE");
        
        _buttonTypeValues = (ButtonType[])Enum.GetValues(typeof(ButtonType));
    }

    private void OnClick(int index)
    {
        var buttonType = _buttonTypeValues[index];

        switch (buttonType)
        {
            case ButtonType.ExitButton:
                if (!CollectManager.Instance.SpinController.IsSpinning)
                {
                    UIManager.Instance.ExitPanel.SetActive(true);
                    _collectText.SetText("Want to exit and collect your rewards? The best one are saved for the last!");
                }
                break;
            case ButtonType.CollectButton:
                CollectButtonClick();
                break;
            case ButtonType.GoBackButton:
                UIManager.Instance.ExitPanel.SetActive(false);
                break;
            case ButtonType.ContinueButton:
                SceneManager.LoadScene("Game");
                break;
            case ButtonType.GiveUpButton:
                SceneManager.LoadScene("Game");
                break;
            case ButtonType.ReviveButton:
                ReviveButtonClick();
                break;
            case ButtonType.ReviveAdButton:
                AdManager.Instance.UserChoseToWatchAd();
                break;
            case ButtonType.SpinButton:
                CollectManager.Instance.SpinController.RotateWheel();
                break;
        }
    }

    public void CollectButtonClick()
    {
        var collectables = CollectManager.Instance.Collectables;
        
        if (collectables.Count <= 0)
        {
            _collectText.SetText("You have no items to collect!");
            return;
        }

        foreach (var collectable in collectables)
        {
            var rewardsGroup = UIManager.Instance.RewardsGroup;
            var reward = Instantiate(_rewardController, rewardsGroup.transform);
            var collectableType = collectable.CollectableType.ToString();

            var collectableName = Regex.Replace(collectableType, @"(\p{Ll})(\p{Lu})", "$1 $2");
            collectableName = Regex.Replace(collectableName, @"(\p{Lu})(\p{Lu}\p{Ll})", "$1 $2");

            var rewardName = collectableName;
            var rewardCount = collectable.CollectableCount;
            var rewardSprite = collectable.CollectableImage.sprite;
            reward.SetReward(rewardName, rewardCount, rewardSprite);
        }

        UIManager.Instance.RewardsPanel.SetActive(true);
    }

    private void ReviveButtonClick()
    {
        var collectables = CollectManager.Instance.Collectables;
        foreach (var collectable in collectables)
        {
            if (collectable.CollectableType == CollectableType.Gold)
            {
                var goldCount = collectable.CollectableCount;
                if (goldCount >= _revivePrice)
                {
                    collectable.CollectableCount -= _revivePrice;
                    collectable.CollectableText.SetText(collectable.CollectableCount.ToString());
                    Revive();
                    break;
                }
            }
        }
    }
    
    private void Revive()
    {
        var exitButton = _buttons[0];
        
        UIManager.Instance.BombPanel.SetActive(false);
        UIManager.Instance.ResetBombPanelAnimation();
        exitButton.gameObject.SetActive(true);
        _revivePrice = _revivePrice * 2;
        _reviveInfoText.SetText(_revivePrice.ToString() + " REVIVE");
        CollectManager.Instance.SpinController.ResetWheel();
    }
}