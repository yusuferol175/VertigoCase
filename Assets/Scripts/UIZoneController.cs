using System;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIZoneController : MonoBehaviour
{
    
    [field: SerializeField] public Image _zoneImage { get; private set; }
    [field: SerializeField] public TMP_Text _zoneText { get; private set; }

    public void SetImage(ZoneType zoneType)
    {
        switch (zoneType)
        {
            case ZoneType.NormalZone:
                SetImageByZone(UIManager.Instance.DefaultZone, Color.white);
                break;
            case ZoneType.SafeZone:
                SetImageByZone(UIManager.Instance.SafeZone, Color.green);
                break;
            case ZoneType.SuperZone:
                SetImageByZone(UIManager.Instance.SuperZone, Color.yellow);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(zoneType), zoneType, null);
        }
    }

    public void SetText(string level) => _zoneText.SetText(level);

    private void SetImageByZone(Sprite zoneSprite, Color textColor)
    {
        _zoneImage.sprite = zoneSprite;
        _zoneText.color = textColor;
    }
}
