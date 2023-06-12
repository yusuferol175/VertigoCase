using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

public class SpinController : MonoBehaviour
{
    public bool IsSpinning => _isSpinning;
    
    private bool _isSpinning;
    private Transform _wheel;
    private Transform _indicator;

    private void Awake()
    {
        _wheel = UIManager.Instance.Wheel;
        _indicator = UIManager.Instance.Indicator;
    }

    public void RotateWheel()
    {
        if (!_isSpinning)
        {
            ZoneManager.Instance.SetZoneInfo();
            
            _wheel.DORotate(new Vector3(0f, 0f, -720f), 2f, RotateMode.FastBeyond360)
                .SetEase(Ease.Linear)
                .SetLoops(2)
                .OnComplete(SelectRandomPiece);

            MoveIndicator();
            
            _isSpinning = true;
            SoundManager.Instance.IsSpinning = true;
            
            StartCoroutine(SoundManager.Instance.SetSpinSound());
        }
    }

    private void SelectRandomPiece()
    {
        var randomIndex = Random.Range(0, 8);

        var rotation = 360 - (randomIndex * 45f);
        SoundManager.Instance.ClickTime = .5f;
        _wheel.DORotate(new Vector3(0f, 0f, - rotation), 2f, RotateMode.FastBeyond360)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                ResetIndicator();
                SoundManager.Instance.ClickTime = .1f;
                SoundManager.Instance.ShotWheelSound();
                SoundManager.Instance.IsSpinning = false;
                CollectManager.Instance.LoadCollectable(randomIndex);
            });
        _indicator.DOLocalRotate(new Vector3(0f, 0f, 15f), .2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void WheelReloadAnimation()
    {
        _wheel.GetComponent<RectTransform>().DOAnchorPosY(-1000f, .5f)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                ResetWheel();
                ResetReloadAnimation();
            });
    }
    
    public void ResetWheel()
    {
        if (ZoneManager.Instance.LevelID > 60)
        {
            ButtonManager.Instance.CollectButtonClick();
            return;
        }
        
        UIManager.Instance.ResetCollectItemInfo();
        ZoneManager.Instance.SetLevelGroup();
        _isSpinning = false;
    }
    private void MoveIndicator()
    {
        _indicator.DOLocalRotate(new Vector3(0f, 0f, 30f), .2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }
    
    private void ResetReloadAnimation()
    {
        SoundManager.Instance.ReloadWheelSound();
        _wheel.GetComponent<RectTransform>().DOAnchorPosY(0f, .5f)
            .SetEase(Ease.Linear);
    }
    
    private void ResetIndicator() => _indicator.DOKill();
}