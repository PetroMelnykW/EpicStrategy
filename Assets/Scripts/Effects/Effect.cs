using UnityEngine;
using Events;
#if UNITY_EDITOR
using UnityEditor;
#endif

public enum EffectType
{
    InstantDamage,
    IntervalDamage,
    DelayDamage,
    Slowdown,
    ArmorReduction
}

public abstract class Effect : MonoBehaviour
{
    protected EffectData _effectData;
    protected EffectsManager _effectsManager;
    protected bool _pause = false;

    public void SetData(EffectData effectData, EffectsManager effectsManager) {
        _effectData = effectData;
        _effectsManager = effectsManager;
    }

    protected void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
    }

    protected void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
        StopAllCoroutines();
    }

    protected void Start() {
        StartEffect();
    }

    protected void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }

    protected abstract void StartEffect();
}

[System.Serializable]
public class EffectData
{
    [SerializeField] private EffectType _type;
    [SerializeField] private float _damage;
    [SerializeField] private DamageType _damageType;
    [SerializeField] private float _duration;
    [SerializeField] private float _interval;
    [SerializeField] private float _delay;
    [Range(0f, 1f)]
    [SerializeField] private float _strength;
    [SerializeField] string _visualEffect;


    public EffectType Type => _type;
    public float Damage => _damage;
    public DamageType DamageType => _damageType;
    public float Duration => _duration;
    public float Interval => _interval;
    public float Delay => _delay;
    public float Strength => _strength;
    public string VisualEffect => _visualEffect;


    #region EDITOR
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(EffectData))]
    private class EffectDataEditor : PropertyDrawer
    {
        private const float INDENT = 2;
        private const int BASIC_LINE_COUNT = 2;
        private int _lineCount = 1;

        private float LineIndent => EditorGUIUtility.singleLineHeight + INDENT;
        private float LineHeight => EditorGUIUtility.singleLineHeight;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            int newlinesCount = 0;
            switch (property.FindPropertyRelative("_type").enumValueIndex) {
                case (int)EffectType.InstantDamage:
                    newlinesCount = 2;
                    break;
                case (int)EffectType.IntervalDamage:
                    newlinesCount = 5;
                    break;
                case (int)EffectType.DelayDamage:
                    newlinesCount = 4;
                    break;
                case (int)EffectType.Slowdown:
                    newlinesCount = 3;
                    break;
                case (int)EffectType.ArmorReduction:
                    newlinesCount = 3;
                    break;
            }

            return LineIndent * (BASIC_LINE_COUNT + newlinesCount);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            Rect labelRect = new Rect(position.x, position.y, position.width, LineHeight);
            EditorGUI.LabelField(labelRect, label);


            int indented = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            _lineCount = 1;

            CreateField("_type", position, property);

            switch (property.FindPropertyRelative("_type").enumValueIndex) {
                case (int)EffectType.InstantDamage:
                    CreateField("_damageType", position, property);
                    CreateField("_damage", position, property);
                    break;
                case (int)EffectType.IntervalDamage:
                    CreateField("_damageType", position, property);
                    CreateField("_damage", position, property);
                    CreateField("_interval", position, property);
                    CreateField("_duration", position, property);
                    CreateField("_visualEffect", position, property);
                    break;
                case (int)EffectType.DelayDamage:
                    CreateField("_damageType", position, property);
                    CreateField("_damage", position, property);
                    CreateField("_delay", position, property);
                    CreateField("_visualEffect", position, property);
                    break;
                case (int)EffectType.Slowdown:
                    CreateField("_strength", position, property);
                    CreateField("_duration", position, property);
                    CreateField("_visualEffect", position, property);
                    break;
                case (int)EffectType.ArmorReduction:
                    CreateField("_strength", position, property);
                    CreateField("_duration", position, property);
                    CreateField("_visualEffect", position, property);
                    break;
            }

            EditorGUI.indentLevel = indented;

            EditorGUI.EndProperty();
        }

        private void CreateField(string name, Rect position, SerializedProperty property) {
            Rect rect = new Rect(position.x, position.y + LineIndent * _lineCount, position.width, LineHeight);
            EditorGUI.PropertyField(rect, property.FindPropertyRelative(name));
            _lineCount++;
        }
    }
#endif
    #endregion
}