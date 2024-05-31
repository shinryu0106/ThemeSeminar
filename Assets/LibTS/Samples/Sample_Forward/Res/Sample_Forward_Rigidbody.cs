using LibTS;
using UnityEngine;

public class Sample_Forward_Rigidbody : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private float _error = 0.1f;
    [SerializeField] private TimeType _timeType = TimeType.Default;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    private Rigidbody _rigidbody = null;

    [Header("参照")]
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _direction = Vector3.forward;

    [Header("デバッグ")]
    [SerializeField] private bool _allowDebug = true;

    private void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
            _rigidbody = gameObject.AddComponent<Rigidbody>();
    }

    private void Update()
    {
        Info_Forward i = default;
        if (_target != null)
        {
            i = _rigidbody.Forward(_target, _moveSpeed, _rotateSpeed, _error, _timeType, _forceMode);
            if (_allowDebug)
            {
                Debug.Log($"移動距離: {i.Distance}, 回転角度: {i.Angle}");
                Debug.Log($"移動方向: {i.Direction}");
                Debug.Log($"目標地点に到達: {i.IsGoal}");
            }
        }
        else
        {
            i = _rigidbody.Forward(_direction, _moveSpeed, _rotateSpeed, _timeType, _forceMode);
            if (_allowDebug)
                Debug.Log($"移動距離: {i.Distance}, 回転角度: {i.Angle}");
        }
    }
}