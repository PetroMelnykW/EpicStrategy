using UnityEngine;

public class TrackingProjectile : Projectile
{
    private const float ACTIVATE_DISTANCE = 0.05f;

    protected override void Movement() {
        if (!_target) {
            Destroy(gameObject);
            return;
        }
        transform.LookAt(_target.AnchorPoint);
        transform.position = Vector3.MoveTowards(transform.position, _target.AnchorPoint, _speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, _target.AnchorPoint) <= ACTIVATE_DISTANCE) {
            Activate();
        }
    }

    private void Activate() {
        foreach (EffectData effect in _effects) {
            _target.EffectsManager.AddEffect(effect);
        }
        Destroy(gameObject);
    }
}
