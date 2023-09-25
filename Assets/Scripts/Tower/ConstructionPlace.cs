using System.Collections;
using UnityEngine;
using Events;
using UnityEngine.UI;
using Money;

public class ConstructionPlace : MonoBehaviour, IRaycastable
{
    [SerializeField] private Vector3 _spawnPosition = Vector3.zero;
    [Space(15)]
    [SerializeField] private Slider _slider;
    [SerializeField] private GameObject _defaultMenu;
    [SerializeField] private GameObject _cancelMenu;
    [SerializeField] private Transform _canvas;

    private GameObject Menu {
        get { 
            return _currentMenu; 
        }
        set { 
            Destroy(_currentMenu);
            _currentMenu = Instantiate(value, _canvas);
            _currentMenu.GetComponent<TowerMenu>().SetConstructionPlace(this);
            _currentMenu.GetComponent<CameraFollower>().SetTarget(gameObject);
            _currentMenu.SetActive(false);
        }
    }

    private GameObject _currentMenu;
    private GameObject _tower;
    private bool _pause = false;
    private bool _construction = false;
    private Coroutine _constructionCoroutine;
    private int _lastPrice;

    public void StartConstruction(GameObject tower, float duration, int price) {
        if (_construction) {
            return;
        }
        Menu = _cancelMenu;
        MoneyManager.SpendMoney(price);
        _lastPrice = price;
        _tower?.SetActive(false);
        _construction = true;
        _constructionCoroutine = StartCoroutine(ConstructionTimer(tower, duration));
        if (_slider) {
            _slider.gameObject.SetActive(true);
            _slider.maxValue = duration;
            _slider.value = 0;
        }
    }

    public void CancelConstruction() {
        Menu = _tower ? _tower.GetComponent<Tower>().Menu : _defaultMenu;
        MoneyManager.AddMoney(_lastPrice);
        _tower?.SetActive(true);
        _construction = false;
        StopCoroutine(_constructionCoroutine);
        if (_slider) {
            _slider.gameObject.SetActive(false);
        }
    }

    public void SellTower(int sellPrice) {
        if (_tower) {
            Menu = _defaultMenu;
            MoneyManager.AddMoney(sellPrice);
            Destroy(_tower);
            _tower = null;
        }
    }

    public void OnRaycastHit() {
        SetMenuActive(true);
    }

    public void OnRaycastMissed() {
        SetMenuActive(false);
    }

    public void SetMenuActive(bool active) {
        Menu?.SetActive(active);
    }

    private void Awake() {
        Observer.Subscribe<PauseEvent>(Pause);
    }

    private void OnDestroy() {
        Observer.Unsubscribe<PauseEvent>(Pause);
    }

    private void Start() {
        Menu = _defaultMenu;
    }

    private void BuildTower(GameObject tower) {
        if (_tower) {
            Destroy(_tower);
        }
        _tower = Instantiate(tower, gameObject.transform);
        _tower.transform.localPosition = _spawnPosition;
        _construction = false;
        Menu = tower.GetComponent<Tower>().Menu;
        if (_slider) {
            _slider.gameObject.SetActive(false);
        }
    }

    private IEnumerator ConstructionTimer(GameObject tower, float duration) {
        float timer = duration;

        while (timer > 0f) {
            yield return null;

            if (!_pause) {
                timer -= Time.deltaTime;
                if (_slider) {
                    _slider.value = duration - timer;
                }
            }
        }

        BuildTower(tower);
    }

    private void Pause(object sender, PauseEvent eventData) {
        _pause = eventData.pause;
    }
}
