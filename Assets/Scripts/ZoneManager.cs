using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ZoneManager : MonoBehaviour
{
    public static ZoneManager Instance;
    public WheelSO[] Zones => _zones;
    public int LevelID => _levelID;

    [SerializeField] private List<Image> _collectableOjectImages;
    [SerializeField] private List<TMP_Text> _collectableOjectText;
    [SerializeField] private Transform _horizontalLayoutGroup;
    [SerializeField] private UIZoneController _levelPrefab;
    [SerializeField] private List<UIZoneController> _levels = new List<UIZoneController>();
    [SerializeField] private GameObject _levelZoneImageGroup;
    [SerializeField] private TMP_Text _safeZoneInfoText;
    [SerializeField] private TMP_Text _superZoneInfoText;
    [SerializeField] private Image _superZoneInfoImage;
    
    private int _levelID = 1;
    private WheelSO[] _zones;
    private WheelSO _currentWheel;
    private float _levelTextSpace;


    private void Awake() => Instance = this;

    private void Start()
    {
        var zones = Resources.LoadAll<WheelSO>("Wheels");

        _zones = zones.OrderBy(zone=> zone.LevelID).ToArray();

        GetZoneArea();
        
        LoadLevelText();
        
        SetMiddleTextColor(0);
        
        SetZoneInfo();
    }

    public void SetZoneInfo()
    {
        var currentLevel = _levelID - 1;
        SetZoneInfoByZoneType(currentLevel, ZoneType.SafeZone, _safeZoneInfoText);
        SetZoneInfoByZoneType(currentLevel, ZoneType.SuperZone, _superZoneInfoText);
    }
    
    private void SetZoneInfoByZoneType(int currentLevel ,ZoneType zoneType, TMP_Text infoText)
    {
        foreach (var zone in _zones)
        {
            if (zone.ZoneType == zoneType && currentLevel < zone.LevelID)
            {
                if (zone.ZoneType == ZoneType.SuperZone)
                    _superZoneInfoImage.sprite = zone._slices[0].Collectable.Sprite;
                
                infoText.SetText(zone.LevelID.ToString());
                break;
            }
        }
    }
    
    public void SetLevelGroup()
    {
        var moveX = _horizontalLayoutGroup.localPosition.x - _levelTextSpace;
        var index = _levelID - 1;
        SetMiddleTextColor(index);
        StartCoroutine(ResetMiddleTextColor(index - 1));
        _horizontalLayoutGroup.transform.DOLocalMoveX(moveX, 1f)
            .OnComplete(GetZoneArea);

        _levelZoneImageGroup.transform.DOLocalMoveX(moveX, 1f);
    }
    public void GetZoneArea()
    {
        foreach (var wheel in _zones)
        {
            if (_levelID == wheel.LevelID)
            {
                _currentWheel = wheel;
                UIManager.Instance.SetWheel(wheel.ZoneType);
                ChangeCollectableImages();
                _levelID++;
                break;
            }
        }
    }

    private void ChangeCollectableImages()
    {
        for (int i = 0; i < _currentWheel._slices.Length; i++)
        {
            var collectable = _currentWheel._slices[i].Collectable;
            _collectableOjectImages[i].sprite = collectable.Sprite;
            _collectableOjectText[i].SetText("x"+_currentWheel._slices[i].CollectableCount.ToString());
        }
    }
    
    private void LoadLevelText()
    {
        for (int i = 0; i < _zones.Length; i++)
        {
            var newLevel = Instantiate(_levelPrefab, _horizontalLayoutGroup.transform);
            var instantiatedZoneImage = newLevel._zoneImage;
            var level = i + 1;
            var zoneType = _zones[i].ZoneType;
            
            instantiatedZoneImage.transform.SetParent(_levelZoneImageGroup.transform);
            newLevel.SetText(level.ToString());
            newLevel.SetImage(zoneType);
            _levels.Add(newLevel);
        }
        Invoke("GetLevelTextSpace",.1f);
    }

    private void SetMiddleTextColor(int index)
    {
        var zoneType = _zones[index].ZoneType;
        var levelText = _levels[index]._zoneText;
        
        switch (zoneType)
        {
            case ZoneType.NormalZone:
                levelText.color = Color.black;
                break;
            case ZoneType.SafeZone:
                levelText.color = Color.white;
                break;
            case ZoneType.SuperZone:
                levelText.color = Color.black;
                break;
        }
    }
    
    private IEnumerator ResetMiddleTextColor(int index)
    {
        yield return new WaitForSeconds(.4f);
        var zoneType = _zones[index].ZoneType;
        var levelText = _levels[index]._zoneText;
        
        switch (zoneType)
        {
            case ZoneType.NormalZone:
                levelText.color = Color.white;
                break;
            case ZoneType.SafeZone:
                levelText.color = Color.green;
                break;
            case ZoneType.SuperZone:
                levelText.color = Color.yellow;
                break;
        }
        yield break;
    }
    
    private void GetLevelTextSpace() => _levelTextSpace = _levels[1].transform.localPosition.x - _levels[0].transform.localPosition.x;
}