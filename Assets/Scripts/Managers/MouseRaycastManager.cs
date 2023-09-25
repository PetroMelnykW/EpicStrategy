using UnityEngine;

public interface IRaycastable
{
    public void OnRaycastHit();

    public void OnRaycastMissed();
}

public class MouseRaycastManager : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;

    private IRaycastable _lastObject;

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            SendRay();
        }
    }

    private void SendRay() {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask)) {
            IRaycastable raycastableObject = hit.collider.GetComponent(typeof(IRaycastable)) as IRaycastable;
            if (raycastableObject != null) {
                _lastObject?.OnRaycastMissed();
                raycastableObject.OnRaycastHit();
                _lastObject = raycastableObject;
            }
        }
        else {
            _lastObject?.OnRaycastMissed();
            _lastObject = null;
        }
    }
}
