using TMPro;
using UnityEngine;
using Events;
using Level;

namespace Money
{
    public class MoneyManager : MonoBehaviour
    {
        // = = = Singleton
        public static int Money => _instance.money;

        private static MoneyManager _instance;

        public static void AddMoney(int money) {
            _instance.money += money;
            Observer.Post(_instance, new MoneyChangeEvent { });
            _instance.UpdateText();
        }
        public static void SpendMoney(int money) {
            _instance.money -= money;
            Observer.Post(_instance, new MoneyChangeEvent { });
            _instance.UpdateText();
        }

        // = = = Object
        [ReadOnly]
        [SerializeField] public int money = 0;
        [SerializeField] private TextMeshProUGUI _moneyText;

        public void UpdateText() {
            if (_moneyText == null) {
                return;
            }
            _moneyText.text = money.ToString();
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
            _instance.money = LevelManager.LevelData.AvailableResources.StartMoney;
            UpdateText();
        }
    }
}

