public class InstantDamage : Effect
{
    protected override void StartEffect() {
        _effectsManager.Enemy.Health.Damage(_effectData.Damage, _effectData.DamageType);
        Destroy(this);
    }
}