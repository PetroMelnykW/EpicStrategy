using System.Collections;
using UnityEngine;

public class IntervalDamage : Effect
{
    protected override void StartEffect() {
        StartCoroutine(DurationTimer());
        StartCoroutine(IntervalDamageTimer());
    }

    private IEnumerator DurationTimer() {
        float timer = _effectData.Duration;

        while (timer > 0) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;
            }
        }
        Destroy(this);
    }

    private IEnumerator IntervalDamageTimer() {
        float timer = 0;

        while (true) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;

                if (timer <= 0) {
                    _effectsManager.Enemy.Health.Damage(_effectData.Damage, _effectData.DamageType);
                    timer = _effectData.Interval;
                }
            }
        }
    }
}