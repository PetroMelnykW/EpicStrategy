using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;

    public Enemy Enemy => _enemy;

    public void AddEffect(EffectData effectData) {
        Effect effect = null;
        switch (effectData.Type) {
            case EffectType.InstantDamage:
                effect = gameObject.AddComponent<InstantDamage>();
                break;
            case EffectType.IntervalDamage:
                effect = gameObject.AddComponent<IntervalDamage>();
                break;
            case EffectType.DelayDamage:
                effect = gameObject.AddComponent<DelayDamage>();
                break;
            case EffectType.Slowdown:
                effect = gameObject.AddComponent<SlowdownEffect>();
                break;
            case EffectType.ArmorReduction:
                effect = gameObject.AddComponent<ArmorReductionEffect>();
                break;
        }
        effect.SetData(effectData, this);
    }
}
