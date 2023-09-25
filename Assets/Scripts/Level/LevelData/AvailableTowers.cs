using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [System.Serializable]
    public class AvailableTowers
    {
        [SerializeField] private List<string> _towers;

        public bool GetTowerAvailable(string tower) {
            return _towers.Contains(tower);
        }
    }
}

