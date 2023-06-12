using System;
using System.Collections.Generic;
using DefaultNamespace;
using DG.Tweening;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CollectableController : MonoBehaviour
{
    public Image CollectableImage => _collectableImage;
    [field: SerializeField] public CollectableType CollectableType { get; set; }
    [field: SerializeField] public TMP_Text CollectableText { get; set; }
    [field: SerializeField] public int CollectableCount { get; set; }
    
    [SerializeField] private Image _collectableImage;
    [SerializeField] private List<Vector2> _collectableImagesDefaultV2 = new List<Vector2>();
    
    private Vector2 _collectableV2;

    public void SetCollectable(Sprite collectableSprite, int count, CollectableType collectableType, Vector2 collectableV2)
    {
        _collectableImage.sprite = collectableSprite;
        
        StartCoroutine(CollectManager.Instance.CollectCounter(CollectableText, count, CollectableCount));
        CollectableType = collectableType;
        _collectableV2 = collectableV2;

        CollectableAnimation(collectableSprite, count);
    }

    public void CollectableAnimation(Sprite collectableSprite, int collectableCount)
    {
        CollectableCount += collectableCount;
        var collectableItemParent = CollectManager.Instance.CollectableItemParent;
        var collectableItemImages = CollectManager.Instance.CollectableItemImages;
        var lastItemImage = collectableItemImages[collectableItemImages.Count - 1];
        
        var delay = 0f;
        
        collectableItemParent.SetActive(true);
        
        foreach (var itemImages in collectableItemImages)
            itemImages.sprite = collectableSprite;

        foreach (var collectableItemImage in collectableItemImages)
        {
            collectableItemImage.transform.DOScale(1f, 0.3f)
                .SetDelay(delay)
                .SetEase(Ease.OutBack);

            collectableItemImage.GetComponent<RectTransform>().DOAnchorPos(_collectableV2, 1f)
                .SetDelay(delay + .5f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    if (collectableItemImage == lastItemImage)
                        ResetAnimation(collectableItemParent, collectableItemImages, lastItemImage);
                });
            
            delay += 0.1f;
        }
    }

    private void ResetAnimation(GameObject collectableItemParent, List<Image> collectableItemImages, Image lastItemImage)
    {
        var delay = 0f;
        
        foreach (var collectableItemImage in collectableItemImages)
        {
            collectableItemImage.transform.DOScale(0f, 0.3f)
                .SetDelay(delay + .8f)
                .SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    if (collectableItemImage == lastItemImage)
                    {
                        collectableItemParent.SetActive(false);
                        ResetCollectableImagesTransforms(collectableItemImages);
                    }
                });

            delay += 0.1f;
        }
    }

    private void ResetCollectableImagesTransforms(List<Image> collectableItemImages)
    {
        for (int i = 0; i < collectableItemImages.Count; i++)
        {
            var posX = _collectableImagesDefaultV2[i].x;
            var posY = _collectableImagesDefaultV2[i].y;
            collectableItemImages[i].transform.localPosition = new Vector3(posX, posY, 0f);
        }
        
        CollectManager.Instance.SpinController.WheelReloadAnimation();
    }
}
