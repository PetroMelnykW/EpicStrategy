using UnityEngine;

public class ProjectileTower : Tower
{
    private const float GIZMOS_SPAWN_POSITION_DRAW_RADIUS = 0.2f;

    [SerializeField] private GameObject _projectile;
    [SerializeField] private Vector3 _spawnPosition;

    private Vector3 SpawnPosition => _spawnPosition + transform.position;

    private Enemy _target;

    protected override void OnTriggerEnter(Collider other) {
        base.OnTriggerEnter(other);
        if (!_target) {
            _target = other.GetComponent<Enemy>();
        }
    }

    protected override void OnTriggerExit(Collider other) {
        base.OnTriggerExit(other);
        if (other.GetComponent<Enemy>() == _target) {
            _target = null;
        }
    }

    protected override void Attack() {
        if (!_target) {
            _target = FindClosedEnemy();
            if (!_target) {
                return;
            }
        }
        GameObject projectileObject = Instantiate(_projectile);
        projectileObject.transform.localPosition = SpawnPosition;
        projectileObject.transform.LookAt(_target.transform);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.SetData(_effects, _target);
    }

    private Enemy FindClosedEnemy() {
        Enemy closedEnemy = null;
        foreach (Enemy enemy in _enemies) {
            if (enemy == null) {
                continue;
            }
            if (closedEnemy) {
                if (Vector3.Distance(gameObject.transform.position, enemy.transform.position) < 
                    Vector3.Distance(gameObject.transform.position, closedEnemy.transform.position)) {
                    closedEnemy = enemy;
                }
            }
            else {
                closedEnemy = enemy;
            }
        }
        return closedEnemy;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(SpawnPosition, GIZMOS_SPAWN_POSITION_DRAW_RADIUS);
    }
}