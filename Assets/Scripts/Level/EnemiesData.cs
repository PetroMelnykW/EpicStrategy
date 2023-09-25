using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemiesData", menuName = "ScriptableObjects/EnemiesData")]
public class EnemiesData : ScriptableObject
{
    [SerializeField] private List<EnemyObjectData> _enemies;

    public GameObject GetEnemy(string enemyName) {
        return _enemies.Find(enemy => enemy.EnemyName == enemyName).EnemyObject;
    }
}

[System.Serializable]
public class EnemyObjectData
{
    [SerializeField] private string _enemyName;
    [SerializeField] private GameObject _enemyObject;

    public string EnemyName => _enemyName;
    public GameObject EnemyObject => _enemyObject;
}
