using UnityEngine;
using Events;
using Level;
using TMPro;

public class LivesManager : MonoBehaviour
{
    // = = = Singleton
    private static LivesManager _instance;

    public static void Damage(int damage) {
        _instance.lives -= damage;
        _instance.UpdateText();
        if (_instance.lives <= 0) {
            Observer.Post(_instance, new DefeatEvent { });
        }
    }

    // = = = Object
    [ReadOnly]
    [SerializeField] public int lives = 0;
    [SerializeField] private TextMeshProUGUI _livesText;

    public void UpdateText() {
        if (_livesText == null) {
            return;
        }
        _livesText.text = Mathf.Clamp(lives, 0, Mathf.Infinity).ToString();
    }

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    private void Start() {
        _instance.lives = LevelManager.LevelData.AvailableResources.StartLives;
        UpdateText();
    }
}
