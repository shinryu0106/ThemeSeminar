using LibTS;
using UnityEngine;

public class Sample_ForwardConstantly_Rigidbody : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private float _error = 0.1f;
    [SerializeField] private TimeType _timeType = TimeType.Default;
    [SerializeField] private ForceMode _forceMode = ForceMode.Force;
    private Rigidbody _rigidbody = null;

    [Header("参照")]
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _direction = Vector3.forward;

    private void Awake()
    {
        if (!TryGetComponent(out _rigidbody))
            _rigidbody = gameObject.AddComponent<Rigidbody>();
    }

    private void Start()
    {
        if (_target != null)
            _rigidbody.ForwardConstantly(_target, _time, _moveSpeed, _rotateSpeed, _error, _timeType, _forceMode);
        else
            _rigidbody.ForwardConstantly(_direction, _time, _moveSpeed, _rotateSpeed, _timeType, _forceMode);
    }
}