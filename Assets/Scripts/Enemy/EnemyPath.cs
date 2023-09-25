using System.Collections.Generic;
using UnityEngine;

public enum PathPointType
{
    Move,
    Teleport,
    End
}

public class EnemyPath : MonoBehaviour
{
    [SerializeField] private List<PathPoint> _path = new List<PathPoint>();

    public List<PathPoint> Path => _path;
    public Vector3 StartPoint => _path[0].Position;
    public Quaternion StartRotation => Quaternion.LookRotation(_path[1].Position - _path[0].Position);

    #region Gizmos
    #if UNITY_EDITOR
    private void OnDrawGizmos() {
        if (Path.Count <= 1) {
            return;
        }
        for (int i = 0; i < Path.Count - 1; i++) {
            Color color;
            switch (Path[i].Type) {
                case PathPointType.Move:
                    color = Color.green;
                    break;
                case PathPointType.Teleport:
                    color = Color.blue;
                    break;
                case PathPointType.End:
                    color = Color.red;
                    break;
                default:
                    color = Color.white;
                    break;
            }
            Debug.DrawLine(Path[i].Position, Path[i + 1].Position, color, 0, false);
        }
    }
    #endif
    #endregion
}

[System.Serializable]
public class PathPoint
{
    [SerializeField] private PathPointType _type = PathPointType.Move;
    [SerializeField] private Vector3 _position;

    public PathPointType Type => _type;
    public Vector3 Position => _position;
}