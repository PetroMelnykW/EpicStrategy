using System.Collections;
using System.Collections.Generic;
using Events;
using System.Linq;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    [SerializeField] protected GameObject _menu;
    [SerializeField] protected float _reload = 1;
    [SerializeField] protected List<EffectData> _effects;

    public GameObject Menu => _menu;

    protected List<Enemy> _enemies = new List<Enemy>();
    protected bool _pause = false;
    protected bool _isAttackTimer = false;

    protected void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
    }

    protected void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
        StopAllCoroutines();
    }

    protected virtual void OnTriggerEnter(Collider other) {
        _enemies.Add(other.GetComponent<Enemy>());
        if (_isAttackTimer == false) {
            _isAttackTimer = true;
            StartCoroutine(AttackTimer());
        }
    }

    protected virtual void OnTriggerExit(Collider other) {
        _enemies.Remove(other.GetComponent<Enemy>());
    }

    protected void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }

    protected IEnumerator AttackTimer() {
        float timer = 0;

        while (true) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    if (!_enemies.Any()) {
                        _isAttackTimer = false;
                        yield break;
                    }

                    Attack();
                    timer = _reload;
                }
            }
        }
    }

    protected abstract void Attack();
}