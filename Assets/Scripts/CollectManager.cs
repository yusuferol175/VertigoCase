using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectManager : MonoBehaviour
{
    public static CollectManager Instance;

    public List<Image> CollectableItemImages => _collectableItemImages;
    public GameObject CollectableItemParent => _collectableItemParent;

    public List<CollectableController> Collectables => _collectables;

    public SpinController SpinController => _spinController;

    [SerializeField] private Transform _verticalLayoutGroup;
    [SerializeField] private CollectableController _collectable;
    [SerializeField] private SpinController _spinController;
    [SerializeField] private List<Image> _collectableItemImages;
    [SerializeField] private GameObject _collectableItemParent;

    private List<CollectableController> _collectables = new List<CollectableController>();

    private Vector2 _collectableV2 = new Vector2(-828f, 350f);

    private void Awake() => Instance = this;

    public void LoadCollectable(int collectableIndex)
    {
        var zones = ZoneManager.Instance.Zones;
        var levelID = ZoneManager.Instance.LevelID - 1;

        foreach (var wheel in zones)
        {
            if (levelID == wheel.LevelID)
            {
                var currentWheel = wheel;
                var slice = currentWheel._slices[collectableIndex];

                var collectable = slice.Collectable;
                var collectableType = slice.Collectable.CollectableType.ToString();

                if (collectable.CollectableType is CollectableType.GrenadeM26 or CollectableType.GrenadeM67)
                {
                    UIManager.Instance.BombPanel.SetActive(true);
                    UIManager.Instance.BombPanelAnimation();
                    
                    var buttons = ButtonManager.Instance.Buttons;
                    var exitButton = buttons[0];
                    var reviveAdButton = buttons[6];
                    
                    exitButton.gameObject.SetActive(false);
                    
                    if (!ButtonManager.Instance.AdState)
                        reviveAdButton.gameObject.SetActive(false);
                }
                else
                {
                    var collectableName = Regex.Replace(collectableType, @"(\p{Ll})(\p{Lu})", "$1 $2");
                    collectableName = Regex.Replace(collectableName, @"(\p{Lu})(\p{Lu}\p{Ll})", "$1 $2");

                    UIManager.Instance.SetCollectItemInfo(slice.CollectableCount, collectableName);

                    if (!CollectableExists(collectable.CollectableType, collectable.Sprite, slice.CollectableCount))
                    {
                        var newCollectable = Instantiate(_collectable, _verticalLayoutGroup.transform);
                    
                        _collectableV2 = new Vector2(_collectableV2.x, _collectableV2.y - 110f);
                        newCollectable.CollectableType = collectable.CollectableType;
                        newCollectable.SetCollectable(collectable.Sprite, slice.CollectableCount,
                            collectable.CollectableType, _collectableV2);
                        _collectables.Add(newCollectable);
                    } 
                }

                break;
            }
        }
    }

    public IEnumerator CollectCounter(TMP_Text counterText, int count, int countNumber)
    {
        while (count >= countNumber)
        {
            counterText.SetText(countNumber.ToString());
            countNumber++;
            yield return new WaitForSeconds(.04f);
        }
    }

    private bool CollectableExists(CollectableType collectableType, Sprite collectableSprite, int count)
    {
        foreach (var collectable in _collectables)
        {
            if (collectable.CollectableType == collectableType)
            {
                collectable.CollectableAnimation(collectableSprite, count);
                StartCoroutine(CollectCounter(collectable.CollectableText, collectable.CollectableCount, collectable.CollectableCount - count));
                
                return true;
            }
        }

        return false;
    }
}