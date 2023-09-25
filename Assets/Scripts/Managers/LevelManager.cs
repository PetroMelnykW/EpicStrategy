using UnityEngine;

namespace Level
{
    public class LevelManager : MonoBehaviour
    {
        // = = = Singleton
        public static LevelData LevelData => _instance.levelData;

        private static LevelManager _instance;

        // = = = Object
        [SerializeField] public LevelData levelData;
        
        private void Awake() {
            if (_instance == null) {
                _instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
    }
}

