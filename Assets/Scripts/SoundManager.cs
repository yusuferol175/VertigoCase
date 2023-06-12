using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _spinClick;
    [SerializeField] private AudioClip _spinReload;
    [SerializeField] private AudioClip _spinShot;

    public float ClickTime = .1f;
    public bool IsSpinning;

    private void Awake() => Instance = this;
    
    public IEnumerator SetSpinSound()
    {
        while (IsSpinning)
        {
            _audioSource.PlayOneShot(_spinClick);
            yield return new WaitForSeconds(ClickTime);
        }
    }

    public void ReloadWheelSound() => _audioSource.PlayOneShot(_spinReload);
    
    public void ShotWheelSound() => _audioSource.PlayOneShot(_spinShot);
}
