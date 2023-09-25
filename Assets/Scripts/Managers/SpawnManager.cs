using Level;
using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnManager : MonoBehaviour
{
    private const int REMAINING_ENEMY_COUNT_FOR_NEXT_WAVE = 10;

    [SerializeField] private KeyCode _nextWaveKeyboardButton = KeyCode.Return;
    [SerializeField] private EnemiesData _enemiesData;
    [SerializeField] private Button _nextWaveScreenButton;
    [SerializeField] private TextMeshProUGUI _nextWaveTimerText;
    [SerializeField] private List<PathData> _paths;
    
    private List<GameObject> _enemies = new List<GameObject>();
    private int _spawnerCount = 0;
    private bool _wavesStarted = false;
    private bool _pause = false;
    private bool _nextWaveAvailable = true;
    private int _waveIndex = -1;
    private float _nextWaveTimer;

    private void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
        _nextWaveScreenButton.onClick.AddListener(StartNextWave);
    }

    private void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
        _nextWaveScreenButton.onClick.RemoveListener(StartNextWave);
        StopAllCoroutines();
    }

    private void Update() {
        if (Input.GetKeyDown(_nextWaveKeyboardButton)) {
            if (_nextWaveAvailable) {
                StartNextWave();
            }
        }
    }

    private void SpawnEnemy(string enemyName, int pathIndex) {
        PathData pathData = _paths.Find(path => path.Index == pathIndex);
        if (pathData == null) {
            Debug.LogError("Attempting to spawn an enemy on a nonexistent path");
            return;
        }
        GameObject enemyObject = Instantiate(_enemiesData.GetEnemy(enemyName));
        Enemy enemy = enemyObject.GetComponent<Enemy>();
        _enemies.Add(enemyObject);
        EnemyPath enemyPath = pathData.EnemyPath;
        enemyObject.transform.position = enemyPath.StartPoint;
        enemyObject.transform.rotation = enemyPath.StartRotation;
        enemy.Movement.SetPath(enemyPath);
    }

    private void StartNextWave() {
        if (!_wavesStarted) {
            StartCoroutine(NextWaveDelegator());
        }
        _waveIndex++;
        _nextWaveTimer = LevelManager.LevelData.WavesData.Waves[_waveIndex].NextWaveDelay;
        _nextWaveAvailable = false;
        _nextWaveScreenButton.gameObject.SetActive(false);
        _nextWaveTimerText.gameObject.SetActive(false);
        foreach (WavePathData wavePathData in LevelManager.LevelData.WavesData.Waves[_waveIndex].Paths) {
            StartCoroutine(Spawner(wavePathData));
            _spawnerCount++;
        }
    }

    private IEnumerator NextWaveDelegator() {
        while (true) {
            yield return null;

            if (_waveIndex == LevelManager.LevelData.WavesData.WavesCount - 1 && _enemies.Count == 0) {
                yield break;
            }
            if (_spawnerCount == 0 && _enemies.Count < REMAINING_ENEMY_COUNT_FOR_NEXT_WAVE) {
                _nextWaveAvailable = true;
                _nextWaveScreenButton.gameObject.SetActive(true);
                _nextWaveTimerText.gameObject.SetActive(true);
                _nextWaveTimerText.text = "Next wave: " + _nextWaveTimer.ToString(".0");
                if (!_pause) {
                    _nextWaveTimer -= Time.deltaTime;

                    if (_nextWaveTimer <= 0) {
                        StartNextWave();
                    }
                }
            }            
        }
    }
    private IEnumerator Spawner(WavePathData wavePathData) {
        float timer = 0;
        float count;
        foreach (EnemySpawnData enemySpawnData in wavePathData.Enemies) {
            count = enemySpawnData.Count;
            while (count > 0) {
                yield return null;

                if (!_pause) {
                    timer -= Time.deltaTime;

                    if (timer <= 0) {
                        SpawnEnemy(enemySpawnData.EnemyName, wavePathData.Index);
                        timer = enemySpawnData.Interval;
                        count--;
                    }
                }
            }
        }
        _spawnerCount--;
    }

    private void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }

    [System.Serializable]
    private class PathData {
        [SerializeField] private int _index;
        [SerializeField] private EnemyPath _enemyPath;

        public int Index => _index;
        public EnemyPath EnemyPath => _enemyPath;
    }
}
