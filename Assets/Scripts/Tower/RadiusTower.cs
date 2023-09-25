public class RadiusTower : Tower
{
    protected override void Attack() {
        foreach (Enemy enemy in _enemies) {
            foreach (EffectData effect in _effects) {
                enemy.EffectsManager.AddEffect(effect);
            }
        }
    }
}