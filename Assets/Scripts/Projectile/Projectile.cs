using System.Collections.Generic;
using Events;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float _speed = 1f;

    protected List<EffectData> _effects;
    protected Enemy _target;
    protected bool _pause = false;
    
    public void SetData(List<EffectData> effects, Enemy enemy) {
        _effects = effects;
        _target = enemy;
    }

    protected void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
    }

    protected void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
        StopAllCoroutines();
    }

    protected void Update() {
        if (!_pause) {
            Movement();
        }    
    }

    protected void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }

    protected abstract void Movement();
}
