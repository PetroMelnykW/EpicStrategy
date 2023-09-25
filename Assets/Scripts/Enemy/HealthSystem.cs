using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public enum DamageType
{
    Physical,
    Magical,
    Fire,
    Poison
}

[System.Serializable]
public class HealthSystem
{
    private event System.Action<object, float> HealthChanged;

    [ReadOnly]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth = 100;
    [Range(0, 1)]
    [SerializeField] private float _physicalProtection = 0;
    [Range(0, 1)]
    [SerializeField] private float _magicalProtection = 0;
    [Range(0, 1)]
    [SerializeField] private float _fireProtection = 0;
    [Range(0, 1)]
    [SerializeField] private float _poisonProtection = 0;
    [SerializeField] private Slider _healthSlider;

    private List<ArmorReductionEffect> _armorReductionEffects = new List<ArmorReductionEffect>();
    private float _armorReduction = 0;

    public void AddArmorReductionEffect(ArmorReductionEffect armorReductionEffect) {
        _armorReductionEffects.Add(armorReductionEffect);
        RecalculateArmorReductionEffect();
    }

    public void RemoveArmorReductionEffect(ArmorReductionEffect armorReductionEffect) {
        _armorReductionEffects.Remove(armorReductionEffect);
        RecalculateArmorReductionEffect();
    }

    public void Awake() {
        _health = _maxHealth;
        if (_healthSlider) {
            _healthSlider.maxValue = _maxHealth;
            _healthSlider.value = _health;
        }
    }

    public void Subscribe(System.Action<object, float> action) {
        HealthChanged += action;
    }

    public void Damage(float damage, DamageType damageType, float armorPenetration = 0) {
        switch (damageType) {
            case DamageType.Physical:
                _health -= damage * (_physicalProtection * (1 - _armorReduction) * (1 - armorPenetration));
                break;
            case DamageType.Magical:
                _health -= damage * (_magicalProtection * (1 - _armorReduction) * (1 - armorPenetration));
                break;
            case DamageType.Fire:
                _health -= damage * (_fireProtection * (1 - _armorReduction) * (1 - armorPenetration));
                break;
            case DamageType.Poison:
                _health -= damage * (_poisonProtection * (1 - _armorReduction) * (1 - armorPenetration));
                break;
        }
        _health -= damage;
        if (_health < 0) {
            _health = 0;
        }
        HealthChanged?.Invoke(this, _health);
        UpdateSlider();
    }

    public void Heal(float heal) {
        _health += heal;
        if (_health > _maxHealth) {
            _health = _maxHealth;
        }
        HealthChanged?.Invoke(this, _health);
        UpdateSlider();
    }

    public void SetArmorReduction(float armorReduction) {
        _armorReduction = armorReduction;
    }

    private void RecalculateArmorReductionEffect() {
        float newArmorReduction = 0;
        if (_armorReductionEffects.Any()) {
            newArmorReduction = _armorReductionEffects.Max(effect => effect.Strength);
        }
        _armorReduction = newArmorReduction;
    }

    private void UpdateSlider() {
        if (_healthSlider) {
            _healthSlider.value = _health;
        }
    }
}
