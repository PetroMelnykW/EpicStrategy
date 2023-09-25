using UnityEngine;
using Events;

namespace Pause
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private KeyCode _pauseButton = KeyCode.Space;
        [SerializeField] private bool _paused = false;
        [SerializeField] private GameObject _pausePanel;

        private void Update() {
            if (Input.GetKeyDown(_pauseButton)) {
                _paused = !_paused;
                SetPause(_paused);
            }
        }
        private void SetPause(bool pause) {
            Observer.Post(this, new PauseEvent { pause = pause });
            UpdatePanelVisible();
        }

        private void UpdatePanelVisible() {
            if (_pausePanel) {
                _pausePanel.SetActive(_paused);
            }
        }
    }

}
