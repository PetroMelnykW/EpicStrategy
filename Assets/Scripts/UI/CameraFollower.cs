using UnityEditor;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    private const float DISTANCE_PERCENTAGE = 0.6f;

    [SerializeField] private bool _lookAtCamera = false;
    [SerializeField] private bool _stayNearTheCamera = false;
    [SerializeField] private GameObject _targetObject;
    [SerializeField] private float _distanceFromCamera = 10f;

    private Transform _cameraTransform;

    public void SetTarget(GameObject target) {
        _targetObject = target;
    }

    private void Start() {
        _cameraTransform = Camera.main.transform;
    }

    private void LateUpdate() {
        if (_lookAtCamera) {
            transform.LookAt(transform.position + _cameraTransform.rotation * Vector3.forward,
                            _cameraTransform.rotation * Vector3.up);
        }
        if (_stayNearTheCamera) {
            Vector3 direction = _targetObject.transform.position - _cameraTransform.position;
            direction.Normalize();
            float distance = Vector3.Distance(_targetObject.transform.position, _cameraTransform.position);
            if (distance > _distanceFromCamera * 1.5) {
                transform.position = _cameraTransform.position + direction * _distanceFromCamera;
            }
            else {
                transform.position = _cameraTransform.position + direction * (distance * DISTANCE_PERCENTAGE);
            }
            
        }
    }

    #region Editor
    #if UNITY_EDITOR
    [CustomEditor(typeof(CameraFollower))]
    private class CameraFollowerEditor : Editor
    {
        SerializedProperty lookAtCamera;
        SerializedProperty stayNearTheCamera;
        SerializedProperty targetObject;
        SerializedProperty distanceFromCamera;

        public override void OnInspectorGUI() {
            CameraFollower cameraFollower = (CameraFollower)target;
            if (cameraFollower == null) return;

            serializedObject.Update();

            EditorGUILayout.PropertyField(lookAtCamera);
            EditorGUILayout.PropertyField(stayNearTheCamera);
            if (cameraFollower._stayNearTheCamera) {
                EditorGUILayout.PropertyField(targetObject);
                EditorGUILayout.PropertyField(distanceFromCamera);
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnEnable() {
            lookAtCamera = serializedObject.FindProperty("_lookAtCamera");
            stayNearTheCamera = serializedObject.FindProperty("_stayNearTheCamera");
            targetObject = serializedObject.FindProperty("_targetObject");
            distanceFromCamera = serializedObject.FindProperty("_distanceFromCamera");
        }
    }
    #endif
    #endregion
}
