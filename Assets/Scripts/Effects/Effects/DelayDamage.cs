using System.Collections;
using UnityEngine;

public class DelayDamage : Effect
{
    protected override void StartEffect() {
        StartCoroutine(DelayTimer());
    }

    protected IEnumerator DelayTimer() {
        float timer = _effectData.Delay;

        while (timer > 0) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;
            }
        }
        _effectsManager.Enemy.Health.Damage(_effectData.Damage, _effectData.DamageType);
        Destroy(this);
    }
}