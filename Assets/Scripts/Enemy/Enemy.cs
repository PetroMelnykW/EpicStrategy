using UnityEngine;
using Money;

public class Enemy : MonoBehaviour
{
    private const float GIZMOS_ANCHOR_POINT_DRAW_RADIUS = 0.2f;

    [SerializeField] private EffectsManager _effectsManager;
    [SerializeField] private HealthSystem _health;
    [SerializeField] private PathFollower _movement;
    [SerializeField] private int _damage = 1; 
    [SerializeField] private int _reward = 1;
    [SerializeField] private Vector3 _anchorPoint;

    public EffectsManager EffectsManager => _effectsManager;
    public HealthSystem Health => _health;
    public PathFollower Movement => _movement;
    public Vector3 AnchorPoint => transform.position + _anchorPoint;

    private void Awake() {
        _health.Awake();
        _health.Subscribe(OnHealthChanged);
        _movement.Subscribe(OnMovementCompleted);
    }

    private void Update() {
        _movement.Update();
    }

    private void Death() {
        MoneyManager.AddMoney(_reward);
        Destroy(gameObject);
    }

    private void PathComplete() {
        LivesManager.Damage(_damage);
        Destroy(gameObject);
    }

    private void OnHealthChanged(object sender, float health) {
        if (health <= 0) {
            Death();
        }
    }

    private void OnMovementCompleted(object sender) {
        PathComplete();
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(AnchorPoint, GIZMOS_ANCHOR_POINT_DRAW_RADIUS * transform.localScale.x);
    }
}
