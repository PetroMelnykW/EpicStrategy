using System.Collections;
using UnityEngine;

public class ArmorReductionEffect : Effect
{
    public float Strength => _effectData.Strength;

    protected override void StartEffect() {
        _effectsManager.Enemy.Health.AddArmorReductionEffect(this);
        StartCoroutine(DurationTimer());
    }

    private IEnumerator DurationTimer() {
        float timer = _effectData.Duration;

        while (timer > 0) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;
            }
        }
        _effectsManager.Enemy.Health.RemoveArmorReductionEffect(this);
        Destroy(this);
    }
}

