using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "wheel", menuName = "Wheel", order = 0)]
    public class WheelSO : ScriptableObject
    {
        public ZoneType ZoneType => _zoneType;
        public int LevelID => _levelID;

        [SerializeField] private ZoneType _zoneType;
        [SerializeField] private int _levelID;
        
        [System.Serializable]
        public struct _sliceData
        {
            public Collectable Collectable => _collectable;
            public int CollectableCount => _collectableCount;
            
            [SerializeField] private Collectable _collectable;
            [SerializeField] private int _collectableCount;
        }
        public _sliceData[] _slices = new _sliceData[8];
    }
}