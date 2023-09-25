using System.Collections.Generic;
using UnityEngine;

public class TowerMenu : MonoBehaviour
{
    [SerializeField] private List<TowerMenuButton> _towerMenuButtons;

    [SerializeField] private ConstructionPlace _constructionPlace;

    private TowerMenuButton _lastButton;

    public void SetConstructionPlace(ConstructionPlace constructionPlace) {
        _constructionPlace = constructionPlace;
    }

    private void Start() {
        foreach (TowerMenuButton button in _towerMenuButtons) {
            button.Subscribe(OnButtonClick);
        }
    }

    private void OnEnable() {
        HideButtonInfo();
    }

    private void OnDisable() {
        HideButtonInfo();
    }

    private void OnButtonClick(TowerMenuButton button) {
        if (button != _lastButton) {
            _lastButton?.SetState(TowerMenuButtunState.Default);
        }
        _lastButton = button;
        switch (button.State) {
            case TowerMenuButtunState.Default:
                ShowButtonInfo(button);
                break;
            case TowerMenuButtunState.Request:
                switch (button.Type) {
                    case TowerMenuButtonType.Build:
                        _constructionPlace.StartConstruction(button.TowerObject, button.ConstructionTime, button.Price);
                        break;
                    case TowerMenuButtonType.Cancel:
                        _constructionPlace.CancelConstruction();
                        break;
                    case TowerMenuButtonType.Sell:
                        _constructionPlace.SellTower(button.SellPrice);
                        break;
                }
                _constructionPlace.SetMenuActive(false);
                break;
        }
    }

    private void ShowButtonInfo(TowerMenuButton button) {

    }

    private void HideButtonInfo() {
        foreach (TowerMenuButton button in _towerMenuButtons) {
            button.SetState(TowerMenuButtunState.Default);
        }
    }
}
