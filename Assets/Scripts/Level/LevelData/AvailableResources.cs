using UnityEngine;

namespace Level
{
    [System.Serializable]
    public class AvailableResources
    {
        [SerializeField] private int _startMoney = 25;
        [SerializeField] private int _startLives = 20;

        public int StartMoney => _startMoney;
        public int StartLives => _startLives;
    }
}
