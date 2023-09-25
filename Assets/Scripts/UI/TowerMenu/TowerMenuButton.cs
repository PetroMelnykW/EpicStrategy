using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using Events;
using Level;
using Money;

public enum TowerMenuButtunState
{
    Default,
    Request,
    Lock
}

public enum TowerMenuButtonType
{
    Build,
    Cancel,
    Sell
}

public class TowerMenuButton : MonoBehaviour
{
    private event System.Action<TowerMenuButton> ButtonClicked;

    [SerializeField] private TowerMenuButtonType _type;
    [SerializeField] private string _towerName;
    [SerializeField] private GameObject _towerObject;
    [SerializeField] private int _price = 10;
    [SerializeField] private int _sellPrice = 10;
    [SerializeField] private float _constructionTime = 3;
    [Space(5)]
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _requestSprite;
    [SerializeField] private Sprite _lockSprite;
    [SerializeField] private Image _image;
    [SerializeField] private Button _button;

    public string TowerName => _towerName;
    public TowerMenuButtonType Type => _type;
    public GameObject TowerObject => _towerObject;
    public int Price => _price;
    public int SellPrice => _sellPrice;
    public float ConstructionTime => _constructionTime;
    public TowerMenuButtunState State => _state;

    private TowerMenuButtunState _state = TowerMenuButtunState.Default;

    public void Subscribe(System.Action<TowerMenuButton> action) {
        ButtonClicked += action;
    }

    public void SetState(TowerMenuButtunState state) {
        if (_state == TowerMenuButtunState.Lock) {
            return;
        }
        _state = state;
        switch (state) {
            case TowerMenuButtunState.Default:
                _image.sprite = _defaultSprite;
                break;
            case TowerMenuButtunState.Request:
                _image.sprite = _requestSprite;
                break;
            case TowerMenuButtunState.Lock:
                _image.sprite = _lockSprite;
                break;
        }
    }

    private void Awake() {
        Observer.Subscribe<MoneyChangeEvent>(UpdateCostAvailable);
        _button.onClick.AddListener(OnMouseButtonClickHandler);
        if (!LevelManager.LevelData.AvailableTowers.GetTowerAvailable(_towerName)) {
            SetState(TowerMenuButtunState.Lock);
        }
    }

    private void OnEnable() {
        UpdateCostAvailable(this, new MoneyChangeEvent { });
    }

    private void OnDestroy() {
        Observer.Unsubscribe<MoneyChangeEvent>(UpdateCostAvailable);
    }

    private void UpdateCostAvailable(object sender, MoneyChangeEvent moneyData) {
        if (_type != TowerMenuButtonType.Build || _state == TowerMenuButtunState.Lock) {
            return;
        }
        _button.interactable = MoneyManager.Money >= _price;
    }

    private void OnMouseButtonClickHandler() {
        ButtonClicked?.Invoke(this);
        switch (_state) {
            case TowerMenuButtunState.Default:
                SetState(TowerMenuButtunState.Request);
                break;
            case TowerMenuButtunState.Request:
                SetState(TowerMenuButtunState.Default);
                break;
        }
    }

    #region Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(TowerMenuButton))]
    private class TowerMenuButtonEditor : Editor 
    {
        SerializedProperty type;
        SerializedProperty towerName;
        SerializedProperty towerObject;
        SerializedProperty price;
        SerializedProperty sellPrice;
        SerializedProperty constructionTime;
        SerializedProperty defaultSprite;
        SerializedProperty requestSprite;
        SerializedProperty lockSprite;
        SerializedProperty image;
        SerializedProperty button;

        public override void OnInspectorGUI() {
            TowerMenuButton towerMenuButton = (TowerMenuButton)target;
            if (towerMenuButton == null) return;

            serializedObject.Update();

            EditorGUILayout.PropertyField(type);
            switch (towerMenuButton._type) {
                case TowerMenuButtonType.Build:
                    EditorGUILayout.PropertyField(towerName);
                    EditorGUILayout.PropertyField(towerObject);
                    EditorGUILayout.PropertyField(price);
                    EditorGUILayout.PropertyField(constructionTime);
                    break;
                case TowerMenuButtonType.Sell:
                    EditorGUILayout.PropertyField(sellPrice);
                    break;
            }
            EditorGUILayout.PropertyField(defaultSprite);
            EditorGUILayout.PropertyField(requestSprite);
            EditorGUILayout.PropertyField(lockSprite);
            EditorGUILayout.PropertyField(image);
            EditorGUILayout.PropertyField(button);

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable() {
            type = serializedObject.FindProperty("_type");
            towerName = serializedObject.FindProperty("_towerName");
            towerObject = serializedObject.FindProperty("_towerObject");
            price = serializedObject.FindProperty("_price");
            sellPrice = serializedObject.FindProperty("_sellPrice");
            constructionTime = serializedObject.FindProperty("_constructionTime");
            defaultSprite = serializedObject.FindProperty("_defaultSprite");
            requestSprite = serializedObject.FindProperty("_requestSprite");
            lockSprite = serializedObject.FindProperty("_lockSprite");
            image = serializedObject.FindProperty("_image");
            button = serializedObject.FindProperty("_button");
        }
    }
    #endif
    #endregion
}
