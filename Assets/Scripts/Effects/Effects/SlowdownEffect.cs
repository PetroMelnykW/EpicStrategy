using System.Collections;
using UnityEngine;

public class SlowdownEffect : Effect
{
    public float Strength => _effectData.Strength;

    protected override void StartEffect() {
        _effectsManager.Enemy.Movement.AddSlowdownEffect(this);
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
        _effectsManager.Enemy.Movement.RemoveSlowdownEffect(this);
        Destroy(this);
    }
}
