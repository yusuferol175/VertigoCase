using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardController : MonoBehaviour
{
    [SerializeField] private TMP_Text _rewardName;
    [SerializeField] private TMP_Text _rewardCount;
    [SerializeField] private Image _rewardImage;

    public void SetReward(string name, int count, Sprite sprite)
    {
        _rewardName.SetText(name);
        _rewardCount.SetText("x"+count.ToString());
        _rewardImage.sprite = sprite;
    }
}
