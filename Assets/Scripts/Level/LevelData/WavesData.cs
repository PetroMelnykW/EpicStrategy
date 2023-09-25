using System.Collections.Generic;
using UnityEngine;

namespace Level
{
    [System.Serializable]
    public class WavesData
    {
        [SerializeField] private List<WaveData> _waves;
        
        public List<WaveData> Waves => _waves;

        public int WavesCount => _waves.Count;
    }

    [System.Serializable]
    public class WaveData 
    {
        [SerializeField] private float _nextWaveDelay; 
        [SerializeField] private List<WavePathData> _paths;

        public float NextWaveDelay => _nextWaveDelay;
        public List<WavePathData> Paths => _paths;
    }

    [System.Serializable]
    public class WavePathData 
    {
        [SerializeField] private int _index;
        [SerializeField] private List<EnemySpawnData> _enemies;

        public int Index => _index;
        public List<EnemySpawnData> Enemies => _enemies;
    }

    [System.Serializable]
    public class EnemySpawnData {
        [SerializeField] private string _enemyName;
        [SerializeField] private int _count = 1;
        [SerializeField] private int _interval = 3;

        public string EnemyName => _enemyName;
        public int Count => _count;
        public int Interval => _interval;
    }
}

