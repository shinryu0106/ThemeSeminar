using LibTS;
using UnityEngine;

public class Sample_ForwardConstantly_Transform : MonoBehaviour
{
    [Header("設定")]
    [SerializeField] private float _time = 1f;
    [SerializeField] private float _moveSpeed = 1f;
    [SerializeField] private float _rotateSpeed = 1f;
    [SerializeField] private TimeType _timeType = TimeType.Default;

    [Header("参照")]
    [SerializeField] private Transform _target = null;
    [SerializeField] private Vector3 _direction = Vector3.forward;

    private void Start()
    {
        if (_target != null)
            transform.ForwardConstantly(_target, _time, _rotateSpeed, _timeType);
        else
            transform.ForwardConstantly(_direction, _time, _moveSpeed, _rotateSpeed, _timeType);
    }
}