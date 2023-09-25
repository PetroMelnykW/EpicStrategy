using UnityEngine;
using Events;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class PathFollower : MonoBehaviour
{
    private event System.Action<object> PathCompleted;

    [SerializeField] private EnemyPath _path;
    [SerializeField] private float _speed = 2;

    private List<SlowdownEffect> _slowdownEffects = new List<SlowdownEffect>();
    private bool _pause = false;
    private bool _stop = false;
    private float _slowdown = 0;

    private PathPoint _nextPathPoint;
    private PathPoint _currentPathPoint;
    private int _nextPathPointIndex = 0;
    private float _interpolateValue;
    private float _pathDistance;

    public void AddSlowdownEffect(SlowdownEffect slowdownEffect) {
        _slowdownEffects.Add(slowdownEffect);
        RecalculateSlowdownEffect();
    }

    public void RemoveSlowdownEffect(SlowdownEffect slowdownEffect) {
        _slowdownEffects.Remove(slowdownEffect);
        RecalculateSlowdownEffect();
    }

    public void Subscribe(System.Action<object> action) {
        PathCompleted += action;
    }

    public void SetPath(EnemyPath path) {
        _path = path;
    }

    private void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
    }

    private void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
    }

    private void Start() {
        NextPoint();
    }

    public void Update() {
        if (_pause || _stop) {
            return;
        }
        switch (_currentPathPoint.Type) {
            case PathPointType.Move:
                MovementUpdate();
                break;
            case PathPointType.Teleport:
                Teleport();
                break;
        }
    }

    private void RecalculateSlowdownEffect() {
        float newSlowdown = 0;
        if (_slowdownEffects.Any()) {
            newSlowdown = _slowdownEffects.Max(effect => effect.Strength);
        }
        _slowdown = newSlowdown;
    }

    private void MovementUpdate() {
        _interpolateValue += Time.deltaTime * (_speed * (1 - _slowdown)) / _pathDistance;
        gameObject.transform.position = Vector3.Lerp(_currentPathPoint.Position, _nextPathPoint.Position, _interpolateValue);
        if (gameObject.transform.position == _nextPathPoint.Position) {
            if (_path.Path[_nextPathPointIndex].Type == PathPointType.End) {
                End();
            }
            else {
                NextPoint();
            }
        }
    }

    private void Teleport() {
        NextPoint();
        if (_currentPathPoint.Type == PathPointType.Teleport) {
            Teleport();
        }
    }

    private void End() {
        _stop = true;
        PathCompleted?.Invoke(this);
    }

    private void NextPoint() {
        _interpolateValue = 0;
        _nextPathPointIndex += 1;
        _nextPathPoint = _path.Path[_nextPathPointIndex];
        _currentPathPoint = _path.Path[_nextPathPointIndex - 1];
        _pathDistance = Vector3.Distance(_currentPathPoint.Position, _nextPathPoint.Position);
        gameObject.transform.LookAt(new Vector3(_nextPathPoint.Position.x, gameObject.transform.position.y, _nextPathPoint.Position.z));
    }

    private void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }
}
