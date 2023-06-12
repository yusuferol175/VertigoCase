using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "collectable", menuName = "Collectable", order = 1)]
    public class Collectable : ScriptableObject
    {
        public Sprite Sprite => _sprite;
        public CollectableType CollectableType => _collectableType;
        
        [SerializeField] private Sprite _sprite;
        [SerializeField] private CollectableType _collectableType;
    }
}