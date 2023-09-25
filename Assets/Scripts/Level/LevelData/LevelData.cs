using UnityEngine;

namespace Level
{
    [CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/LevelData")]
    public class LevelData : ScriptableObject
    {
        [SerializeField] private AvailableTowers _availableTowers;
        [SerializeField] private AvailableResources _availableResources;
        [SerializeField] private WavesData _wavesData;

        public AvailableTowers AvailableTowers => _availableTowers;
        public AvailableResources AvailableResources => _availableResources;
        public WavesData WavesData => _wavesData;
    }
}
