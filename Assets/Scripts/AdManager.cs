using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    public static AdManager Instance;
    
    private RewardedAd _rewardedAd;

    private void Awake() => Instance = this;

    private void Start()
    {
        string adUnitId;
#if UNITY_ANDROID
        adUnitId = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
            adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            adUnitId = "unexpected_platform";
#endif

        _rewardedAd = new RewardedAd(adUnitId);
        
        _rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;

        AdRequest request = new AdRequest.Builder().Build();

        _rewardedAd.LoadAd(request);
    }

    private void HandleUserEarnedReward(object sender, Reward args) => Revive();

    public void UserChoseToWatchAd()
    {
        if (_rewardedAd.IsLoaded())
            _rewardedAd.Show();
    }

    private void Revive()
    {
        var buttons = ButtonManager.Instance.Buttons;
        var exitButton = buttons[0];
        var reviveAdButton = buttons[6];

        ButtonManager.Instance.AdState = false;
        
        UIManager.Instance.BombPanel.SetActive(false);
        UIManager.Instance.ResetBombPanelAnimation();
        
        exitButton.gameObject.SetActive(true);
        reviveAdButton.gameObject.SetActive(false);
        
        CollectManager.Instance.SpinController.ResetWheel();
    }
}