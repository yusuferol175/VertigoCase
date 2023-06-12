using System;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [field: SerializeField, Header("Transforms")]
    public Transform Wheel { get; private set; }

    [field: SerializeField] public Transform Indicator { get; private set; }

    [field: SerializeField, Header("Wheel Images")]
    public Image WheelImage { get; private set; }

    [field: SerializeField] public Sprite BronzeWheel { get; private set; }
    [field: SerializeField] public Sprite SilverWheel { get; private set; }
    [field: SerializeField] public Sprite GoldWheel { get; private set; }

    [field: SerializeField, Header("Indicator Images")]
    public Image IndicatorImage { get; private set; }

    [field: SerializeField] public Sprite BronzeIndicator { get; private set; }
    [field: SerializeField] public Sprite SilverIndicator { get; private set; }
    [field: SerializeField] public Sprite GoldIndicator { get; private set; }

    [field: SerializeField, Header("Zone Sprites")]
    public Sprite SuperZone { get; private set; }

    [field: SerializeField] public Sprite SafeZone { get; private set; }
    [field: SerializeField] public Sprite DefaultZone { get; private set; }

    [field: SerializeField, Header("Spin Panel")]
    public TMP_Text SpinTypeText;

    [field: SerializeField] public TMP_Text CollectItemCount;
    
    [field: SerializeField, Header("Rewards Panel")]
    public GameObject ExitPanel { get; private set; }
    
    [field: SerializeField] public GameObject RewardsPanel;
    
    [field: SerializeField] public GameObject RewardsGroup{ get; private set; }
    
    [field: SerializeField, Header("Bomb Panel")]
    public GameObject BombPanel { get; private set; }
    
    [field: SerializeField] public GameObject BombText{ get; private set; }
    
    [field: SerializeField] public GameObject BombCard{ get; private set; }


    private Color32 _bronzeColor = new Color32(205, 127, 50, 255);
    private Color32 _silverColor = new Color32(192, 192, 192, 255);
    private Color32 _goldColor = new Color32(218, 165, 32, 255);
    private void Awake() => Instance = this;

    public void SetWheel(ZoneType zoneType)
    {
        switch (zoneType)
        {
            case ZoneType.NormalZone:
                SetWheelByZone(BronzeWheel, BronzeIndicator, "BRONZE SPIN", _bronzeColor);
                break;
            case ZoneType.SafeZone:
                SetWheelByZone(SilverWheel, SilverIndicator, "SILVER SPIN", _silverColor);
                break;
            case ZoneType.SuperZone:
                SetWheelByZone(GoldWheel, GoldIndicator, "GOLDEN SPIN", _goldColor);
                break;
        }
    }
    
    public void SetCollectItemInfo(int collectItemCount ,string collectableName) => CollectItemCount.SetText("Up To x"+collectItemCount.ToString() + " " + collectableName);

    public void ResetCollectItemInfo() => CollectItemCount.SetText("");

    private void SetWheelByZone(Sprite wheelSprite, Sprite indicatorSprite, string spintTypeInfo, Color32 color)
    {
        WheelImage.sprite = wheelSprite;
        IndicatorImage.sprite = indicatorSprite;
        SpinTypeText.SetText(spintTypeInfo);
        SpinTypeText.color = color;
        CollectItemCount.color = color;
    }

    public void BombPanelAnimation()
    {
        BombText.transform.DOLocalMoveY(450f, .8f)
            .SetEase(Ease.Linear)
            .OnComplete(CardAnimation);

        void CardAnimation()
        {
            BombCard.transform.DOLocalMoveY(0f, .8f)
                .SetEase(Ease.Linear);
        }
    }
    
    public void ResetBombPanelAnimation()
    {
        BombText.transform.DOLocalMoveY(630f, .1f)
            .SetEase(Ease.Linear)
            .OnComplete(CardAnimation);

        void CardAnimation()
        {
            BombCard.transform.DOLocalMoveY(-900f, .1f)
                .SetEase(Ease.Linear);
        }
    }
}